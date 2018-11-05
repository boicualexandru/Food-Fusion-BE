using System.Security.Claims;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Services.Authorization
{
    public interface IResourceAuthorizationService<T> where T : OperationAuthorizationRequirement, new()
    {
        ResourceAuthorizationService<T> WithUser(ClaimsPrincipal user);
        ResourceAuthorizationService<T> WithRequirement(T requirement);
        ResourceAuthorizationService<T> WithResource(object resource);
        bool IsAuthorized();
    }
}
