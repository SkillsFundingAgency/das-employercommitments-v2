using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class CreateCohortWithDraftApprenticeshipRequestToAddDraftApprenticeshipViewModelMapperTests
    {
        private AddDraftApprenticeshipViewModelMapper _mapper;
        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private GetProviderResponse _providerResponse;
        private AccountLegalEntityResponse _accountLegalEntityResponse;
        private ApprenticeRequest _source;
        private AddDraftApprenticeshipViewModel _result;
        private TrainingProgrammeApiClientMock _trainingProgrammeApiClient;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _providerResponse = autoFixture.Create<GetProviderResponse>();
            _accountLegalEntityResponse = autoFixture.Build<AccountLegalEntityResponse>().With(x => x.LevyStatus, ApprenticeshipEmployerType.Levy).Create();

            _source = autoFixture.Build<ApprenticeRequest>()
                .With(x=>x.StartMonthYear, "062020")
                .With(x=>x.AccountId, 12345)
                .Without(x=>x.TransferSenderId).Create();

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _commitmentsApiClient.Setup(x => x.GetProvider(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_providerResponse);
            _commitmentsApiClient.Setup(x => x.GetAccountLegalEntity(_source.AccountLegalEntityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_accountLegalEntityResponse);

            _trainingProgrammeApiClient = new TrainingProgrammeApiClientMock(); ;

            _mapper = new AddDraftApprenticeshipViewModelMapper(
                _commitmentsApiClient.Object,
                _trainingProgrammeApiClient.Object);

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
        public void CoursesAreMappedCorrectly()
        {
            Assert.AreEqual(_trainingProgrammeApiClient.All, _result.Courses);
        }

        [Test]
        public async Task TransferFundedCohortsAllowStandardCoursesOnlyWhenEmployerIsLevy()
        {
            _source.TransferSenderId = "test";
            _result = await _mapper.Map(TestHelper.Clone(_source));
            Assert.IsFalse(_result.Courses.Any(x => x is Framework));
        }

        [TestCase("12345")]
        [TestCase(null)]
        public async Task NonLevyCohortsAllowStandardCoursesOnlyRegardlessOfTransferStatus(string transferSenderId)
        {
            _source.TransferSenderId = transferSenderId;
            _accountLegalEntityResponse.LevyStatus = ApprenticeshipEmployerType.NonLevy;
            _result = await _mapper.Map(TestHelper.Clone(_source));
            Assert.IsFalse(_result.Courses.Any(x => x is Framework));
        }

    }
}
