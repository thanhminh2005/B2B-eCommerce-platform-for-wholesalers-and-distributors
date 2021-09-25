namespace API.Contracts
{
    public class ApiRoute
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/" + Version;

        public static class Accounts
        {
            public const string Post = Base + "/account/";
        }

        public static class Distributors
        {
            public const string GetAll = Base + "/distributor/";
            public const string Get = Base + "/distributor/{id}";
            public const string Create = Base + "/distributor";
            public const string Update = Base + "/distributor/";
            public const string Delete = Base + "/distributor/{id}";
        }
        public static class Retailers
        {
            public const string GetAll = Base + "/retailer/";
            public const string Get = Base + "/retailer/{id}";
            public const string Create = Base + "/retailer";
            public const string Update = Base + "/retailer/";
            public const string Delete = Base + "/retailer/{id}";
        }

        public static class Users
        {
            public const string Count = Base + "/user/count";
            public const string GetAll = Base + "/user/";
            public const string GetPaging = Base + "/user/page";
            public const string Get = Base + "/user/{id}";
            public const string Create = Base + "/user";
            public const string UpdatePassword = Base + "/update-password";
            public const string Update = Base + "/user/";
            public const string Delete = Base + "/user/{id}";
        }

        public static class Roles
        {
            public const string GetAll = Base + "/role/";
            public const string Get = Base + "/role/{id}";
            public const string Create = Base + "/role";
            public const string Update = Base + "/role/";
            public const string Delete = Base + "/role/{id}";
        }

        public static class Categories
        {
            public const string GetAll = Base + "/Category/";
            public const string Get = Base + "/Category/{id}";
            public const string Create = Base + "/Category";
            public const string Update = Base + "/Category/";
        }

        public static class Products
        {
            public const string Get = Base + "/Product/{id}";
            public const string GetDistributor = Base + "/{id}/Product";
            public const string Filter = Base + "/Product/page";
            public const string Create = Base + "/Product";
            public const string Update = Base + "/Product/";
            public const string Delete = Base + "/Product/";
        }

        public static class PaymentMethods
        {
            public const string GetAll = Base + "/payment-method/";
            public const string Get = Base + "/payment-method/{id}";
            public const string Create = Base + "/payment-method";
            public const string Update = Base + "/payment-method/";
            public const string Delete = Base + "/payment-method/{id}";
        }

        public static class RetailerPaymentMethods
        {
            public const string GetAll = Base + "/retailer/payment-method/";
            public const string Get = Base + "/retailer/payment-method/{id}";
            public const string Create = Base + "/retailer/payment-method";
            public const string Update = Base + "/retailer/payment-method/";
            public const string Delete = Base + "/retailer/payment-method/{id}";
        }
    }

}
