using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

public class WhenMappingIndexRequestToViewModel
{
    [Test, MoqAutoData]
    public async Task Then_Maps_AccountHashedId(
        IndexRequest request,
        IndexViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.AccountHashedId.Should().Be(request.AccountHashedId);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_EmployerAccountLegalEntityPublicHashedId(
        IndexRequest request,
        IndexViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.AccountLegalEntityHashedId.Should().Be(request.AccountLegalEntityHashedId);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_ReservationId(
        IndexRequest request,
        IndexViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.ReservationId.Should().Be(request.ReservationId);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_StartMonthYear(
        IndexRequest request,
        IndexViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.StartMonthYear.Should().Be(request.StartMonthYear);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_CourseCode(
        IndexRequest request,
        IndexViewModelMapper mapper)
    {
        var viewModel = await mapper.Map(request);

        viewModel.CourseCode.Should().Be(request.CourseCode);
    }
    
    [MoqInlineAutoData(ApprenticeshipEmployerType.Levy, true)]
    [MoqInlineAutoData(ApprenticeshipEmployerType.NonLevy, false)]
    public async Task Then_LevyStatus_Is_Mapped(
        ApprenticeshipEmployerType levyStatus,
        bool expectedIsLevy,
        [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient,
        AccountResponse accountResponse,
        IndexRequest indexRequest,
        IndexViewModelMapper mapper)
    {
        // Arrange
        accountResponse.LevyStatus = levyStatus;
        commitmentsApiClient
            .Setup(x => x.GetAccount(indexRequest.AccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountResponse);
        
        //Act
        var result = await mapper.Map(indexRequest);

        //Assert           
        result.IsLevyFunded.Should().Be(expectedIsLevy);
    }
}