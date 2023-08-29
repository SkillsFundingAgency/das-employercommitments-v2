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
using SFA.DAS.Encoding;


namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship
{
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
        private Mock<IEncodingService> _encodingService;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _encodingService.Setup(x => x.Decode(It.IsAny<string>(), It.IsAny<EncodingType>()))
                .Returns(123);

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
            _commitmentsApiClient.Setup(x => x.GetCohort(123, It.IsAny<CancellationToken>()))
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

            _mapper = new AddDraftApprenticeshipRequestToSelectCourseViewModelMapper(_commitmentsApiClient.Object, _encodingService.Object);

            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void AccountHashedIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AccountHashedId, _result.AccountHashedId);
        }

        [Test]
        public void AccountLegalEntityHashedIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AccountLegalEntityHashedId, _result.AccountLegalEntityHashedId);
        }

        [Test]
        public void CohortReferenceIsMappedCorrectly()
        {
            Assert.AreEqual(_source.CohortReference, _result.CohortReference);
        }

        [Test]
        public void CourseCodeIsMappedCorrectly()
        {
            Assert.AreEqual(_source.CourseCode, _result.CourseCode);
        }

        [Test]
        public void CoursesAreMappedCorrectly()
        {
            Assert.AreEqual(_allTrainingProgrammes, _result.Courses);
        }

        [Test]
        public void ProviderIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ProviderId, _result.ProviderId);
        }

        [Test]
        public void ReservationIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ReservationId, _result.ReservationId);
        }

        [Test]
        public void StartDateIsMappedCorrectly()
        {
            Assert.AreEqual(_source.StartMonthYear, _result.StartMonthYear);
        }

        [Test]
        public void DeliveryIsMappedCorrectly()
        {
            Assert.AreEqual(_source.DeliveryModel, _result.DeliveryModel);
        }
    }
}
