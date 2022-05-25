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
    public class BranchRepository : Repository<Branch>, IBranchRepository
    {
        public BranchRepository(InRetailContext inRetailContext) : base(inRetailContext)
        {
        }

        public Task<Branch> GetBranchByIdAsync(int id)
        {
            return GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Branch> GetBranchByNameAsync(string name, int organizationId)
        {
            return GetAll().FirstOrDefaultAsync(x => x.BranchName == name && x.OrganizationId == organizationId);
        }

        public Task<Branch> GetBranchByNameAndIdAsync(string name, int organizationId, int Id)
        {
            return GetAll().FirstOrDefaultAsync(x => x.BranchName == name && x.OrganizationId == organizationId && x.Id != Id);
        }

        public async Task<string> GetAllBranchAsync(int OrganizationId)
        {
            string json = "";
            //var query = from org in InRetailDbContext.Organizations
            //            join brnch in InRetailDbContext.Branches on org.Id equals brnch.OrganizationId
            //                 select new BranchModel
            //                 {
            //                     Id = brnch.Id,
            //                     Code = brnch.Code,
            //                     Description = brnch.Description,
            //                     OrganizationName = org.OrganizationName,
            //                     OrganizationId = org.Id,
            //                     BranchName = brnch.BranchName
            //                 };

            //var organizationList = await query.ToListAsync();
            //json = JsonConvert.SerializeObject(organizationList);
            var paramOrganizationId = new SqlParameter(ConstHelper.spParamOrganizationId, (object)OrganizationId ?? DBNull.Value);

            var branchList = await InRetailDbContext.BranchModels.FromSqlRaw(
                ConstHelper.spGetAllBranches
                + " " + ConstHelper.spParamOrganizationId,
                paramOrganizationId).ToListAsync();
            json = JsonConvert.SerializeObject(branchList);

            return json;
        }

        public async Task<string> SearchBranchesAsync(int? OrganizationId, DateTime? FromDate, DateTime? ToDate)
        {
            string json = "";
            var paramOrganizationId = new SqlParameter(ConstHelper.spParamOrganizationId, (object)OrganizationId ?? DBNull.Value);
            var paramFromDate = new SqlParameter(ConstHelper.spParamFromDate, (object)FromDate ?? DBNull.Value);
            var paramToDate = new SqlParameter(ConstHelper.spParamToDate, (object)ToDate ?? DBNull.Value);

            List<BranchModel> branchList = null;
            
            branchList = await InRetailDbContext.BranchModels.FromSqlRaw(
            ConstHelper.spSearchBranches 
            + " " + ConstHelper.spParamOrganizationId
            + ", " + ConstHelper.spParamFromDate
            + ", " + ConstHelper.spParamToDate,
            paramOrganizationId,
            paramFromDate,
            paramToDate).ToListAsync();

            json = JsonConvert.SerializeObject(branchList);

            return json;
        }

        public async Task<string> GetBranchCode(int OrganizationId)
        {
            var codeObj = await GetAll().Where(x => x.OrganizationId == OrganizationId).CountAsync();
            var nextId = 1;
            if (codeObj != 0)
                nextId = codeObj + 1;
            string code = nextId.ToString().PadLeft(ConstHelper.BRANCH_CODE_LENGTH, '0');
            var existCode = await GetAll().Where(x => x.Code == code && x.OrganizationId == OrganizationId).CountAsync();
            while (existCode != 0)
            {
                nextId = nextId + 1;
                code = nextId.ToString().PadLeft(ConstHelper.BRANCH_CODE_LENGTH, '0');
                existCode = await GetAll().Where(x => x.Code == code && x.OrganizationId == OrganizationId).CountAsync();
            }
            return code;
        }
    }
}
