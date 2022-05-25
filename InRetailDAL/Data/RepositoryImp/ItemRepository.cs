using InRetailDAL.ConstFiles;
using InRetailDAL.Data.IRepository;
using InRetailDAL.Models;
using InRetailDAL.ViewModel;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Data.RepositoryImp
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        public ItemRepository(InRetailContext inRetailContext) : base(inRetailContext)
        {
        }

        public Task<Item> GetItemByIdAsync(int id)
        {
            return GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<Item> GetCategoryByIdAsync(int id)
        {
            return GetAll().FirstOrDefaultAsync(x => x.Id == id && x.Type == ConstHelper.TYPE_CATEGORY);
        }

        public Task<Item> GetItemByNameAsync(string name, int branchId)
        {
            return GetAll().FirstOrDefaultAsync(x => x.Name == name 
            && x.BranchId == branchId && x.Type == ConstHelper.TYPE_PRODUCT);
        }

        public Task<Item> GetCatgoryByNameAsync(string name, int branchId)
        {
            return GetAll().FirstOrDefaultAsync(x => x.Name == name 
            && x.BranchId == branchId && x.Type == ConstHelper.TYPE_CATEGORY);
        }

        public Task<Item> GetItemByNameAndIdAsync(string name, int branchId, int id)
        {
            return GetAll().FirstOrDefaultAsync(x => x.Name == name && x.BranchId == branchId
            && x.Id != id && x.Type == ConstHelper.TYPE_PRODUCT);
        }

        public Task<Item> GetCatgoryByNameAndIdAsync(string name, int branchId, int id)
        {
            return GetAll().FirstOrDefaultAsync(x => x.Name == name && x.BranchId == branchId
            && x.Id != id && x.Type == ConstHelper.TYPE_CATEGORY);
        }

        public async Task<string> GetAllItemAsync(int BranchId, int CategoryId = 0)
        {
            //string json = "";
            //var query = from item in InRetailDbContext.Items
            //            join brnch in InRetailDbContext.Branches on item.BranchId equals brnch.Id
            //                 select new ItemModel
            //                 {
            //                     Id = item.Id,
            //                     Code = item.Code,
            //                     BranchName = brnch.BranchName,
            //                     BranchId = brnch.Id,
            //                     IsActive = item.IsActive,
            //                     Name = item.Name,
            //                     ParentId = item.ParentId,
            //                     Price = item.Price,
            //                     Type = item.Type
            //                 };

            //var organizationList = await query.ToListAsync();
            //json = JsonConvert.SerializeObject(organizationList);

            var paramBranchId = new SqlParameter(ConstHelper.spParamBranchId, (object)BranchId ?? DBNull.Value);
            string spName = ConstHelper.spGetAllItems;
            if (CategoryId > 0)
                spName = ConstHelper.spGetAllCatgories;
            var itemList = await InRetailDbContext.ItemModels.FromSqlRaw(
                spName
                + " " + ConstHelper.spParamBranchId,
                paramBranchId).ToListAsync();

            string json = JsonConvert.SerializeObject(itemList);

            return json;
        }

        public async Task<string> GetAllItemNamesAsync(int BranchId)
        {
            var paramBranchId = new SqlParameter(ConstHelper.spParamBranchId, (object)BranchId ?? DBNull.Value);
            var itemList = await InRetailDbContext.ItemNames.FromSqlRaw(
                ConstHelper.spGetAllItemNames
                + " " + ConstHelper.spParamBranchId,
                paramBranchId).ToListAsync();

            string json = JsonConvert.SerializeObject(itemList);

            return json;
        }

        public async Task<string> SearchItemsByNamesAsync(string Name, int BranchId)
        {
            var paramBranchId = new SqlParameter(ConstHelper.spParamBranchId, (object)BranchId ?? DBNull.Value);
            var paramName = new SqlParameter(ConstHelper.spParamItemName, (object)Name ?? DBNull.Value);
            var itemList = await InRetailDbContext.ItemModels.FromSqlRaw(
                ConstHelper.spSearchItemsByName
                + " " + ConstHelper.spParamItemName
                +", " + ConstHelper.spParamBranchId,
                paramName,
                paramBranchId).ToListAsync();

            string json = JsonConvert.SerializeObject(itemList);

            return json;
        }

        public async Task<string> SearchItemsAsync(int? BranchId, DateTime? FromDate, DateTime? ToDate, int? ParentId, decimal? Price, string? ItemName, int CategoryId = 0)
        {
            string json = "";
            var paramBranchId = new SqlParameter(ConstHelper.spParamBranchId, (object)BranchId ?? DBNull.Value);
            var paramFromDate = new SqlParameter(ConstHelper.spParamFromDate, (object)FromDate ?? DBNull.Value);
            var paramToDate = new SqlParameter(ConstHelper.spParamToDate, (object)ToDate ?? DBNull.Value);
            var paramParentId = new SqlParameter(ConstHelper.spParamParentId, (object)ParentId ?? DBNull.Value);
            var paramItemName = new SqlParameter(ConstHelper.spParamItemName, (object)ItemName ?? DBNull.Value);
            var paramPrice = new SqlParameter(ConstHelper.spParamPrice, (object)Price ?? DBNull.Value);

            string spName = ConstHelper.spSearchItems;
            if (CategoryId > 0)
                spName = ConstHelper.spSearchCatgories;

            List<ItemModel> itemList = null;

            itemList = await InRetailDbContext.ItemModels.FromSqlRaw(
            spName
            + " " + ConstHelper.spParamBranchId
            + ", " + ConstHelper.spParamFromDate
            + ", " + ConstHelper.spParamToDate
            + ", " + ConstHelper.spParamParentId
            + ", " + ConstHelper.spParamPrice
            + ", " + ConstHelper.spParamItemName,
            paramBranchId,
            paramFromDate,
            paramToDate,
            paramParentId,
            paramPrice,
            paramItemName).ToListAsync();

            json = JsonConvert.SerializeObject(itemList);

            return json;
        }

        public async Task<string> GetItemCode(int BranchId)
        {
            var codeObj = await GetAll().Where(x => x.BranchId == BranchId).CountAsync();
            var nextId = 1;
            if (codeObj != 0)
                nextId = codeObj + 1;
            string code = nextId.ToString().PadLeft(ConstHelper.ITEM_CODE_LENGTH, '0');
            var existCode = await GetAll().Where(x => x.Code == code && x.BranchId == BranchId).CountAsync();
            while (existCode != 0)
            {
                nextId = nextId + 1;
                code = nextId.ToString().PadLeft(ConstHelper.ITEM_CODE_LENGTH, '0');
                existCode = await GetAll().Where(x => x.Code == code && x.BranchId == BranchId).CountAsync();
            }
            return code;
        }

        public async Task<string> GetItemCount(int? BranchId)
        {
            string json = "";
            var paramBranchId = new SqlParameter(ConstHelper.spParamBranchId, (object)BranchId ?? DBNull.Value);

            List<ItemCountModel> itemCount = null;

            itemCount = await InRetailDbContext.ItemCountModels.FromSqlRaw(
            ConstHelper.spGetItemCount
            + " " + ConstHelper.spParamBranchId,
            paramBranchId).ToListAsync();

            json = JsonConvert.SerializeObject(itemCount[0]);
            return json;
        }
    }
}
