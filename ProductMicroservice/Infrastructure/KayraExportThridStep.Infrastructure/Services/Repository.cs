using KayraExportThridStep.Application.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportThridStep.Infrastructure.Services
{
    public class Repository<T> : IRepository<T> where T : class, new()
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public Repository(string connectionString, string tableName)
        {
            _connectionString = connectionString;
            _tableName = tableName;
        }

        public async Task<List<T>> GetAllAsync()
        {
            var list = new List<T>();

            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand($"SELECT * FROM {_tableName}", connection);

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
            using var cmd = new SqlCommand(
                $"SELECT * FROM {_tableName} WHERE ProductId = @id", connection);

            cmd.Parameters.AddWithValue("@id", id);

            await connection.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return Map(reader);
        }

        private static T Map(SqlDataReader reader)
        {
            var entity = new T();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                if (!reader.HasColumn(prop.Name) || reader[prop.Name] == DBNull.Value)
                    continue;

                prop.SetValue(entity, reader[prop.Name]);
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
                if (reader.GetName(i).Equals(columnName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}

