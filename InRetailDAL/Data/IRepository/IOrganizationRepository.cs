using InRetailDAL.Models;
using InRetailDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Data.IRepository
{
    public interface IOrganizationRepository : IRepository<Organization>
    {
        Task<Organization> GetOrganizationByIdAsync(int id);
        Task<Organization> GetOrganizationByNameAsync(string organizationName);
        Task<Organization> GetOrganizationByNameAndIdAsync(string organizationName, int orgnizationId);
        Task<string> GetAllOrganizationAsync();
        Task<string> GetOrganizationCode();
    }
}
