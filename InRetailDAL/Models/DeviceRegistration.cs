using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InRetailDAL.Models
{
    public class DeviceRegistration
    {
        public int Id { get; set; }
        public int OrganizationId { get; set; }
        public string IMEI { get; set; }
        public bool IsPasswordChanged { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
