namespace API.MoMo
{
    public class IPNRequest
    {
        public string PartnerCode { get; set; }
        public string OrderId { get; set; }
        public string RequestId { get; set; }
        public long Amount { get; set; }
        public long OrderInfo { get; set; }
        public string OrderType { get; set; }
        public int TransId { get; set; }
        public int ResultCode { get; set; }
        public string Message { get; set; }
        public string Paytype { get; set; }
        public long ResponseTime { get; set; }
        public string ExtraData { get; set; }
        public string Signature { get; set; }
    }
}
