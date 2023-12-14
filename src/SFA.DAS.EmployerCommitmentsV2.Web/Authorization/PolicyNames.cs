namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization;

public static class PolicyNames
{
    public static string HasEmployerViewerTransactorOwnerAccount => nameof(HasEmployerViewerTransactorOwnerAccount);
    public static string AccessCohort => nameof(AccessCohort);
    public static string AccessApprenticeship => nameof(AccessApprenticeship);
}