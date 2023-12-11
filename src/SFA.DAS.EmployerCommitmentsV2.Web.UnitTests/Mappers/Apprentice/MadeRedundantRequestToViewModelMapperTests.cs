using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class MadeRedundantRequestToViewModelMapperTests
{
    private const string ExpectedFullName = "FirstName LastName";

    private Mock<ICommitmentsApiClient> mockCommitmentsApiClient;

    [SetUp]
    public void SetUp()
    {
        mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

        mockCommitmentsApiClient
            .Setup(r => r.GetApprenticeship(It.IsAny<long>(), CancellationToken.None))
            .ReturnsAsync(GetApprenticeshipResponse());
    }
    [Test, MoqAutoData]
    public async Task ApprenticeshipHashedId_IsMapped(MadeRedundantRequest request)
    {
        var mapper = new MadeRedundantRequestToViewModelMapper(mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(request.ApprenticeshipHashedId));
    }

    [Test, MoqAutoData]
    public async Task AccountHashedId_IsMapped(MadeRedundantRequest request)
    {
        var mapper = new MadeRedundantRequestToViewModelMapper(mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.AccountHashedId, Is.EqualTo(request.AccountHashedId));
    }

    [Test, MoqAutoData]
    public async Task ApprenticeName_IsMapped(MadeRedundantRequest request)
    {
        var mapper = new MadeRedundantRequestToViewModelMapper(mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.ApprenticeName, Is.EqualTo(ExpectedFullName));
    }

    [Test, MoqAutoData]
    public async Task StopMonth_IsMapped(MadeRedundantRequest request)
    {
        var mapper = new MadeRedundantRequestToViewModelMapper(mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.StopMonth, Is.EqualTo(request.StopMonth));
    }

    [Test, MoqAutoData]
    public async Task StopYear_IsMapped(MadeRedundantRequest request)
    {
        var mapper = new MadeRedundantRequestToViewModelMapper(mockCommitmentsApiClient.Object);
        var result = await mapper.Map(request);

        Assert.That(result.StopYear, Is.EqualTo(request.StopYear));
    }

    private GetApprenticeshipResponse GetApprenticeshipResponse()
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