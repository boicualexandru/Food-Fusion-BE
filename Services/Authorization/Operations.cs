using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Services.Authorization
{
    public static class Operations<T> where T : OperationAuthorizationRequirement, new()
    {
        public static T Create =
            new T { Name = nameof(Create) };
        public static T Read =
            new T { Name = nameof(Read) };
        public static T Update =
            new T { Name = nameof(Update) };
        public static T Delete =
            new T { Name = nameof(Delete) };
    }
}
