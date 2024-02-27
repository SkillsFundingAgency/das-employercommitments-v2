namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization;

public static class PolicyNames
{
    public static string HasActiveAccount => nameof(HasActiveAccount);
    public static string HasEmployerTransactorOwnerAccount => nameof(HasEmployerTransactorOwnerAccount);
    public static string AccessCohort => nameof(AccessCohort);
    public static string AccessApprenticeship => nameof(AccessApprenticeship);
    public static string AccessDraftApprenticeship => nameof(AccessDraftApprenticeship);
}