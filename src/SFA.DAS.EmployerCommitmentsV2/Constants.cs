namespace SFA.DAS.EmployerCommitmentsV2;

public static class Constants
{
    public static class ApprenticesSearch
    {
        public const int NumberOfApprenticesPerSearchPage = 100;
        public const int NumberOfApprenticesRequiredForSearch = 10;
        public const int NumberOfApprenticesPerDownloadPage = 200;
    }
    
    public static class WebConstants
    {
        public const int MaxNumberOfEmployerAccountsAllowedOnClaim = 50;
    }

    public static class ApprenticeshipConstants
    {
        public const string AlreadyApprovedOrDeclinedMessage = "You cannot approve or decline this change because it has already been approved or declined.";
        public const string ApprovalRequestStatus = "ApprovalRequestStatus";
    }
}