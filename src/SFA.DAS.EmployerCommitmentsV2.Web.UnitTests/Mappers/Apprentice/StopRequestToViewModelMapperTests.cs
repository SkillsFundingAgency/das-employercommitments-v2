using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class StopRequestToViewModelMapperTests
    {
        private Mock<ICommitmentsApiClient> mockCommitmentsApiClient;
        private DateTime StartDate = DateTime.Now.AddYears(-1);

        [SetUp]
        public void SetUp()
        {
            mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            mockCommitmentsApiClient
                .Setup(r => r.GetApprenticeship(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(GetApprenticeshipResponse());
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(StopRequest request)
        {
            var mapper = new StopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.That(result.ApprenticeshipHashedId, Is.EqualTo(request.ApprenticeshipHashedId));
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(StopRequest request)
        {
            var mapper = new StopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.That(result.AccountHashedId, Is.EqualTo(request.AccountHashedId));
        }


        [Test, MoqAutoData]
        public async Task IsCoPJourney_IsMapped(StopRequest request)
        {
            var mapper = new StopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.That(result.IsCoPJourney, Is.EqualTo(request.IsCoPJourney));
        }

        [Test, MoqAutoData]
        public async Task StartDate_IsMapped(StopRequest request)
        {
            var mapper = new StopRequestToViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.That(result.StartDate, Is.EqualTo(StartDate));
        }

        private GetApprenticeshipResponse GetApprenticeshipResponse()
        {
            return new GetApprenticeshipResponse
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Uln = "1234567890",
                CourseName = "Test Apprenticeship",
                StartDate = StartDate
            };
        }
    }
}
