namespace API.DTOs.RetailerPaymentMethods
{
    public class GetRetailerPaymentMethodsRequest
    {
        public string RetailerId { get; set; }
        public string PaymentMethodId { get; set; }
    }
}
