using System.Threading.Tasks;
using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers
{
    [TestFixture]
    public class SelectProviderViewModelToSelectProviderRequestMapperTests
    {
        private SelectProviderViewModelToSelectProviderRequestMapper _mapper;
        private SelectProviderViewModel _source;
        private SelectProviderRequest _result;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _mapper = new SelectProviderViewModelToSelectProviderRequestMapper();
            _source = autoFixture.Create<SelectProviderViewModel>();
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
        public void CourseCodeIsMappedCorrectly()
        {
            Assert.AreEqual(_source.CourseCode, _result.CourseCode);
        }

        [Test]
        public void ReservationIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ReservationId, _result.ReservationId);
        }

        [Test]
        public void StartMonthYearIsMappedCorrectly()
        {
            Assert.AreEqual(_source.StartMonthYear, _result.StartMonthYear);
        }
    }
}
