using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class APIResult<T>
    {
        public T Result { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public List<string> Roles { get; set; } = new List<string>();
    }
}
