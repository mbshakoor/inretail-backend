using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Dtos
{
    public class DeviceRegisterationDto
    {
        public string ErrorMessage { get; set; }
        public int OrganizationId { get; set; }
        public string IMEI { get; set; }
    }
}
