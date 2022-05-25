using InRetailDAL.ConstFiles;
using InRetailDAL.Data.IRepository;
using InRetailDAL.Models;
using InRetailDAL.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Services.ServiceImp
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepository _branchRepository;
        private readonly IOrganizationRepository _organizationRepository;

        public BranchService(IBranchRepository branchRepository, IOrganizationRepository organizationRepository)
        {
            _branchRepository = branchRepository;
            _organizationRepository = organizationRepository;
        }

        public async Task<string> GetAllBranchAsync(int OrganizationId)
        {
            return await _branchRepository.GetAllBranchAsync(OrganizationId);
        }

        public async Task<Branch> GetBranchByIdAsync(int Id)
        {
            return await _branchRepository.GetBranchByIdAsync(Id);
        }

        public async Task<Branch> AddBranchAsync(Branch branch)
        {
            Branch response = new Branch();
            var tempOrg = await _organizationRepository.GetOrganizationByIdAsync(branch.OrganizationId);
            if (tempOrg == null)
            {
                response.OrganizationId = ConstHelper.INVALID_ORGANIZATION; // if organization is invalid
            }
            else
            {
                var tempBranch = await _branchRepository.GetBranchByNameAsync(branch.BranchName, branch.OrganizationId);

                if (tempBranch != null)
                { 
                    response.Id = ConstHelper.BRANCH_ALREADY_EXIST; // if branch already exist
                }
                else
                {
                    branch.CreatedOn = DateTime.Now;
                    branch.IsActive = true;
                    branch.UpdatedOn = DateTime.Now;
                    branch.Code = await _branchRepository.GetBranchCode(branch.OrganizationId);

                    response = await _branchRepository.AddAsync(branch);
                }
            }
            return response;
        }

        public async Task<Branch> AddAdminBranchAsync(int OrganizationId)
        {
            Branch branch = new Branch();
            branch.CreatedOn = DateTime.Now;
            branch.IsActive = true;
            branch.UpdatedOn = DateTime.Now;
            branch.OrganizationId = OrganizationId;
            branch.BranchName = ConstHelper.ADMIN_BRANCH;
            branch.Code = await _branchRepository.GetBranchCode(branch.OrganizationId);

            return await _branchRepository.AddAsync(branch);
        }

        public async Task<Branch> UpdateBranchAsync(Branch branch)
        {
            Branch response = new Branch();
            var tempOrg = await _organizationRepository.GetOrganizationByIdAsync(branch.OrganizationId);
            if (tempOrg == null)
            {
                response.OrganizationId = ConstHelper.INVALID_ORGANIZATION; // if organization is invalid
            }
            else
            {
                var tempBranch1 = await _branchRepository.GetBranchByNameAndIdAsync(branch.BranchName, branch.OrganizationId, branch.Id);

                if (tempBranch1 != null)
                {
                    response.Id = ConstHelper.OTHER_BRANCH_WITH_SAME_NAME; // if branch already exist
                }
                else
                {
                    Branch tempBranch = await _branchRepository.GetBranchByIdAsync(branch.Id);
                    tempBranch.Description = branch.Description;
                    tempBranch.BranchName = branch.BranchName;
                    tempBranch.UpdatedOn = DateTime.Now;
                    if (tempBranch.OrganizationId != branch.OrganizationId)
                        tempBranch.Code = await _branchRepository.GetBranchCode(branch.OrganizationId);
                    tempBranch.OrganizationId = branch.OrganizationId;
                    response = await _branchRepository.UpdateAsync(tempBranch);
                }
            }
            return response;
        }

        public async Task<string> SearchBranchesAsync(int? OrganizationId, DateTime? FromDate, DateTime? ToDate)
        {
            return await _branchRepository.SearchBranchesAsync(OrganizationId, FromDate, ToDate);
        }

    }
}
