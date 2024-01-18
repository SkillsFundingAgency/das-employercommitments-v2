using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ApprenticeshipNeverStartedViewModelMapperTests
{
    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

    private readonly DateTime _referenceDate = DateTime.UtcNow;

    private readonly long _apprenticeshipId;

    [SetUp]
    public void SetUp()
    {
        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

        _mockCommitmentsApiClient
            .Setup(r => r.GetApprenticeship(_apprenticeshipId, CancellationToken.None))
            .ReturnsAsync(GetApprenticeshipResponse(_referenceDate));
    }

    [Test, MoqAutoData]
    public async Task ApprenticeshipHashedId_IsMapped(ApprenticeshipNeverStartedRequest request)
    {
        request.ApprenticeshipId = _apprenticeshipId;
        var mapper = new ApprenticeshipNeverStartedViewModelMapper(_mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(request.ApprenticeshipHashedId));
    }

    [Test, MoqAutoData]
    public async Task AccountHashedId_IsMapped(ApprenticeshipNeverStartedRequest request)
    {
        request.ApprenticeshipId = _apprenticeshipId;
        var mapper = new ApprenticeshipNeverStartedViewModelMapper(_mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.AccountHashedId, Is.EqualTo(request.AccountHashedId));
    }

    [Test, MoqAutoData]
    public async Task ApprenticeshipId_IsMapped(ApprenticeshipNeverStartedRequest request)
    {
        request.ApprenticeshipId = _apprenticeshipId;
        var mapper = new ApprenticeshipNeverStartedViewModelMapper(_mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.ApprenticeshipId, Is.EqualTo(_apprenticeshipId));
    }

    [Test, MoqAutoData]
    public async Task StartDate_IsMapped(ApprenticeshipNeverStartedRequest request)
    {
        request.ApprenticeshipId = _apprenticeshipId;
        var mapper = new ApprenticeshipNeverStartedViewModelMapper(_mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(_referenceDate, Is.EqualTo(result.PlannedStartDate));
    }

    [Test, MoqAutoData]
    public async Task IsCopJourney_IsAlwaysFalse(ApprenticeshipNeverStartedRequest request)
    {
        request.ApprenticeshipId = _apprenticeshipId;
        var mapper = new ApprenticeshipNeverStartedViewModelMapper(_mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.IsCoPJourney, Is.EqualTo(false));
    }

    [Test, MoqAutoData]
    public async Task StopMonth_IsSameAsPlannedDtartDateMonth(ApprenticeshipNeverStartedRequest request)
    {
        request.ApprenticeshipId = _apprenticeshipId;
        var mapper = new ApprenticeshipNeverStartedViewModelMapper(_mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(_referenceDate.Month, Is.EqualTo(result.StopMonth));
    }

    [Test, MoqAutoData]
    public async Task StopYear_IsSameAsPlannedDtartDateYear(ApprenticeshipNeverStartedRequest request)
    {
        request.ApprenticeshipId = _apprenticeshipId;
        var mapper = new ApprenticeshipNeverStartedViewModelMapper(_mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(_referenceDate.Year, Is.EqualTo(result.StopYear));
    }
    private GetApprenticeshipResponse GetApprenticeshipResponse(DateTime referenceDate)
    {
        return new GetApprenticeshipResponse
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Uln = "1234567890",
            CourseName = "Test Apprenticeship",
            StartDate = referenceDate,
            Id = _apprenticeshipId
        };
    }
}