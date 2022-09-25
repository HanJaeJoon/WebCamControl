using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace WebCameraControl.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class CheckLoginAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Session.GetString("IsPasswordChecked") != "1")
            {
                throw new Exception("Unauthorized!");
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
