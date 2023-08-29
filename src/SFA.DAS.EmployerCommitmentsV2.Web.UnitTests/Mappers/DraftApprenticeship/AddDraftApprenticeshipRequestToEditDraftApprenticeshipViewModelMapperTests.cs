using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using System.Threading.Tasks;
using SFA.DAS.Encoding;


namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship
{
    [TestFixture]
    public class AddDraftApprenticeshipRequestToEditDraftApprenticeshipViewModelMapperTests
    {
        private AddDraftApprenticeshipRequestToEditDraftApprenticeshipViewModelMapper _mapper;
        private EditDraftApprenticeshipViewModel _result;
        private AddDraftApprenticeshipRequest _source;
        private Mock<IEncodingService> _encodingService;

        [SetUp]
        public async Task Arrange()
        {
            var autoFixture = new Fixture();

            _encodingService.Setup(x => x.Decode(It.IsAny<string>(), It.IsAny<EncodingType>()))
                .Returns(123);

            _source = autoFixture.Build<AddDraftApprenticeshipRequest>().Create();

            _mapper = new AddDraftApprenticeshipRequestToEditDraftApprenticeshipViewModelMapper(_encodingService.Object);

            _result = await _mapper.Map(TestHelper.Clone(_source));
        }

        [Test]
        public void AccountHashedIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.AccountHashedId, _result.AccountHashedId);
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

        public void DraftApprenticeshipHashedIdIsMappedCorrectly()
        {
            Assert.AreEqual(_source.DraftApprenticeshipHashedId, _result.DraftApprenticeshipHashedId);
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
