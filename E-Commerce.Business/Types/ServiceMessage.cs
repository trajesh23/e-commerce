using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Business.Types
{
    public class ServiceMessage
    {
        public bool IsSucceed { get; set; }
        public int Count { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class ServiceMessage<T>
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Count { get; set; }
        public T? Data { get; set; }
    }
}
