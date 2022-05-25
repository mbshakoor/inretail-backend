using InRetailDAL.ConstFiles;
using InRetailDAL.Data.IRepository;
using InRetailDAL.Models;
using InRetailDAL.Services.IService;
using InRetailDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Services.ServiceImp
{
    public class SaleOrderService : ISaleOrderService
    {
        private readonly ISaleOrderRepository _saleOrderRepository;
        private readonly ISaleOrderDetailRepository _orderDetailRepository;

        public SaleOrderService(ISaleOrderRepository saleOrderRepository, ISaleOrderDetailRepository orderDetailRepository)
        {
            _saleOrderRepository = saleOrderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<string> GetAllSaleOrderAsync()
        {
            return await _saleOrderRepository.GetAllSaleOrderAsync();
        }

        public async Task<SaleOrder> GetSaleOrderByIdAsync(int Id)
        {
            return await _saleOrderRepository.GetSaleOrderByIdAsync(Id);
        }

        public async Task<SaleOrderDetail> GetSaleOrderDetailByIdAsync(int Id)
        {
            return await _orderDetailRepository.GetSaleOrderDetailByIdAsync(Id);
        }

        public async Task<List<SaleOrderDetailModel>> GetSaleOrderDetailBySOIdAsync(int Id)
        {
            return await _orderDetailRepository.GetSaleOrderDetailBySOIdAsync(Id);
        }

        public async Task<SaleOrder> AddSaleOrderAsync(SaleOrder saleOrder)
        {
            saleOrder.CreatedOn = DateTime.Now;
            saleOrder.UpdatedOn = DateTime.Now;
            saleOrder.CreatedBy = saleOrder.CreatedBy == 0 ? ConstHelper.APP_ADMIN_USER_ID : saleOrder.CreatedBy;
            saleOrder.UpdatedBy = saleOrder.CreatedBy;
            saleOrder.OrderNo = await _saleOrderRepository.GetSaleOrderNo(saleOrder.BranchId);

            return await _saleOrderRepository.AddAsync(saleOrder);
        }

        public async Task<SaleOrderDetail> AddSaleOrderDetailAsync(SaleOrderDetail saleOrderDetail)
        {
            saleOrderDetail.CreatedOn = DateTime.Now;
            saleOrderDetail.UpdatedOn = DateTime.Now;
            saleOrderDetail.Total = saleOrderDetail.Price * saleOrderDetail.Quantity;
            return await _orderDetailRepository.AddAsync(saleOrderDetail);
        }

        public async Task<SaleOrder> UpdateSaleOrderAsync(SaleOrder order)
        {
            SaleOrder tempSaleOrder = await _saleOrderRepository.GetSaleOrderByIdAsync(order.Id);
            tempSaleOrder.CustomerId = order.CustomerId;
            tempSaleOrder.UpdatedOn = DateTime.Now;
            tempSaleOrder.UpdatedBy = tempSaleOrder.CreatedBy == 0 ? ConstHelper.APP_ADMIN_USER_ID : tempSaleOrder.CreatedBy;

            return await _saleOrderRepository.UpdateAsync(tempSaleOrder);
        }

        public async Task<string> SearchSaleOrderAsync(int? BranchId, string? ContactNo, string? CustomerName, DateTime? FromDate,
            DateTime? ToDate, string? OrderNo, string? ItemName, decimal? MinPrice, decimal? MaxPrice)
        {
            return await _saleOrderRepository.SearchSaleOrderAsync(BranchId, ContactNo, CustomerName, FromDate,
                ToDate, OrderNo, ItemName, MinPrice, MaxPrice);
        }

        public async Task<string> GetSaleOrderSummary(int? OrganizationId, DateTime? FromDate, DateTime? ToDate)
        {
            return await _saleOrderRepository.GetSaleOrderSummary(OrganizationId, FromDate, ToDate);
        }

        public async Task<string> UpateSaleOrderDetail(int SaleOrderId, List<SaleOrderDetail> saleOrderDetails)
        {
            await _orderDetailRepository.DeleteSaleOrderDetail(SaleOrderId);

            foreach (var detail in saleOrderDetails)
            {
                detail.Id = 0;
                detail.SaleOrderId = SaleOrderId;
                await AddSaleOrderDetailAsync(detail);
            }

            return "true";
        }
    }
}
