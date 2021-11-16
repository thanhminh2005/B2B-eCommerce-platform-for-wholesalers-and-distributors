using System.Text.Json.Serialization;

namespace API.MoMo
{
    public class RefundRequest
    {
        [JsonPropertyName("partnerCode")]
        public string PartnerCode { get; set; }

        [JsonPropertyName("orderId")]
        public string OrderId { get; set; }

        [JsonPropertyName("requestId")]
        public string RequestId { get; set; }

        [JsonPropertyName("amount")]
        public long Amount { get; set; }

        [JsonPropertyName("transId")]
        public long TransId { get; set; }



    }
}
