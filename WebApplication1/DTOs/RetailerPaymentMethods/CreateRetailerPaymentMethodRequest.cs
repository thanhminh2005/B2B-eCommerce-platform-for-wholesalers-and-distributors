namespace API.DTOs.RetailerPaymentMethods
{
    public class CreateRetailerPaymentMethodRequest
    {
        public string RetailerId { get; set; }
        public string PaymentMethodId { get; set; }
        public string Detail { get; set; }
    }
}
