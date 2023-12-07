using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class SelectProviderViewModel
{

    public string AccountHashedId { get; set; }
    public Guid? ReservationId { get; set; }
    public string AccountLegalEntityHashedId { get; set; }
    public string LegalEntityName { get; set; }

    public string StartMonthYear { get; set; }
    public string CourseCode { get; set; }
    public string ProviderId { get; set; }
    public string TransferSenderId { get; set; }
    public Origin Origin { get; set; }
    public string EncodedPledgeApplicationId { get; set; }
}