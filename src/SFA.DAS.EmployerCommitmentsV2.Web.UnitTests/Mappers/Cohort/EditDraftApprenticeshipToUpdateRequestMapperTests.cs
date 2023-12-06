using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Authentication;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class WhenIMapDraftApprenticeshipToUpdateRequest
    {
        private GetDraftApprenticeshipResponse _getDraftApprenticeshipResponse;
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

        private UpdateDraftApprenticeshipRequestMapper _mapper;
        private EditDraftApprenticeshipViewModel _source;
        private Func<Task<UpdateDraftApprenticeshipApimRequest>> _act;

        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();

            var birthDate = fixture.Create<DateTime?>();
            var startDate = fixture.Create<DateTime?>();
            var endDate = fixture.Create<DateTime?>();

            _getDraftApprenticeshipResponse = fixture.Create<GetDraftApprenticeshipResponse>();

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            _mapper = new UpdateDraftApprenticeshipRequestMapper(_mockCommitmentsApiClient.Object, Mock.Of<IAuthenticationService>());

            _source = fixture.Build<EditDraftApprenticeshipViewModel>()
                .With(x => x.DeliveryModel, DeliveryModel.PortableFlexiJob)
                .With(x => x.CourseCode, fixture.Create<int>().ToString())
                .With(x => x.BirthDay, birthDate?.Day)
                .With(x => x.BirthMonth, birthDate?.Month)
                .With(x => x.BirthYear, birthDate?.Year)
                .With(x => x.EndMonth, endDate?.Month)
                .With(x => x.EndYear, endDate?.Year)
                .With(x => x.StartMonth, startDate?.Month)
                .With(x => x.StartYear, startDate?.Year)
                .With(x => x.IsOnFlexiPaymentPilot, false)
                .Without(x => x.StartDate)
                .Without(x => x.Courses)
                .Create();

            _mockCommitmentsApiClient.Setup(x => x.GetDraftApprenticeship(_source.CohortId.Value, _source.DraftApprenticeshipId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_getDraftApprenticeshipResponse);

            _act = async () => await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public async Task ThenReservationIdIsMappedCorrectly()
        {
            var result = await _act();
            Assert.That(result.ReservationId, Is.EqualTo(_source.ReservationId));
        }

        [Test]
        public async Task ThenFirstNameIsMappedCorrectly()
        {
            var result = await _act();
            Assert.That(result.FirstName, Is.EqualTo(_source.FirstName));
        }

        [Test]
        public async Task ThenDateOfBirthIsMappedCorrectly()
        {
            var result = await _act();
            Assert.That(result.DateOfBirth, Is.EqualTo(_source.DateOfBirth.Date));
        }

        [Test]
        public async Task ThenUniqueLearnerNumberIsMappedToNull()
        {
            var result = await _act();
            Assert.That(result.Uln, Is.EqualTo(_source.Uln));
        }

        [Test]
        public async Task ThenEmailIsMappedCorrectly()
        {
            var result = await _act();
            Assert.That(result.Email, Is.EqualTo(_source.Email));
        }

        [Test]
        public async Task ThenDeliveryModelIsMappedCorrectly()
        {
            var result = await _act();
            Assert.That(result.DeliveryModel, Is.EqualTo(_source.DeliveryModel));
        }

        [Test]
        public async Task ThenCourseCodeIsMappedCorrectly()
        {
            var result = await _act();
            Assert.That(result.CourseCode, Is.EqualTo(_source.CourseCode));
        }

        [Test]
        public async Task ThenCostIsMappedCorrectly()
        {
            var result = await _act();
            Assert.That(result.Cost, Is.EqualTo(_source.Cost));
        }

        [Test]
        public async Task ThenEmploymentPriceIsMappedCorrectly()
        {
            var result = await _act();
            Assert.That(result.EmploymentPrice, Is.EqualTo(_source.EmploymentPrice));
        }

        [Test]
        public async Task ThenStartDateIsMappedCorrectly()
        {
            var result = await _act();
            Assert.That(result.StartDate, Is.EqualTo(_source.StartDate.Date));
        }

        [Test]
        public async Task ThenEndDateIsMappedCorrectly()
        {
            var result = await _act();
            Assert.That(result.EndDate, Is.EqualTo(_source.EndDate.Date));
        }

        [Test]
        public async Task ThenEmploymentEndDateIsMappedCorrectly()
        {
            var result = await _act();
            Assert.That(result.EmploymentEndDate, Is.EqualTo(_source.EmploymentEndDate.Date));
        }

        [Test]
        public async Task ThenOriginatorReferenceIsMappedCorrectly()
        {
            var result = await _act();
            Assert.That(result.Reference, Is.EqualTo(_source.Reference));
        }

        [TestCase("Option 1")]
        [TestCase("")]
        [TestCase(null)]
        public async Task ThenCourseOptionIsMappedCorrectly(string option)
        {
            _getDraftApprenticeshipResponse.TrainingCourseOption = option;
            var result = await _act();
            Assert.That(result.CourseOption, Is.EqualTo(_getDraftApprenticeshipResponse.TrainingCourseOption));
        }

        [Test]
        public async Task ThenIsOnFlexiPaymentPilotIsMappedCorrectly()
        {
            var result = await _act();
            Assert.That(result.IsOnFlexiPaymentPilot, Is.EqualTo(_source.IsOnFlexiPaymentPilot));
        }

        [Test]
        public async Task ThenActualStartDateIsMappedCorrectly()
        {
            var result = await _act();
            Assert.That(result.ActualStartDate, Is.EqualTo(_source.ActualStartDate));
        }
    }
}