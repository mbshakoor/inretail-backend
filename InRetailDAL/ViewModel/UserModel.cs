using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.ViewModel
{
    public class UserModel
    {
        public int Id { get; set; }
        public int BranchId { get; set; }
        public int OrganizationId { get; set; }
        public string BranchName { get; set; }
        public string Username { get; set; }
        public string OrganizationName { get; set; }
        public string ContactNo { get; set; }
        public string LoginId { get; set; }
    }

    public class UserModelResponse
    {
        public string ErrorMessage { get; set; }
        public List<UserModel> UserModel { get; set; }
    }

}
