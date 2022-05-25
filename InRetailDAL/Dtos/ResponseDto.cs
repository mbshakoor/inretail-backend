using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Dtos
{
    public class ResponseDto
    {
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public int BranchId { get; set; }
    }

    public class CreateBrachResponseDto
    {
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
    }

    public class CreateOrgResponseDto
    {
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
    }

    public class SaleOrderResponseDto
    {
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string OrderNo { get; set; }
        public int BranchId { get; set; }
    }
}
