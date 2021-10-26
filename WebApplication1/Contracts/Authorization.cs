namespace API.Contracts
{
    public class Authorization
    {
        public const string AD = "Administrator";
        public const string DT = "Distributor";
        public const string RT = "Retailer";
        public const string USERS = DT + RT;
        public const string ALLROLES = AD + DT + RT;
    }
}
