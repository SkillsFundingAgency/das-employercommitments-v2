
namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class SelectFundingViewModel
{

    public string AccountHashedId { get; set; }
    public Guid? ReservationId { get; set; }
    public string AccountLegalEntityHashedId { get; set; }

    public string StartMonthYear { get; set; }
    public string CourseCode { get; set; }
    public string ProviderId { get; set; }
    public string TransferSenderId { get; set; }
    public string EncodedPledgeApplicationId { get; set; }
    
    public bool IsLevyAccount { get; set; }
    public bool HasDirectTransfersAvailable { get; set; }
    public bool HasUnallocatedReservationsAvailable { get; set; }
    public bool HasAdditionalReservationFundsAvailable { get; set; }
    public FundingType? FundingType { get; set; }
}


public enum FundingType : short
{
    DirectTransfers = 1,
    UnallocatedReservations = 2,
    AdditionalReservations = 3,
    CurrentLevy = 4
}
