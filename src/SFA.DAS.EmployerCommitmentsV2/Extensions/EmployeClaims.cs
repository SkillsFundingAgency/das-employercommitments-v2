namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class EmployeClaims
    {
        public const string Subject = "sub"; // same value as Upn (i.e. user id)
        public const string IdentifyingParty = "idp";
        public const string Upn = "http://das/employer/identity/claims/id";
        public const string EmailAddress = "http://das/employer/identity/claims/email_address";
        public const string GivenName = "http://das/employer/identity/claims/given_name";
        public const string FamilyName = "http://das/employer/identity/claims/family_name";
        public const string DisplayName = "http://das/employer/identity/claims/display_name";
        public const string RequiresVerification = "http://das/employer/identity/claims/requires_verification";
    }
}