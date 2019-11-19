using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class DraftRequestMapperTests
    {

        [Test, MoqAutoData]
        public async Task ThenCallsCommitmentsApiClient(
            [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient,
            DraftRequest request,
            DraftRequestMapper mapper)
        {
            await mapper.Map(request);

            commitmentsApiClient.Verify(x => x.GetCohorts(It.Is<GetCohortsRequest>(y => y.AccountId == request.AccountId), CancellationToken.None));
        }

        [Test, MoqAutoData]
        public async Task ThenApiResponseIsFiltered(
            [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient,
            DraftRequest request,
            [Frozen] long accountId,
            List<CohortSummary> cohortList,
            DraftRequestMapper mapper)
        {
            cohortList.Add(new CohortSummary{ AccountId = accountId, WithParty = Party.Employer, IsDraft = true});
            var expectedItems = cohortList.Where(x => x.WithParty == Party.Employer && x.IsDraft);
            var response = new GetCohortsResponse(cohortList);
            commitmentsApiClient
                .Setup(x => x.GetCohorts(It.Is<GetCohortsRequest>(y => y.AccountId == request.AccountId), CancellationToken.None))
                .ReturnsAsync(response);

            var result = await mapper.Map(request);

            Assert.AreEqual(expectedItems.Count(), result.Cohorts.Count());
            Assert.True(result.Cohorts.Any(x => x.AccountId == accountId));
            Assert.False(result.Cohorts.Any(x => x.IsDraft == false || x.WithParty != Party.Employer));

        }

        [Test, MoqAutoData]
        public async Task ThenLinkGeneratorIsCalled(
            [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient,
            DraftRequest request,
            List<CohortSummary> cohortList,
            [Frozen] Mock<ILinkGenerator> linkGenerator,
            DraftRequestMapper mapper)
        {
            var response = new GetCohortsResponse(cohortList);
            commitmentsApiClient
                .Setup(x => x.GetCohorts(It.Is<GetCohortsRequest>(y => y.AccountId == request.AccountId), CancellationToken.None))
                .ReturnsAsync(response);

            await mapper.Map(request);

            linkGenerator.Verify(x => x.CommitmentsLink(It.IsAny<string>()));

        }

        [Test, MoqAutoData]
        public async Task ThenMapsRequestToViewModel(
            [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient,
            DraftRequest request,
            List<CohortSummary> cohortList,
            DraftRequestMapper mapper)
        {
            var response = new GetCohortsResponse(cohortList);
            var expectedItems = cohortList.Where(x => x.WithParty == Party.Employer && x.IsDraft);
            commitmentsApiClient
                .Setup(x => x.GetCohorts(It.Is<GetCohortsRequest>(y => y.AccountId == request.AccountId), CancellationToken.None))
                .ReturnsAsync(response);

            var result = await mapper.Map(request);

            Assert.AreEqual(request.AccountId,result.AccountId);
            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
            Assert.AreEqual(expectedItems, result.Cohorts);
            Assert.False(string.IsNullOrWhiteSpace(result.BackLink));
        }

    }
}
