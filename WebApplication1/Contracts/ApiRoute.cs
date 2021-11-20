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
            public const string CheckEmail = Base + "/user/check/email";
            public const string CheckUsername = Base + "/user/check/username";
            public const string Vertified = Base + "/user/email-vertification";
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
            public const string GetAll = Base + "/category/";
            public const string Get = Base + "/category/{id}";
            public const string Create = Base + "/category";
            public const string Update = Base + "/category/";
        }

        public static class SubCategories
        {
            public const string GetAll = Base + "/category/sub-category";
            public const string Get = Base + "/category/sub-category/{id}";
            public const string Create = Base + "/category/sub-category";
            public const string Update = Base + "/category/sub-category/";
        }

        public static class Products
        {
            public const string Get = Base + "/product/{id}";
            public const string GetDistributor = Base + "/distributor/{id}/product";
            public const string GetAllProductDistributor = Base + "/{distributorId}/product";
            public const string RetailerGetDistributor = Base + "/Retailer/distributor/{id}/product";
            public const string Filter = Base + "/product/page";
            public const string Create = Base + "/product";
            public const string Update = Base + "/product/";
            public const string Delete = Base + "/product/";
            public const string Recommendation = Base + "/product/recommendation";
            public const string GetAll = Base + "/product/";
            //public const string GetAll = Base + "/product/";
        }

        public static class Banners
        {
            public const string Create = Base + "/banner";
            public const string Get = Base + "/banner/{id}";
            public const string GetAll = Base + "/banner/";
            public const string GetDistributor = Base + "/distributor/{id}/banner";
            public const string Update = Base + "/banner/";
            public const string Delete = Base + "/banner/";
        }

        public static class HomeBanners
        {
            public const string Create = Base + "/home/banner";
            public const string Get = Base + "/home/banner/{id}";
            public const string GetAll = Base + "/home/banner/";
            public const string Update = Base + "/home/banner/";
            public const string Delete = Base + "/home/banner/";
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
        public static class OrderDetails
        {
            public const string GetAll = Base + "/order/product/";
            public const string Get = Base + "/order/product/{id}";
            public const string Create = Base + "/order/product";
            public const string Update = Base + "/order/product/";
            public const string Delete = Base + "/order/product/{id}";
        }
        public static class Orders
        {
            public const string GetAll = Base + "/order/";
            public const string Get = Base + "/order/{id}";
            public const string Create = Base + "/order/";
            public const string Update = Base + "/order";
            public const string Delete = Base + "/order/{id}";
        }

        public static class Sessions
        {
            public const string GetAll = Base + "/session/";
            public const string Get = Base + "/session/{id}";
            public const string Create = Base + "/session/";
            public const string Update = Base + "/session";
            public const string Delete = Base + "/session/{id}";
        }

        public static class Checkouts
        {
            public const string Confirm = Base + "/checkout/";
        }

        public static class Prices
        {
            public const string GetAll = Base + "/product/{productId}/price/";
            public const string Get = Base + "/product/price/{id}";
            public const string Create = Base + "/product/price/";
            public const string Update = Base + "/product/price";
            public const string Delete = Base + "/product/price/{id}";
        }

        public static class MembershipRanks
        {
            public const string GetAll = Base + "/membership-rank/";
            public const string Get = Base + "/membership-rank/{id}";
            public const string Create = Base + "/membership-rank/";
            public const string Update = Base + "/membership-rank";
            public const string Delete = Base + "/membership-rank/{id}";
        }

        public static class CustomerRanks
        {
            public const string GetAll = Base + "/distributor/customer-rank";
            public const string Get = Base + "/distributor/customer-rank/{id}";
            public const string Create = Base + "/distributor/customer-rank/";
            public const string Update = Base + "/distributor/customer-rank";
            public const string Delete = Base + "/distributor/customer-rank/{id}";
        }

        public static class Memberships
        {
            public const string GetAll = Base + "/membership/";
            public const string Get = Base + "/membership/distributor/{distributor-id}/customer/{retailer-id}";
            public const string Create = Base + "/membership/";
            public const string Update = Base + "/membership";
            public const string Delete = Base + "/membership/{id}";
        }

        public static class Notifications
        {
            public const string Clients = Base + "/manager/notification/clients";
            public const string Role = Base + "/manager/notification/role";
        }

        public static class Momo
        {
            public const string Create = Base + "/momo/";
            public const string IPN = Base + "/momo/ipn";
        }
    }

}
