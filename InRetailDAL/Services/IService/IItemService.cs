using InRetailDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Services.IService
{
    public interface IItemService
    {
        Task<string> GetAllItemAsync(int BranchId);
        Task<string> GetAllItemNamesAsync(int BranchId);
        Task<string> SearchItemsByNamesAsync(string Name, int BranchId);
        Task<string> GetAllCategoriesAsync(int BranchId);
        Task<Item> GetItemByIdAsync(int Id);
        Task<Item> AddItemAsync(Item Item);
        Task<Item> UpdateItemAsync(Item Item);
        Task<Item> AddCategoryAsync(Item Item);
        Task<Item> UpdateCategoryAsync(Item Item);
        Task<string> SearchItemsAsync(int? BranchId, DateTime? FromDate, DateTime? ToDate, int? ParentId, decimal? Price, string? ItemName);
        Task<string> SearchCategoriesAsync(int? BranchId, DateTime? FromDate, DateTime? ToDate, int? ParentId, decimal? Price, string? ItemName);
        Task<string> GetItemCount(int? BranchId);
        Task<int> ImportExcelData(string fileName, int branchId);
    }
}
