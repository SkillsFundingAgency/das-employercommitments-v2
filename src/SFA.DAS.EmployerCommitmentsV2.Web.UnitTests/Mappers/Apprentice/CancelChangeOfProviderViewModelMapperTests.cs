using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
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
    public class CancelChangeOfProviderViewModelMapperTests
    {
        private Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

        private ChangeOfProviderRequest _request;
        private GetApprenticeshipResponse _apprenticeshipResponse;

        private CancelChangeOfProviderRequestViewModelMapper _mapper;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();

            _request = autoFixture.Create<ChangeOfProviderRequest>();
            _apprenticeshipResponse = autoFixture.Create<GetApprenticeshipResponse>();

            _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _mockCommitmentsApiClient.Setup(m => m.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_apprenticeshipResponse);

            _mapper = new CancelChangeOfProviderRequestViewModelMapper(_mockCommitmentsApiClient.Object);
        }

        [Test]
        public async Task ApprenticeshipHashedId_IsMapped()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_request.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test]
        public async Task AccountHashedId_IsMapped()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_request.AccountHashedId, result.AccountHashedId);
        }

        [Test]
        public async Task ProviderId_IsMapped()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_request.ProviderId, result.ProviderId);
        }

        [Test]
        public async Task ProviderName_IsMapped()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_request.ProviderName, result.ProviderName);
        }

        [Test]
        public async Task NewStartDate_IsMapped()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_request.NewStartMonth, result.NewStartMonth);
            Assert.AreEqual(_request.NewStartYear, result.NewStartYear);
        }

        [Test]
        public async Task NewEndDate_IsMapped()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_request.NewEndMonth, result.NewEndMonth);
            Assert.AreEqual(_request.NewEndYear, result.NewEndYear);
        }

        [Test]
        public async Task NewPrice_IsMapped()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_request.NewPrice, result.NewPrice);
        }

        [Test]
        public async Task EmployerWillAdd_IsMapped()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_request.EmployerWillAdd, result.EmployerWillAdd);
        }

        [Test]
        public async Task ApprenticeName_IsMapped()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual($"{_apprenticeshipResponse.FirstName} {_apprenticeshipResponse.LastName}", result.ApprenticeName);
        }

        [Test]
        public async Task OldProviderName_IsMapped()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_apprenticeshipResponse.ProviderName, result.OldProviderName);
        }

        [Test]
        public async Task WhenRequestingTheCancelChangeOfProviderRequestPage_ThenTheGetApprenticeshipIsCalledOnce()
        {
            await _mapper.Map(_request);

            _mockCommitmentsApiClient.Verify(m => m.GetApprenticeship(_request.ApprenticeshipId.Value, It.IsAny<CancellationToken>()), Times.Once());
        }
    }
}
