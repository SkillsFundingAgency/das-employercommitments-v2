using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship
{
    [TestFixture]
    public class ViewDraftApprenticeshipViewModelMapperTests
    {
        private ViewDraftApprenticeshipViewModelMapper _mapper;
        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private TrainingProgrammeApiClientMock _trainingProgrammeApiClient;
        private ViewDraftApprenticeshipRequest _request;
        private ViewDraftApprenticeshipViewModel _result;
        private GetDraftApprenticeshipResponse _draftApprenticeship;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();
            _request = autoFixture.Create<ViewDraftApprenticeshipRequest>();

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();

            _trainingProgrammeApiClient = new TrainingProgrammeApiClientMock();

            _draftApprenticeship = autoFixture.Create<GetDraftApprenticeshipResponse>();
            _draftApprenticeship.CourseCode = _trainingProgrammeApiClient.All.First().Id;

            _commitmentsApiClient.Setup(x =>
                    x.GetDraftApprenticeship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_draftApprenticeship);

            _mapper = new ViewDraftApprenticeshipViewModelMapper(_commitmentsApiClient.Object, _trainingProgrammeApiClient.Object);

            _result = await _mapper.Map(_request) as ViewDraftApprenticeshipViewModel;
        }

        [Test]
        public void ThenAccountHashedIdIsMappedCorrectly()
        {
            Assert.AreEqual(_request.Request.AccountHashedId, _result.AccountHashedId);
        }

        [Test]
        public void ThenCohortReferenceIsMappedCorrectly()
        {
            Assert.AreEqual(_request.Request.CohortReference, _result.CohortReference);
        }

        [Test]
        public void ThenFirstNameIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.FirstName, _result.FirstName);
        }

        [Test]
        public void ThenLastNameIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.LastName, _result.LastName);
        }

        [Test]
        public void ThenUlnIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.Uln, _result.Uln);
        }

        [Test]
        public void DateOfBirthIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.DateOfBirth, _result.DateOfBirth);
        }

        [Test]
        public void ThenCostIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.Cost, _result.Cost);
        }

        [Test]
        public void ThenStartDateIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.StartDate, _result.StartDate);
        }

        [Test]
        public void ThenEndDateIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.EndDate, _result.EndDate);
        }

        [Test]
        public void ThenReferenceIsMappedCorrectly()
        {
            Assert.AreEqual(_draftApprenticeship.Reference, _result.Reference);
        }

        [Test]
        public void ThenLegalEntityNameIsMappedCorrectly()
        {
            Assert.AreEqual(_request.Cohort.LegalEntityName, _result.LegalEntityName);
        }

        [Test]
        public void ThenTrainingCourseIsMappedCorrectly()
        {
            Assert.AreEqual(_trainingProgrammeApiClient.All.First().ExtendedTitle, _result.TrainingCourse);
        }
    }
}
