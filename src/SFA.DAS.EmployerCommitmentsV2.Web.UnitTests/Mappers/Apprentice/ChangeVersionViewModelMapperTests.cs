using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ChangeVersionViewModelMapperTests
{
    private Fixture _fixture;

    private ChangeVersionRequest _request;
    private GetApprenticeshipResponse _getApprenticeshipResponse;
    private GetTrainingProgrammeResponse _getCurrentVersionResponse;
    private GetNewerTrainingProgrammeVersionsResponse _getNewerTrainingProgrammeVersionsResponse;

    private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

    private ChangeVersionViewModelMapper _mapper;

    [SetUp]
    public void Arrange()
    {
        _fixture = new Fixture();

        _request = _fixture.Create<ChangeVersionRequest>();

        _getApprenticeshipResponse = _fixture.Build<GetApprenticeshipResponse>()
            .With(x => x.Version, "1.1")
            .With(x => x.StandardUId, "ST0001_1.1")
            .Create();

        _getCurrentVersionResponse = _fixture.Create<GetTrainingProgrammeResponse>();

        _getNewerTrainingProgrammeVersionsResponse = GetTrainingProgrammeVersions();

        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

        _mockCommitmentsApiClient.Setup(c => c.GetApprenticeship(_request.ApprenticeshipId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getApprenticeshipResponse);

        _mockCommitmentsApiClient.Setup(c => c.GetTrainingProgrammeVersionByStandardUId(_getApprenticeshipResponse.StandardUId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getCurrentVersionResponse);

        _mockCommitmentsApiClient.Setup(c => c.GetNewerTrainingProgrammeVersions(_getApprenticeshipResponse.StandardUId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getNewerTrainingProgrammeVersionsResponse);

        _mapper = new ChangeVersionViewModelMapper(_mockCommitmentsApiClient.Object);
    }

    [Test]
    public async Task Then_CurrentVersionIsMapped()
    {
        var viewModel = await _mapper.Map(_request);

        viewModel.CurrentVersion.Should().Be(_getApprenticeshipResponse.Version);
    }

    [Test]
    public async Task Then_CurrentVersionInfoIsMapped()
    {
        var viewModel = await _mapper.Map(_request);

        viewModel.StandardTitle.Should().Be(_getCurrentVersionResponse.TrainingProgramme.Name);
        viewModel.StandardUrl.Should().Be(_getCurrentVersionResponse.TrainingProgramme.StandardPageUrl);
    }

    [Test]
    public async Task Then_NewerVersionsAreMapped()
    {
        var viewModel = await _mapper.Map(_request);

        viewModel.NewerVersions.Count().Should().Be(1);
        viewModel.NewerVersions.Should().Contain("1.2");
    }

    private GetNewerTrainingProgrammeVersionsResponse GetTrainingProgrammeVersions()
    {
        var version = _fixture.Build<TrainingProgramme>()
            .With(x => x.Version, "1.2")
            .Create();
                
        return new GetNewerTrainingProgrammeVersionsResponse
        {
            NewerVersions = new List<TrainingProgramme> { version }
        };
    }
}