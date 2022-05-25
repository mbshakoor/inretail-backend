using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Dtos
{
    public class ItemUpdateDto
    {
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int ParentId { get; set; }
        public int BranchId { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class ItemUpdateObj
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int ParentId { get; set; }
        public int BranchId { get; set; }
        public int UpdatedBy { get; set; }
    }
}
