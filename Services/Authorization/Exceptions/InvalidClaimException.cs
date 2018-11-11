using Common;

namespace Services.Authorization.Exceptions
{
    public class InvalidClaimException : CustomWebException
    {
        public override string Message => "Invalid Claim.";
    }
}
