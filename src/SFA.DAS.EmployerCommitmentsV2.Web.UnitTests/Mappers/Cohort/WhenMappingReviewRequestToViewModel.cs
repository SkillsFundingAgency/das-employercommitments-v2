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
    public class WhenMappingReviewRequestToViewModel
    {
        [Test]
        public void OnlyTheCohortsReadyForReviewAreMapped()
        {
            var fixture = new WhenMappingReviewRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_OnlyTheCohorts_ReadyForReviewForEmployer_Are_Mapped();
        }

        [Test]
        public void Then_TheCohortReferenceIsMapped()
        {
            var fixture = new WhenMappingReviewRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_CohortReference_Is_Mapped();
        }

        [Test]
        public void Then_ProviderNameIsMapped()
        {
            var fixture = new WhenMappingReviewRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_ProviderName_Is_Mapped();
        }

        [Test]
        public void Then_NumberOfApprenticesAreMapped()
        {
            var fixture = new WhenMappingReviewRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_NumberOfApprentices_Are_Mapped();
        }

        [Test]
        public void Then_LastMessage_IsMapped_Correctly()
        {
            var fixture = new WhenMappingReviewRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_LastMessage_Is_MappedCorrectly();
        }

        [Test]
        public void Then_Cohort_OrderBy_OnDateCreated_Correctly()
        {
            var fixture = new WhenMappingReviewRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_Ordered_By_DateCreated();
        }

        [Test]
        public void Then_AccountHashedId_IsMapped()
        {
            var fixture = new WhenMappingReviewRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_AccountHashedId_IsMapped();
        }
    }

    public class WhenMappingReviewRequestToViewModelFixture
    {
        public Mock<IEncodingService> EncodingService { get; set; }
        public Mock<ICommitmentsApiClient> CommitmentsApiClient { get; set; }
        public CohortsByAccountRequest ReviewRequest { get; set; }
        public GetCohortsResponse GetCohortsResponse { get; set; }
        public ReviewRequestViewModelMapper Mapper { get; set; }
        public ReviewViewModel ReviewViewModel { get; set; }

        public long AccountId => 1;

        public string AccountHashedId => "1AccountHashedId";

        public WhenMappingReviewRequestToViewModelFixture()
        {
            EncodingService = new Mock<IEncodingService>();
            CommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            ReviewRequest = new CohortsByAccountRequest() { AccountId = AccountId, AccountHashedId = AccountHashedId };
            GetCohortsResponse = CreateGetCohortsResponse();
           
            CommitmentsApiClient.Setup(c => c.GetCohorts(It.Is<GetCohortsRequest>(r => r.AccountId == AccountId), CancellationToken.None)).Returns(Task.FromResult(GetCohortsResponse));
            EncodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference)).Returns((long y, EncodingType z) => y + "_Encoded");

            Mapper = new ReviewRequestViewModelMapper(CommitmentsApiClient.Object, EncodingService.Object);
        }

        public WhenMappingReviewRequestToViewModelFixture Map()
        {
            ReviewViewModel = Mapper.Map(ReviewRequest).Result;
            return this;
        }

        public void Verify_OnlyTheCohorts_ReadyForReviewForEmployer_Are_Mapped()
        {
            Assert.AreEqual(2, ReviewViewModel.Cohorts.Count());

            Assert.IsNotNull(GetCohortInReviewViewModel(1));
            Assert.IsNotNull(GetCohortInReviewViewModel(2));
        }

        public void Verify_CohortReference_Is_Mapped()
        {
            EncodingService.Verify(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference), Times.Exactly(2));

            Assert.AreEqual("1_Encoded", GetCohortInReviewViewModel(1).CohortReference);
            Assert.AreEqual("2_Encoded", GetCohortInReviewViewModel(2).CohortReference);
        }

        public void Verify_ProviderName_Is_Mapped()
        {
            Assert.AreEqual("Provider1", GetCohortInReviewViewModel(1).ProviderName);
            Assert.AreEqual("Provider2", GetCohortInReviewViewModel(2).ProviderName);
        }

        public void Verify_NumberOfApprentices_Are_Mapped()
        {
            Assert.AreEqual(100, GetCohortInReviewViewModel(1).NumberOfApprentices);
            Assert.AreEqual(200, GetCohortInReviewViewModel(2).NumberOfApprentices);
        }

        public void Verify_LastMessage_Is_MappedCorrectly()
        {
            Assert.AreEqual("No message added", GetCohortInReviewViewModel(1).LastMessage);
            Assert.AreEqual("This is latestMessage from provider", GetCohortInReviewViewModel(2).LastMessage);
        }

        public void Verify_Ordered_By_DateCreated()
        {
            Assert.AreEqual("2_Encoded",ReviewViewModel.Cohorts.First().CohortReference);
            Assert.AreEqual("1_Encoded", ReviewViewModel.Cohorts.Last().CohortReference);
        }

        public void Verify_AccountHashedId_IsMapped()
        {
            Assert.AreEqual(AccountHashedId, ReviewViewModel.AccountHashedId);
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
                    IsDraft = false,
                    WithParty = Party.Employer,
                    CreatedOn = DateTime.Now.AddMinutes(-5),
                    LatestMessageFromProvider = new Message("This is latestMessage from provider", DateTime.Now.AddMinutes(-2))
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
            return long.Parse(cohortReference.Replace("_Encoded",""));
        }

        private ReviewCohortSummaryViewModel GetCohortInReviewViewModel(long id)
        {
            return ReviewViewModel.Cohorts.FirstOrDefault(x => GetCohortId(x.CohortReference) == id);
        }
    }
}
