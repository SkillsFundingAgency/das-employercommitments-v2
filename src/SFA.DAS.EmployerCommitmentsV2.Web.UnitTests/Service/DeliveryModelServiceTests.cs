using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Services;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Service
{
    [TestFixture]
    public class DeliveryModelServiceTests
    {
        private DeliveryModelService _mapper;
        private Mock<IApprovalsApiClient> _outerApiClient;
        private Mock<IEncodingService> _encodingService;
        private ProviderCourseDeliveryModels _response;
        private long _providerId;
        private string _courseCode;

        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();
            _providerId = fixture.Create<long>();
            _courseCode = fixture.Create<string>();
            _response = fixture.Create<ProviderCourseDeliveryModels>();

            _outerApiClient = new Mock<IApprovalsApiClient>();
            _outerApiClient.Setup(x => x.GetProviderCourseDeliveryModels(_providerId, _courseCode, 1234, It.IsAny<CancellationToken>())).ReturnsAsync(_response);
            _encodingService = new Mock<IEncodingService>();
            _encodingService.Setup(x => x.Decode(It.IsAny<string>(), EncodingType.PublicAccountLegalEntityId))
                .Returns(1234);

            _mapper = new DeliveryModelService(_outerApiClient.Object, _encodingService.Object);
        }

        [Test]
        public async Task ThenReturnsTrueWhenMultipleDeliveryModelsExist()
        {
            var result = await _mapper.HasMultipleDeliveryModels(_providerId, _courseCode, "ALEID");
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ThenReturnsFalseWhenMultipleDeliveryModelsDoNotExist()
        {
            _response.DeliveryModels = new List<DeliveryModel> { DeliveryModel.Regular };
            var result = await _mapper.HasMultipleDeliveryModels(_providerId, _courseCode, "ALEID");
            Assert.IsFalse(result);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public async Task ThenReturnsFalseWhenCourseIsEmpty(string courseCode)
        {
            _response.DeliveryModels = new List<DeliveryModel> { DeliveryModel.Regular, DeliveryModel.FlexiJobAgency };
            var result = await _mapper.HasMultipleDeliveryModels(_providerId, courseCode, "PALID");
            Assert.IsFalse(result);
        }
    }
}
