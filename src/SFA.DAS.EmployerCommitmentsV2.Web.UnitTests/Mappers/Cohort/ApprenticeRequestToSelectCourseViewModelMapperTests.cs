using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class ApprenticeRequestToSelectCourseViewModelMapperTests
    {
        private ApprenticeRequestToSelectCourseViewModelMapper _mapper;
        private ApprenticeRequest _source;
        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private AccountLegalEntityResponse _accountLegalEntityResponse;
        private List<TrainingProgramme> _standardTrainingProgrammes;
        private List<TrainingProgramme> _allTrainingProgrammes;
        private SelectCourseViewModel _result;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _standardTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
            _allTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
            _accountLegalEntityResponse = autoFixture.Build<AccountLegalEntityResponse>().With(x => x.LevyStatus, ApprenticeshipEmployerType.Levy).Create();

            _source = autoFixture.Build<ApprenticeRequest>()
                .With(x => x.StartMonthYear, "062020")
                .With(x => x.AccountId, 12345)
                .With(x => x.CourseCode, "Course1")
                .With(x => x.DeliveryModel, DeliveryModel.PortableFlexiJob)
                .Without(x => x.TransferSenderId).Create();

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _commitmentsApiClient.Setup(x => x.GetAccountLegalEntity(_source.AccountLegalEntityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_accountLegalEntityResponse);
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

            _mapper = new ApprenticeRequestToSelectCourseViewModelMapper(_commitmentsApiClient.Object);

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

        [Test]
        public void TransferSenderIdIsMappedCorrectly()
        {
            Assert.That(_result.TransferSenderId, Is.EqualTo(_source.TransferSenderId));
        }
    }
}
