using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Dtos
{
    public class CategoryInsertModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
        public int BranchId { get; set; }
        public int CreatedBy { get; set; }
    }
}
