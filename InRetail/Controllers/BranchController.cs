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
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;
        private readonly IMapper _mapper;
        public BranchController(IBranchService branchService, IMapper mapper)
        {
            _branchService = branchService;
            _mapper = mapper;
        }

        [HttpGet("getAllBranches")]
        public async Task<BranchModelResponse> GetAllBranches(int OrganizationId)
        {
            BranchModelResponse response = new BranchModelResponse();
            try
            {
                var result = await _branchService.GetAllBranchAsync(OrganizationId);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_BRANCH_LIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    response.BranchModel = JsonConvert.DeserializeObject<List<BranchModel>>(result);
                }
            }
            catch(Exception exc)
            { 
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("getBranchById")]
        public async Task<BranchUpdateDto> GetBranchById(int Id)
        {
            BranchUpdateDto response = new BranchUpdateDto();
            try
            {
                var result = await _branchService.GetBranchByIdAsync(Id);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_BRANCH_FOUND;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<BranchUpdateDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpPost("createBranch")]
        public async Task<CreateBrachResponseDto> CreateBranch( BranchInsertDto branch)
        {
            CreateBrachResponseDto response = new CreateBrachResponseDto();
            try
            {
                Branch branch1 = _mapper.Map<Branch>(branch);
                var result = await _branchService.AddBranchAsync(branch1);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_BRANCH_CREATE;
                if (result.OrganizationId == ConstHelper.INVALID_ORGANIZATION)
                    response.ErrorMessage = ErrorHelper.ERROR_INVALID_ORGANIZATION;
                if (result.Id == ConstHelper.BRANCH_ALREADY_EXIST)
                    response.ErrorMessage = ErrorHelper.BRANCH_ALREADY_EXIST;

                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<CreateBrachResponseDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpPost("updateBranch")] // date formate : 2020-03-07T14:49:48.549Z
        public async Task<CreateBrachResponseDto> UpdateBranch( BranchUpdateObj branch)
        {
            CreateBrachResponseDto response = new CreateBrachResponseDto();
            try
            {
                Branch branch1 = _mapper.Map<Branch>(branch);
                var result = await _branchService.UpdateBranchAsync(branch1);

                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_BRANCH_UPDATE;
                if (result.OrganizationId == ConstHelper.INVALID_ORGANIZATION)
                    response.ErrorMessage = ErrorHelper.ERROR_INVALID_ORGANIZATION;
                if (result.Id == ConstHelper.BRANCH_ALREADY_EXIST)
                    response.ErrorMessage = ErrorHelper.BRANCH_ALREADY_EXIST;

                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<CreateBrachResponseDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("searchBranches")]
        public async Task<BranchModelResponse> SearchBranches(int? OrganizationId, DateTime? FromDate, DateTime? ToDate)
        {
            BranchModelResponse response = new BranchModelResponse();
            try
            {
                FromDate = ConstHelper.GetFromDate(FromDate);
                ToDate = ConstHelper.GetToDate(ToDate);
                var result = await _branchService.SearchBranchesAsync(OrganizationId, FromDate, ToDate);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_BRANCH_LIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    response.BranchModel = JsonConvert.DeserializeObject<List<BranchModel>>(result);
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
