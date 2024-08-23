namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses
{
    public class GetAccountReservationsStatusResponse
    {
        public bool CanAutoCreateReservations { get; set; }
        public bool HasReachedReservationsLimit { get; set; }
        public int UnallocatedPendingReservations { get; set; }
    }
}
