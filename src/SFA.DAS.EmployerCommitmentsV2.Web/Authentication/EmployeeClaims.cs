namespace SFA.DAS.EmployerCommitmentsV2.Web.Authentication
{
    public static class EmployeeClaims
    {
        public static readonly string IdamsUserIdClaimTypeIdentifier = "http://das/employer/identity/claims/id";
        public static readonly string IdamsUserEmailClaimTypeIdentifier = "http://das/employer/identity/claims/email_address";
        public static readonly string IdamsUserDisplayNameClaimTypeIdentifier = "http://das/employer/identity/claims/display_name";
        public static readonly string AccountsClaimsTypeIdentifier = "http://das/employer/identity/claims/associatedAccounts";
    }
}
