using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Dtos
{
    public class SaleOrderInsertDto
    {
        public string CustomerName { get; set; }
        public string ContactNo { get; set; }
        public int BranchId { get; set; }
        public int CreatedBy { get; set; }
        public ICollection<SaleOrderDetailInsertDto> SaleOrderDetail { get; set; }
    }
}
