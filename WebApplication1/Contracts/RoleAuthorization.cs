using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Contracts
{
    public class RoleAuthorization : Attribute, IAuthorizationFilter
    {
        private readonly string _role;

        public RoleAuthorization(string role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = "";
            if (context.HttpContext.User.Claims == null || !context.HttpContext.User.Claims.Any())
            {
                context.Result = new UnauthorizedResult();
            }
            else
            {
                var userIdClaim = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "userId");
                if (userIdClaim != null)
                {
                    var id = userIdClaim.Value;
                    var roleClaim = context.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role);
                    if (roleClaim != null)
                    {
                        role = roleClaim.Value;
                    }
                }
                else
                {
                    context.Result = new ForbidResult("Invalid Token");
                }
                if (!_role.Equals(role))
                {
                    context.Result = new UnauthorizedResult();
                }
            }
        }
    }
}
