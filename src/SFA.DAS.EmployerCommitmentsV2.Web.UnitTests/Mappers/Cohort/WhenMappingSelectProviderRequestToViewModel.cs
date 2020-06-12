using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class WhenMappingSelectProviderRequestToViewModel
    {
        private SelectProviderRequest _request;
        private Mock<ICommitmentsApiClient> _commitmentsApiClientMock;
        private AccountLegalEntityResponse _commitmentsApiClientResponse;
        private SelectProviderViewModelMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var autoFixture = new Fixture();
            _request = autoFixture.Freeze<SelectProviderRequest>();
            _commitmentsApiClientResponse = autoFixture.Freeze<AccountLegalEntityResponse>();

            _commitmentsApiClientMock = autoFixture.Freeze<Mock<ICommitmentsApiClient>>();
            _commitmentsApiClientMock
                .Setup(x => x.GetAccountLegalEntity(_request.AccountLegalEntityId, CancellationToken.None))
                .ReturnsAsync(_commitmentsApiClientResponse);

            _mapper = new SelectProviderViewModelMapper(_commitmentsApiClientMock.Object);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsReservationId()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_request.ReservationId, result.ReservationId);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsAccountHashedId()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_request.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsLegalEntityName()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_commitmentsApiClientResponse.LegalEntityName, result.LegalEntityName);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsCourseCode()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_request.CourseCode, result.CourseCode);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsStartMonthYear()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_request.StartMonthYear, result.StartMonthYear);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsEmployerAccountLegalEntityPublicHashedId()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_request.AccountLegalEntityHashedId, result.AccountLegalEntityHashedId);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsTransferSenderId()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_request.TransferSenderId, result.TransferSenderId);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsOrigin()
        {
            var result = await _mapper.Map(_request);

            Assert.AreEqual(_request.ReservationId.HasValue ? Origin.Reservations : Origin.Apprentices, result.Origin);
        }
    }
}