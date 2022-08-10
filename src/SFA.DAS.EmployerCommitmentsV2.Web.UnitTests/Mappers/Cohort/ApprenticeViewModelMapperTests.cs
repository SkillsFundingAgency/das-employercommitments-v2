using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
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
        private Mock<IAuthorizationService> _mockAuthorizationService;
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

            _mockAuthorizationService = new Mock<IAuthorizationService>();
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

            _mapper = new ApprenticeViewModelMapper(_commitmentsApiClient.Object, _mockAuthorizationService.Object);

            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void AccountLegalEntityIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AccountLegalEntityId, _result.AccountLegalEntityId);
        }

        [Test]
        public void AccountLegalEntityHashedIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AccountLegalEntityHashedId, _result.AccountLegalEntityHashedId);
        }

        [Test]
        public void AccountLegalEntityNameIsMappedCorrectly()
        {
            Assert.AreEqual(_accountLegalEntityResponse.LegalEntityName, _result.LegalEntityName);
        }

        [Test]
        public void StartDateIsMappedCorrectly()
        {
            Assert.AreEqual(new MonthYearModel(_source.StartMonthYear).Date, _result.StartDate.Date);
        }

        [Test]
        public void ReservationIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ReservationId, _result.ReservationId);
        }

        [Test]
        public void CourseCodeIsMappedCorrectly()
        {
            Assert.AreEqual(_source.CourseCode, _result.CourseCode);
        }

        [Test]
        public void ProviderIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ProviderId, _result.ProviderId);
        }

        [Test]
        public void ProviderNameIsMappedCorrectly()
        {
            Assert.AreEqual(_providerResponse.Name, _result.ProviderName);
        }

        [Test]
        public void TransferSenderIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.TransferSenderId, _result.TransferSenderId);
        }

        [Test]
        public void EncodedPledgeApplicationIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.EncodedPledgeApplicationId, _result.EncodedPledgeApplicationId);
        }

        [Test]
        public void OriginIsMappedCorrectly()
        {
            Assert.AreEqual(_source.Origin, _result.Origin);
        }

        [Test]
        public void AutoCreatedReservationIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AutoCreated, _result.AutoCreatedReservation);
        }
    }
}
