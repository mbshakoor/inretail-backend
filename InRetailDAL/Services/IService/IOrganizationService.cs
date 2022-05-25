using InRetailDAL.Dtos;
using InRetailDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Services.IService
{
    public interface IOrganizationService
    {
        Task<string> GetAllOrganizationAsync();
        Task<Organization> GetOrganizationByIdAsync(int Id);
        Task<OrganizationResponseDto> AddOrganizationAsync(Organization organization);
        Task<Organization> UpdateOrganizationAsync(Organization organization);
    }
}
