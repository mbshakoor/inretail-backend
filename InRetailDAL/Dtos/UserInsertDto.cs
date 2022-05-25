using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Dtos
{
    public class UserInsertDto
    {
        public int BranchId { get; set; }
        public string Username { get; set; }
        public string ContactNo { get; set; }
        public string LoginId { get; set; }
        public string Password { get; set; }
        public int  CreatedBy { get; set; }
    }
}
