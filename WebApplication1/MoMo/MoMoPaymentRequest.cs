namespace API.MoMo
{
    public class MoMoPaymentRequest
    {
        public string OrderId { get; set; }
        public string OrderInfo { get; set; }
        public string RedirectUrl { get; set; }
        public long Amount { get; set; }
    }
}
