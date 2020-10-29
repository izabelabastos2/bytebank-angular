using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vale.Geographic.Api.Models.Perimeters
{
    public class GetRequestModel
    {
        public Guid? site { get; set; }

        public bool? only_names { get; set; }
    }
}
