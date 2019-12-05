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
using SFA.DAS.EmployerUrlHelper;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class WhenMappingWithTransferSenderRequestToViewModel
    {
        [Test]
        public void OnlyTheCohortsWithTransferSenderAreMapped()
        {
            var fixture = new WhenMappingWithTransferSenderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_Only_TheCohorts_WithTransferSender_Are_Mapped();
        }

        [Test]
        public void Then_TheCohortReferenceIsMapped()
        {
            var fixture = new WhenMappingWithTransferSenderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_CohortRefrence_Is_Mapped();
        }

        [Test]
        public void Then_ProviderNameIsMapped()
        {
            var fixture = new WhenMappingWithTransferSenderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_ProviderName_Is_Mapped();
        }

        [Test]
        public void Then_TransferSenderNameIsMapped()
        {
            var fixture = new WhenMappingWithTransferSenderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_TransferSenderName_Is_Mapped();
        }

        [Test]
        public void Then_TransferSenderIdIsMapped()
        {
            var fixture = new WhenMappingWithTransferSenderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_TransferSenderId_Is_Mapped();
        }

        [Test]
        public void Then_NumberOfApprenticesAreMapped()
        {
            var fixture = new WhenMappingWithTransferSenderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_NumberOfApprentices_Are_Mapped();
        }

        [Test]
        public void Then_OrderBy_OnDateTransfered_Correctly()
        {
            var fixture = new WhenMappingWithTransferSenderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_Ordered_By_OnDateTransfered();
        }

        [Test]
        public void Then_AccountHashedId_IsMapped()
        {
            var fixture = new WhenMappingWithTransferSenderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_AccountHashedId_IsMapped();
        }

        [Test]
        public void Then_BacklinkUrl_IsMapped()
        {
            var fixture = new WhenMappingWithTransferSenderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_BackLinkUrl_Is_Mapped();
        }

        [Test]
        public void When_More_Than_One_TransferSender_Title_IsMapped()
        {
            var fixture = new WhenMappingWithTransferSenderRequestToViewModelFixture();
            fixture.Map();

            fixture.Verify_When_More_Than_One_TransferSender_Title_Is_Mapped();
        }

        [Test]
        public void When_Only_One_TransferSender_Title_IsMapped()
        {
            var fixture = new WhenMappingWithTransferSenderRequestToViewModelFixture();
            fixture.SetOnlyOneTransferSender();
            fixture.Map();

            fixture.Verify_When_One_TransferSender_Title_Is_Mapped();
        }
    }

    public class WhenMappingWithTransferSenderRequestToViewModelFixture
    {
        public Mock<IEncodingService> EncodingService { get; set; }
        public Mock<ICommitmentsApiClient> CommitmentsApiClient { get; set; }
        public Mock<ILinkGenerator> LinkGenerator { get; set; }
        public WithTransferSenderRequest WithTransferSenderRequest { get; set; }
        public GetCohortsResponse GetCohortsResponse { get; set; }
        public WithTransferSenderRequestViewModelMapper Mapper { get; set; }
        public WithTransferSenderViewModel WithTransferSenderViewModel { get; set; }

        public long AccountId => 1;

        public string AccountHashedId => "1AccountHashedId";

        public WhenMappingWithTransferSenderRequestToViewModelFixture()
        {
            EncodingService = new Mock<IEncodingService>();
            CommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            LinkGenerator = new Mock<ILinkGenerator>();

            WithTransferSenderRequest = new WithTransferSenderRequest() { AccountId = AccountId, AccountHashedId = AccountHashedId };
            GetCohortsResponse = CreateGetCohortsResponse();

            CommitmentsApiClient.Setup(c => c.GetCohorts(It.Is<GetCohortsRequest>(r => r.AccountId == AccountId), CancellationToken.None)).Returns(Task.FromResult(GetCohortsResponse));
            EncodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference)).Returns((long y, EncodingType z) => y + "_Encoded");
            LinkGenerator.Setup(x => x.CommitmentsLink($"accounts/{AccountHashedId}/apprentices/cohorts")).Returns("BackLinkUrl");

            Mapper = new WithTransferSenderRequestViewModelMapper(CommitmentsApiClient.Object, EncodingService.Object, LinkGenerator.Object);
        }

        public WhenMappingWithTransferSenderRequestToViewModelFixture Map()
        {
            WithTransferSenderViewModel = Mapper.Map(WithTransferSenderRequest).Result;
            return this;
        }

        public void Verify_Only_TheCohorts_WithTransferSender_Are_Mapped()
        {
            Assert.AreEqual(2, WithTransferSenderViewModel.Cohorts.Count());

            Assert.IsNotNull(GetCohortInTransferSenderViewModel(1));
            Assert.IsNotNull(GetCohortInTransferSenderViewModel(2));
        }

        public void Verify_CohortRefrence_Is_Mapped()
        {
            EncodingService.Verify(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference), Times.Exactly(2));

            Assert.AreEqual("1_Encoded", GetCohortInTransferSenderViewModel(1).CohortReference);
            Assert.AreEqual("2_Encoded", GetCohortInTransferSenderViewModel(2).CohortReference);
        }

        public void Verify_ProviderName_Is_Mapped()
        {
            Assert.AreEqual("Provider1", GetCohortInTransferSenderViewModel(1).ProviderName);
            Assert.AreEqual("Provider2", GetCohortInTransferSenderViewModel(2).ProviderName);
        }

        public void Verify_TransferSenderName_Is_Mapped()
        {
            Assert.AreEqual("TransferSender1", GetCohortInTransferSenderViewModel(1).TransferSenderName);
            Assert.AreEqual("TransferSender2", GetCohortInTransferSenderViewModel(2).TransferSenderName);
        }

        public void Verify_TransferSenderId_Is_Mapped()
        {
            Assert.AreEqual(1, GetCohortInTransferSenderViewModel(1).TransferSenderId);
            Assert.AreEqual(2, GetCohortInTransferSenderViewModel(2).TransferSenderId);
        }

        public void Verify_NumberOfApprentices_Are_Mapped()
        {
            Assert.AreEqual(100, GetCohortInTransferSenderViewModel(1).NumberOfApprentices);
            Assert.AreEqual(200, GetCohortInTransferSenderViewModel(2).NumberOfApprentices);
        }

        public void Verify_Ordered_By_OnDateTransfered()
        {
            Assert.AreEqual("1_Encoded", WithTransferSenderViewModel.Cohorts.First().CohortReference);
            Assert.AreEqual("2_Encoded", WithTransferSenderViewModel.Cohorts.Last().CohortReference);
        }

        public void Verify_BackLinkUrl_Is_Mapped()
        {
            LinkGenerator.Verify(x => x.CommitmentsLink($"accounts/{AccountHashedId}/apprentices/cohorts"), Times.Once);
            Assert.AreEqual("BackLinkUrl", WithTransferSenderViewModel.BackLink);
        }

        public void Verify_AccountHashedId_IsMapped()
        {
            Assert.AreEqual(AccountHashedId, WithTransferSenderViewModel.AccountHashedId);
        }

        public void Verify_When_More_Than_One_TransferSender_Title_Is_Mapped()
        {
            Assert.AreEqual(WithTransferSenderRequestViewModelMapper.Title + "s", WithTransferSenderViewModel.Title);
        }

        public void Verify_When_One_TransferSender_Title_Is_Mapped()
        {
            Assert.AreEqual(WithTransferSenderRequestViewModelMapper.Title, WithTransferSenderViewModel.Title);
        }

        public void SetOnlyOneTransferSender()
        {
            foreach (var resp in GetCohortsResponse.Cohorts)
            {
                resp.TransferSenderId = 1;
                resp.TransferSenderName = "TransferSender1";
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
                    TransferSenderId = 1,
                    TransferSenderName = "TransferSender1",
                    ProviderName = "Provider1",
                    NumberOfDraftApprentices = 100,
                    IsDraft = false,
                    WithParty = Party.TransferSender,
                    LatestMessageFromEmployer = new Message("this is the last message from Employer", DateTime.Now.AddMinutes(-10)),
                    LatestMessageFromProvider = new Message("This is latestMessage from provider", DateTime.Now.AddMinutes(-11))
                },
                new CohortSummary
                {
                    CohortId = 2,
                    AccountId = 1,
                    ProviderId = 2,
                    TransferSenderId = 2,
                    TransferSenderName = "TransferSender2",
                    ProviderName = "Provider2",
                    NumberOfDraftApprentices = 200,
                    IsDraft = false,
                    WithParty = Party.TransferSender,
                    CreatedOn = DateTime.Now.AddMinutes(-8),
                    LatestMessageFromProvider = new Message("This is latestMessage from provider", DateTime.Now.AddMinutes(-8)),
                    LatestMessageFromEmployer = new Message("This is latestMessage from Employer", DateTime.Now.AddMinutes(-7))
                },
                new CohortSummary
                {
                    CohortId = 3,
                    AccountId = 1,
                    ProviderId = 2,
                    TransferSenderId = 2,
                    TransferSenderName = "TransferSender2",
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

        private WithTransferSenderCohortSummaryViewModel GetCohortInTransferSenderViewModel(long id)
        {
            return WithTransferSenderViewModel.Cohorts.FirstOrDefault(x => GetCohortId(x.CohortReference) == id);
        }
    }
}
