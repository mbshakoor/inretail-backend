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
    public class SaleOrderRepository : Repository<SaleOrder>, ISaleOrderRepository
    {
        public SaleOrderRepository(InRetailContext inRetailContext) : base(inRetailContext)
        {
        }

        public Task<SaleOrder> GetSaleOrderByIdAsync(int id)
        {
            return GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<string> GetAllSaleOrderAsync()
        {
            var saleOrderList = await InRetailDbContext.BranchModels.FromSqlRaw(ConstHelper.spGetAllBranches).ToListAsync();
            string json = JsonConvert.SerializeObject(saleOrderList);

            return json;
        }

        public async Task<string> SearchSaleOrderAsync(int? BranchId, string? ContactNo, string? CustomerName, DateTime? FromDate, 
            DateTime? ToDate, string? OrderNo, string? ItemName, decimal? MinPrice, decimal? MaxPrice)
        {
            string json = "";
            var paramBranchId = new SqlParameter(ConstHelper.spParamBranchId, (object)BranchId ?? DBNull.Value);
            var paramContactNo = new SqlParameter(ConstHelper.spParamContactNo, (object)ContactNo ?? DBNull.Value);
            var paramCustomerName = new SqlParameter(ConstHelper.spParamCustomerName, (object)CustomerName ?? DBNull.Value);
            var paramFromDate = new SqlParameter(ConstHelper.spParamFromDate, (object)FromDate ?? DBNull.Value);
            var paramToDate = new SqlParameter(ConstHelper.spParamToDate, (object)ToDate ?? DBNull.Value);
            var paramOrderNo = new SqlParameter(ConstHelper.spParamOrderNo, (object)OrderNo ?? DBNull.Value);
            var paramItemName = new SqlParameter(ConstHelper.spParamItemName, (object)OrderNo ?? DBNull.Value);
            var paramMinPrice = new SqlParameter(ConstHelper.spParamMinPrice, (object)MinPrice ?? DBNull.Value);
            var paramMaxPrice = new SqlParameter(ConstHelper.spParamMaxPrice, (object)MaxPrice ?? DBNull.Value);

            List<SaleOrderModel> orderList = null;

            orderList = await InRetailDbContext.SaleOrderModels.FromSqlRaw(
            ConstHelper.spSearchSaleOrders
            + " " + ConstHelper.spParamBranchId
            + ", " + ConstHelper.spParamContactNo
            + ", " + ConstHelper.spParamCustomerName
            + ", " + ConstHelper.spParamFromDate
            + ", " + ConstHelper.spParamToDate
            + ", " + ConstHelper.spParamOrderNo
            + ", " + ConstHelper.spParamItemName
            + ", " + ConstHelper.spParamMinPrice
            + ", " + ConstHelper.spParamMaxPrice,
            paramBranchId,
            paramContactNo,
            paramCustomerName,
            paramFromDate,
            paramToDate,
            paramOrderNo,
            paramItemName,
            paramMinPrice,
            paramMaxPrice).ToListAsync();

            json = JsonConvert.SerializeObject(orderList);
            return json;
            //string json = "";
            //var query = from order in InRetailDbContext.SaleOrders
            //            join cust in InRetailDbContext.Customers on order.CustomerId equals cust.Id
            //            join brnch in InRetailDbContext.Branches on order.BranchId equals brnch.Id
            //            where order.CreatedOn >= fromDate && order.CreatedOn <= toDate
            //            && (order.CustomerId == CustomerId || CustomerId == 0)
            //            && (order.CustomerId == CustomerId || CustomerId == 0)
            //            select new SaleOrderModel
            //            {
            //                Id = order.Id,
            //                ContactNo = cust.ContactNo,
            //                BranchId = brnch.Id,
            //                BranchName = brnch.BranchName,
            //                CreatedOn = order.CreatedOn,
            //                CustomerId = cust.Id,
            //                CustomerName = cust.CustomerName,
            //                OrderNo = order.OrderNo
            //            };

            ////var organizationList = await query.ToListAsync();
            //var organizationList = await InRetailDbContext.SaleOrders.FromSqlRaw<SaleOrder>("").ToListAsync();
            //json = JsonConvert.SerializeObject(organizationList);

        }

        public async Task<string> GetSaleOrderSummary(int? OrganizationId, DateTime? FromDate, DateTime? ToDate)
        {
            string json = "";
            var paramOrgId = new SqlParameter(ConstHelper.spParamOrganizationId, (object)OrganizationId ?? DBNull.Value);
            var paramFromDate = new SqlParameter(ConstHelper.spParamFromDate, (object)FromDate ?? DBNull.Value);
            var paramToDate = new SqlParameter(ConstHelper.spParamToDate, (object)ToDate ?? DBNull.Value);

            List<SaleOrderSummary> orderList = null;

            orderList = await InRetailDbContext.SaleOrderSummaries.FromSqlRaw(
            ConstHelper.spGetSaleOrderSummary
            + " " + ConstHelper.spParamOrganizationId
            + ", " + ConstHelper.spParamFromDate
            + ", " + ConstHelper.spParamToDate,
            paramOrgId,
            paramFromDate,
            paramToDate).ToListAsync();

            json = JsonConvert.SerializeObject(orderList[0]);
            return json;
        }

        public async Task<string> GetSaleOrderNo(int BranchId)
        {
            var codeObj = await GetAll().Where(x => x.BranchId == BranchId).CountAsync();
            var nextId = 1;
            if (codeObj != 0)
                nextId = codeObj + 1;
            string code = nextId.ToString().PadLeft(ConstHelper.ORDER_NO_LENGTH, '0');
            var existCode = await GetAll().Where(x => x.OrderNo == code && x.BranchId == BranchId).CountAsync();
            while (existCode != 0)
            {
                nextId = nextId + 1;
                code = nextId.ToString().PadLeft(ConstHelper.ORDER_NO_LENGTH, '0');
                existCode = await GetAll().Where(x => x.OrderNo == code && x.BranchId == BranchId).CountAsync();
            }
            return code;
        }
    }
}
