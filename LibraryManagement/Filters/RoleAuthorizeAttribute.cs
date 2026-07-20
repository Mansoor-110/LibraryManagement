using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection.Metadata.Ecma335;

namespace LibraryManagement.Filters
{
    public class RoleAuthorizeAttribute: ActionFilterAttribute
    {
        private  string[] _allowedRoles;
        public RoleAuthorizeAttribute(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string role = context.HttpContext.Session.GetString("Role");
            if (string.IsNullOrEmpty(role) || Array.IndexOf(_allowedRoles, role) == -1)
            {

                context.Result = new RedirectToActionResult("Index", "Home",null);
                return;
            }

            base.OnActionExecuting(context);
        }

    }
}
