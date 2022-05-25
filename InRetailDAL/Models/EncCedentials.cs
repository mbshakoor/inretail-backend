using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Models
{
    public class EncCedentials
    {
        public string TokenKey { get; set; }
        public string EncryptionKey { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
