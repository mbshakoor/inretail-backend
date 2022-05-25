using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.ViewModel
{
    public class SaleOrderDetailModel
    {
        public int Id { get; set; }
        public int SaleOrderId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
    }
}
