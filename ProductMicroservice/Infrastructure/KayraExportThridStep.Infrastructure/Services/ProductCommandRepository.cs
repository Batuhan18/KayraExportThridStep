using KayraExportThridStep.Application.CQRS.Commands;
using KayraExportThridStep.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace KayraExportThridStep.Infrastructure.Services
{
    public class ProductCommandRepository : IProductCommandRepository
    {
        private readonly string _connectionString;

        public ProductCommandRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlServer")
               ?? throw new Exception("SqlServer connection string not found");
        }

        public async Task<int> CreateAsync(CreateProductCommand command)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_Product_Create", connection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductName", command.ProductName);
            cmd.Parameters.AddWithValue("@ProductPrice", command.ProductPrice);
            cmd.Parameters.AddWithValue("@ProductImageUrl", command.ProductImageUrl);

            await connection.OpenAsync();
            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_Product_Delete", connection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductId", id);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task UpdateAsync(UpdateProductCommand command)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("sp_Product_Update", connection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductId", command.ProductId);
            cmd.Parameters.AddWithValue("@ProductName", command.ProductName);
            cmd.Parameters.AddWithValue("@ProductPrice", command.ProductPrice);
            cmd.Parameters.AddWithValue("@ProductImageUrl", command.ProductImageUrl);

            await connection.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
