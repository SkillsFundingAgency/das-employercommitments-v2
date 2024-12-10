namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;

public class GetSelectFundingOptionsResponse
{
    public bool IsLevyAccount { get; set; }
    public bool HasDirectTransfersAvailable { get; set; }
    public bool HasUnallocatedReservationsAvailable { get; set; }
    public bool HasAdditionalReservationFundsAvailable { get; set; }
}