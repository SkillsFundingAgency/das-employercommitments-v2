using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

public class WhenMappingAssignRequestToViewModel
{
    [Test, MoqAutoData]
    public async Task Then_Maps_AccountHashedId(
        AssignRequest request,
        AssignViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.AccountHashedId.Should().Be(request.AccountHashedId);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_EmployerAccountLegalEntityPublicHashedId(
        AssignRequest request,
        AssignViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.AccountLegalEntityHashedId.Should().Be(request.AccountLegalEntityHashedId);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_LegalEntityName(
        [Frozen] AssignRequest request,
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
        AssignRequest request,
        AssignViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.ReservationId.Should().Be(request.ReservationId);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_StartMonthYear(
        AssignRequest request,
        AssignViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.StartMonthYear.Should().Be(request.StartMonthYear);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_CourseCode(
        AssignRequest request,
        AssignViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.CourseCode.Should().Be(request.CourseCode);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_UkPrn(
        AssignRequest request,
        AssignViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.ProviderId.Should().Be(request.ProviderId);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_TransferSenderId(
        AssignRequest request,
        AssignViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.TransferSenderId.Should().Be(request.TransferSenderId);
    }


    [Test, MoqAutoData]
    public async Task Then_Maps_FundingType(
        AssignRequest request,
        AssignViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.FundingType.Should().Be(request.FundingType);
    }

}