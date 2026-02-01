using KayraExportThridStep.Auth.Application.Dtos;
using KayraExportThridStep.Auth.Application.Interfaces;
using KayraExportThridStep.Auth.Core.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportThridStep.Auth.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly string _connectionString;

        public UserService(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlServer")
                ?? throw new ArgumentNullException("SqlServer connection string not found");
        }
        public async Task<User?> GetByEmailAsync(string email)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(
                "SELECT * FROM Users WHERE Email = @Email",
                connection
            );

            cmd.Parameters.AddWithValue("@Email", email);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapUser(reader);
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(
                "SELECT * FROM Users WHERE UserId = @UserId",
                connection
            );

            cmd.Parameters.AddWithValue("@UserId", userId);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
                return null;

            return MapUser(reader);

        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            using var connection = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(
                "SELECT * FROM Users WHERE Username = @Username",
                connection
            );

            cmd.Parameters.AddWithValue("@Username", username);

            await connection.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
                return null;

            return MapUser(reader);
        }

        public async Task<ServiceResult> RegisterAsync(RegisterDto dto)
        {
            try
            {
                var existingUser = await GetByUsernameAsync(dto.Username);
                if (existingUser != null)
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "Bu kullanıcı adı zaten kullanılıyor"
                    };
                }
                var existingEmail = await GetByEmailAsync(dto.Email);
                if (existingEmail != null)
                {
                    return new ServiceResult
                    {
                        Success = false,
                        Message = "Bu email adresi zaten kullanılıyor"
                    };
                }
                var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

                using var connection = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand(
                    @"INSERT INTO Users (Username, Email, PasswordHash, Role, CreatedAt, IsActive)
                      VALUES (@Username, @Email, @PasswordHash, 'User', GETDATE(), 1);
                      SELECT CAST(SCOPE_IDENTITY() as int);",
                    connection
                );

                cmd.Parameters.AddWithValue("@Username", dto.Username);
                cmd.Parameters.AddWithValue("@Email", dto.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);

                await connection.OpenAsync();
                var userId = (int)await cmd.ExecuteScalarAsync();
                return new ServiceResult
                {
                    Success = true,
                    Message = "Kayıt başarılı",
                    Data = new { UserId = userId }
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult
                {
                    Success = false,
                    Message = $"Kayıt sırasında hata: {ex.Message}"
                };
            }
        }


        public async Task<User?> ValidateUserAsync(string username, string password)
        {
            var user = await GetByUsernameAsync(username);

            if (user == null || !user.IsActive)
            {
                return null;
            }

            // Şifreyi doğrula
            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return null;
            }

            return user;
        }


        private static User MapUser(SqlDataReader reader)
        {
            return new User
            {
                UserId = reader.GetInt32(reader.GetOrdinal("UserId")),
                Username = reader.GetString(reader.GetOrdinal("Username")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                Role = reader.GetString(reader.GetOrdinal("Role")),
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive"))
            };
        }
    }
}
