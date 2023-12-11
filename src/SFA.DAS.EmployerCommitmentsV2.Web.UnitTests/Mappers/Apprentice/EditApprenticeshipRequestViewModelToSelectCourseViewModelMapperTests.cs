using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

[TestFixture]
public class EditApprenticeshipRequestViewModelToSelectCourseViewModelMapperTests
{
    private EditApprenticeshipRequestViewModelToSelectCourseViewModelMapper _mapper;
    private EditApprenticeshipRequestViewModel _source;
    private Mock<ICommitmentsApiClient> _commitmentsApiClient;
    private GetCohortResponse _getCohortResponse;
    private GetApprenticeshipResponse _getApprenticeshipResponse;
    private List<TrainingProgramme> _standardTrainingProgrammes;
    private List<TrainingProgramme> _allTrainingProgrammes;
    private SelectCourseViewModel _result;
    private long _cohortId;
    private Fixture _autoFixture;

    [SetUp]
    public async Task Arrange()
    {
        _autoFixture = new Fixture();
        _cohortId = _autoFixture.Create<long>();

        _standardTrainingProgrammes = _autoFixture.CreateMany<TrainingProgramme>().ToList();
        _allTrainingProgrammes = _autoFixture.CreateMany<TrainingProgramme>().ToList();

        _getApprenticeshipResponse = _autoFixture.Build<GetApprenticeshipResponse>()
            .With(x => x.CohortId, _cohortId)
            .Create();

        _getCohortResponse = _autoFixture.Build<GetCohortResponse>()
            .With(x => x.LevyStatus, ApprenticeshipEmployerType.Levy)
            .With(x => x.WithParty, Party.Employer)
            .Without(x => x.TransferSenderId)
            .Create();

        _source = _autoFixture.Build<EditApprenticeshipRequestViewModel>()
            .With(x=>x.DateOfBirth, new DateModel())
            .With(x=>x.StartDate, new MonthYearModel(""))
            .With(x=>x.EndDate, new MonthYearModel(""))
            .With(x=>x.EmploymentEndDate, new MonthYearModel(""))
            .With(x => x.CourseCode, "Course1")
            .With(x => x.DeliveryModel, DeliveryModel.PortableFlexiJob)
            .Create();

        _commitmentsApiClient = new Mock<ICommitmentsApiClient>();

        _commitmentsApiClient.Setup(x => x.GetApprenticeship(_source.ApprenticeshipId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getApprenticeshipResponse);
        _commitmentsApiClient.Setup(x => x.GetCohort(_getApprenticeshipResponse.CohortId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getCohortResponse);
        _commitmentsApiClient.Setup(x => x.GetAllTrainingProgrammeStandards(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAllTrainingProgrammeStandardsResponse()
            {
                TrainingProgrammes = _standardTrainingProgrammes
            });
        _commitmentsApiClient
            .Setup(x => x.GetAllTrainingProgrammes(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAllTrainingProgrammesResponse
            {
                TrainingProgrammes = _allTrainingProgrammes
            });

        _mapper = new EditApprenticeshipRequestViewModelToSelectCourseViewModelMapper(_commitmentsApiClient.Object);

        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void CourseCodeIsMappedCorrectly()
    {
        Assert.That(_result.CourseCode, Is.EqualTo(_source.CourseCode));
    }

    [Test]
    public void CoursesAreMappedCorrectlyToAllTrainingCourses()
    {
        Assert.That(_result.Courses, Is.EqualTo(_allTrainingProgrammes));
    }

    [Test]
    public async Task CoursesAreMappedCorrectlyToAllStandardTrainingCoursesWhenFundedByTransferSender()
    {
        _getCohortResponse.TransferSenderId = _autoFixture.Create<long>();

        _result = await _mapper.Map(TestHelper.Clone(_source));

        Assert.That(_result.Courses, Is.EqualTo(_standardTrainingProgrammes));
    }

    [Test]
    public async Task CoursesAreMappedCorrectlyToAllStandardTrainingCoursesWhenEmployerIsNonLevy()
    {
        _getCohortResponse.LevyStatus = ApprenticeshipEmployerType.NonLevy;

        _result = await _mapper.Map(TestHelper.Clone(_source));

        Assert.That(_result.Courses, Is.EqualTo(_standardTrainingProgrammes));
    }
}