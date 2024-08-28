using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TallentexResult.Controllers
{
    public class SessionExpire : Attribute, IActionFilter
    {
        // Do something before the action executes.
        public void OnActionExecuting(ActionExecutingContext context)
        {
            //if (string.IsNullOrEmpty(context.HttpContext.Request.Query["fno"].ToString()))
            //{
            //    context.HttpContext.Session.SetString("fno", "31080361");
            //}
            //else
            //{
            //    context.HttpContext.Session.SetString("fno", context.HttpContext.Request.Query["fno"].ToString());
            //}

            // Check whether the allow anonymous is on or not in ASP.NET Core
            bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                             .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));

            if (hasAllowAnonymous) return;

            //if there is no session whitch key is "fno", user will not access to specified action and redirect to login page.
            var fno = context.HttpContext.Session.GetString("fno");
            if (fno == null)
            {
                context.Result = new RedirectToActionResult("index", "home", null);
            }
        }

        // Do something after the action executes.
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
