namespace API.DTOs.RetailerPaymentMethods
{
    public class UpdateRetailerPaymentMethodRequest
    {
        public string Id { get; set; }
        public string PaymentMethodId { get; set; }
        public string Detail { get; set; }
    }
}
