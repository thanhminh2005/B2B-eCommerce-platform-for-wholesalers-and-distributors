﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public static class Users
        {
            public const string Count = Base + "/user/count";
            public const string GetAll = Base + "/user/";
            public const string GetPaging = Base + "/user/page";
            public const string Get = Base + "/user/{id}";
            public const string Create = Base + "/user";
            public const string UpdatePassword = Base + "/update-password/{id}";
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
            public const string Create = Base + "/Product";
        }
    }

}
