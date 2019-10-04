using System.Collections.Generic;

namespace Vale.Geographic.Api.Filters
{
    public class Error
    {
        public IEnumerable<string> messages { get; set; }
        public int status { get; set; }
    }
}