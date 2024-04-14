using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.MApplication.DTO
{
    public class DataForGridDTO<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int NumberOfPages { get; set; }
    }
}
