using System;
using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class CreateCohortRequestMapperTests
    {
        private CreateCohortRequestMapper _mapper;
        private ApprenticeViewModel _source;
        private CreateCohortRequest _result;

        [SetUp]
        public async Task Arrange()
        {
            var fixture = new Fixture();

            var birthDate = fixture.Create<DateTime?>();
            var startDate = fixture.Create<DateTime?>();
            var endDate = fixture.Create<DateTime?>();
            var employmentEndDate = fixture.Create<DateTime?>();

            _mapper = new CreateCohortRequestMapper();

            _source = fixture.Build<ApprenticeViewModel>()
                .With(x => x.BirthDay, birthDate?.Day)
                .With(x => x.BirthMonth, birthDate?.Month)
                .With(x => x.BirthYear, birthDate?.Year)
                .With(x => x.Cost, birthDate?.Year)
                .With(x => x.EndMonth, endDate?.Month)
                .With(x => x.EndYear, endDate?.Year)
                .With(x => x.StartMonth, startDate?.Month)
                .With(x => x.StartYear, startDate?.Year)
                .With(x => x.EmploymentEndMonth, employmentEndDate?.Month)
                .With(x => x.EmploymentEndYear, employmentEndDate?.Year)
                .Without(x => x.StartDate)
                .Without(x => x.Courses)
                .Create();

            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void ThenReservationIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ReservationId, _result.ReservationId);
        }

        [Test]
        public void ThenFirstNameIsMappedCorrectly()
        {
            Assert.AreEqual(_source.FirstName, _result.FirstName);
        }

        [Test]
        public void ThenDateOfBirthIsMappedCorrectly()
        {
            Assert.AreEqual(_source.DateOfBirth.Date, _result.DateOfBirth);
        }

        [Test]
        public void ThenUniqueLearnerNumberIsMappedCorrectly()
        {
            Assert.AreEqual(_source.Uln, _result.Uln);
        }

        [Test]
        public void ThenEmailIsMappedCorrectly()
        {
            Assert.AreEqual(_source.Email, _result.Email);
        }

        [Test]
        public void ThenDeliveryModelIsMappedCorrectly()
        {
            Assert.AreEqual(_source.DeliveryModel, _result.DeliveryModel);
        }

        [Test]
        public void ThenCourseCodeIsMappedCorrectly()
        {
            Assert.AreEqual(_source.CourseCode, _result.CourseCode);
        }

        [Test]
        public void ThenCostIsMappedCorrectly()
        {
            Assert.AreEqual(_source.Cost, _result.Cost);
        }

        [Test]
        public void ThenEmploymentPriceIsMappedCorrectly()
        {
            Assert.AreEqual(_source.EmploymentPrice, _result.EmploymentPrice);
        }

        [Test]
        public void ThenStartDateIsMappedCorrectly()
        {
            Assert.AreEqual(_source.StartDate.Date, _result.StartDate);
        }

        [Test]
        public void ThenEndDateIsMappedCorrectly()
        {
            Assert.AreEqual(_source.EndDate.Date, _result.EndDate);
        }

        [Test]
        public void ThenEmploymentEndDateIsMappedCorrectly()
        {
            Assert.AreEqual(_source.EmploymentEndDate.Date, _result.EmploymentEndDate);
        }

        [Test]
        public void ThenOriginatorReferenceIsMappedCorrectly()
        {
            Assert.AreEqual(_source.Reference, _result.OriginatorReference);
        }

        [Test]
        public void ThenProviderIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ProviderId, _result.ProviderId);
        }

        [Test]
        public void ThenAccountIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AccountId, _result.AccountId);
        }

        [Test]
        public void ThenTransferSenderIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.DecodedTransferSenderId, _result.TransferSenderId);
        }

        [Test]
        public void ThenPledgeApplicationIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.PledgeApplicationId, _result.PledgeApplicationId);
        }

        [Test]
        public void ThenIsOnFlexiPaymentPilotIsMappedCorrectly()
        {
            Assert.AreEqual(_source.IsOnFlexiPaymentPilot, _result.IsOnFlexiPaymentPilot);
        }
    }
}
