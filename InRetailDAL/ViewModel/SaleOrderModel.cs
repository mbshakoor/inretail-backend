using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.ViewModel
{
    public class SaleOrderModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string CustomerName { get; set; }
        public string ContactNo { get; set; }
        public string OrderNo { get; set; }
        public decimal Total { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    public class SaleOrderSummary
    {
        public int TotalOrders { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class SaleOrderSummaryResponse
    {
        public string ErrorMessage { get; set; }
        public List<SaleOrderSummary> SaleOrderSummary { get; set; }
    }

    public class SaleOrderModelResponse
    {
        public string ErrorMessage { get; set; }
        public List<SaleOrderModel> SaleOrderModel { get; set; }
    }
}
