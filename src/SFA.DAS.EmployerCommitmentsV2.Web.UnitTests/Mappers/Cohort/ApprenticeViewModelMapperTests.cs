using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class ApprenticeViewModelMapperTests
    {
        private ApprenticeViewModelMapper _mapper;
        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private GetProviderResponse _providerResponse;
        private AccountLegalEntityResponse _accountLegalEntityResponse;
        private ApprenticeRequest _source;
        private ApprenticeViewModel _result;
        private TrainingProgramme _courseStandard;
        private TrainingProgramme _course;
        private List<TrainingProgramme> _allTrainingProgrammes;
        private List<TrainingProgramme> _standardTrainingProgrammes;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _course = autoFixture.Create<TrainingProgramme>();
            _courseStandard = autoFixture.Create<TrainingProgramme>();
            _providerResponse = autoFixture.Create<GetProviderResponse>();
            _accountLegalEntityResponse = autoFixture.Build<AccountLegalEntityResponse>().With(x=>x.LevyStatus, ApprenticeshipEmployerType.Levy).Create();
            _source = autoFixture.Create<ApprenticeRequest>();
            _source.StartMonthYear = "062020";
            _source.TransferSenderId = string.Empty;
            _source.AccountId = 12345;

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _commitmentsApiClient.Setup(x => x.GetProvider(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_providerResponse);
            _commitmentsApiClient.Setup(x => x.GetAccountLegalEntity(_source.AccountLegalEntityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_accountLegalEntityResponse);
            _standardTrainingProgrammes = new List<TrainingProgramme>{_courseStandard};
            _commitmentsApiClient
                .Setup(x => x.GetAllTrainingProgrammeStandards(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAllTrainingProgrammeStandardsResponse
                {
                    TrainingProgrammes = _standardTrainingProgrammes
                });
            _allTrainingProgrammes = new List<TrainingProgramme>{_courseStandard, _course};
            _commitmentsApiClient
                .Setup(x => x.GetAllTrainingProgrammes(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAllTrainingProgrammesResponse
                {
                    TrainingProgrammes = _allTrainingProgrammes
                });

            _mapper = new ApprenticeViewModelMapper(_commitmentsApiClient.Object);

            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void AccountLegalEntityIdIsMappedCorrectly()
        {
            Assert.That(_result.AccountLegalEntityId, Is.EqualTo(_source.AccountLegalEntityId));
        }

        [Test]
        public void AccountLegalEntityHashedIdIsMappedCorrectly()
        {
            Assert.That(_result.AccountLegalEntityHashedId, Is.EqualTo(_source.AccountLegalEntityHashedId));
        }

        [Test]
        public void AccountLegalEntityNameIsMappedCorrectly()
        {
            Assert.That(_result.LegalEntityName, Is.EqualTo(_accountLegalEntityResponse.LegalEntityName));
        }

        [Test]
        public void StartDateIsMappedCorrectly()
        {
            Assert.That(_result.StartDate.Date, Is.EqualTo(new MonthYearModel(_source.StartMonthYear).Date));
        }

        [Test]
        public void ReservationIdIsMappedCorrectly()
        {
            Assert.That(_result.ReservationId, Is.EqualTo(_source.ReservationId));
        }

        [Test]
        public void CourseCodeIsMappedCorrectly()
        {
            Assert.That(_result.CourseCode, Is.EqualTo(_source.CourseCode));
        }

        [Test]
        public void ProviderIdIsMappedCorrectly()
        {
            Assert.That(_result.ProviderId, Is.EqualTo(_source.ProviderId));
        }

        [Test]
        public void ProviderNameIsMappedCorrectly()
        {
            Assert.That(_result.ProviderName, Is.EqualTo(_providerResponse.Name));
        }

        [Test]
        public void TransferSenderIdIsMappedCorrectly()
        {
            Assert.That(_result.TransferSenderId, Is.EqualTo(_source.TransferSenderId));
        }

        [Test]
        public void EncodedPledgeApplicationIdIsMappedCorrectly()
        {
            Assert.That(_result.EncodedPledgeApplicationId, Is.EqualTo(_source.EncodedPledgeApplicationId));
        }

        [Test]
        public void OriginIsMappedCorrectly()
        {
            Assert.That(_result.Origin, Is.EqualTo(_source.Origin));
        }

        [Test]
        public void AutoCreatedReservationIsMappedCorrectly()
        {
            Assert.That(_result.AutoCreatedReservation, Is.EqualTo(_source.AutoCreated));
        }
    }
}
