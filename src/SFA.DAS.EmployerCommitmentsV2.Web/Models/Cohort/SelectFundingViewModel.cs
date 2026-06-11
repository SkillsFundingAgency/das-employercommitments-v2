
using System.ComponentModel;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

public class SelectFundingViewModel
{
    public string AccountHashedId { get; set; }
    public bool IsLevyAccount { get; set; }
    public bool HasDirectTransfersAvailable { get; set; }
    public bool HasLtmTransfersAvailable { get; set; }
    public bool HasUnallocatedReservationsAvailable { get; set; }
    public bool HasAdditionalReservationFundsAvailable { get; set; }
    public FundingType? FundingType { get; set; }
    public Guid? ApprenticeshipSessionKey { get; set; }

}


public enum FundingType : short
{
    [Description("Transfer funds from a connection")]
    DirectTransfers = 1,
    [Description("Reserved funds")]
    UnallocatedReservations = 2,
    [Description("New reserved funds")]
    AdditionalReservations = 3,
    [Description("Current levy funds")]
    CurrentLevy = 4,
    [Description("Levy transfer funds")]
    LtmTransfers = 5
}
