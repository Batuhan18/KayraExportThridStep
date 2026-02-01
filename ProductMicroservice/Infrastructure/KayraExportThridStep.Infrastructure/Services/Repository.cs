using KayraExportThridStep.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace KayraExportThridStep.Infrastructure.Services
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public Repository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlServer")
                ?? throw new ArgumentNullException("SqlServer connection string not found");

            // Table attribute'den tablo adını al, yoksa class adını kullan
            var tableAttribute = typeof(T).GetCustomAttribute<TableAttribute>();
            _tableName = tableAttribute?.Name ?? typeof(T).Name;
        }

        public async Task<List<T>> GetAllAsync()
        {
            var list = new List<T>();
            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand($"SELECT * FROM [{_tableName}]", connection);
            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                list.Add(Map(reader));
            }
            return list;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);

            // Id kolonunu dinamik bul (ProductId, CategoryId, vs.)
            var idColumnName = GetIdColumnName();

            using var cmd = new SqlCommand(
                $"SELECT * FROM [{_tableName}] WHERE [{idColumnName}] = @id",
                connection
            );
            cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;
            return Map(reader);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var idColumnName = GetIdColumnName();

            using var cmd = new SqlCommand(
                $"DELETE FROM [{_tableName}] WHERE [{idColumnName}] = @id",
                connection
            );
            cmd.Parameters.Add("@id", System.Data.SqlDbType.Int).Value = id;
            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        private string GetIdColumnName()
        {
            // ProductId, CategoryId gibi Id ile biten property'yi bul
            var props = typeof(T).GetProperties();
            var idProp = props.FirstOrDefault(p =>
                p.Name.EndsWith("Id", StringComparison.OrdinalIgnoreCase) &&
                p.PropertyType == typeof(int));

            return idProp?.Name ?? "Id";
        }

        private static T Map(SqlDataReader reader)
        {
            var entity = new T();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                if (!reader.HasColumn(prop.Name))
                    continue;
                var value = reader[prop.Name];
                if (value == DBNull.Value)
                    continue;
                prop.SetValue(entity, value);
            }
            return entity;
        }
    }

    internal static class SqlExtensions
    {
        public static bool HasColumn(this SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i)
                          .Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}