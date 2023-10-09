using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Encoding;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship
{
    [TestFixture]
    public class EditDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapperTests
    {
        private EditDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapper _mapper;
        private EditDraftApprenticeshipViewModel _source;
        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private Mock<IEncodingService> _encodingService;
        private GetCohortResponse _getCohortResponse;
        private AddDraftApprenticeshipRequest _result;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            var birthDate = autoFixture.Create<DateTime?>();
            var startDate = autoFixture.Create<DateTime?>();
            var endDate = autoFixture.Create<DateTime?>();
            var employmentEndDate = autoFixture.Create<DateTime?>();

            _getCohortResponse = autoFixture.Build<GetCohortResponse>()
                .With(x => x.LevyStatus, ApprenticeshipEmployerType.Levy)
                .With(x => x.WithParty, Party.Employer)
                .Without(x => x.TransferSenderId)
                .Create();

            _source = autoFixture.Build<EditDraftApprenticeshipViewModel>()
                .With(x => x.BirthDay, birthDate?.Day)
                .With(x => x.BirthMonth, birthDate?.Month)
                .With(x => x.BirthYear, birthDate?.Year)
                .With(x => x.EndMonth, endDate?.Month)
                .With(x => x.EndYear, endDate?.Year)
                .With(x => x.StartMonth, startDate?.Month)
                .With(x => x.StartYear, startDate?.Year)
                .With(x => x.EmploymentEndMonth, employmentEndDate?.Month)
                .With(x => x.EmploymentEndYear, employmentEndDate?.Year)
                .With(x => x.CourseCode, "Course1")
                .With(x => x.DeliveryModel, DeliveryModel.PortableFlexiJob)
                .Without(x => x.StartDate)
                .Without(x => x.Courses)
                .Create();

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _commitmentsApiClient.Setup(x => x.GetCohort(_source.CohortId.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getCohortResponse);

            _encodingService = new Mock<IEncodingService>();
            _encodingService.Setup(x => x.Decode(It.Is<string>(c => c == _source.CohortReference), It.Is<EncodingType>(y => y == EncodingType.CohortReference)))
                .Returns(_source.CohortId.Value);
            
            _mapper = new EditDraftApprenticeshipViewModelToAddDraftApprenticeshipRequestMapper(_commitmentsApiClient.Object, _encodingService.Object);

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
            Assert.AreEqual(_getCohortResponse.AccountLegalEntityId, _result.AccountLegalEntityId);
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
        public void CostIsMappedCorrectly()
        {
            Assert.AreEqual(_source.Cost, _result.Cost);
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
        public void DraftApprenticeshipHashedIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.DraftApprenticeshipHashedId, _result.DraftApprenticeshipHashedId);
        }

        [Test]
        public void EmploymentPriceIsMappedCorrectly()
        {
            Assert.AreEqual(_source.EmploymentPrice, _result.EmploymentPrice);
        }

        [Test]
        public void EmploymentEndDateIsMappedCorrectly()
        {
            Assert.AreEqual(_source.EmploymentEndDate.Date, _result.EmploymentEndDate.Value);
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
    }
}
