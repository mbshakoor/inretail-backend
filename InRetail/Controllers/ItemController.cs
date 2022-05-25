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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;
        public ItemController(IItemService itemService, IMapper mapper)
        {
            _itemService = itemService;
            _mapper = mapper;
        }

        [HttpGet("getAllItems")]
        public async Task<ItemModelResponse> GetAllItems(int BranchId)
        {
            ItemModelResponse response = new ItemModelResponse();
            try
            {
                var result = await _itemService.GetAllItemAsync(BranchId);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_ITEM_LIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    response.ItemModel = JsonConvert.DeserializeObject<List<ItemModel>>(result);
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("getAllItemNames")]
        public async Task<ItemNameDtoResponse> GetAllItemNames(int BranchId)
        {
            ItemNameDtoResponse response = new ItemNameDtoResponse();
            try
            {
                var result = await _itemService.GetAllItemNamesAsync(BranchId);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_ITEM_NAME_LIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    response.ItemNameDto = JsonConvert.DeserializeObject<List<ItemNameDto>>(result);
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("searchItemsByName")]
        public async Task<ItemModelResponse> SearchItemsByName(string Name, int BranchId)
        {
            ItemModelResponse response = new ItemModelResponse();
            try
            {
                var result = await _itemService.SearchItemsByNamesAsync(Name, BranchId);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_ITEM_LIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    response.ItemModel = JsonConvert.DeserializeObject<List<ItemModel>>(result);
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("getAllCategories")]
        public async Task<ItemModelResponse> GetAllCategories(int BranchId)
        {
            ItemModelResponse response = new ItemModelResponse();
            try
            {
                var result = await _itemService.GetAllCategoriesAsync(BranchId);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_CATEGORY_LIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    response.ItemModel = JsonConvert.DeserializeObject<List<ItemModel>>(result);
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("getItemById")]
        public async Task<ItemUpdateDto> GetItemById(int Id)
        {
            ItemUpdateDto response = new ItemUpdateDto();
            try
            {
                var result = await _itemService.GetItemByIdAsync(Id);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_ITEM_FOUND;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<ItemUpdateDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("getCatgoryById")]
        public async Task<CategoryUpdateDto> GetCatgoryById(int Id)
        {
            CategoryUpdateDto response = new CategoryUpdateDto();
            try
            {
                var result = await _itemService.GetItemByIdAsync(Id);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_ITEM_FOUND;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<CategoryUpdateDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpPost("createItem")]
        public async Task<ResponseDto> CreateItem( ItemInsertDto item)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                Item item1 = _mapper.Map<Item>(item);
                var result = await _itemService.AddItemAsync(item1);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_ITEM_CREATE;
                if(result.BranchId == ConstHelper.INVALID_BRANCH)
                    response.ErrorMessage = ErrorHelper.ERROR_INVALID_BRANCH;
                if (result.ParentId == ConstHelper.INVALID_PARENT_CATEGORY)
                    response.ErrorMessage = ErrorHelper.ERROR_INVALID_PARENT_CATEGORY;
                if (result.Id == ConstHelper.ITEM_ALREADY_EXIST)
                    response.ErrorMessage = ErrorHelper.ITEM_ALREADY_EXIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<ResponseDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpPost("createCatgory")]
        public async Task<ResponseDto> CreateCategory( CategoryInsertModel category)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                Item category1 = _mapper.Map<Item>(category);
                var result = await _itemService.AddCategoryAsync(category1);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_ITEM_CREATE;
                if (result.BranchId == ConstHelper.INVALID_BRANCH)
                    response.ErrorMessage = ErrorHelper.ERROR_INVALID_BRANCH;
                if (result.ParentId == ConstHelper.INVALID_PARENT_CATEGORY)
                    response.ErrorMessage = ErrorHelper.ERROR_INVALID_PARENT_CATEGORY;
                if (result.Id == ConstHelper.CATEGORY_ALREADY_EXIST)
                    response.ErrorMessage = ErrorHelper.CATEGORY_ALREADY_EXIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<ResponseDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpPost("updateItem")] // date formate : 2020-03-07T14:49:48.549Z
        public async Task<ResponseDto> UpdateItem( ItemUpdateObj item)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                Item item1 = _mapper.Map<Item>(item);
                var result = await _itemService.UpdateItemAsync(item1);

                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_ITEM_UPDATE;
                if (result.BranchId == ConstHelper.INVALID_BRANCH)
                    response.ErrorMessage = ErrorHelper.ERROR_INVALID_BRANCH;
                if (result.ParentId == ConstHelper.INVALID_PARENT_CATEGORY)
                    response.ErrorMessage = ErrorHelper.ERROR_INVALID_PARENT_CATEGORY;
                if (result.Id == ConstHelper.ITEM_ALREADY_EXIST)
                    response.ErrorMessage = ErrorHelper.ITEM_ALREADY_EXIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<ResponseDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpPost("updateCategory")] // date formate : 2020-03-07T14:49:48.549Z
        public async Task<ResponseDto> UpdateCategory(CategoryUpdateObj category)
        {
            ResponseDto response = new ResponseDto();
            try
            {
                Item category1 = _mapper.Map<Item>(category);
                var result = await _itemService.UpdateCategoryAsync(category1);

                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_CATEGORY_UPDATE;
                if (result.BranchId == ConstHelper.INVALID_BRANCH)
                    response.ErrorMessage = ErrorHelper.ERROR_INVALID_BRANCH;
                if (result.ParentId == ConstHelper.INVALID_PARENT_CATEGORY)
                    response.ErrorMessage = ErrorHelper.ERROR_INVALID_PARENT_CATEGORY;
                if (result.Id == ConstHelper.CATEGORY_ALREADY_EXIST)
                    response.ErrorMessage = ErrorHelper.CATEGORY_ALREADY_EXIST;

                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response = _mapper.Map<ResponseDto>(result);
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("searchItems")]
        public async Task<ItemModelResponse> SearchItems(int? BranchId, DateTime? FromDate, DateTime? ToDate, int? ParentId, decimal? Price, string? ItemName)
        {
            ItemModelResponse response = new ItemModelResponse();
            try
            {
                FromDate = ConstHelper.GetFromDate(FromDate);
                ToDate = ConstHelper.GetToDate(ToDate);
                var result = await _itemService.SearchItemsAsync(BranchId, FromDate, ToDate, ParentId, Price, ItemName);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_ITEM_LIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    response.ItemModel = JsonConvert.DeserializeObject<List<ItemModel>>(result);
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("searchCategories")]
        public async Task<ItemModelResponse> SearchCategories(int? BranchId, DateTime? FromDate, DateTime? ToDate, int? ParentId, decimal? Price, string? ItemName)
        {
            ItemModelResponse response = new ItemModelResponse();
            try
            {
                FromDate = ConstHelper.GetFromDate(FromDate);
                ToDate = ConstHelper.GetToDate(ToDate);
                var result = await _itemService.SearchCategoriesAsync(BranchId, FromDate, ToDate, ParentId, Price, ItemName);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_ITEM_LIST;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    response.ItemModel = JsonConvert.DeserializeObject<List<ItemModel>>(result);
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [HttpGet("getItemCount")]
        public async Task<ItemCountModelResponse> GetItemCount(int? BranchId)
        {
            ItemCountModelResponse response = new ItemCountModelResponse();
            try
            {
                var result = await _itemService.GetItemCount(BranchId);
                if (result == null)
                    response.ErrorMessage = ErrorHelper.NO_ITEM_COUNT;
                if (string.IsNullOrEmpty(response.ErrorMessage))
                {
                    response.ErrorMessage = ErrorHelper.SUCCESS;
                    response.ItemCountModel = JsonConvert.DeserializeObject<ItemCountModel>(result);
                }
            }
            catch (Exception exc)
            {
                response.ErrorMessage = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }

        [AllowAnonymous]
        [HttpGet("importExcelFile")]
        public async Task<string> GetItemCount(string fileName, int BranchId)
        {
            string response = ErrorHelper.SUCCESS;
            try
            {
                var result = await _itemService.ImportExcelData(fileName, BranchId);
                if (result == ConstHelper.ImpInvalidColumns)
                    response = ErrorHelper.IMP_INVALID_COLS;
                else if (result == ConstHelper.ImpZeroCategory)
                    response = ErrorHelper.IMP_NO_CATEGORY;
                else if (result == ConstHelper.ImportError)
                    response = ErrorHelper.IMP_GENERIC_ERROR;
                else if (result == ConstHelper.ImpEmptyFile)
                    response = ErrorHelper.IMP_EMPTY_FILE;
            }
            catch (Exception exc)
            {
                response = ErrorHelper.ExceptionError(exc);
            }
            return response;
        }
    }
}
