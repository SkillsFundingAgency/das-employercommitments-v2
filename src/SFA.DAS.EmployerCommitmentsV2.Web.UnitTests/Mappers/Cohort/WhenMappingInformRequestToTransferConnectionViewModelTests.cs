using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.EAS.Account.Api.Client;
using SFA.DAS.EAS.Account.Api.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class WhenMappingInformRequestToTransferConnectionViewModelTests
    {
        private Mock<IAccountApiClient> _accountApiClient;
        private InformRequestToSelectTransferConnectionViewModelMapper _mapper;        
        private InformRequest _informRequest;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();
            _accountApiClient = new Mock<IAccountApiClient>();
            _informRequest = autoFixture.Create<InformRequest>(); 

            _accountApiClient.Setup(x => x.GetTransferConnections(_informRequest.AccountHashedId))
                .ReturnsAsync(new List<TransferConnectionViewModel>
                {
                    new TransferConnectionViewModel
                    {
                        FundingEmployerAccountId = 1234,
                        FundingEmployerAccountName = "FirstAccountName",
                        FundingEmployerHashedAccountId = "FAN"
                    },
                    new TransferConnectionViewModel
                    {
                        FundingEmployerAccountId = 1235,
                        FundingEmployerAccountName = "SecondAccountName",
                        FundingEmployerHashedAccountId = "SAN"
                    }

                });

            _mapper = new InformRequestToSelectTransferConnectionViewModelMapper(_accountApiClient.Object);
        }

        [Test]
        public async Task Then_AccountHashedId_Is_Mapped()
        {
            //Act
            var result = await _mapper.Map(_informRequest);

            //Assert           
            Assert.AreEqual(_informRequest.AccountHashedId, result.AccountHashedId);
        }


        [Test]
        public async Task Then_Non_Empty_List_Of_TransferConnections_Is_Mapped()
        {   
            //Act
            var result = await _mapper.Map(_informRequest);

            //Assert           
            Assert.AreEqual(2, result.TransferConnections.Count);
        }

        [Test]
        public async Task Then_Empty_List_Of_TransferConnections_Is_Mapped()
        {
            //Arrange
            _accountApiClient.Setup(x => x.GetTransferConnections(_informRequest.AccountHashedId))
                .ReturnsAsync(new List<TransferConnectionViewModel>());

            //Act
            var result = await _mapper.Map(_informRequest);

            //Assert           
            Assert.AreEqual(0, result.TransferConnections.Count);
        }

        [Test]
        public async Task Then_GetTransferConnections_Is_Called()
        {
            //Act
            var result = await _mapper.Map(_informRequest);

            //Assert
            _accountApiClient.Verify(x => x.GetTransferConnections(It.Is<String>(c => c == _informRequest.AccountHashedId)),                   
                Times.Once);
        }
    }
}
