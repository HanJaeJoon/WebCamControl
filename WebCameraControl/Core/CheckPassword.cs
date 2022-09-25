using Microsoft.AspNetCore.Mvc.Filters;

namespace WebCameraControl.Core
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class CheckLoginAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Session.GetString("IsPasswordChecked") != "1")
            {
                // context.Result = new UnauthorizedResult();
                throw new Exception("Unauthorized!");
            }
        }
    }
}
