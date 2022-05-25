using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Models
{
    public class Users
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string Username { get; set; }
        public string ContactNo { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int CreatedBy { get; set; }
        public int UpdatedBy { get; set; }
        //public Branch Branch { get; set; }

    }
}
