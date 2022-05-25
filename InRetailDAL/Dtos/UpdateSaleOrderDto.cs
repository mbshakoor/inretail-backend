﻿using InRetailDAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Dtos
{
    public class UpdateSaleOrderDto
    {
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public string CustomerName { get; set; }
        public string ContactNo { get; set; }
        public int BranchId { get; set; }
        public List<SaleOrderDetailUpdateDto> SaleOrderDetail { get; set; }
    }

}
