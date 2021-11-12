namespace API.DTOs.CustomerRanks
{
    public class CreateCustomerRankRequest
    {
        public string DistributorId { get; set; }
        public string MembershipRankId { get; set; }
        public int Threshold { get; set; }
        public double DiscountRate { get; set; }
    }
}
