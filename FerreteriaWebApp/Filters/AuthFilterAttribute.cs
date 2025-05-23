using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FerreteriaWebApp.Filters
{
    public class AuthFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            if (controller == "Auth") // tu LoginController se llama AuthController.cs
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            if (HttpContext.Current.Session["NombreUsuario"] == null)
            {
                filterContext.Result = new RedirectResult("~/Auth/LoginView");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}