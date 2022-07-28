using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace AwIngenieriaSoftware
{
    public static class IdentityExtensions
    {
      

        public static string GetRol(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst("Role");
            
            return claim?.Value ?? string.Empty;
        }


        public static string GetFullName(this IIdentity identity)
        {
            ClaimsIdentity claimsIdentity = identity as ClaimsIdentity;
            Claim claim = claimsIdentity?.FindFirst(CustomClaimTypes.FullName);
            //foreach (var item in claimsIdentity.Claims)
            //{
            //    Console.WriteLine(item);
            //}
            return claim?.Value ?? string.Empty;
        }
    }
}