using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string OrganizationName { get; set; }
        public string ContactNo { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime ExpiredOn { get; set; }
        //public ICollection<Branch> Branches { get; set; }
    }
}
