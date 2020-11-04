using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetChangeOfPartyRequestsResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ViewChangeViewModelMapperTests
    {
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

        private GetChangeOfPartyRequestsResponse _changeOfPartyResponse;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();

            _changeOfPartyResponse = autoFixture.Build<GetChangeOfPartyRequestsResponse>()
                                        .With(r => r.ChangeOfPartyRequests, GetChangeOfPartyRequests())
                                        .Create();

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            _mockCommitmentsApiClient.Setup(c => c.GetChangeOfPartyRequests(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_changeOfPartyResponse);
        }

        [Test]
        public void ChangeOfPartyRequestIsCalled()
        {

        }

        
        private List<ChangeOfPartyRequest> GetChangeOfPartyRequests()
        {
            return new List<ChangeOfPartyRequest>
            {
                new ChangeOfPartyRequest
                {
                    Price = 100
                }
            };
        }
    }
}
