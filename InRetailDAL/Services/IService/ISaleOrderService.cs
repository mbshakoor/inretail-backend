using InRetailDAL.Models;
using InRetailDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Services.IService
{
    public interface ISaleOrderService
    {
        Task<string> GetAllSaleOrderAsync();
        Task<SaleOrder> GetSaleOrderByIdAsync(int Id);
        Task<SaleOrderDetail> GetSaleOrderDetailByIdAsync(int Id);
        Task<List<SaleOrderDetailModel>> GetSaleOrderDetailBySOIdAsync(int Id);
        Task<SaleOrder> AddSaleOrderAsync(SaleOrder saleOrder);
        Task<SaleOrderDetail> AddSaleOrderDetailAsync(SaleOrderDetail saleOrderDetail);
        Task<SaleOrder> UpdateSaleOrderAsync(SaleOrder order);
        Task<string> SearchSaleOrderAsync(int? BranchId, string? ContactNo, string? CustomerName, DateTime? FromDate,
            DateTime? ToDate, string? OrderNo, string? ItemName, decimal? MinPrice, decimal? MaxPrice);
        Task<string> GetSaleOrderSummary(int? OrganizationId, DateTime? FromDate, DateTime? ToDate);
        Task<string> UpateSaleOrderDetail(int SaleOrderId, List<SaleOrderDetail> saleOrderDetails);
    }
}
