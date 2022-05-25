using InRetailDAL.ConstFiles;
using InRetailDAL.Data.IRepository;
using InRetailDAL.Helper;
using InRetailDAL.Models;
using InRetailDAL.Services.IService;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Services.ServiceImp
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _ItemRepository;
        private readonly IBranchRepository _branchRepository;
        private readonly IConfiguration _configuration;

        public ItemService(IItemRepository ItemRepository, IBranchRepository branchRepository, IConfiguration configuration)
        {
            _ItemRepository = ItemRepository;
            _branchRepository = branchRepository;
            _configuration = configuration;
        }

        public async Task<string> GetAllItemAsync(int BranchId)
        {
            return await _ItemRepository.GetAllItemAsync(BranchId);
        }

        public async Task<string> GetAllItemNamesAsync(int BranchId)
        {
            return await _ItemRepository.GetAllItemNamesAsync(BranchId);
        }

        public async Task<string> SearchItemsByNamesAsync(string Name, int BranchId)
        {
            return await _ItemRepository.SearchItemsByNamesAsync(Name, BranchId);
        }

        public async Task<string> GetAllCategoriesAsync(int BranchId)
        {
            return await _ItemRepository.GetAllItemAsync(BranchId, 1);
        }

        public async Task<Item> GetItemByIdAsync(int Id)
        {
            return await _ItemRepository.GetItemByIdAsync(Id);
        }

        public async Task<Item> AddItemAsync(Item Item)
        {
            Item response = new Item();
            var branch = await _branchRepository.GetBranchByIdAsync(Item.BranchId);
            if (branch == null)
            {
                response.BranchId = ConstHelper.INVALID_BRANCH;
            }
            else
            {
                var parent = await _ItemRepository.GetCategoryByIdAsync(Item.ParentId);
                if (parent == null)
                {
                    response.ParentId = ConstHelper.INVALID_PARENT_CATEGORY;
                }
                else
                {
                    var tempItem = await _ItemRepository.GetItemByNameAsync(Item.Name, Item.BranchId);
                    if (tempItem != null)
                    {
                        response.Id = ConstHelper.ITEM_ALREADY_EXIST;
                    }
                    else
                    {
                        Item.CreatedOn = DateTime.Now;
                        Item.IsActive = true;
                        Item.Code = await _ItemRepository.GetItemCode(Item.BranchId);
                        Item.UpdatedOn = DateTime.Now;
                        Item.UpdatedBy = Item.UpdatedBy;
                        Item.Type = ConstHelper.TYPE_PRODUCT;
                        response = await _ItemRepository.AddAsync(Item);
                    }
                }
            }
            return response;
        }

        public async Task<Item> UpdateItemAsync(Item Item)
        {
            Item response = new Item();
            var branch = await _branchRepository.GetBranchByIdAsync(Item.BranchId);
            if (branch == null)
            {
                response.BranchId = ConstHelper.INVALID_BRANCH;
            }
            else
            {
                var parent = await _ItemRepository.GetCategoryByIdAsync(Item.ParentId);
                if (parent == null)
                {
                    response.ParentId = ConstHelper.INVALID_PARENT_CATEGORY;
                }
                else
                {
                    var tempItem1 = await _ItemRepository.GetItemByNameAndIdAsync(Item.Name, Item.BranchId, Item.Id);
                    if (tempItem1 != null)
                    {
                        response.Id = ConstHelper.ITEM_ALREADY_EXIST;
                    }
                    else
                    {
                        Item tempItem = await _ItemRepository.GetItemByIdAsync(Item.Id);
                        tempItem.Description = Item.Description;
                        tempItem.Name = Item.Name;
                        tempItem.ParentId = Item.ParentId;
                        tempItem.Price = Item.Price;
                        tempItem.IsActive = Item.IsActive;
                        tempItem.UpdatedOn = DateTime.Now;
                        tempItem.UpdatedBy = Item.UpdatedBy;
                        tempItem.Type = ConstHelper.TYPE_PRODUCT;
                        if (Item.BranchId != tempItem.BranchId)
                            tempItem.Code = await _ItemRepository.GetItemCode(Item.BranchId);
                        tempItem.BranchId = Item.BranchId;

                        response = await _ItemRepository.UpdateAsync(tempItem);
                    }
                }
            }
            return response;
        }

        public async Task<Item> AddCategoryAsync(Item Item)
        {
            Item response = new Item();
            var branch = await _branchRepository.GetBranchByIdAsync(Item.BranchId);
            if (branch == null)
            {
                response.BranchId = ConstHelper.INVALID_BRANCH;
            }
            else
            {
                var parent = await _ItemRepository.GetCategoryByIdAsync(Item.ParentId);
                if (Item.ParentId > 0 && parent == null)
                {
                    response.ParentId = ConstHelper.INVALID_PARENT_CATEGORY;
                }
                else
                {
                    var tempItem = await _ItemRepository.GetCatgoryByNameAsync(Item.Name, Item.BranchId);
                    if (tempItem != null)
                    {
                        response.Id = ConstHelper.ITEM_ALREADY_EXIST;
                    }
                    else
                    {
                        Item.CreatedOn = DateTime.Now;
                        Item.IsActive = true;
                        Item.Code = await _ItemRepository.GetItemCode(Item.BranchId);
                        Item.UpdatedOn = DateTime.Now;
                        Item.UpdatedBy = Item.UpdatedBy;
                        Item.Type = ConstHelper.TYPE_CATEGORY;
                        response = await _ItemRepository.AddAsync(Item);
                    }
                }
            }
            return response;
        }

        public async Task<Item> UpdateCategoryAsync(Item Item)
        {
            Item response = new Item();
            var branch = await _branchRepository.GetBranchByIdAsync(Item.BranchId);
            if (branch == null)
            {
                response.BranchId = ConstHelper.INVALID_BRANCH;
            }
            else
            {
                var parent = await _ItemRepository.GetCategoryByIdAsync(Item.ParentId);
                if (Item.ParentId > 0 && parent == null)
                {
                    response.ParentId = ConstHelper.INVALID_PARENT_CATEGORY;
                }
                else
                {
                    var tempItem1 = await _ItemRepository.GetCatgoryByNameAndIdAsync(Item.Name, Item.BranchId, Item.Id);
                    if (tempItem1 != null)
                    {
                        response.Id = ConstHelper.ITEM_ALREADY_EXIST;
                    }
                    else
                    {
                        Item tempItem = await _ItemRepository.GetItemByIdAsync(Item.Id);
                        tempItem.Description = Item.Description;
                        tempItem.Name = Item.Name;
                        tempItem.ParentId = Item.ParentId;
                        tempItem.Price = Item.Price;
                        //tempItem.IsActive = Item.IsActive;
                        tempItem.UpdatedOn = DateTime.Now;
                        tempItem.UpdatedBy = Item.UpdatedBy;
                        tempItem.Type = ConstHelper.TYPE_CATEGORY;

                        if (Item.BranchId != tempItem.BranchId)
                            tempItem.Code = await _ItemRepository.GetItemCode(Item.BranchId);
                        tempItem.BranchId = Item.BranchId;

                        response = await _ItemRepository.UpdateAsync(tempItem);
                    }
                }
            }
            return response;
        }

        public async Task<string> SearchItemsAsync(int? BranchId, DateTime? FromDate, DateTime? ToDate, int? ParentId, decimal? Price, string? ItemName)
        {
            return await _ItemRepository.SearchItemsAsync(BranchId, FromDate, ToDate, ParentId, Price, ItemName);
        }

        public async Task<string> SearchCategoriesAsync(int? BranchId, DateTime? FromDate, DateTime? ToDate, int? ParentId, decimal? Price, string? ItemName)
        {
            return await _ItemRepository.SearchItemsAsync(BranchId, FromDate, ToDate, ParentId, Price, ItemName, 1);
        }

        public async Task<string> GetItemCount(int? BranchId)
        {
            return await _ItemRepository.GetItemCount(BranchId);
        }

        public async Task<int> ImportExcelData(string fileName, int branchId)
        {
            ExcelImportHelper helper = new ExcelImportHelper(_configuration, _ItemRepository);
            int importStatus = await helper.ImportExceFileData(fileName, branchId);
            return importStatus;
        }
    }
}
