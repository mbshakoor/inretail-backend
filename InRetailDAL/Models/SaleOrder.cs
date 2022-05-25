using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Models
{
    public class SaleOrder
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string OrderNo { get; set; }
        public int BranchId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public int CreatedBy { get; set; }
        //public Branch Branch { get; set; }
    }
}
