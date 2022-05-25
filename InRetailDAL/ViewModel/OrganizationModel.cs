using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.ViewModel
{
    public class OrganizationModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string OrganizationName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class OrganizationModelResponse
    {
        public string ErrorMessage { get; set; }
        public List<OrganizationModel> OrganizationModel { get; set; }
        
    }
}
