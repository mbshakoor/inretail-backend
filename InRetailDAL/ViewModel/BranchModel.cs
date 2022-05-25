using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.ViewModel
{
    public class BranchModel
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string Code { get; set; }
        public string BranchName { get; set; }
        public string OrganizationName { get; set; }
        public string Description { get; set; }
    }

    public class BranchModelResponse
    {
        public string ErrorMessage { get; set; }
        public List<BranchModel> BranchModel { get; set; }
    }
}
