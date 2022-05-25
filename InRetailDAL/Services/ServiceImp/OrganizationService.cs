using InRetailDAL.ConstFiles;
using InRetailDAL.Data.IRepository;
using InRetailDAL.Dtos;
using InRetailDAL.Models;
using InRetailDAL.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Services.ServiceImp
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IBranchService _branchService;
        private readonly IUserService _userService;

        public OrganizationService(IOrganizationRepository organizationRepository,
            IBranchService branchService, IUserService userService)
        {
            _organizationRepository = organizationRepository;
            _branchService = branchService;
            _userService = userService;
        }

        public async Task<string> GetAllOrganizationAsync()
        {
            return await _organizationRepository.GetAllOrganizationAsync();
        }

        public async Task<Organization> GetOrganizationByIdAsync(int Id)
        {
            return await _organizationRepository.GetOrganizationByIdAsync(Id);
        }

        public async Task<OrganizationResponseDto> AddOrganizationAsync(Organization organization)
        {
            OrganizationResponseDto response = null;
            var tempOrg = await _organizationRepository.GetOrganizationByNameAsync(organization.OrganizationName);
            if (tempOrg != null)
            {
                response = new OrganizationResponseDto();
                response.ErrorMessage = ErrorHelper.ORGANIZATION_ALREADY_EXIST;
            }
            else
            {
                organization.CreatedOn = DateTime.Now;
                organization.UpdatedOn = DateTime.Now;
                organization.ExpiredOn = DateTime.Now;
                organization.IsActive = true;
                organization.Code = await _organizationRepository.GetOrganizationCode();
                var newOrgnization = await _organizationRepository.AddAsync(organization);
                if (newOrgnization != null)
                {
                    response = new OrganizationResponseDto();
                    var branch = await _branchService.AddAdminBranchAsync(organization.Id);
                    var user = await _userService.AddAdminUserAsync(branch.Id, organization.ContactNo);
                    response.Code = organization.Code;
                    response.UserName = user.LoginId;
                    response.Password = user.Password;
                }
            }
            return response;
        }

        public async Task<Organization> UpdateOrganizationAsync(Organization organization)
        {
            Organization tempOrg = await _organizationRepository.GetOrganizationByNameAndIdAsync(organization.OrganizationName, organization.Id);
            if (tempOrg != null)
            {
                tempOrg.Id = ConstHelper.OTHER_ORG_WITH_SAME_NAME; // if the other organization exist with the same name
            }
            else
            {
                tempOrg = await _organizationRepository.GetOrganizationByIdAsync(organization.Id);
                tempOrg.Description = organization.Description;
                tempOrg.OrganizationName = organization.OrganizationName;
                tempOrg.UpdatedOn = DateTime.Now;
                tempOrg = await _organizationRepository.UpdateAsync(tempOrg);
            }
            return tempOrg;
        }
    }
}
