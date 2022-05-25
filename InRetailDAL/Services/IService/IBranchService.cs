using InRetailDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Services.IService
{
    public interface IBranchService
    {
        Task<string> GetAllBranchAsync(int OrganizationId);
        Task<Branch> GetBranchByIdAsync(int Id);
        Task<Branch> AddBranchAsync(Branch branch);
        Task<Branch> AddAdminBranchAsync(int OrganizationId);
        Task<Branch> UpdateBranchAsync(Branch branch);
        Task<string> SearchBranchesAsync(int? OrganizationId, DateTime? FromDate, DateTime? ToDate);
    }
}
