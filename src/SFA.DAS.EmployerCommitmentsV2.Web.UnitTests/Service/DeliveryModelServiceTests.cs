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

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Service
{
    [TestFixture]
    public class DeliveryModelServiceTests
    {
        private DeliveryModelService _mapper;
        private Mock<IApprovalsApiClient> _outerApiClient;
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
            _outerApiClient.Setup(x => x.GetProviderCourseDeliveryModels(_providerId, _courseCode, It.IsAny<CancellationToken>())).ReturnsAsync(_response);

            _mapper = new DeliveryModelService(_outerApiClient.Object);
        }

        [Test]
        public async Task ThenReturnsTrueWhenMultipleDeliveryModelsExist()
        {
            var result = await _mapper.HasMultipleDeliveryModels(_providerId, _courseCode);
            Assert.IsTrue(result);
        }

        [Test]
        public async Task ThenReturnsFalseWhenMultipleDeliveryModelsDoNotExist()
        {
            _response.DeliveryModels = new List<DeliveryModel> { DeliveryModel.Regular };
            var result = await _mapper.HasMultipleDeliveryModels(_providerId, _courseCode);
            Assert.IsFalse(result);
        }
    }
}
