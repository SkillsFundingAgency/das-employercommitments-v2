using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship
{
    [TestFixture]
    public class AddDraftApprenticeshipViewModelMapperTests
    {
        private AddDraftApprenticeshipViewModelMapper _mapper;
        private AddDraftApprenticeshipRequest _source;
        private AddDraftApprenticeshipViewModel _result;

        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private Mock<IEncodingService> _encodingService;
        private Mock<IAuthorizationService> _authorizationService;
        private string _encodedTransferSenderId;
        private GetCohortResponse _cohort;
        private List<TrainingProgramme> _allTrainingProgrammes;
        private List<TrainingProgramme> _standardTrainingProgrammes;
        
        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _allTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
            _standardTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
            
            _cohort = autoFixture.Create<GetCohortResponse>();
            _cohort.WithParty = Party.Employer;
            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _commitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_cohort);
            
            _commitmentsApiClient
                .Setup(x => x.GetAllTrainingProgrammeStandards(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAllTrainingProgrammeStandardsResponse
                {
                    TrainingProgrammes = _standardTrainingProgrammes
                });
            
            _commitmentsApiClient
                .Setup(x => x.GetAllTrainingProgrammes(It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetAllTrainingProgrammesResponse
                {
                    TrainingProgrammes = _allTrainingProgrammes
                });

            _encodedTransferSenderId = autoFixture.Create<string>();
            _encodingService = new Mock<IEncodingService>();
            _encodingService
                .Setup(x => x.Encode(It.IsAny<long>(), It.Is<EncodingType>(e => e == EncodingType.PublicAccountId)))
                .Returns(_encodedTransferSenderId);

            _authorizationService = new Mock<IAuthorizationService>();

            _mapper = new AddDraftApprenticeshipViewModelMapper(_commitmentsApiClient.Object, _encodingService.Object, _authorizationService.Object);

            _source = autoFixture.Create<AddDraftApprenticeshipRequest>();
            _source.StartMonthYear = "092020";

            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void CourseCodeIsMappedCorrectly()
        {
            Assert.AreEqual(_source.CourseCode, _result.CourseCode);
        }

        [Test]
        public void StartDateIsMappedCorrectly()
        {
            Assert.AreEqual(new MonthYearModel(_source.StartMonthYear).Date, _result.StartDate.Date);
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
        public void ReservationIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ReservationId, _result.ReservationId);
        }

        [Test]
        public void AutoCreatedReservationIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AutoCreated, _result.AutoCreatedReservation);
        }

        [Test]
        public void TransferSenderHashedIdIsMappedCorrectly()
        {
            Assert.AreEqual(_encodedTransferSenderId, _result.TransferSenderHashedId);
        }
       

        [Test]
        public void ProviderNameIsMappedCorrectly()
        {
            Assert.AreEqual(_cohort.ProviderName, _result.ProviderName);
        }

        [Test]
        public void LegalEntityNameIsMappedCorrectly()
        {
            Assert.AreEqual(_cohort.LegalEntityName, _result.LegalEntityName);
        }

        [Test]
        public void ThrowsWhenCohortNotWithEditingParty()
        {
            _cohort.WithParty = Party.Provider;
            Assert.ThrowsAsync<CohortEmployerUpdateDeniedException>(() => _mapper.Map(_source));
        }

        [Test]
        public void IsOnFlexiPaymentPilotIsFalse()
        {
            Assert.IsFalse(_result.IsOnFlexiPaymentPilot);
        }
    }
}
