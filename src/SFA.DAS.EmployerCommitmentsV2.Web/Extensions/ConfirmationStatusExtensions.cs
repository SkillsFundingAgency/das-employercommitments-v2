using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Extensions
{
    public static class ConfirmationStatusExtensions
    {
        public static string ToDisplayString(this ConfirmationStatus? status)
        {
            switch (status)
            {
                case ConfirmationStatus.Confirmed: return "Confirmed";
                case ConfirmationStatus.Unconfirmed: return "Unconfirmed";
                case ConfirmationStatus.Overdue: return "Overdue";
                default: return "N/A";
            }
        }
    }
}