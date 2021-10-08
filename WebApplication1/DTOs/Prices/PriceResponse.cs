using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs.Prices
{
    public class PriceResponse
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public double Value { get; set; }
        public int Volume { get; set; }
    }
}
