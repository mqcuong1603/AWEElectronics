using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Web.Filters
{
    public class AuthorizeSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            
            if (session["IsLoggedIn"] == null || !(bool)session["IsLoggedIn"])
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Account" },
                        { "action", "Login" }
                    });
            }

            base.OnActionExecuting(filterContext);
        }
    }

    public class AuthorizeRoleAttribute : ActionFilterAttribute
    {
        public string[] AllowedRoles { get; set; }

        public AuthorizeRoleAttribute(params string[] roles)
        {
            AllowedRoles = roles;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpSessionStateBase session = filterContext.HttpContext.Session;
            
            if (session["IsLoggedIn"] == null || !(bool)session["IsLoggedIn"])
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Account" },
                        { "action", "Login" }
                    });
                return;
            }

            string userRole = session["UserRole"] as string;
            bool hasAccess = false;

            if (AllowedRoles != null && AllowedRoles.Length > 0)
            {
                foreach (var role in AllowedRoles)
                {
                    if (string.Equals(role, userRole, StringComparison.OrdinalIgnoreCase))
                    {
                        hasAccess = true;
                        break;
                    }
                }
            }

            if (!hasAccess)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Account" },
                        { "action", "AccessDenied" }
                    });
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
