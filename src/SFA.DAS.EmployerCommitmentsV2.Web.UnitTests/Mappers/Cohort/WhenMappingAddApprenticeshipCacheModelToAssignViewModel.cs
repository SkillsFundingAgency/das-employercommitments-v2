using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

public class WhenMappingAddApprenticeshipCacheModelToAssignViewModel
{
    [Test, MoqAutoData]
    public async Task Then_Maps_AccountHashedId(
        AddApprenticeshipCacheModel request,
        AssignViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.AccountHashedId.Should().Be(request.AccountHashedId);
    }


    [Test, MoqAutoData]
    public async Task Then_Maps_LegalEntityName(
        [Frozen] AddApprenticeshipCacheModel request,
        [Frozen] AccountLegalEntityResponse response,
        [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClientMock,
        AssignViewModelMapper mapper)
    {
        commitmentsApiClientMock
            .Setup(x => x.GetAccountLegalEntity(request.AccountLegalEntityId, default))
            .ReturnsAsync(response);

        var viewModel = await mapper.Map(request);

        viewModel.LegalEntityName.Should().Be(response.LegalEntityName);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_ReservationId(
        AddApprenticeshipCacheModel request,
        AssignViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.ReservationId.Should().Be(request.ReservationId);
    }


    [Test, MoqAutoData]
    public async Task Then_Maps_ApprenticeshipSessionKey(
        AddApprenticeshipCacheModel request,
        AssignViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.ApprenticeshipSessionKey.Should().Be(request.ApprenticeshipSessionKey);
    }


    [Test, MoqAutoData]
    public async Task Then_Maps_FundingType(
        AddApprenticeshipCacheModel request,
        AssignViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.FundingType.Should().Be(request.FundingType);
    }

}