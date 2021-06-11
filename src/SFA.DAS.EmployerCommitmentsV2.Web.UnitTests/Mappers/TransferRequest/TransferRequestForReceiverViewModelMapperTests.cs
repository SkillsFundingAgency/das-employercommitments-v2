using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.TransferRequest;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.TransferRequest;
using SFA.DAS.Encoding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class TransferConfirmationViewModelMapperTests
    {
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;
        private Mock<IEncodingService> _mockEncodingService;
        
        private GetTransferRequestResponse _getTransferRequestResponse;
        private TransferRequestRequest _request;
        
        private TransferRequestForReceiverViewModelMapper _mapper;

        private const long AccountIdFirst = 12;
        private const long TransferRequestIdFirst = 34;
        private const long ReceivingEmployerAccountIdFirst = 56;

        [SetUp]
        public void SetUp()
        {
            //Arrange
            var autoFixture = new Fixture();
            _request = autoFixture.Build<TransferRequestRequest>()
                .With(x => x.AccountHashedId, $"A{AccountIdFirst}")
                .With(x => x.TransferRequestHashedId, $"A{TransferRequestIdFirst}")
                .Create();

            _getTransferRequestResponse = autoFixture.Build<GetTransferRequestResponse>()
                .With(x => x.ReceivingEmployerAccountId, ReceivingEmployerAccountIdFirst)
                .With(x => x.TransferRequestId, TransferRequestIdFirst)
                .Create();
            
            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockCommitmentsApiClient.Setup(r => r.GetTransferRequestForReceiver(It.IsAny<long>(), It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(_getTransferRequestResponse);
            
            _mockEncodingService = new Mock<IEncodingService>();
            
            _mockEncodingService.Setup(t => t.Encode(It.IsAny<long>(), EncodingType.AccountId))
                .Returns((long value, EncodingType encodingType) => $"A{value}");
            _mockEncodingService.Setup(t => t.Encode(It.IsAny<long>(), EncodingType.PublicAccountId))
                .Returns((long value, EncodingType encodingType) => $"P{value}");
            _mockEncodingService.Setup(t => t.Encode(It.IsAny<long>(), EncodingType.TransferRequestId))
                .Returns((long value, EncodingType encodingType) => $"T{value}");
            _mockEncodingService.Setup(t => t.Encode(It.IsAny<long>(), EncodingType.CohortReference))
                .Returns((long value, EncodingType encodingType) => $"C{value}");
            _mockEncodingService.Setup(t => t.Decode(It.IsAny<string>(), It.IsAny<EncodingType>()))
                .Returns((string value, EncodingType encodingType) => long.Parse(Regex.Replace(value, "[A-Za-z ]", "")));

            _mapper = new TransferRequestForReceiverViewModelMapper(_mockCommitmentsApiClient.Object, _mockEncodingService.Object);
        }

        [Test]
        public async Task GetTransferRequestForReceiverIsCalled()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            _mockCommitmentsApiClient.Verify(t => t.GetTransferRequestForReceiver(_request.AccountId, _request.TransferRequestId, It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task TransferReceiverHashedAccountId_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual($"A{_getTransferRequestResponse.ReceivingEmployerAccountId}", result.TransferReceiverHashedAccountId);
        }

        [Test]
        public async Task TransferSenderPublicHashedAccountId_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual($"P{_getTransferRequestResponse.SendingEmployerAccountId}", result.TransferSenderPublicHashedAccountId);
        }

        [Test]
        public async Task TransferSenderName_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_getTransferRequestResponse.TransferSenderName, result.TransferSenderName);
        }

        [Test]
        public async Task HashedCohortReference_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual($"C{_getTransferRequestResponse.CommitmentId}", result.HashedCohortReference);
        }

        [Test]
        public async Task TransferApprovalStatusDesc_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_getTransferRequestResponse.Status.ToString(), result.TransferApprovalStatusDesc);
        }

        [Test]
        public async Task TransferApprovalSetBy_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_getTransferRequestResponse.ApprovedOrRejectedByUserName, result.TransferApprovalSetBy);
        }

        [Test]
        public async Task TransferApprovalSetOn_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_getTransferRequestResponse.ApprovedOrRejectedOn, result.TransferApprovalSetOn);
        }

        [Test]
        public async Task TotalCost_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_getTransferRequestResponse.TransferCost, result.TotalCost);
        }

        [Test]
        public async Task FundingCap_IsMapped()
        {
            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(_getTransferRequestResponse.FundingCap, result.FundingCap);
        }

        [TestCase(TransferApprovalStatus.Pending, 1500.0, 12000, true)]
        [TestCase(TransferApprovalStatus.Approved, 1500.0, 12000, true)]
        [TestCase(TransferApprovalStatus.Pending, 12000.0, 12000, false)]
        [TestCase(TransferApprovalStatus.Approved, 12000.0, 12000, false)]
        [TestCase(TransferApprovalStatus.Pending, 13000.0, 12000, false)]
        [TestCase(TransferApprovalStatus.Approved, 13000.0, 12000, false)]
        [TestCase(TransferApprovalStatus.Rejected, 1500.0, 12000, false)]
        [TestCase(TransferApprovalStatus.Rejected, 12000.0, 12000, false)]
        [TestCase(TransferApprovalStatus.Rejected, 13000.0, 12000, false)]
        public async Task TrainingName_IsMapped(TransferApprovalStatus transferApprovalStatus, decimal transferCost, int fundingCap, bool showFundingCapWarning)
        {
            // Arrange
            _getTransferRequestResponse.Status = transferApprovalStatus;
            _getTransferRequestResponse.TransferCost = transferCost;
            _getTransferRequestResponse.FundingCap = fundingCap;

            //Act
            var result = await _mapper.Map(_request);

            //Assert
            Assert.AreEqual(showFundingCapWarning, result.ShowFundingCapWarning);
        }
    }
}
