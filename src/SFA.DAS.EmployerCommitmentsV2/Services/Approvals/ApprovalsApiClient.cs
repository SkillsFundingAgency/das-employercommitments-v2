using System.Web;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using AddDraftApprenticeshipResponse = SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.AddDraftApprenticeshipResponse;
using CreateCohortResponse = SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses.CreateCohortResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Services.Approvals;

public class ApprovalsApiClient(IOuterApiClient client) : IApprovalsApiClient
{
    public async Task<GetPledgeApplicationResponse> GetPledgeApplication(int pledgeApplicationId, CancellationToken cancellationToken = default)
    {
        return await client.Get<GetPledgeApplicationResponse>($"PledgeApplications/{pledgeApplicationId}");
    }

    public async Task<ProviderCourseDeliveryModels> GetProviderCourseDeliveryModels(long providerId, string courseCode, long accountLegalEntityId = 0, CancellationToken cancellationToken = default)
    {
        return await client.Get<ProviderCourseDeliveryModels>($"Providers/{providerId}/courses?trainingCode={courseCode}&accountLegalEntityId={accountLegalEntityId}");
    }

    public async Task<GetEditDraftApprenticeshipResponse> GetEditDraftApprenticeship(long accountId, long cohortId,
        long draftApprenticeshipId, CancellationToken cancellationToken = default)
    {
        return await client.Get<GetEditDraftApprenticeshipResponse>($"employer/{accountId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit");
    }

    public async Task<GetViewDraftApprenticeshipResponse> GetViewDraftApprenticeship(long accountId, long cohortId, long draftApprenticeshipId, CancellationToken cancellationToken = default)
    {
        return await client.Get<GetViewDraftApprenticeshipResponse>($"employer/{accountId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/view");
    }

    public async Task<GetEditApprenticeshipResponse> GetEditApprenticeship(long accountId, long apprenticeshipId, CancellationToken cancellationToken = default)
    {
        return await client.Get<GetEditApprenticeshipResponse>($"employer/{accountId}/apprentices/{apprenticeshipId}/edit");
    }

    public async Task<GetEditApprenticeshipDeliveryModelResponse> GetEditApprenticeshipDeliveryModel(long accountId, long apprenticeshipId, CancellationToken cancellationToken = default)
    {
        return await client.Get<GetEditApprenticeshipDeliveryModelResponse>($"employer/{accountId}/apprentices/{apprenticeshipId}/edit/delivery-model");
    }

    public async Task<GetApprenticeshipDetailsResponse> GetApprenticeshipDetails(long providerId, long apprenticeshipId, CancellationToken cancellationToken = default)
    {
        return await client.Get<GetApprenticeshipDetailsResponse>($"provider/{providerId}/apprentices/{apprenticeshipId}/details");
    }

    public async Task<GetEditDraftApprenticeshipSelectDeliveryModelResponse> GetEditDraftApprenticeshipSelectDeliveryModel(long providerId, long cohortId, long draftApprenticeshipId, string courseCode, CancellationToken cancellationToken = default)
    {
        return await client.Get<GetEditDraftApprenticeshipSelectDeliveryModelResponse>($"provider/{providerId}/unapproved/{cohortId}/apprentices/{draftApprenticeshipId}/edit/select-delivery-model?courseCode={courseCode}");
    }

    public async Task<GetCohortDetailsResponse> GetCohortDetails(long accountId, long cohortId, CancellationToken cancellationToken = default)
    {
        return await client.Get<GetCohortDetailsResponse>($"employer/{accountId}/unapproved/{cohortId}");
    }

    public async Task<GetUserAccountsResponse> GetEmployerUserAccounts(string email, string userId)
    {
        return await client.Get<GetUserAccountsResponse>($"AccountUsers/{userId}/accounts?email={HttpUtility.UrlEncode(email)}");
    }

    public async Task PostCohortDetails(long accountId, long cohortId, PostCohortDetailsRequest request, CancellationToken cancellationToken = default)
    {
        await client.Post<PostCohortDetailsRequest>($"employer/{accountId}/unapproved/{cohortId}", request);
    }

    public async Task UpdateDraftApprenticeship(long cohortId, long draftApprenticeshipId, UpdateDraftApprenticeshipApimRequest request, CancellationToken cancellationToken = default)
    {
        await client.Put<object>($"cohorts/{cohortId}/draft-apprenticeships/{draftApprenticeshipId}", request);
    }

    public async Task<AddDraftApprenticeshipResponse> AddDraftApprenticeship(long cohortId, AddDraftApprenticeshipApimRequest request, CancellationToken cancellationToken = default)
    {
        return await client.Post<AddDraftApprenticeshipResponse>($"cohorts/{cohortId}/draft-apprenticeships", request);
    }

    public async Task<CreateCohortResponse> CreateCohort(CreateCohortApimRequest request, CancellationToken cancellationToken = default)
    {
        return await client.Post<CreateCohortResponse>($"cohorts", request);
    }

    public async Task<GetAddFirstDraftApprenticeshipResponse> GetAddFirstDraftApprenticeshipDetails(long accountId, long accountLegalEntityId, long providerId, string courseCode, DateTime? startDate = null, CancellationToken cancellationToken = default)
    {
        var dateAsString = startDate.HasValue ? startDate.Value.ToString("yyyy-MM-dd") : null;   
        return await client.Get<GetAddFirstDraftApprenticeshipResponse>($"employer/{accountId}/unapproved/add/apprenticeship?accountLegalEntityId={accountLegalEntityId}&providerId={providerId}&courseCode={courseCode}&StartDate={dateAsString}");
    }

    public Task<GetAddAnotherDraftApprenticeshipResponse> GetAddAnotherDraftApprenticeshipDetails(long accountId, long cohortId, string courseCode, DateTime? startDate = null,
        CancellationToken cancellationToken = default)
    {
        var dateAsString = startDate.HasValue ? startDate.Value.ToString("yyyy-MM-dd") : null;
        return client.Get<GetAddAnotherDraftApprenticeshipResponse>($"employer/{accountId}/unapproved/{cohortId}/apprentices/add/details?courseCode={courseCode}&StartDate={dateAsString}");
    }

    public async Task<GetManageApprenticeshipDetailsResponse> GetManageApprenticeshipDetails(long accountId, long apprenticeshipId, CancellationToken cancellationToken = default)
    {
        return await client.Get<GetManageApprenticeshipDetailsResponse>($"employer/{accountId}/apprenticeships/{apprenticeshipId}/details");
    }

    public async Task<GetLegalEntitiesForAccountResponse> GetLegalEntitiesForAccount(string cohortId, long accountId)
    {
        return await client.Get<GetLegalEntitiesForAccountResponse>($"{accountId}/cohorts/{cohortId}/unapproved/add/legal-entity");
    }

    public Task<GetAccountLegalEntityResponse> GetAccountLegalEntity(long accountLegalEntityId, CancellationToken cancellationToken = default)
    {
        return client.Get<GetAccountLegalEntityResponse>($"accountlegalentity/{accountLegalEntityId}");
    }

    public Task<GetSelectProviderDetailsResponse> GetSelectProviderDetails(long accountId, long accountLegalEntityId, CancellationToken cancellationToken = default)
    {
        return client.Get<GetSelectProviderDetailsResponse>($"{accountId}/unapproved/add/select-provider?accountLegalEntityId={accountLegalEntityId}");
    }
    public Task<GetSelectFundingOptionsResponse> GetSelectFundingOptions(long accountId, CancellationToken cancellationToken = default)
    {
        return client.Get<GetSelectFundingOptionsResponse>($"{accountId}/unapproved/add/select-funding");
    }

    public Task<GetSelectDirectTransferConnectionResponse> GetSelectDirectTransferConnection(long accountId, CancellationToken cancellationToken = default)
    {
        return client.Get<GetSelectDirectTransferConnectionResponse>($"{accountId}/unapproved/add/select-funding/select-direct-connection");
    }

    public Task<GetSelectLevyTransferConnectionResponse> GetSelectLevyTransferConnection(long accountId, CancellationToken cancellationToken = default)
    {
        return client.Get<GetSelectLevyTransferConnectionResponse>($"{accountId}/unapproved/add/select-funding/select-accepted-levy-connection");
    }

    public Task<GetFundingBandDataResponse> GetFundingBandDataByCourseCodeAndStartDate(string courseCode, DateTime? startDate, CancellationToken cancellationToken = default)
    {
        if (startDate == null)
        {
            return client.Get<GetFundingBandDataResponse>($"TrainingCourses/{courseCode}/funding-band");
        }

        return client.Get<GetFundingBandDataResponse>($"TrainingCourses/{courseCode}/funding-band?startDate={startDate.Value.ToString("yyyy-MM-dd")}");
    }

    public Task<GetAgreementNotSignedResponse> GetAgreementNotSigned(long accountId, CancellationToken cancellationToken = default)
    {
        return client.Get<GetAgreementNotSignedResponse>($"{accountId}/unapproved/AgreementNotSigned");
    }

    public async Task<ValidateEditApprenticeshipResponse> EditApprenticeship(long accountId, long apprenticeshipId, ValidateEditApprenticeshipRequest request, CancellationToken cancellationToken = default)
    {
        return await client.Put<ValidateEditApprenticeshipResponse>($"employer/{accountId}/apprentices/{apprenticeshipId}", request);
    }

    public async Task<ConfirmEditApprenticeshipResponse> ConfirmEditApprenticeship(long accountId, long apprenticeshipId, ConfirmEditApprenticeshipRequest request, CancellationToken cancellationToken = default)
    {
        return await client.Post<ConfirmEditApprenticeshipResponse>($"employer/{accountId}/apprentices/{apprenticeshipId}/edit/confirm", request);
    }
}