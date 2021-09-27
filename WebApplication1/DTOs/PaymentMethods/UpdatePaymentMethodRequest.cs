namespace API.DTOs.PaymentMethods
{
    public class UpdatePaymentMethodRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
