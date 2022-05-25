using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.ViewModel
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Type { get; set; }
        public string ParentName { get; set; }
        public int ParentId { get; set; }
        public bool IsActive { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
    }

    public class ItemModelResponse
    {
        public string ErrorMessage { get; set; }
        public List<ItemModel> ItemModel { get; set; }
    }

    public class ItemCountModel
    {
        public int ItemCount { get; set; }
        public int CategoryCount { get; set; }
    }

    public class ItemCountModelResponse
    {
        public string ErrorMessage { get; set; }
        public ItemCountModel ItemCountModel { get; set; }
    }

}
