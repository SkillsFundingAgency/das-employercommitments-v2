using System;
using SFA.DAS.CommitmentsV2.Types;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class DownloadRequest
{
    public string AccountHashedId { get; set; }
    public string SearchTerm { get; set; }
    public string SelectedProvider { get; set; }
    public string SelectedCourse { get; set; }
    public ApprenticeshipStatus? SelectedStatus { get; set; }
    public Alerts? SelectedAlert { get; set; }
    public DateTime? SelectedEndDate { get; set; }
    public ConfirmationStatus? SelectedApprenticeConfirmation { get; set; }
}