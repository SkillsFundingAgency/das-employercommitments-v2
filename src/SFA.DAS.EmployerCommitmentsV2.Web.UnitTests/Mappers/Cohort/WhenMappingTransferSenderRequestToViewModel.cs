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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class WhenMappingTransferSenderRequestToViewModel
    {
        WhenMappingTransferSenderRequestToViewModelFixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new WhenMappingTransferSenderRequestToViewModelFixture();
        }

        [Test]
        public void OnlyTheCohortsWithTransferSenderAreMapped()
        {
            _fixture.Map();
            _fixture.Verify_Only_TheCohorts_WithTransferSender_Are_Mapped();
        }

        [Test]
        public void Then_TheCohortReferenceIsMapped()
        {
            _fixture.Map();
            _fixture.Verify_CohortReference_Is_Mapped();
        }

        [Test]
        public void Then_ProviderNameIsMapped()
        {
            _fixture.Map();
            _fixture.Verify_ProviderName_Is_Mapped();
        }

        [Test]
        public void Then_TransferSenderNameIsMapped()
        {
            _fixture.Map();
            _fixture.Verify_TransferSenderName_Is_Mapped();
        }

        [Test]
        public void Then_TransferSenderIdIsMapped()
        {
            _fixture.Map();
            _fixture.Verify_TransferSenderId_Is_Mapped();
        }

        [Test]
        public void Then_NumberOfApprenticesAreMapped()
        {
            _fixture.Map();
            _fixture.Verify_NumberOfApprentices_Are_Mapped();
        }

        [Test]
        public void Then_OrderBy_OnDateTransfered_Correctly()
        {
            _fixture.Map();
            _fixture.Verify_Ordered_By_OnDateTransfered();
        }

        [Test]
        public void Then_OrderBy_OnDateCreated_Correctly()
        {
            _fixture.MakeTheMessagesNull().SetCreatedOn();
            _fixture.Map();
            _fixture.Verify_Ordered_By_OnDateCreated();
        }

        [Test]
        public void Then_OrderBy_LatestMessageByEmployer_Correctly()
        {
            _fixture.MakeTheMessagesNull().SetLatestMessageFromEmployer();
            _fixture.Map();
            _fixture.Verify_Ordered_By_LatestMessageByEmployer();
        }

        [Test]
        public void Then_OrderBy_LatestMessageByProvider_Correctly()
        {
            _fixture.MakeTheMessagesNull().SetLatestMessageFromProvider();
            _fixture.Map();
            _fixture.Verify_Ordered_By_LatestMessageByProvider();
        }

        [Test]
        public void Then_AccountHashedId_IsMapped()
        {
            _fixture.Map();
            _fixture.Verify_AccountHashedId_IsMapped();
        }

        [Test]
        public void When_More_Than_One_TransferSender_Title_IsMapped()
        {
            _fixture.Map();
            _fixture.Verify_When_More_Than_One_TransferSender_Title_Is_Mapped();
        }

        [Test]
        public void When_Only_One_TransferSender_Title_IsMapped()
        {
            _fixture.SetOnlyOneTransferSender();
            _fixture.Map();
            _fixture.Verify_When_One_TransferSender_Title_Is_Mapped();
        }
    }

    public class WhenMappingTransferSenderRequestToViewModelFixture
    {
        public Mock<IEncodingService> EncodingService { get; set; }
        public Mock<ICommitmentsApiClient> CommitmentsApiClient { get; set; }
        public CohortsByAccountRequest CohortsByAccountRequest { get; set; }
        public GetCohortsResponse GetCohortsResponse { get; set; }
        public WithTransferSenderRequestViewModelMapper Mapper { get; set; }
        public WithTransferSenderViewModel WithTransferSenderViewModel { get; set; }

        public long AccountId => 1;

        public string AccountHashedId => "1AccountHashedId";

        public WhenMappingTransferSenderRequestToViewModelFixture()
        {
            EncodingService = new Mock<IEncodingService>();
            CommitmentsApiClient = new Mock<ICommitmentsApiClient>();

            CohortsByAccountRequest = new CohortsByAccountRequest() { AccountId = AccountId, AccountHashedId = AccountHashedId };
            GetCohortsResponse = CreateGetCohortsResponse();

            CommitmentsApiClient.Setup(c => c.GetCohorts(It.Is<GetCohortsRequest>(r => r.AccountId == AccountId), CancellationToken.None)).ReturnsAsync(GetCohortsResponse);
            EncodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference)).Returns((long y, EncodingType z) => y + "_Encoded");

            Mapper = new WithTransferSenderRequestViewModelMapper(CommitmentsApiClient.Object, EncodingService.Object);
        }

        public WhenMappingTransferSenderRequestToViewModelFixture Map()
        {
            WithTransferSenderViewModel = Mapper.Map(CohortsByAccountRequest).Result;
            return this;
        }

        public void Verify_Only_TheCohorts_WithTransferSender_Are_Mapped()
        {
            Assert.AreEqual(2, WithTransferSenderViewModel.Cohorts.Count());

            Assert.IsNotNull(GetCohortInTransferSenderViewModel(1));
            Assert.IsNotNull(GetCohortInTransferSenderViewModel(2));
        }

        public void Verify_CohortReference_Is_Mapped()
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

        public void Verify_Ordered_By_OnDateCreated()
        {
            Assert.AreEqual("2_Encoded", WithTransferSenderViewModel.Cohorts.First().CohortReference);
            Assert.AreEqual("1_Encoded", WithTransferSenderViewModel.Cohorts.Last().CohortReference);
        }

        public void Verify_Ordered_By_LatestMessageByEmployer()
        {
            Assert.AreEqual("2_Encoded", WithTransferSenderViewModel.Cohorts.First().CohortReference);
            Assert.AreEqual("1_Encoded", WithTransferSenderViewModel.Cohorts.Last().CohortReference);
        }

        public void Verify_Ordered_By_LatestMessageByProvider()
        {
            Assert.AreEqual("2_Encoded", WithTransferSenderViewModel.Cohorts.First().CohortReference);
            Assert.AreEqual("1_Encoded", WithTransferSenderViewModel.Cohorts.Last().CohortReference);
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

        public WhenMappingTransferSenderRequestToViewModelFixture MakeTheMessagesNull()
        {
            foreach (var c in GetCohortsResponse.Cohorts)
            {
                c.LatestMessageFromEmployer = c.LatestMessageFromProvider = null;
            }

            return this;
        }

        public WhenMappingTransferSenderRequestToViewModelFixture SetCreatedOn()
        {
            GetCohortsResponse.Cohorts.First(x => x.CohortId == 1).CreatedOn = DateTime.Now.AddMinutes(-5);
            GetCohortsResponse.Cohorts.First(x => x.CohortId == 2).CreatedOn = DateTime.Now.AddMinutes(-7);
            GetCohortsResponse.Cohorts.First(x => x.CohortId == 3).CreatedOn = DateTime.Now.AddMinutes(-9);
            GetCohortsResponse.Cohorts.First(x => x.CohortId == 4).CreatedOn = DateTime.Now.AddMinutes(-10);
            return this;
        }

        public WhenMappingTransferSenderRequestToViewModelFixture SetLatestMessageFromEmployer()
        {
            GetCohortsResponse.Cohorts.First(x => x.CohortId == 1).LatestMessageFromEmployer = 
                new Message("1st Message", DateTime.Now.AddMinutes(-6));
            GetCohortsResponse.Cohorts.First(x => x.CohortId == 2).LatestMessageFromEmployer =
                new Message("2nd Message", DateTime.Now.AddMinutes(-7));
            GetCohortsResponse.Cohorts.First(x => x.CohortId == 3).LatestMessageFromEmployer = 
                new Message("3rd Message", DateTime.Now.AddMinutes(-8));
            GetCohortsResponse.Cohorts.First(x => x.CohortId == 4).LatestMessageFromEmployer = 
                new Message("4th Message", DateTime.Now.AddMinutes(-9));
            
            return this;
        }

        public WhenMappingTransferSenderRequestToViewModelFixture SetLatestMessageFromProvider()
        {
            GetCohortsResponse.Cohorts.First(x => x.CohortId == 1).LatestMessageFromProvider =
                new Message("1st Message", DateTime.Now.AddMinutes(-6));
            GetCohortsResponse.Cohorts.First(x => x.CohortId == 2).LatestMessageFromProvider =
                new Message("2nd Message", DateTime.Now.AddMinutes(-7));
            GetCohortsResponse.Cohorts.First(x => x.CohortId == 3).LatestMessageFromProvider =
                new Message("3rd Message", DateTime.Now.AddMinutes(-8));
            GetCohortsResponse.Cohorts.First(x => x.CohortId == 4).LatestMessageFromProvider =
                new Message("4th Message", DateTime.Now.AddMinutes(-9));

            return this;
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
