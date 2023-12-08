namespace SFA.DAS.EmployerCommitmentsV2.Web.Authorization;

public static class AuthorizationContextKey
{
    public const string CohortId = nameof(CohortId);
    public const string ApprenticeshipId = nameof(ApprenticeshipId);
    public const string PartyId = nameof(PartyId);
    public const string Party = nameof(Party);
    public const string AccountId = nameof(AccountId);
    public const string UserRef = nameof(UserRef);
    public const string AccountLegalEntityId = nameof(AccountLegalEntityId);
    public const string DraftApprenticeshipId = nameof(DraftApprenticeshipId);
    public const string DecodedTransferSenderId = nameof(DecodedTransferSenderId);
    public const string TransferRequestId = nameof(TransferRequestId);
    public const string PledgeApplicationId = nameof(PledgeApplicationId);
}