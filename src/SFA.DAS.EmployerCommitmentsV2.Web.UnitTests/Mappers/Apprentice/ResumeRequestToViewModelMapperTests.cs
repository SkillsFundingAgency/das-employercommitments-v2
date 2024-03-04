using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ResumeRequestToViewModelMapperTests
{
    private const string ExpectedFullName = "FirstName LastName";
    private const string ExpectedCourseName = "Test Apprenticeship";
    private const string ExpectedUln = "1234567890";

    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

    [SetUp]
    public void SetUp()
    {
        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

        _mockCommitmentsApiClient
            .Setup(r => r.GetApprenticeship(It.IsAny<long>(), CancellationToken.None))
            .ReturnsAsync(GetApprenticeshipResponse());
    }
    [Test, MoqAutoData]
    public async Task ApprenticeshipHashedId_IsMapped(ResumeRequest request)
    {
        var mapper = new ResumeRequestToViewModelMapper(_mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(request.ApprenticeshipHashedId));
    }

    [Test, MoqAutoData]
    public async Task AccountHashedId_IsMapped(ResumeRequest request)
    {
        var mapper = new ResumeRequestToViewModelMapper(_mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.AccountHashedId, Is.EqualTo(request.AccountHashedId));
    }

    [Test, MoqAutoData]
    public async Task ApprenticeName_IsMapped(ResumeRequest request)
    {
        var mapper = new ResumeRequestToViewModelMapper(_mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.ApprenticeName, Is.EqualTo(ExpectedFullName));
    }

    [Test, MoqAutoData]
    public async Task CourseName_IsMapped(ResumeRequest request)
    {
        var mapper = new ResumeRequestToViewModelMapper(_mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.Course, Is.EqualTo(ExpectedCourseName));
    }

    [Test, MoqAutoData]
    public async Task ULN_IsMapped(ResumeRequest request)
    {
        var mapper = new ResumeRequestToViewModelMapper(_mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.ULN, Is.EqualTo(ExpectedUln));
    }

    private static GetApprenticeshipResponse GetApprenticeshipResponse()
    {
        return new GetApprenticeshipResponse
        {
            FirstName = "FirstName",
            LastName = "LastName",
            Uln = "1234567890",
            CourseName = "Test Apprenticeship"
        };
    }
}