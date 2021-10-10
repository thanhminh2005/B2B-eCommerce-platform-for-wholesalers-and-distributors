namespace API.DTOs.Prices
{
    public class CreatePriceRequest
    {
        public string ProductId { get; set; }
        public double Value { get; set; }
        public int Volume { get; set; }
    }
}
