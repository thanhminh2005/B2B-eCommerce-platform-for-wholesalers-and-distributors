using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Prices
{
    public class CreatePriceRequest
    {
        public string ProductId { get; set; }
        public double Value { get; set; }
        public int Volume { get; set; }
    }
}
