using System.Linq;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Encoding;
using System;
using SFA.DAS.CommitmentsV2.Types;
using System.Collections.Generic;
using SFA.DAS.CommitmentsV2.Api.Client;
using System.Threading;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class WhenMappingDraftRequestToViewModel
    {
        [Test]
        public void OnlyTheCohortsReadyForDraftAreMapped()
        {
            var fixture = new WhenMappingDraftRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_OnlyTheDraftCohorts_WithEmployer_Are_Mapped();
        }

        [Test]
        public void Then_TheCohortReferenceIsMapped()
        {
            var fixture = new WhenMappingDraftRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_CohortRefrence_Is_Mapped();
        }

        [Test]
        public void Then_ProviderNameIsMapped()
        {
            var fixture = new WhenMappingDraftRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_ProviderName_Is_Mapped();
        }

        [Test]
        public void Then_NumberOfApprenticesAreMapped()
        {
            var fixture = new WhenMappingDraftRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_NumberOfApprentices_Are_Mapped();
        }

        [Test]
        public void Then_Cohort_OrderBy_OnDateCreated_Correctly()
        {
            var fixture = new WhenMappingDraftRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_Ordered_By_DateCreated();
        }

        [Test]
        public void Then_AccountHashedId_IsMapped()
        {
            var fixture = new WhenMappingDraftRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_AccountHashedId_IsMapped();
        }
    }

    public class WhenMappingDraftRequestToViewModelFixture
    {
        public Mock<IEncodingService> EncodingService { get; set; }
        public Mock<ICommitmentsApiClient> CommitmentsApiClient { get; set; }
        public CohortsByAccountRequest DraftRequest { get; set; }
        public GetCohortsResponse GetCohortsResponse { get; set; }
        public DraftRequestMapper Mapper { get; set; }
        public DraftViewModel DraftViewModel { get; set; }

        public long AccountId => 1;

        public string AccountHashedId => "1AccountHashedId";

        public WhenMappingDraftRequestToViewModelFixture()
        {
            EncodingService = new Mock<IEncodingService>();
            CommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            DraftRequest = new CohortsByAccountRequest() { AccountId = AccountId, AccountHashedId = AccountHashedId };
            GetCohortsResponse = CreateGetCohortsResponse();

            CommitmentsApiClient.Setup(c => c.GetCohorts(It.Is<GetCohortsRequest>(r => r.AccountId == AccountId), CancellationToken.None)).Returns(Task.FromResult(GetCohortsResponse));
            EncodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference)).Returns((long y, EncodingType z) => y + "_Encoded");

            Mapper = new DraftRequestMapper(CommitmentsApiClient.Object, EncodingService.Object);
        }

        public WhenMappingDraftRequestToViewModelFixture Map()
        {
            DraftViewModel = Mapper.Map(DraftRequest).Result;
            return this;
        }

        public void Verify_OnlyTheDraftCohorts_WithEmployer_Are_Mapped()
        {
            Assert.AreEqual(2, DraftViewModel.Cohorts.Count());

            Assert.IsNotNull(GetDraftCohortWithId(2));
            Assert.IsNotNull(GetDraftCohortWithId(3));
        }

        public void Verify_CohortRefrence_Is_Mapped()
        {
            EncodingService.Verify(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference), Times.Exactly(2));

            Assert.AreEqual("2_Encoded", GetDraftCohortWithId(2).CohortReference);
            Assert.AreEqual("3_Encoded", GetDraftCohortWithId(3).CohortReference);
        }

        public void Verify_ProviderName_Is_Mapped()
        {
            Assert.AreEqual("Provider2", GetDraftCohortWithId(2).ProviderName);
            Assert.AreEqual("Provider3", GetDraftCohortWithId(3).ProviderName);
        }

        public void Verify_NumberOfApprentices_Are_Mapped()
        {
            Assert.AreEqual(200, GetDraftCohortWithId(2).NumberOfApprentices);
            Assert.AreEqual(300, GetDraftCohortWithId(3).NumberOfApprentices);
        }

        public void Verify_Ordered_By_DateCreated()
        {
            Assert.AreEqual("3_Encoded", DraftViewModel.Cohorts.First().CohortReference);
            Assert.AreEqual("2_Encoded", DraftViewModel.Cohorts.Last().CohortReference);
        }

        public void Verify_AccountHashedId_IsMapped()
        {
            Assert.AreEqual(AccountHashedId, DraftViewModel.AccountHashedId);
        }

        private GetCohortsResponse CreateGetCohortsResponse()
        {
            IEnumerable<CohortSummary> cohorts = new List<CohortSummary>()
            {
                new CohortSummary
                {
                    CohortId = 1,
                    AccountId = 1,
                    ProviderId = 1,
                    ProviderName = "Provider1",
                    NumberOfDraftApprentices = 100,
                    IsDraft = false,
                    WithParty = Party.Employer,
                    CreatedOn = DateTime.Now.AddMinutes(-3)
                },
                new CohortSummary
                {
                    CohortId = 2,
                    AccountId = 1,
                    ProviderId = 2,
                    ProviderName = "Provider2",
                    NumberOfDraftApprentices = 200,
                    IsDraft = true,
                    WithParty = Party.Employer,
                    CreatedOn = DateTime.Now.AddMinutes(-5),
                },
                new CohortSummary
                {
                    CohortId = 3,
                    AccountId = 1,
                    ProviderId = 3,
                    ProviderName = "Provider3",
                    NumberOfDraftApprentices = 300,
                    IsDraft = true,
                    WithParty = Party.Employer,
                    CreatedOn = DateTime.Now.AddMinutes(-1)
                },
                 new CohortSummary
                {
                    CohortId = 4,
                    AccountId = 1,
                    ProviderId = 4,
                    ProviderName = "Provider4",
                    NumberOfDraftApprentices = 400,
                    IsDraft = false,
                    WithParty = Party.Provider,
                    CreatedOn = DateTime.Now
                },
            };

            return new GetCohortsResponse(cohorts);
        }

        private long GetCohortId(string cohortReference)
        {
            return long.Parse(cohortReference.Replace("_Encoded", ""));
        }

        private DraftCohortSummaryViewModel GetDraftCohortWithId(long id)
        {
            return DraftViewModel.Cohorts.FirstOrDefault(x => GetCohortId(x.CohortReference) == id);
        }
    }
}
