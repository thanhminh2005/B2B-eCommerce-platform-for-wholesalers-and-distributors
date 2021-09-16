using System;
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

        public static class Users
        {
            public const string Count = Base + "/user/count";
            public const string GetAll = Base + "/user/";
            public const string Get = Base + "/user/{id}";
            public const string Create = Base + "/user";
            public const string UpdatePassword = Base + "/user";
            public const string Update = Base + "/user/{Id}";
            public const string Delete = Base + "/user/{id}";
        }
    }

}
