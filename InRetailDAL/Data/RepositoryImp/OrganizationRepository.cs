using InRetailDAL.ConstFiles;
using InRetailDAL.Data.IRepository;
using InRetailDAL.Models;
using InRetailDAL.ViewModel;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Data.RepositoryImp
{
    public class OrganizationRepository : Repository<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(InRetailContext inRetailContext) : base(inRetailContext)
        {
        }

        public Task<Organization> GetOrganizationByIdAsync(int id)
        {
            return GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Organization> GetOrganizationByNameAsync(string organizationName)
        {
            return GetAll().FirstOrDefaultAsync(x => x.OrganizationName == organizationName);
        }

        public Task<Organization> GetOrganizationByNameAndIdAsync(string organizationName, int orgnizationId)
        {
            return GetAll().FirstOrDefaultAsync(x => x.OrganizationName == organizationName && x.Id != orgnizationId);
        }

        public async Task<string> GetAllOrganizationAsync()
        {
            //string json = "";
            //var query = from org in InRetailDbContext.Organizations
            //                 select new OrganizationModel
            //                 {
            //                     Id = org.Id,
            //                     Code = org.Code,
            //                     Description = org.Description,
            //                     IsActive = org.IsActive,
            //                     OrganizationName = org.OrganizationName
            //                 };

            //var organizationList = await query.ToListAsync();
            var organizationList = await InRetailDbContext.OrganizationModels.FromSqlRaw(
                ConstHelper.spGetAllOrganizations).ToListAsync();

            string json = JsonConvert.SerializeObject(organizationList);

            return json;
        }

        public async Task<string> GetOrganizationCode()
        {
            var codeObj = GetAll().OrderByDescending(x => x.Id).FirstOrDefault();
            var nextId = 1;
            if (codeObj != null)
                nextId = codeObj.Id + 1;

            string code = nextId.ToString().PadLeft(ConstHelper.ORGANIZATION_CODE_LENGTH, '0');

            return code;
        }

    }
}
