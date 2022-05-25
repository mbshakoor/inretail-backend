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
    public class SaleOrderDetailRepository : Repository<SaleOrderDetail>, ISaleOrderDetailRepository
    {
        public SaleOrderDetailRepository(InRetailContext inRetailContext) : base(inRetailContext)
        {
        }

        public async Task<SaleOrderDetail> GetSaleOrderDetailByIdAsync(int id)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<SaleOrderDetailModel>> GetSaleOrderDetailBySOIdAsync(int SaleOrderId)
        {
            //string json = "";
            var paramSaleOrderId = new SqlParameter(ConstHelper.spParamSaleOrderId, (object)SaleOrderId ?? DBNull.Value);

            List<SaleOrderDetailModel> orderDetailList = null;

            orderDetailList = await InRetailDbContext.SaleOrderDetailModels.FromSqlRaw(
            ConstHelper.spGetSaleOrderDetailBySOId
            + " " + ConstHelper.spParamSaleOrderId,
            paramSaleOrderId).ToListAsync();

            //json = JsonConvert.SerializeObject(orderDetailList);
            return orderDetailList;
        }

        public async Task<List<SaleOrderDetail>> GetSaleOrderDetailAsync(int SaleOrderId)
        {
            return await GetAll().Where(x => x.SaleOrderId == SaleOrderId).ToListAsync();
        }

        public async Task<string> DeleteSaleOrderDetail(int SaleOrderId)
        {
            string json = "true";
            var paramSaleOrderId = new SqlParameter(ConstHelper.spParamSaleOrderId, (object)SaleOrderId ?? DBNull.Value);


            await InRetailDbContext.Database.ExecuteSqlRawAsync(
            "EXEC	[dbo].[spDeleteSaleOrderDetail] @SaleOrderId = " + SaleOrderId);

            return json;
           
        }

    }
}
