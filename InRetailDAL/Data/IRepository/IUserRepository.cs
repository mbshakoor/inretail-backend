using InRetailDAL.Models;
using InRetailDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Data.IRepository
{
    public interface IUserRepository : IRepository<Users>
    {
        Task<Users> GetUserByIdAsync(int id);
        Users GetUserByNameAsync(string username, int branchId);
        Users GetUserByNameAndIdAsync(string username, int branchId, int id);
        Task<string> GetAllUserAsync(int BranchId);
        Task<Users> LoginUserAsync(string username, string password);
        Users LoginUserAsync(string username, string password, int OrganizationId);
        Task<Users> AuthenticateUserAsync(int UserId, string password);
        Task<string> SearchUsersAsync(int? BranchId, DateTime? FromDate, DateTime? ToDate, string? ContactNo, string? Username);
        Task<UsersOTP> CreateUserOTP(int userId);
        Task<bool> VerifyUserOTP(int userId, string OTP);
    }
}
