using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CfSample.DTO
{
    public class CreatePurchaseDTO
    {
        public decimal Price { get; set; }
        public string ProductName { get; set; }
    }
}
