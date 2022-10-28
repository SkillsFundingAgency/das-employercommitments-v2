using AutoFixture;
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
    public class ConfirmWhenApprenticeshipStoppedViewModelMapperTests
    {
        private Mock<ICommitmentsApiClient> mockCommitmentsApiClient;
        private GetApprenticeshipResponse ApprenticeshipDetails;

        [SetUp]
        public void SetUp()
        {
            var fixture = new Fixture();

            mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            ApprenticeshipDetails = fixture.Create<GetApprenticeshipResponse>();

            mockCommitmentsApiClient
                .Setup(r => r.GetApprenticeship(It.IsAny<long>(), CancellationToken.None))
                .ReturnsAsync(() => ApprenticeshipDetails);
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(ConfirmWhenApprenticeshipStoppedRequest request)
        {
            var mapper = new ConfirmWhenApprenticeshipStoppedViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(request.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(ConfirmWhenApprenticeshipStoppedRequest request)
        {
            var mapper = new ConfirmWhenApprenticeshipStoppedViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task ApprenticeName_IsMapped(ConfirmWhenApprenticeshipStoppedRequest request)
        {
            var mapper = new ConfirmWhenApprenticeshipStoppedViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual($"{ApprenticeshipDetails.FirstName} {ApprenticeshipDetails.LastName}", result.ApprenticeName);
        }

        [Test, MoqAutoData]
        public async Task CourseName_IsMapped(ConfirmWhenApprenticeshipStoppedRequest request)
        {
            var mapper = new ConfirmWhenApprenticeshipStoppedViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(ApprenticeshipDetails.CourseName, result.Course);
        }

        [Test, MoqAutoData]
        public async Task ULN_IsMapped(ConfirmWhenApprenticeshipStoppedRequest request)
        {
            var mapper = new ConfirmWhenApprenticeshipStoppedViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(ApprenticeshipDetails.Uln, result.ULN);
        }

        [Test, MoqAutoData]
        public async Task StopDate_IsMapped(ConfirmWhenApprenticeshipStoppedRequest request)
        {
            var mapper = new ConfirmWhenApprenticeshipStoppedViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(ApprenticeshipDetails.StopDate, result.StopDate);
        }
    }
}