using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Services.Authorization
{
    public class ResourceAuthorizationService<T> : IResourceAuthorizationService<T> where T : OperationAuthorizationRequirement, new()
    {
        private readonly IAuthorizationService _authorizationService;

        private ClaimsPrincipal _user;
        private T _requirement;
        private object _resource;

        public ResourceAuthorizationService(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public ResourceAuthorizationService<T> WithUser(ClaimsPrincipal user)
        {
            _user = user;
            return this;
        }

        public ResourceAuthorizationService<T> WithRequirement(T requirement)
        {
            _requirement = requirement;
            return this;
        }

        public ResourceAuthorizationService<T> WithResource(object resource)
        {
            _resource = resource;
            return this;
        }

        public bool IsAuthorized()
        {
            var isAuthorized = _authorizationService.AuthorizeAsync(_user, _resource, _requirement)?
                .Result?.Succeeded ?? false;

            return isAuthorized;
        }
    }
}
