using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Dtos
{
    public class BranchUpdateDto
    {
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string BranchName { get; set; }
        public string Description { get; set; }
    }

    public class BranchUpdateObj
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string BranchName { get; set; }
        public string Description { get; set; }
    }
}
