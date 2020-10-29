using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Vale.Geographic.Api.Models.Auth
{
    public class UserInfo
    {
        public string EmployeeID { get; set; }
        public string  EmploymentStatus { get; set; }
        public string FirstName { get; set; }
        public string LocationStateProvince { get; set; }
        public string NameIdentifier { get; set; }
        public string UserFullName { get; set; }
        public string ValeCSPID { get; set; }
        public string city { get; set; }
        public string cn { get; set; }
        public string locationCountry { get; set; }
        public string mail { get; set; }
        public string sn { get; set; }
        public string sub { get; set; }
        public object groupMembership { get; set; }
    }
}
