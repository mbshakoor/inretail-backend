using InRetailDAL.Dtos;
using InRetailDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Services.IService
{
    public interface IUserService
    {
        Task<string> GetAllUserAsync(int BranchId);
        Task<Users> GetUserByIdAsync(int Id);
        Task<Users> AddUserAsync(Users user);
        Task<Users> AddAdminUserAsync(int BranchId, string ContactNo);
        Task<Users> UpdateUserAsync(Users user);
        Task<UserResponseDto> RegisterUserDeviceAsync(string username, string password, string OrgnanizationNo, string IMEI, string ipAddress);
        Task<UserResponseDto> LoginUserAsync(string username, string password, int OrganizationId, string ipAddress);
        Task<Users> AuthenticateUserAsync(int UserId, string password);
        Task<Users> ResetPassword(Users user);
        Task<Users> ChangePassword(Users user, string newpassword);
        Task<string> SearchUsersAsync(int? BranchId, DateTime? FromDate, DateTime? ToDate, string? ContactNo, string? Username);
        void GetEncCredentials();

        DeviceRegistration IsIMEIRegisterd(string IMEI);
        bool UpdateIMEIRegisteredStatus(string IMEI);
        UserResponseDto RefreshToken(string token, string ipAddress);
        bool RevokeToken(string token, string ipAddress);

        Task<bool> VerifyUserOTP(int UserId, string OTP);
    }
}
