using InRetailDAL.ConstFiles;
using InRetailDAL.Data.IRepository;
using InRetailDAL.Models;
using InRetailDAL.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Data.RepositoryImp
{
    public class UserRepository : Repository<Users>, IUserRepository
    {
        public UserRepository(InRetailContext inRetailContext) : base(inRetailContext)
        {
        }

        public Task<Users> GetUserByIdAsync(int id)
        {
            return GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Users GetUserByNameAsync(string username, int branchId)
        {
            var branch = InRetailDbContext.Branches.SingleOrDefault(x => x.Id == branchId);
            var query = from user in InRetailDbContext.Users
                        join brnch in InRetailDbContext.Branches on user.BranchId equals brnch.Id
                        join org in InRetailDbContext.Organizations on brnch.OrganizationId equals org.Id
                        where user.LoginId == username 
                        && org.Id == branch.OrganizationId
                        select user;
            var tempUser = query.FirstOrDefault();
            return tempUser;
        }

        public Users GetUserByNameAndIdAsync(string username, int branchId, int id)
        {
            var branch = InRetailDbContext.Branches.SingleOrDefault(x => x.Id == branchId);
            var query = from user in InRetailDbContext.Users
                        join brnch in InRetailDbContext.Branches on user.BranchId equals brnch.Id
                        join org in InRetailDbContext.Organizations on brnch.OrganizationId equals org.Id
                        where user.LoginId == username && user.Id != id
                        && org.Id == branch.OrganizationId
                        select user;
            var tempUser = query.FirstOrDefault();
            return tempUser;
        }

        public Task<Users> LoginUserAsync(string username, string password)
        {
            return GetAll().FirstOrDefaultAsync(x => x.LoginId == username && x.Password == password);
        }

        public Users LoginUserAsync(string username, string password, int OrganizationId)
        {
            var query = from user in InRetailDbContext.Users
                        join brnch in InRetailDbContext.Branches on user.BranchId equals brnch.Id
                        join org in InRetailDbContext.Organizations on brnch.OrganizationId equals org.Id
                        where user.LoginId == username && user.Password == password
                        && org.Id == OrganizationId
                        select user;
            var tempUser = query.FirstOrDefault();
            return tempUser;
        }

        public Task<Users> AuthenticateUserAsync(int UserId, string password)
        {
            return GetAll().FirstOrDefaultAsync(x => x.Id == UserId && x.Password == password);
        }

        public async Task<string> GetAllUserAsync(int BranchId)
        {
            //string json = "";
            //var query = from user in InRetailDbContext.Users
            //            join brnch in InRetailDbContext.Branches on user.BranchId equals brnch.Id
            //            join org in InRetailDbContext.Organizations on brnch.OrganizationId equals org.Id
            //            select new UserModel
            //                 {
            //                     Id = user.Id,
            //                     OrganizationName = org.OrganizationName,
            //                     OrganizationId = org.Id,
            //                     BranchName = brnch.BranchName,
            //                     BranchId = brnch.Id,
            //                     ContactNo = user.ContactNo,
            //                     LoginId = user.LoginId
            //                 };

            //var organizationList = await query.ToListAsync();
            //json = JsonConvert.SerializeObject(organizationList);

            var paramBranchId = new SqlParameter(ConstHelper.spParamBranchId, (object)BranchId ?? DBNull.Value);
            var userList = await InRetailDbContext.UserModels.FromSqlRaw(
                ConstHelper.spGetAllUsers
                + " " + ConstHelper.spParamBranchId, 
                paramBranchId).ToListAsync();

            string json = JsonConvert.SerializeObject(userList);

            return json;
        }

        public async Task<string> SearchUsersAsync(int? BranchId, DateTime? FromDate, DateTime? ToDate, string? ContactNo, string? Username)
        {
            string json = "";
            var paramBranchId = new SqlParameter(ConstHelper.spParamBranchId, (object)BranchId ?? DBNull.Value);
            var paramFromDate = new SqlParameter(ConstHelper.spParamFromDate, (object)FromDate ?? DBNull.Value);
            var paramToDate = new SqlParameter(ConstHelper.spParamToDate, (object)ToDate ?? DBNull.Value);
            var paramUsername = new SqlParameter(ConstHelper.spParamUserName, (object)Username ?? DBNull.Value);
            var paramContactNo = new SqlParameter(ConstHelper.spParamContactNo, (object)ContactNo ?? DBNull.Value);

            List<UserModel> userList = null;

            userList = await InRetailDbContext.UserModels.FromSqlRaw(
            ConstHelper.spSearchUsers 
            + " " + ConstHelper.spParamBranchId
            + ", " + ConstHelper.spParamFromDate
            + ", " + ConstHelper.spParamToDate
            + ", " + ConstHelper.spParamContactNo
            + ", " + ConstHelper.spParamUserName,
            paramBranchId,
            paramFromDate,
            paramToDate,
            paramContactNo,
            paramUsername).ToListAsync();

            json = JsonConvert.SerializeObject(userList);

            return json;
        }

        public async Task<UsersOTP> CreateUserOTP(int userId)
        {
            UsersOTP otp = new UsersOTP();
            otp.UserId = userId;
            otp.Code = GeneralHelper.GetRandomOTP();
            otp.CreatedOn = DateTime.Now;
            otp.IsActive = true;

            await InRetailDbContext.UsersOTPs.AddAsync(otp);
            await InRetailDbContext.SaveChangesAsync();

            return otp;
        }

        public async Task<bool> VerifyUserOTP(int userId, string OTP)
        {
            bool flag = false;
            var query = from otp in InRetailDbContext.UsersOTPs
                        where otp.UserId == userId && otp.Code == OTP && otp.IsActive == true
                        select otp;
            UsersOTP userOtp = await query.FirstOrDefaultAsync();

            if (userOtp != null)
            {
                userOtp.IsActive = false;
                InRetailDbContext.Entry(userOtp).State = EntityState.Modified;
                flag = true;
                await InRetailDbContext.SaveChangesAsync();
            }
            return flag;
        }
    }
}
