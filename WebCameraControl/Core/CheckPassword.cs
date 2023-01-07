using Microsoft.AspNetCore.Mvc.Filters;

namespace WebCameraControl.Core;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class CheckLoginAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.HttpContext.Session.GetString("IsPasswordChecked") != "1")
        {
            // 관리자 인증 부분 생략 , 향후 관리자 인증 넣을시 여기 다시 주석해제애야함
            // context.Result = new UnauthorizedResult();
            // throw new Exception("Unauthorized!");
        }
    }
}