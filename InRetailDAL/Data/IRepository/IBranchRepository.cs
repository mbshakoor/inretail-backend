using InRetailDAL.Models;
using InRetailDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Data.IRepository
{
    public interface IBranchRepository : IRepository<Branch>
    {
        Task<Branch> GetBranchByIdAsync(int id);
        Task<string> GetAllBranchAsync(int OrganizationId);
        Task<Branch> GetBranchByNameAsync(string name, int organizationId);
        Task<Branch> GetBranchByNameAndIdAsync(string name, int organizationId, int Id);
        Task<string> SearchBranchesAsync(int? OrganizationId, DateTime? FromDate, DateTime? ToDate);
        Task<string> GetBranchCode(int OrganizationId);
    }
}
