using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRS_Base.Models
{
    public partial class Vendor
    {

        public Vendor(Vendor value)
        {
            this.ID = value.ID;
            this.Name = value.Name;
            this.EmailId = value.EmailId;
            this.MobileNumber = value.MobileNumber;
            this.Address = value.Address;
            this.BOA = value.BOA;
        }

        public int ID { get; set; }
        public string Name { get; set; }

        public string MobileNumber { get; set; }
        public string EmailId { get; set; }

        public string Address { get; set; }
        public string BOA { get; set; }
    }
}
