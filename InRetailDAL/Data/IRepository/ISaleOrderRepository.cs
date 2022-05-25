using InRetailDAL.Models;
using InRetailDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Data.IRepository
{
    public interface ISaleOrderRepository : IRepository<SaleOrder>
    {
        Task<SaleOrder> GetSaleOrderByIdAsync(int id);
        Task<string> GetAllSaleOrderAsync();
        Task<string> SearchSaleOrderAsync(int? BranchId, string? ContactNo, string? CustomerName, DateTime? FromDate,
            DateTime? ToDate, string? OrderNo, string? ItemName, decimal? MinPrice, decimal? MaxPrice);
        Task<string> GetSaleOrderNo(int BranchId);
        Task<string> GetSaleOrderSummary(int? OrganizationId, DateTime? FromDate, DateTime? ToDate);
    }
}
