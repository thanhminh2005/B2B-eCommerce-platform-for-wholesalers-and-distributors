namespace API.DTOs.CustomerRanks
{
    public class UpdateCustomerRankRequest
    {
        public string Id { get; set; }
        public string MembershipRankId { get; set; }
        public int Threshold { get; set; }
        public double DiscountRate { get; set; }
    }
}
