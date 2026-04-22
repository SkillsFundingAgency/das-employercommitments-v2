using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ConfirmStopRequestToViewModelMapperTests
{
    [Test, MoqAutoData]
    public async Task ApprenticeshipHashedId_IsMapped(ConfirmStopRequest request,
        [Greedy] ConfirmStopRequestToViewModelMapper mapper)
    {
        var result = await mapper.Map(request);
        result.ApprenticeshipHashedId.Should().Be(request.ApprenticeshipHashedId);
    }

    [Test, MoqAutoData]
    public async Task AccountHashedId_IsMapped(ConfirmStopRequest request,
        [Greedy] ConfirmStopRequestToViewModelMapper mapper)
    {
        var result = await mapper.Map(request);
        result.AccountHashedId.Should().Be(request.AccountHashedId);
    }

    [Test, MoqAutoData]
    public async Task MadeRedundant_IsMapped(ConfirmStopRequest request,
        [Greedy] ConfirmStopRequestToViewModelMapper mapper)
    {
        var result = await mapper.Map(request);
        result.MadeRedundant.Should().Be(request.MadeRedundant);
    }

    [Test, MoqAutoData]
    public async Task ApprenticeName_IsMapped(ConfirmStopRequest request,
        long accountId,
        GetManageApprenticeshipDetailsResponse response,
        [Frozen] Mock<IApprovalsApiClient> approvalsApiClient,
        [Frozen] Mock<IEncodingService> encodingService,
        [Greedy] ConfirmStopRequestToViewModelMapper mapper)
    {
        Setup(request, accountId, response, approvalsApiClient, encodingService);

        var result = await mapper.Map(request);

        var expectedname = $"{response.Apprenticeship.FirstName} {response.Apprenticeship.LastName}";
        result.ApprenticeName.Should().Be(expectedname);
    }

    [Test, MoqAutoData]
    public async Task CourseName_IsMapped(ConfirmStopRequest request,
        long accountId,
        GetManageApprenticeshipDetailsResponse response,
        [Frozen] Mock<IApprovalsApiClient> approvalsApiClient,
        [Frozen] Mock<IEncodingService> encodingService,
        [Greedy] ConfirmStopRequestToViewModelMapper mapper)
    {
        Setup(request, accountId, response, approvalsApiClient, encodingService);
        var result = await mapper.Map(request);
        result.Course.Should().Be(response.Apprenticeship.CourseName);
    }

    [Test, MoqAutoData]
    public async Task ULN_IsMapped(ConfirmStopRequest request,
        long accountId,
        GetManageApprenticeshipDetailsResponse response,
        [Frozen] Mock<IApprovalsApiClient> approvalsApiClient,
        [Frozen] Mock<IEncodingService> encodingService,
        [Greedy] ConfirmStopRequestToViewModelMapper mapper)
    {
        Setup(request, accountId, response, approvalsApiClient, encodingService);
        var result = await mapper.Map(request);
        result.ULN.Should().Be(response.Apprenticeship.Uln);
    }

    [Test, MoqAutoData]
    public async Task WhenApprenticeship_Status_IsWaitingToStart_StopDate_IsMapped(ConfirmStopRequest request,
        long accountId,
        GetManageApprenticeshipDetailsResponse response,
        [Frozen] Mock<IApprovalsApiClient> approvalsApiClient,
        [Frozen] Mock<IEncodingService> encodingService,
        [Greedy] ConfirmStopRequestToViewModelMapper mapper)
    {
        Setup(request, accountId, response, approvalsApiClient, encodingService);
        response.Apprenticeship.Status = CommitmentsV2.Types.ApprenticeshipStatus.WaitingToStart;
        var result = await mapper.Map(request);
        result.StopDate.Should().Be(response.Apprenticeship.StartDate);
    }

    [Test, MoqAutoData]
    public async Task WhenApprenticeship_Status_IsLive_StopDate_IsMapped(ConfirmStopRequest request,
        long accountId,
        GetManageApprenticeshipDetailsResponse response,
        [Frozen] Mock<IApprovalsApiClient> approvalsApiClient,
        [Frozen] Mock<IEncodingService> encodingService,
        [Greedy] ConfirmStopRequestToViewModelMapper mapper)
    {
        Setup(request, accountId, response, approvalsApiClient, encodingService);
        response.Apprenticeship.Status = CommitmentsV2.Types.ApprenticeshipStatus.Live;
        var result = await mapper.Map(request);

        result.StopDate.Year.Should().Be(2020);
        result.StopDate.Month.Should().Be(6);
    }

    private static void SetStartDate(ConfirmStopRequest request)
    {
        request.StopMonth = 6;
        request.StopYear = 2020;
    }

    private static void Setup(ConfirmStopRequest request, long accountId, GetManageApprenticeshipDetailsResponse response, Mock<IApprovalsApiClient> approvalsApiClient, Mock<IEncodingService> encodingService)
    {
        SetStartDate(request);

        encodingService.Setup(x => x.Decode(request.AccountHashedId, EncodingType.AccountId)).Returns(accountId);

        approvalsApiClient.Setup(x => x.GetManageApprenticeshipDetails(accountId, request.ApprenticeshipId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(response);
    }
}