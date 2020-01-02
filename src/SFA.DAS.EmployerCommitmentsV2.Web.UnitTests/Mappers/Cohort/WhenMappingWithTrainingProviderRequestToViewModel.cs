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
    public class WhenMappingWithTrainingProviderRequestToViewModel
    {
        [Test]
        public void OnlyTheCohortsWithTrainingProviderAreMapped()
        {
            var fixture = new WhenMappingWithTrainingProviderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_Only_TheCohorts_WithTrainingProvider_Are_Mapped();
        }

        [Test]
        public void Then_TheCohortReferenceIsMapped()
        {
            var fixture = new WhenMappingWithTrainingProviderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_CohortRefrence_Is_Mapped();
        }

        [Test]
        public void Then_ProviderNameIsMapped()
        {
            var fixture = new WhenMappingWithTrainingProviderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_ProviderName_Is_Mapped();
        }

        [Test]
        public void Then_NumberOfApprenticesAreMapped()
        {
            var fixture = new WhenMappingWithTrainingProviderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_NumberOfApprentices_Are_Mapped();
        }

        [Test]
        public void Then_LastMessage_IsMapped_Correctly()
        {
            var fixture = new WhenMappingWithTrainingProviderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_LastMessage_Is_MappedCorrectly();
        }

        [Test]
        public void Then_Cohort_OrderBy_OnDateCreated_Correctly()
        {
            var fixture = new WhenMappingWithTrainingProviderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_Ordered_By_DateCreated();
        }

        [Test]
        public void Then_AccountHashedId_IsMapped()
        {
            var fixture = new WhenMappingWithTrainingProviderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_AccountHashedId_IsMapped();
        }

        [Test]
        public void When_More_Than_One_Training_Provider_Title_IsMapped()
        {
            var fixture = new WhenMappingWithTrainingProviderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_When_More_Than_One_Training_Provider_Title_IsMapped();
        }

        [Test]
        public void When_Only_One_Training_Provider_Title_IsMapped()
        {
            var fixture = new WhenMappingWithTrainingProviderRequestToViewModelFixture();
            fixture.SetOnlyOneTrainingProvider();
            fixture.Map();

            fixture.Verify_When_One_Training_Provider_Title_IsMapped();
        }
    }

    public class WhenMappingWithTrainingProviderRequestToViewModelFixture
    {
        public Mock<IEncodingService> EncodingService { get; set; }
        public Mock<ICommitmentsApiClient> CommitmentsApiClient { get; set; }
        public CohortsByAccountRequest CohortsByAccountRequest { get; set; }
        public GetCohortsResponse GetCohortsResponse { get; set; }
        public WithTrainingProviderRequestViewModelMapper Mapper { get; set; }
        public WithTrainingProviderViewModel WithTrainingProviderViewModel { get; set; }

        public long AccountId => 1;

        public string AccountHashedId => "1AccountHashedId";

        public WhenMappingWithTrainingProviderRequestToViewModelFixture()
        {
            EncodingService = new Mock<IEncodingService>();
            CommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            CohortsByAccountRequest = new CohortsByAccountRequest() { AccountId = AccountId, AccountHashedId = AccountHashedId };
            GetCohortsResponse = CreateGetCohortsResponse();

            CommitmentsApiClient.Setup(c => c.GetCohorts(It.Is<GetCohortsRequest>(r => r.AccountId == AccountId), CancellationToken.None)).Returns(Task.FromResult(GetCohortsResponse));
            EncodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference)).Returns((long y, EncodingType z) => y + "_Encoded");

            Mapper = new WithTrainingProviderRequestViewModelMapper(CommitmentsApiClient.Object, EncodingService.Object);
        }

        public WhenMappingWithTrainingProviderRequestToViewModelFixture Map()
        {
            WithTrainingProviderViewModel = Mapper.Map(CohortsByAccountRequest).Result;
            return this;
        }

        public void Verify_Only_TheCohorts_WithTrainingProvider_Are_Mapped()
        {
            Assert.AreEqual(2, WithTrainingProviderViewModel.Cohorts.Count());

            Assert.IsNotNull(GetCohortInTrainingProviderViewModel(1));
            Assert.IsNotNull(GetCohortInTrainingProviderViewModel(2));
        }

        public void Verify_CohortRefrence_Is_Mapped()
        {
            EncodingService.Verify(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference), Times.Exactly(2));

            Assert.AreEqual("1_Encoded", GetCohortInTrainingProviderViewModel(1).CohortReference);
            Assert.AreEqual("2_Encoded", GetCohortInTrainingProviderViewModel(2).CohortReference);
        }

        public void Verify_ProviderName_Is_Mapped()
        {
            Assert.AreEqual("Provider1", GetCohortInTrainingProviderViewModel(1).ProviderName);
            Assert.AreEqual("Provider2", GetCohortInTrainingProviderViewModel(2).ProviderName);
        }

        public void Verify_NumberOfApprentices_Are_Mapped()
        {
            Assert.AreEqual(100, GetCohortInTrainingProviderViewModel(1).NumberOfApprentices);
            Assert.AreEqual(200, GetCohortInTrainingProviderViewModel(2).NumberOfApprentices);
        }

        public void Verify_LastMessage_Is_MappedCorrectly()
        {
            Assert.AreEqual("this is the last message from Employer", GetCohortInTrainingProviderViewModel(1).LastMessage);
            Assert.AreEqual("No message added", GetCohortInTrainingProviderViewModel(2).LastMessage);
        }

        public void Verify_Ordered_By_DateCreated()
        {
            Assert.AreEqual("1_Encoded", WithTrainingProviderViewModel.Cohorts.First().CohortReference);
            Assert.AreEqual("2_Encoded", WithTrainingProviderViewModel.Cohorts.Last().CohortReference);
        }

        public void Verify_AccountHashedId_IsMapped()
        {
            Assert.AreEqual(AccountHashedId, WithTrainingProviderViewModel.AccountHashedId);
        }

        public void Verify_When_More_Than_One_Training_Provider_Title_IsMapped()
        {
            Assert.AreEqual("Apprentice details with training providers", WithTrainingProviderViewModel.Title);
        }

        public void Verify_When_One_Training_Provider_Title_IsMapped()
        {
            Assert.AreEqual("Apprentice details with training provider", WithTrainingProviderViewModel.Title);
        }

        public void SetOnlyOneTrainingProvider()
        {
            foreach (var resp in GetCohortsResponse.Cohorts)
            {
                resp.ProviderId = 1;
                resp.ProviderName = "provider1";
            }
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
                    WithParty = Party.Provider,
                    CreatedOn = DateTime.Now.AddMinutes(-10),
                    LatestMessageFromEmployer = new Message("this is the last message from Employer", DateTime.Now.AddMinutes(-10))
                },
                new CohortSummary
                {
                    CohortId = 2,
                    AccountId = 1,
                    ProviderId = 2,
                    ProviderName = "Provider2",
                    NumberOfDraftApprentices = 200,
                    IsDraft = false,
                    WithParty = Party.Provider,
                    CreatedOn = DateTime.Now.AddMinutes(-8),
                    LatestMessageFromProvider = new Message("This is latestMessage from provider", DateTime.Now.AddMinutes(-8))
                },
                new CohortSummary
                {
                    CohortId = 3,
                    AccountId = 1,
                    ProviderId = 2,
                    ProviderName = "Provider3",
                    NumberOfDraftApprentices = 300,
                    IsDraft = false,
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
                    IsDraft = true,
                    WithParty = Party.Employer,
                    CreatedOn = DateTime.Now
                },
            };

            return new GetCohortsResponse(cohorts);
        }

        private long GetCohortId(string cohortReference)
        {
            return long.Parse(cohortReference.Replace("_Encoded", ""));
        }

        private WithTrainingProviderCohortSummaryViewModel GetCohortInTrainingProviderViewModel(long id)
        {
            return WithTrainingProviderViewModel.Cohorts.FirstOrDefault(x => GetCohortId(x.CohortReference) == id);
        }
    }
}
