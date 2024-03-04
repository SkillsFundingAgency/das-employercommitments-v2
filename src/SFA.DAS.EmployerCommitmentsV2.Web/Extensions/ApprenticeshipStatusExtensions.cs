using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

public static class ApprenticeshipStatusExtensions
{
    public static string GetDisplayClass(this ApprenticeshipStatus status)
    {
        switch (status)
        {
            case ApprenticeshipStatus.Stopped: return "govuk-tag--red";
            case ApprenticeshipStatus.WaitingToStart: return "govuk-tag--yellow";
            case ApprenticeshipStatus.Paused: return "govuk-tag--grey";
            case ApprenticeshipStatus.Live: return "govuk-tag--blue";
            case ApprenticeshipStatus.Completed: return "govuk-tag--green";
            default: return string.Empty;
        }
    }
}