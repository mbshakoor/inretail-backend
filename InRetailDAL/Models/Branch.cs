using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Models
{
    public class Branch
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Code { get; set; }
        public string BranchName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        //public ICollection<User> Users { get; set; }
        //public ICollection<Item> Items { get; set; }
        //public Organization Organization { get; set; }


    }
}
