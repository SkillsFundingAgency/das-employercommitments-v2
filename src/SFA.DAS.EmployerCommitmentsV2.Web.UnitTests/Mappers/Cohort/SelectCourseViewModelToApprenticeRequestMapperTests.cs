using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class SelectCourseViewModelToApprenticeRequestMapperTests
    {
        private SelectCourseViewModelToApprenticeRequestMapper _mapper;
        private ApprenticeRequest _result;
        private SelectCourseViewModel _source;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _source = autoFixture.Build<SelectCourseViewModel>().Without(x => x.Courses).Create();

            _mapper = new SelectCourseViewModelToApprenticeRequestMapper();

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

        [Test]
        public void EncodedPledgeApplicationIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.EncodedPledgeApplicationId, _result.EncodedPledgeApplicationId);
        }

        [Test]
        public void ShowTrainingDetailsIsMappedCorrectly()
        {
            Assert.AreEqual(_source.ShowTrainingDetails, _result.ShowTrainingDetails);
        }
    }
}
