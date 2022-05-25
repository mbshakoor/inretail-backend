using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Models
{
    public class UsersOTP
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }


    }
}
