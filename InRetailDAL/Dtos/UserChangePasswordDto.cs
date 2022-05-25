using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Dtos
{
    public class UserChangePasswordDto
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }

    public class UserIMEIChangePasswordDto
    {
        public int Id { get; set; }
        public string IMEI { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
    }
}
