using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship
{
    [TestFixture]
    public class DeleteDraftApprenticeshipViewModelMapperTests 
    {
        [Test, MoqAutoData]
        public async Task ThenCallsCommitmentsApiToGetCohort(
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            GetCohortResponse cohortResponse,
            DeleteApprenticeshipRequest request,
            DeleteDraftApprenticeshipViewModelMapper mapper)
        {
            cohortResponse.WithParty = Party.Employer;
            mockApiClient
                .Setup(x => x.GetCohort(request.CohortId, CancellationToken.None))
                .ReturnsAsync(cohortResponse);

            await mapper.Map(request);

            mockApiClient.Verify(x => x.GetCohort(request.CohortId, CancellationToken.None),Times.Once);
        }

        [Test, MoqAutoData]
        public void WhenCohortIsNotWithEmployer_ThenThrowsException(
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            GetCohortResponse cohortResponse,
            DeleteApprenticeshipRequest request,
            DeleteDraftApprenticeshipViewModelMapper mapper)
        {
            cohortResponse.WithParty = Party.Provider;
            mockApiClient
                .Setup(x => x.GetCohort(request.CohortId, CancellationToken.None))
                .ReturnsAsync(cohortResponse);

            Assert.ThrowsAsync<CohortEmployerUpdateDeniedException>(  async () => await mapper.Map(request));
        }

        [Test, MoqAutoData]
        public async Task ThenCallsCommitmentsApiToGetDraftApprenticeship(
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            GetCohortResponse cohortResponse,
            GetDraftApprenticeshipResponse draftApprenticeshipResponse,
            DeleteApprenticeshipRequest request,
            DeleteDraftApprenticeshipViewModelMapper mapper)
        {
            cohortResponse.WithParty = Party.Employer;
            mockApiClient
                .Setup(x => x.GetCohort(request.CohortId, CancellationToken.None))
                .ReturnsAsync(cohortResponse);
            mockApiClient
                .Setup(x => x.GetDraftApprenticeship(request.CohortId, request.DraftApprenticeshipId, CancellationToken.None))
                .ReturnsAsync(draftApprenticeshipResponse);

            await mapper.Map(request);

            mockApiClient.Verify(x => x.GetDraftApprenticeship(request.CohortId, request.DraftApprenticeshipId, CancellationToken.None),Times.Once);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsRequestValuesToViewModel(
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            GetCohortResponse cohortResponse,
            GetDraftApprenticeshipResponse draftApprenticeshipResponse,
            DeleteApprenticeshipRequest request,
            DeleteDraftApprenticeshipViewModelMapper mapper)
        {
            var expectedFullName = $"{draftApprenticeshipResponse.FirstName} {draftApprenticeshipResponse.LastName}";
            cohortResponse.WithParty = Party.Employer;
            mockApiClient
                .Setup(x => x.GetCohort(request.CohortId, CancellationToken.None))
                .ReturnsAsync(cohortResponse);
            mockApiClient
                .Setup(x => x.GetDraftApprenticeship(request.CohortId, request.DraftApprenticeshipId, CancellationToken.None))
                .ReturnsAsync(draftApprenticeshipResponse);

            var result = await mapper.Map(request);
            Assert.AreEqual(draftApprenticeshipResponse.FirstName, result.FirstName);
            Assert.AreEqual(draftApprenticeshipResponse.LastName, result.LastName);
            Assert.AreEqual(expectedFullName, result.FullName );
            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
            Assert.AreEqual(request.DraftApprenticeshipHashedId, result.DraftApprenticeshipHashedId);
            Assert.AreEqual(request.AccountHashedId, result.AccountHashedId);
            Assert.AreEqual(request.Origin, result.Origin);
            Assert.AreEqual(request.CohortReference, result.CohortReference);

        }



    }
}
