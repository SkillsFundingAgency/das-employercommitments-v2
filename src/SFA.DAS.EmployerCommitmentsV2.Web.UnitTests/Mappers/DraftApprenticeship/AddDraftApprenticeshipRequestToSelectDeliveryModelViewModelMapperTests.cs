using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using SFA.DAS.Authorization.Services;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship
{
    [TestFixture]
    public class AddDraftApprenticeshipRequestToSelectDeliveryModelViewModelMapperTests
    {
        private AddDraftApprenticeshipRequestToSelectDeliveryModelViewModelMapper _mapper;
        private AddDraftApprenticeshipRequest _source;
        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private GetCohortResponse _getCohortResponse;
        private Mock<IApprovalsApiClient> _approvalsApiClient;
        private Mock<IAuthorizationService> _authService;
        private ProviderCourseDeliveryModels _providerCourseDeliveryModels;
        private long _providerId;
        private string _courseCode;
        private long _cohortId;
        private long _accountLegalEntityId;
        private SelectDeliveryModelViewModel _result;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _providerId = autoFixture.Create<long>();
            _courseCode = autoFixture.Create<string>();
            _cohortId = autoFixture.Create<long>();
            _accountLegalEntityId = autoFixture.Create<long>();

            _source = autoFixture.Build<AddDraftApprenticeshipRequest>()
                .With(x => x.StartMonthYear, "062020")
                .With(x => x.CourseCode, "Course1")
                .With(x => x.ProviderId, _providerId)
                .With(x => x.CourseCode, _courseCode)
                .With(x => x.AccountLegalEntityId, _accountLegalEntityId)
                .With(x => x.CohortId, _cohortId)
                .With(x => x.DeliveryModel, DeliveryModel.PortableFlexiJob)
                .Create();

            _getCohortResponse = autoFixture.Build<GetCohortResponse>()
                .With(x => x.LevyStatus, ApprenticeshipEmployerType.Levy)
                .With(x => x.WithParty, Party.Employer)
                .With(x => x.ProviderId, _providerId)
                .Without(x => x.TransferSenderId)
                .Create();

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _commitmentsApiClient.Setup(x => x.GetCohort(_source.CohortId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getCohortResponse);

            _providerCourseDeliveryModels = autoFixture.Create<ProviderCourseDeliveryModels>();

            _approvalsApiClient = new Mock<IApprovalsApiClient>();
            _approvalsApiClient.Setup(x => x.GetProviderCourseDeliveryModels(_providerId, _courseCode, _accountLegalEntityId, It.IsAny<CancellationToken>())).ReturnsAsync(_providerCourseDeliveryModels);

            _mapper = new AddDraftApprenticeshipRequestToSelectDeliveryModelViewModelMapper(_commitmentsApiClient.Object, _approvalsApiClient.Object);

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
        public void CohortIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.CohortId, _result.CohortId);
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
    }
}