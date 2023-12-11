using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship;

[TestFixture]
public class AddDraftApprenticeshipRequestToSelectCourseViewModelMapperTests
{
    private AddDraftApprenticeshipRequestToSelectCourseViewModelMapper _mapper;
    private AddDraftApprenticeshipRequest _source;
    private Mock<ICommitmentsApiClient> _commitmentsApiClient;
    private GetCohortResponse _getCohortResponse;
    private List<TrainingProgramme> _standardTrainingProgrammes;
    private List<TrainingProgramme> _allTrainingProgrammes;
    private SelectCourseViewModel _result;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();

        _standardTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
        _allTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
        _getCohortResponse = autoFixture.Build<GetCohortResponse>()
            .With(x => x.LevyStatus, ApprenticeshipEmployerType.Levy)
            .With(x => x.WithParty, Party.Employer)
            .Without(x => x.TransferSenderId)
            .Create();

        _source = autoFixture.Build<AddDraftApprenticeshipRequest>()
            .With(x => x.StartMonthYear, "062020")
            .With(x => x.CourseCode, "Course1")
            .With(x => x.DeliveryModel, DeliveryModel.PortableFlexiJob)
            .With(x => x.EmploymentEndDate, autoFixture.Create<System.DateTime?>())
            .Create();

        _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _commitmentsApiClient.Setup(x => x.GetCohort(_source.CohortId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getCohortResponse);
        _commitmentsApiClient
            .Setup(x => x.GetAllTrainingProgrammeStandards(It.IsAny<CancellationToken>()))
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

        _mapper = new AddDraftApprenticeshipRequestToSelectCourseViewModelMapper(_commitmentsApiClient.Object);

        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void AccountHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountHashedId, Is.EqualTo(_source.AccountHashedId));
    }

    [Test]
    public void AccountLegalEntityHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountLegalEntityHashedId, Is.EqualTo(_source.AccountLegalEntityHashedId));
    }

    [Test]
    public void AccountLegalEntityIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountLegalEntityId, Is.EqualTo(_source.AccountLegalEntityId));
    }

    [Test]
    public void CohortIdIsMappedCorrectly()
    {
        Assert.That(_result.CohortId, Is.EqualTo(_source.CohortId));
    }

    [Test]
    public void CohortReferenceIsMappedCorrectly()
    {
        Assert.That(_result.CohortReference, Is.EqualTo(_source.CohortReference));
    }

    [Test]
    public void CourseCodeIsMappedCorrectly()
    {
        Assert.That(_result.CourseCode, Is.EqualTo(_source.CourseCode));
    }

    [Test]
    public void CoursesAreMappedCorrectly()
    {
        Assert.That(_result.Courses, Is.EqualTo(_allTrainingProgrammes));
    }

    [Test]
    public void ProviderIdIsMappedCorrectly()
    {
        Assert.That(_result.ProviderId, Is.EqualTo(_source.ProviderId));
    }

    [Test]
    public void ReservationIdIsMappedCorrectly()
    {
        Assert.That(_result.ReservationId, Is.EqualTo(_source.ReservationId));
    }

    [Test]
    public void StartDateIsMappedCorrectly()
    {
        Assert.That(_result.StartMonthYear, Is.EqualTo(_source.StartMonthYear));
    }

    [Test]
    public void DeliveryIsMappedCorrectly()
    {
        Assert.That(_result.DeliveryModel, Is.EqualTo(_source.DeliveryModel));
    }
}