using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace AwIngenieriaSoftware
{
    public static class CustomClaimTypes
    {
        public const string RolName = "RolName";
        public const string FullName = "FullName";
    }
}