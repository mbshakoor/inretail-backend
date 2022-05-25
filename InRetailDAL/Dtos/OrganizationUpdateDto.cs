using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Dtos
{
    public class OrganizationUpdateDto
    {
        public string ErrorMessage { get; set; }
        public int Id { get; set; }
        public string OrganizationName { get; set; }
        public string Description { get; set; }
        public string ContactNo { get; set; }
    }

    public class OrganizationUpdateObj
    {
        public int Id { get; set; }
        public string OrganizationName { get; set; }
        public string Description { get; set; }
        public string ContactNo { get; set; }
    }

    public class OrganizationResponseDto
    {
        public string ErrorMessage { get; set; }
        public string Code { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
