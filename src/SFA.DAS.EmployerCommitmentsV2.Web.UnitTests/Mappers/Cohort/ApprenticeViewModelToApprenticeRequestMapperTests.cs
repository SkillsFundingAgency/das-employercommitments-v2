using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class ApprenticeViewModelToApprenticeRequestMapperTests
    {
        private ApprenticeViewModelToApprenticeRequestMapper _mapper;
        private ApprenticeRequest _result;
        private ApprenticeViewModel _source;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            var birthDate = autoFixture.Create<DateTime?>();
            var startDate = autoFixture.Create<DateTime?>();
            var endDate = autoFixture.Create<DateTime?>();
            var employmentEndDate = autoFixture.Create<DateTime?>();

            _source = autoFixture.Build<ApprenticeViewModel>()
                .With(x => x.BirthDay, birthDate?.Day)
                .With(x => x.BirthMonth, birthDate?.Month)
                .With(x => x.BirthYear, birthDate?.Year)
                .With(x => x.Cost, 1600)
                .With(x => x.EndMonth, endDate?.Month)
                .With(x => x.EndYear, endDate?.Year)
                .With(x => x.StartMonth, startDate?.Month)
                .With(x => x.StartYear, startDate?.Year)
                .With(x => x.EmploymentEndMonth, employmentEndDate?.Month)
                .With(x => x.EmploymentEndYear, employmentEndDate?.Year)
                .Without(x => x.StartDate)
                .Without(x => x.Courses)
                .Create();

            _mapper = new ApprenticeViewModelToApprenticeRequestMapper();

            _result = await _mapper.Map(TestHelper.Clone(_source));
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
        public void LegalEntityNameIsMappedCorrectly()
        {
            Assert.AreEqual(_source.LegalEntityName, _result.LegalEntityName);
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
        public void DeliveryModelIsMappedCorrectly()
        {
            Assert.AreEqual(_source.DeliveryModel, _result.DeliveryModel);
        }
    }
}
