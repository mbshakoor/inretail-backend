using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InRetailDAL.Dtos;
using InRetailDAL.Models;
using InRetailDAL.Services.IService;
using InRetailDAL.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using InRetailDAL.ConstFiles;
using Newtonsoft.Json;

namespace InRetail.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _organizationService;
        private readonly IMapper _mapper;
        public OrganizationController(IOrganizationService organizationService, IMapper mapper)
        {
            _organizationService = organizationService;
            _mapper = mapper;
        }

        [HttpGet("getAllOrganization")]
        public async Task<OrganizationModelResponse> GetAllOrganizations()
        {
            OrganizationModelResponse response = new OrganizationModelResponse();
            try
            {
                var result = await _organizationService.GetAllOrganizationAsync();
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_ORGANIZATION_LIST;
                if (string.IsNullOrEmpty(response.ErrorMessage)) {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    response.OrganizationModel = JsonConvert.DeserializeObject<List<OrganizationModel>>(result); ;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("getOrganizationById")]
        public async Task<OrganizationUpdateDto> GetOrganizationById(int Id)
        {
            OrganizationUpdateDto response = new OrganizationUpdateDto();
            try
            {
                var result = await _organizationService.GetOrganizationByIdAsync(Id);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_ORGANIZATION_FOUND;

                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<OrganizationUpdateDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            { 
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpPost("createOrganization")]
        public async Task<ActionResult<OrganizationResponseDto>> CreateOrganization( OrganizationInsertDto organization)
        {
            OrganizationResponseDto response = new OrganizationResponseDto();
            try
            {
                Organization organization1 = _mapper.Map<Organization>(organization);
                var result = await _organizationService.AddOrganizationAsync(organization1);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_ORGANIZATION_CREATE;
                if (!string.IsNullOrEmpty(result.ErrorMessage))
                {
                    response = result;
                }
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = result;
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }

            return response;
        }

        [HttpPost("updateOrganization")]
        public async Task<ActionResult<CreateOrgResponseDto>> UpdateOrganization( OrganizationUpdateObj organization)
        {
            CreateOrgResponseDto response = new CreateOrgResponseDto();
            try
            {
                Organization organization1 = _mapper.Map<Organization>(organization);
                var result = await _organizationService.UpdateOrganizationAsync(organization1);

                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_ORGANIZATION_UPDATE;
                if (result.Id == ConstHelper.OTHER_ORG_WITH_SAME_NAME)
                    response.ErrorMessage = ErrorHelper.ORGANIZATION_ALREADY_EXIST;

                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<CreateOrgResponseDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }
    }
}
