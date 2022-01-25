namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared
{
    public class NotificationParameters
    {
        public bool ShowNotification { get; set; } = false;
        public string NotificationTitle { get; set; }
        public string NotificationBody { get; set; }
    }
}
