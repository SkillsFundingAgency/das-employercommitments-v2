namespace SFA.DAS.EmployerCommitmentsV2.Infrastructure;

public static class EmployerClaims
{
    public const string Subject = "sub"; // same value as Upn (i.e. user id)
    public const string IdentifyingParty = "idp";
    public const string Upn = "http://das/employer/identity/claims/id";
    public const string EmailAddress = "http://das/employer/identity/claims/email_address";
    public const string GivenName = "http://das/employer/identity/claims/given_name";
    public const string FamilyName = "http://das/employer/identity/claims/family_name";
    public const string DisplayName = "http://das/employer/identity/claims/display_name";
    public const string RequiresVerification = "http://das/employer/identity/claims/requires_verification";
    public static string AccountsClaimsTypeIdentifier => "http://das/employer/identity/claims/associatedAccounts";
    public static string IdamsUserIdClaimTypeIdentifier => "http://das/employer/identity/claims/id";
}