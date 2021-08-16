using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class WhenIMapDraftApprenticeshipToUpdateRequest
    {
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        private GetTrainingProgrammeResponse _trainingProgrammeResponse;
        private UpdateDraftApprenticeshipRequestMapper _mapper;
        private EditDraftApprenticeshipViewModel _source;
        private Func<Task<UpdateDraftApprenticeshipRequest>> _act;

        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();

            var birthDate = fixture.Create<DateTime?>();
            var startDate = fixture.Create<DateTime?>();
            var endDate = fixture.Create<DateTime?>();

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            _mapper = new UpdateDraftApprenticeshipRequestMapper(_mockCommitmentsApiClient.Object);

            _source = fixture.Build<EditDraftApprenticeshipViewModel>()
                .With(x => x.CourseCode, fixture.Create<int>().ToString())
                .With(x => x.BirthDay, birthDate?.Day)
                .With(x => x.BirthMonth, birthDate?.Month)
                .With(x => x.BirthYear, birthDate?.Year)
                .With(x => x.EndMonth, endDate?.Month)
                .With(x => x.EndYear, endDate?.Year)
                .With(x => x.StartMonth, startDate?.Month)
                .With(x => x.StartYear, startDate?.Year)
                .Without(x => x.StartDate)
                .Without(x => x.Courses)
                .Create();

            _trainingProgrammeResponse = fixture.Create<GetTrainingProgrammeResponse>();

            _mockCommitmentsApiClient.Setup(x => x.GetCalculatedTrainingProgrammeVersion(int.Parse(_source.CourseCode), _source.StartDate.Date.Value, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_trainingProgrammeResponse);

            _act = async () => await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public async Task ThenReservationIdIsMappedCorrectly()
        {
            var result = await _act();
            Assert.AreEqual(_source.ReservationId, result.ReservationId);
        }

        [Test]
        public async Task ThenFirstNameIsMappedCorrectly()
        {
            var result = await _act();
            Assert.AreEqual(_source.FirstName, result.FirstName);
        }

        [Test]
        public async Task ThenDateOfBirthIsMappedCorrectly()
        {
            var result = await _act();
            Assert.AreEqual(_source.DateOfBirth.Date, result.DateOfBirth);
        }

        [Test]
        public async Task ThenUniqueLearnerNumberIsMappedToNull()
        {
            var result = await _act();
            Assert.AreEqual(_source.Uln, result.Uln);
        }

        [Test]
        public async Task ThenEmailIsMappedCorrectly()
        {
            var result = await _act();
            Assert.AreEqual(_source.Email, result.Email);
        }

        [Test]
        public async Task ThenCourseCodeIsMappedCorrectly()
        {
            var result = await _act();
            Assert.AreEqual(_source.CourseCode, result.CourseCode);
        }

        [Test]
        public async Task ThenCostIsMappedCorrectly()
        {
            var result = await _act();
            Assert.AreEqual(_source.Cost, result.Cost);
        }

        [Test]
        public async Task ThenStartDateIsMappedCorrectly()
        {
            var result = await _act();
            Assert.AreEqual(_source.StartDate.Date, result.StartDate);
        }

        [Test]
        public async Task ThenEndDateIsMappedCorrectly()
        {
            var result = await _act();
            Assert.AreEqual(_source.EndDate.Date, result.EndDate);
        }

        [Test]
        public async Task ThenOriginatorReferenceIsMappedCorrectly()
        {
            var result = await _act();
            Assert.AreEqual(_source.Reference, result.Reference);
        }

        [Test]
        public async Task And_CalculatedStandardUIdHasChanged_Then_ResetCourseOption()
        {
            _trainingProgrammeResponse.TrainingProgramme.StandardUId = "ST0001_1.0";
            var result = await _act();
            Assert.IsNull(result.CourseOption);
        }

        [Test]
        public async Task And_CalculatedStandardUIdHasNotChanged_Then_MapCourseOption()
        {
            _trainingProgrammeResponse.TrainingProgramme.StandardUId = _source.StandardUId;
            var result = await _act();
            Assert.AreEqual(_source.CourseOption, result.CourseOption);
        }
    }
}