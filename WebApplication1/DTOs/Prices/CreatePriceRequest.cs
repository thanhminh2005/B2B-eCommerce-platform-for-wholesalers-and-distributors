using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Prices
{
    public class CreatePriceRequest
    {
        [Required]
        public string ProductId { get; set; }
        public double Value { get; set; }
        public int Volume { get; set; }
    }
}
