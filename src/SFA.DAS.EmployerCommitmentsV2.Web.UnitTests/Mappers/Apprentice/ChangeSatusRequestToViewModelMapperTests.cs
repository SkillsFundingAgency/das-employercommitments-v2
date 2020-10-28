using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    [TestFixture]
    public class WhenMapping_ChangeSatusRequestToViewModelMapperTests
    {
        private ChangeStatusRequestToViewModelMapper _mapper;
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

        [SetUp]
        public void Arrange()
        {
            

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            
            _mapper = new ChangeStatusRequestToViewModelMapper(_mockCommitmentsApiClient.Object);
            
            _mockCommitmentsApiClient.Setup(a => a.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetApprenticeshipResponse
                {
                    Status = ApprenticeshipStatus.Completed
                });
        }

        [Test, MoqAutoData]
        public async Task ThenApprenticeshipHashedIdIsMappedCorrectly(ChangeStatusRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(request.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task ThenAccountHashedIdIsMappedCorrectly(ChangeStatusRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task ThenCurrentStatusIsMapped(ChangeStatusRequest request)
        {
            var result = await _mapper.Map(request);

            Assert.AreEqual(ApprenticeshipStatus.Completed, result.CurrentStatus);
        }
    }
}
