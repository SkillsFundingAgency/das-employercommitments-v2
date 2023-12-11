using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using DeliveryModel = SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship;

[TestFixture]
public class ViewDraftApprenticeshipViewModelMapperTests
{
    private ViewDraftApprenticeshipViewModelMapper _mapper;
    private Mock<ICommitmentsApiClient> _commitmentsApiClient;
    private Mock<IApprovalsApiClient> _approvalsApiClient;
    private ViewDraftApprenticeshipRequest _request;
    private ViewDraftApprenticeshipViewModel _result;
    private GetViewDraftApprenticeshipResponse _draftApprenticeship;
    private TrainingProgramme _course;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();
        _request = autoFixture.Create<ViewDraftApprenticeshipRequest>();

        _draftApprenticeship = autoFixture.Create<GetViewDraftApprenticeshipResponse>();
        _course = autoFixture.Create<TrainingProgramme>();
            
        _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _commitmentsApiClient
            .Setup(x => x.GetTrainingProgramme(_draftApprenticeship.CourseCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetTrainingProgrammeResponse
            {
                TrainingProgramme = _course
            });

        _approvalsApiClient = new Mock<IApprovalsApiClient>();

        _approvalsApiClient.Setup(x =>
                x.GetViewDraftApprenticeship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_draftApprenticeship);

        _mapper = new ViewDraftApprenticeshipViewModelMapper(_commitmentsApiClient.Object, _approvalsApiClient.Object);

        _result = await _mapper.Map(_request) as ViewDraftApprenticeshipViewModel;
    }

    [Test]
    public void ThenAccountHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountHashedId, Is.EqualTo(_request.Request.AccountHashedId));
    }

    [Test]
    public void ThenCohortReferenceIsMappedCorrectly()
    {
        Assert.That(_result.CohortReference, Is.EqualTo(_request.Request.CohortReference));
    }

    [Test]
    public void ThenFirstNameIsMappedCorrectly()
    {
        Assert.That(_result.FirstName, Is.EqualTo(_draftApprenticeship.FirstName));
    }

    [Test]
    public void ThenLastNameIsMappedCorrectly()
    {
        Assert.That(_result.LastName, Is.EqualTo(_draftApprenticeship.LastName));
    }

    [Test]
    public void ThenEmailIsMappedCorrectly()
    {
        Assert.That(_result.Email, Is.EqualTo(_draftApprenticeship.Email));
    }

    [Test]
    public void ThenUlnIsMappedCorrectly()
    {
        Assert.That(_result.Uln, Is.EqualTo(_draftApprenticeship.Uln));
    }

    [Test]
    public void DateOfBirthIsMappedCorrectly()
    {
        Assert.That(_result.DateOfBirth, Is.EqualTo(_draftApprenticeship.DateOfBirth));
    }

    [Test]
    public void ThenCostIsMappedCorrectly()
    {
        Assert.That(_result.Cost, Is.EqualTo(_draftApprenticeship.Cost));
    }

    [Test]
    public void ThenEmploymentPriceIsMappedCorrectly()
    {
        Assert.That(_result.EmploymentPrice, Is.EqualTo(_draftApprenticeship.EmploymentPrice));
    }

    [Test]
    public void ThenStartDateIsMappedCorrectly()
    {
        Assert.That(_result.StartDate, Is.EqualTo(_draftApprenticeship.StartDate));
    }

    [Test]
    public void ThenEndDateIsMappedCorrectly()
    {
        Assert.That(_result.EndDate, Is.EqualTo(_draftApprenticeship.EndDate));
    }

    [Test]
    public void ThenEmploymentEndDateIsMappedCorrectly()
    {
        Assert.That(_result.EmploymentEndDate, Is.EqualTo(_draftApprenticeship.EmploymentEndDate));
    }

    [Test]
    public void ThenReferenceIsMappedCorrectly()
    {
        Assert.That(_result.Reference, Is.EqualTo(_draftApprenticeship.Reference));
    }

    [Test]
    public void ThenLegalEntityNameIsMappedCorrectly()
    {
        Assert.That(_result.LegalEntityName, Is.EqualTo(_request.Cohort.LegalEntityName));
    }

    [TestCase(DeliveryModel.Regular, DeliveryModel.Regular)]
    [TestCase(DeliveryModel.PortableFlexiJob, DeliveryModel.PortableFlexiJob)]
    public async Task ThenDeliveryModelIsMappedCorrectly(DeliveryModel delivery, DeliveryModel display)
    {
        _draftApprenticeship.DeliveryModel = delivery;
        _result = await _mapper.Map(_request) as ViewDraftApprenticeshipViewModel;
        Assert.That(_result.DeliveryModel, Is.EqualTo(display));
    }

    [Test]
    public void ThenTrainingCourseIsMappedCorrectly()
    {
        Assert.That(_result.TrainingCourse, Is.EqualTo(_course.Name));
    }

    [Test]
    public void ThenVersionIsMappedCorrectly()
    {
        Assert.That(_result.Version, Is.EqualTo(_draftApprenticeship.TrainingCourseVersion));
    }

    [Test]
    public void ThenHasStandardOptionsIsMappedCorrectly()
    {
        Assert.That(_result.HasStandardOptions, Is.EqualTo(_draftApprenticeship.HasStandardOptions));
    }

    [Test]
    public async Task And_TrainingCourseOptionIsNull_Then_CourseOptionIsMappedToEmptyString()
    {
        _draftApprenticeship.TrainingCourseOption = null;

        _result = await _mapper.Map(_request) as ViewDraftApprenticeshipViewModel;

        Assert.That(_result.CourseOption, Is.EqualTo(string.Empty));
    }

    [Test]
    public async Task And_TrainingCourseOptionIsEmptyString_Then_CourseOptionIsMappedToToBeConfirmed()
    {
        _draftApprenticeship.TrainingCourseOption = "";

        _result = await _mapper.Map(_request) as ViewDraftApprenticeshipViewModel;

        Assert.That(_result.CourseOption, Is.EqualTo("To be confirmed"));
    }

    [Test]
    public void Then_CourseOptionIsMappedCorrectly()
    {
        Assert.That(_result.CourseOption, Is.EqualTo(_draftApprenticeship.TrainingCourseOption));
    }

    [Test]
    public void Then_RecognisePriorLearningIsMappedCorrectly()
    {
        Assert.That(_result.RecognisePriorLearning, Is.EqualTo(_draftApprenticeship.RecognisePriorLearning));
    }

    [Test]
    public void Then_DurationReducedByIsMappedCorrectly()
    {
        Assert.That(_result.DurationReducedBy, Is.EqualTo(_draftApprenticeship.DurationReducedBy));
    }

    [Test]
    public void Then_PriceReducedByIsMappedCorrectly()
    {
        Assert.That(_result.PriceReducedBy, Is.EqualTo(_draftApprenticeship.PriceReducedBy));
    }

    [Test]
    public void Then_IsOnFlexiPaymentPilotIsMappedCorrectly()
    {
        Assert.That(_result.IsOnFlexiPaymentPilot, Is.EqualTo(_draftApprenticeship.IsOnFlexiPaymentPilot));
    }

    [Test]
    public void Then_ActualStartDateByIsMappedCorrectly()
    {
        Assert.That(_result.ActualStartDate, Is.EqualTo(_draftApprenticeship.ActualStartDate));
    }

    [Test]
    public void Then_TrainingTotalHoursIsMappedCorrectly()
    {
        Assert.That(_result.TrainingTotalHours, Is.EqualTo(_draftApprenticeship.TrainingTotalHours));
    }

    [Test]
    public void Then_DurationReducedByHoursIsMappedCorrectly()
    {
        Assert.That(_result.DurationReducedByHours, Is.EqualTo(_draftApprenticeship.DurationReducedByHours));
    }
}