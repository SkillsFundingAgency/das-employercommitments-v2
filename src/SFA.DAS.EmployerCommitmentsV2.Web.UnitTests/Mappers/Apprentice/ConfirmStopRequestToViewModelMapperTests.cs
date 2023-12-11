using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ConfirmStopRequestToViewModelMapperTests
{
    private const string ExpectedFullName = "FirstName LastName";
    private const string ExpectedCourseName = "Test Apprenticeship";
    private const string ExpectedUln = "1234567890";
    private DateTime ExpectedStartDateTime = DateTime.Now.AddYears(-2);

    private Mock<ICommitmentsApiClient> mockCommitmentsApiClient;
    private GetApprenticeshipResponse ApprenticeshipDetails;

    [SetUp]
    public void SetUp()
    {
        mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

        ApprenticeshipDetails = GetApprenticeshipResponse();

        mockCommitmentsApiClient
            .Setup(r => r.GetApprenticeship(It.IsAny<long>(), CancellationToken.None))
            .ReturnsAsync(() => ApprenticeshipDetails);
    }

    [Test, MoqAutoData]
    public async Task ApprenticeshipHashedId_IsMapped(ConfirmStopRequest request)
    {
        var mapper = new ConfirmStopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(request.ApprenticeshipHashedId));
    }

    [Test, MoqAutoData]
    public async Task AccountHashedId_IsMapped(ConfirmStopRequest request)
    {
        var mapper = new ConfirmStopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.AccountHashedId, Is.EqualTo(request.AccountHashedId));
    }

    [Test, MoqAutoData]
    public async Task MadeRedundant_IsMapped(ConfirmStopRequest request)
    {
        var mapper = new ConfirmStopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.MadeRedundant, Is.EqualTo(request.MadeRedundant));
    }

    [Test, MoqAutoData]
    public async Task ApprenticeName_IsMapped(ConfirmStopRequest request)
    {
        var mapper = new ConfirmStopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.ApprenticeName, Is.EqualTo(ExpectedFullName));
    }

    [Test, MoqAutoData]
    public async Task CourseName_IsMapped(ConfirmStopRequest request)
    {
        var mapper = new ConfirmStopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.Course, Is.EqualTo(ExpectedCourseName));
    }

    [Test, MoqAutoData]
    public async Task ULN_IsMapped(ConfirmStopRequest request)
    {
        var mapper = new ConfirmStopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.ULN, Is.EqualTo(ExpectedUln));
    }

    [Test, MoqAutoData]
    public async Task WhenApprenticeship_Status_IsWaitingToStart_StopDate_IsMapped(ConfirmStopRequest request)
    {
        ApprenticeshipDetails.Status = CommitmentsV2.Types.ApprenticeshipStatus.WaitingToStart;
        var mapper = new ConfirmStopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.StopDate, Is.EqualTo(ExpectedStartDateTime));
    }

    [Test, MoqAutoData]
    public async Task WhenApprenticeship_Status_IsLive_StopDate_IsMapped(ConfirmStopRequest request)
    {
        request.StopMonth = 6;
        request.StopYear = 2020;
        ApprenticeshipDetails.Status = CommitmentsV2.Types.ApprenticeshipStatus.Live;

        var mapper = new ConfirmStopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.StopDate.Year, Is.EqualTo(2020));
        Assert.That(result.StopDate.Month, Is.EqualTo(6));
    }

    private GetApprenticeshipResponse GetApprenticeshipResponse()
    {
        return new GetApprenticeshipResponse
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Uln = ExpectedUln,
            CourseName = ExpectedCourseName,
            StartDate = ExpectedStartDateTime
        };
    }
}