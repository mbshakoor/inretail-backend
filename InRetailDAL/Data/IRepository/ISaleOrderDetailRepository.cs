using InRetailDAL.Models;
using InRetailDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Data.IRepository
{
    public interface ISaleOrderDetailRepository : IRepository<SaleOrderDetail>
    {
        Task<SaleOrderDetail> GetSaleOrderDetailByIdAsync(int id);
        Task<List<SaleOrderDetailModel>> GetSaleOrderDetailBySOIdAsync(int SaleOrderId);
        Task<List<SaleOrderDetail>> GetSaleOrderDetailAsync(int SaleOrderId);
        Task<string> DeleteSaleOrderDetail(int SaleOrderId);
    }
}
