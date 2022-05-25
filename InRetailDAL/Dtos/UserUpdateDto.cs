using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Dtos
{
    public class UserUpdateDto
    {
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string Username { get; set; }
        public string ContactNo { get; set; }
        public string LoginId { get; set; }
        //public string Password { get; set; }
        public int  UpdatedBy { get; set; }
    }

    public class UserUpdateObj
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string Username { get; set; }
        public string ContactNo { get; set; }
        public string LoginId { get; set; }
        //public string Password { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class RegisterUserDeviceDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string OrgnanizationNo { get; set; }
        public string IMEI { get; set; }
    }

}
