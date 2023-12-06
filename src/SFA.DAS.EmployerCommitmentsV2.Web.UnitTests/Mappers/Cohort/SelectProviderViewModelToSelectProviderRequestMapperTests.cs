using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class SelectProviderViewModelToSelectProviderRequestMapperTests
    {
        private SelectProviderRequestMapper _mapper;
        private SelectProviderViewModel _source;
        private SelectProviderRequest _result;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _mapper = new SelectProviderRequestMapper();
            _source = autoFixture.Create<SelectProviderViewModel>();
            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void AccountHashedIdIsMappedCorrectly()
        {
            Assert.That(_result.AccountHashedId, Is.EqualTo(_source.AccountHashedId));
        }

        [Test]
        public void AccountLegalEntityHashedIdIsMappedCorrectly()
        {
            Assert.That(_result.AccountLegalEntityHashedId, Is.EqualTo(_source.AccountLegalEntityHashedId));
        }

        [Test]
        public void CourseCodeIsMappedCorrectly()
        {
            Assert.That(_result.CourseCode, Is.EqualTo(_source.CourseCode));
        }

        [Test]
        public void ReservationIdIsMappedCorrectly()
        {
            Assert.That(_result.ReservationId, Is.EqualTo(_source.ReservationId));
        }

        [Test]
        public void StartMonthYearIsMappedCorrectly()
        {
            Assert.That(_result.StartMonthYear, Is.EqualTo(_source.StartMonthYear));
        }
    }
}
