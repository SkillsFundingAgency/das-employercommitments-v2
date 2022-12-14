using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.Encoding;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.CommitmentsV2.Api.Client;
using System.Threading;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ApprenticeshipNeverStartedViewModelMapperTests
    {
        private Mock<ICommitmentsApiClient> mockCommitmentsApiClient;

        private readonly DateTime _referenceDate = DateTime.UtcNow;

        [SetUp]
        public void SetUp()
        {
            mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            mockCommitmentsApiClient
               .Setup(r => r.GetApprenticeship(It.IsAny<long>(), CancellationToken.None))
               .ReturnsAsync(GetApprenticeshipResponse(_referenceDate));
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipHashedId_IsMapped(ApprenticeshipNeverStartedRequest request)
        {
            var mapper = new ApprenticeshipNeverStartedViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(request.ApprenticeshipHashedId, result.ApprenticeshipHashedId);
        }

        [Test, MoqAutoData]
        public async Task AccountHashedId_IsMapped(ApprenticeshipNeverStartedRequest request)
        {
            var mapper = new ApprenticeshipNeverStartedViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
        }

        [Test, MoqAutoData]
        public async Task ApprenticeshipId_IsMapped(ApprenticeshipNeverStartedRequest request)
        {
            var mapper = new ApprenticeshipNeverStartedViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(request.ApprenticeshipId, result.ApprenticeshipId);
        }


        [Test, MoqAutoData]
        public async Task StartDate_IsMapped(ApprenticeshipNeverStartedRequest request)
        {
            var mapper = new ApprenticeshipNeverStartedViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(result.PlannedStartDate, _referenceDate);
        }

        [Test, MoqAutoData]
        public async Task IsCopJourney_IsAlwaysFalse(ApprenticeshipNeverStartedRequest request)
        {
            var mapper = new ApprenticeshipNeverStartedViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(result.IsCoPJourney, false);
        }
        [Test,MoqAutoData]
        public async Task StopMonth_IsSameAsPlannedDtartDateMonth(ApprenticeshipNeverStartedRequest request)
        {
            var mapper = new ApprenticeshipNeverStartedViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(result.StopMonth, _referenceDate.Month);
        }
        [Test, MoqAutoData]
        public async Task StopYear_IsSameAsPlannedDtartDateYear(ApprenticeshipNeverStartedRequest request)
        {
            var mapper = new ApprenticeshipNeverStartedViewModelMapper(mockCommitmentsApiClient.Object);
            var result = await mapper.Map(request);

            Assert.AreEqual(result.StopYear, _referenceDate.Year);
        }
        private static GetApprenticeshipResponse GetApprenticeshipResponse(DateTime referenceDate)
        {
            return new GetApprenticeshipResponse
            {
                FirstName = "FirstName",
                LastName = "LastName",
                Uln = "1234567890",
                CourseName = "Test Apprenticeship",
                StartDate = referenceDate,
            };
        }
    }
}
