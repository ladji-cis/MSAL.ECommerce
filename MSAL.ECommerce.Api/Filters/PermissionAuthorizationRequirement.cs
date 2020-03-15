using Microsoft.AspNetCore.Authorization;

namespace MSAL.ECommerce.Api.Filters
{
    public class PermissionAuthorizationRequirement : IAuthorizationRequirement
    {
        //Add any custom requirement properties if you have them
    }

    //public class PermissionAuthorizationHandler : AttributeAuthorizationHandler<PermissionAuthorizationRequirement, PermissionAttribute>
    //{
    //    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionAuthorizationRequirement requirement, IEnumerable<PermissionAttribute> attributes)
    //    {
    //        foreach (var permissionAttribute in attributes)
    //        {
    //            if (!await AuthorizeAsync(context.User, permissionAttribute.Name))
    //            {
    //                return;
    //            }
    //        }

    //        context.Succeed(requirement);
    //    }

    //    private Task<bool> AuthorizeAsync(ClaimsPrincipal user, string permission)
    //    {
    //        //Implement your custom user permission logic here
    //    }
    //}
}
