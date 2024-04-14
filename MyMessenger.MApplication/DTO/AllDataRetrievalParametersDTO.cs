using MyMessenger.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyMessenger.MApplication.DTO
{
    public class AllDataRetrievalParametersDTO
    {
        public Dictionary<string, bool>? Sort { get; set; }
        public int? PageNumber { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
        public string? Subs { get; set; }
    }
}
