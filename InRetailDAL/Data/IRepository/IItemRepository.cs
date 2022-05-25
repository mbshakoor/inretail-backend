using InRetailDAL.Models;
using InRetailDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Data.IRepository
{
    public interface IItemRepository : IRepository<Item>
    {
        Task<Item> GetItemByIdAsync(int id);
        Task<Item> GetCategoryByIdAsync(int id);
        Task<Item> GetItemByNameAsync(string name, int branchId);
        Task<Item> GetCatgoryByNameAsync(string name, int branchId);
        Task<Item> GetItemByNameAndIdAsync(string name, int branchId, int id);
        Task<Item> GetCatgoryByNameAndIdAsync(string name, int branchId, int id);
        Task<string> GetAllItemAsync(int BranchId, int CategoryId = 0);
        Task<string> GetAllItemNamesAsync(int BranchId);
        Task<string> SearchItemsByNamesAsync(string Name, int BranchId);
        Task<string> GetItemCode(int BranchId);
        Task<string> SearchItemsAsync(int? BranchId, DateTime? FromDate, DateTime? ToDate, int? ParentId, decimal? Price, string? ItemName, int CategoryId = 0);
        Task<string> GetItemCount(int? BranchId);
    }
}
