using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class ApprenticeRequestToSelectDeliveryModelViewModelMapperTests
    {
        private ApprenticeRequestToSelectDeliveryModelViewModelMapper _mapper;
        private ApprenticeRequest _source;
        private Mock<IApprovalsApiClient> _approvalsApiClient;
        private Mock<IFjaaAgencyService> _fjaaAgencyService;
        private ProviderCourseDeliveryModels _providerCourseDeliveryModels;
        private long _providerId;
        private int _agencyId;
        private string _courseCode;
        private SelectDeliveryModelViewModel _result;
        
        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _providerId = autoFixture.Create<long>();
            _courseCode = autoFixture.Create<string>();
            _agencyId = autoFixture.Create<int>();

            _source = autoFixture.Build<ApprenticeRequest>()
                .With(x => x.StartMonthYear, "062020")
                .With(x => x.AccountId, 12345)
                .With(x => x.CourseCode, "Course1")
                .With(x => x.ProviderId, _providerId)
                .With(x => x.CourseCode, _courseCode)
                .With(x => x.DeliveryModel, DeliveryModel.PortableFlexiJob)
                .Without(x => x.TransferSenderId).Create();
                        
            _providerCourseDeliveryModels = autoFixture.Create<ProviderCourseDeliveryModels>();

            _approvalsApiClient = new Mock<IApprovalsApiClient>();
            _approvalsApiClient.Setup(x => x.GetProviderCourseDeliveryModels(_providerId, _courseCode, It.IsAny<CancellationToken>())).ReturnsAsync(_providerCourseDeliveryModels);

            _fjaaAgencyService = new Mock<IFjaaAgencyService>();
            _fjaaAgencyService.Setup(x => x.AgencyExists(_agencyId)).ReturnsAsync(false);

            _mapper = new ApprenticeRequestToSelectDeliveryModelViewModelMapper(_approvalsApiClient.Object, _fjaaAgencyService.Object);
            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void AccountHashedIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AccountHashedId, _result.AccountHashedId);
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
        public void CourseCodeIsMappedCorrectly()
        {
            Assert.AreEqual(_source.CourseCode, _result.CourseCode);
        }

        [Test]
        public void DeliveryModelIsMappedCorrectly()
        {
            Assert.AreEqual(_source.DeliveryModel, _result.DeliveryModel);
        }

        [Test]
        public void DeliveryModelsAreMappedCorrectly()
        {
            Assert.AreEqual(_providerCourseDeliveryModels.DeliveryModels, _result.DeliveryModels);
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
        public void TransferSenderIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.TransferSenderId, _result.TransferSenderId);
        }
    }
}
