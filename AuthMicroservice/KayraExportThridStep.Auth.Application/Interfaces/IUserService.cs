using KayraExportThridStep.Auth.Application.Dtos;
using KayraExportThridStep.Auth.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KayraExportThridStep.Auth.Application.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult> RegisterAsync(RegisterDto dto);
        Task<User?> ValidateUserAsync(string username, string password);
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
    }

    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
