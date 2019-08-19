using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.Commitments.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Testing;
using StructureMap.Query;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.DraftApprenticeshipControllerTests
{
    [TestFixture]
    [Parallelizable]
    public class AddDraftApprenticeshipTests : FluentTest<AddDraftApprenticeshipTestsFixture>
    {
        [Test]
        public async Task WhenGettingAction_ThenShouldReturnView()
        {
            await TestAsync(
                f => f.Get(),
                (f, r) => r.Should().NotBeNull()
                    .And.BeOfType<ViewResult>()
                    .Which.Model.Should().NotBeNull()
                    .And.Match<AddDraftApprenticeshipViewModel>(m => 
                        m.AccountHashedId == f.Request.AccountHashedId &&
                        m.CohortReference == f.Request.CohortReference &&
                        m.CohortId == f.Request.CohortId &&
                        m.AccountLegalEntityHashedId == f.Request.AccountLegalEntityHashedId &&
                        m.AccountLegalEntityId == f.Request.AccountLegalEntityId &&
                        m.ReservationId == f.Request.ReservationId &&
                        m.StartDate.MonthYear == f.Request.StartMonthYear &&
                        m.CourseCode == f.Request.CourseCode &&
                        m.ProviderName == f.Cohort.ProviderName &&
                        m.Courses == f.Courses));
        }

        [Test]
        public async Task WhenGettingActionAndCohortIsNotWithEmployer_ThenShouldRedirectToCohortDetailsUrl()
        {
            await TestAsync(
                f => f.SetCohortWithParty(Party.Provider),
                f => f.Get(),
                (f, r) => r.Should().NotBeNull()
                    .And.BeOfType<RedirectResult>().Which
                    .Url.Should().Be(f.CohortDetailsUrl));
        }

        [Test]
        public async Task WhenGettingActionAndCohortIsFundedByTransfer_ThenShouldReturnViewWithStandardTrainingProgrammesOnly()
        {
            await TestAsync(
                f => f.SetCohortFundedByTransfer(),
                f => f.Get(),
                (f, r) => r.Should().NotBeNull()
                    .And.BeOfType<ViewResult>()
                    .Which.Model.Should().NotBeNull()
                    .And.Match<AddDraftApprenticeshipViewModel>(m =>
                        m.Courses == f.StandardCourses));
        }

        [Test]
        public async Task WhenPostingAction_ThenShouldAddDraftApprenticeship()
        {
            await TestAsync(
                f => f.Post(),
                f => f.CommitmentsService.Verify(c => c.AddDraftApprenticeshipToCohort(f.ViewModel.CohortId.Value, f.AddDraftApprenticeshipRequest)));
        }
        
        [Test]
        public async Task WhenPostingAction_ThenShouldRedirectToCohortDetailsUrl()
        {
            await TestAsync(
                f => f.Post(),
                (f, r) => r.Should().NotBeNull()
                    .And.BeOfType<RedirectResult>().Which
                    .Url.Should().Be(f.CohortDetailsUrl));
        }

        [Test]
        public async Task WhenPostingActionAndCommitmentsApiModelExceptionIsThrown_ThenShouldReturnView()
        {
            await TestAsync(
                f => f.SetCommitmentsApiModelException(),
                f => f.Post(),
                (f, r) =>
                {
                    f.Controller.ModelState.ErrorCount.Should().Be(f.CommitmentsApiModelException.Errors.Count);
                    
                    r.Should().NotBeNull()
                        .And.BeOfType<ViewResult>()
                        .Which.Model.Should().Be(f.ViewModel)
                        .And.Match<AddDraftApprenticeshipViewModel>(m =>
                            m.ProviderName == f.Cohort.ProviderName &&
                            m.Courses == f.Courses);
                });
        }
    }

    public class AddDraftApprenticeshipTestsFixture
    {
        public CohortDetails Cohort { get; set; }
        public AddDraftApprenticeshipRequest Request { get; set; }
        public AddDraftApprenticeshipViewModel ViewModel { get; set; }
        public CommitmentsV2.Api.Types.Requests.AddDraftApprenticeshipRequest AddDraftApprenticeshipRequest { get; set; }
        public IReadOnlyList<ITrainingProgramme> StandardCourses { get; set; }
        public IReadOnlyList<ITrainingProgramme> Courses { get; set; }
        public string CohortDetailsUrl { get; set; }
        public CommitmentsApiModelException CommitmentsApiModelException { get; set; }
        public Mock<ICommitmentsService> CommitmentsService { get; set; }
        public Mock<ITrainingProgrammeApiClient> TrainingProgrammeApiClient { get; set; }
        public Mock<IModelMapper> ModelMapper { get; set; }
        public Mock<ILinkGenerator> LinkGenerator { get; set; }
        public DraftApprenticeshipController Controller { get; set; }

        public AddDraftApprenticeshipTestsFixture()
        {
            Cohort = new CohortDetails
            {
                CohortId = 1,
                ProviderName = "Foobar",
                WithParty = Party.Employer
            };
            
            Request = new AddDraftApprenticeshipRequest
            {
                AccountHashedId = "AAA000",
                CohortReference = "BBB111",
                CohortId = Cohort.CohortId,
                AccountLegalEntityHashedId = "CCC222",
                AccountLegalEntityId = 2,
                ReservationId = Guid.NewGuid(),
                StartMonthYear = "092019",
                CourseCode = "DDD333"
            };
            
            ViewModel = new AddDraftApprenticeshipViewModel
            {
                AccountHashedId = Request.AccountHashedId,
                CohortReference = Request.CohortReference,
                CohortId = Request.CohortId,
                AccountLegalEntityHashedId = Request.AccountLegalEntityHashedId,
                AccountLegalEntityId = Request.AccountLegalEntityId,
                ReservationId = Request.ReservationId,
                StartDate = new MonthYearModel(Request.StartMonthYear),
                CourseCode = Request.CourseCode
            };

            AddDraftApprenticeshipRequest = new CommitmentsV2.Api.Types.Requests.AddDraftApprenticeshipRequest();
            StandardCourses = new List<ITrainingProgramme>();
            Courses = new List<ITrainingProgramme>();
            CohortDetailsUrl = $"accounts/{Request.AccountHashedId}/apprentices/{Request.CohortReference}/details";
            CommitmentsApiModelException = new CommitmentsApiModelException(new List<ErrorDetail> { new ErrorDetail("Foo", "Bar") });
            CommitmentsService = new Mock<ICommitmentsService>();
            TrainingProgrammeApiClient = new Mock<ITrainingProgrammeApiClient>();
            ModelMapper = new Mock<IModelMapper>();
            LinkGenerator = new Mock<ILinkGenerator>();
            
            Controller = new DraftApprenticeshipController(
                CommitmentsService.Object, 
                LinkGenerator.Object,
                TrainingProgrammeApiClient.Object,
                ModelMapper.Object
                );

            CommitmentsService.Setup(c => c.GetCohortDetail(Cohort.CohortId)).ReturnsAsync(Cohort);
            TrainingProgrammeApiClient.Setup(c => c.GetAllTrainingProgrammes()).ReturnsAsync(Courses);
            TrainingProgrammeApiClient.Setup(c => c.GetStandardTrainingProgrammes()).ReturnsAsync(StandardCourses);
            ModelMapper.Setup(m => m.Map<CommitmentsV2.Api.Types.Requests.AddDraftApprenticeshipRequest>(ViewModel)).Returns(AddDraftApprenticeshipRequest);
            LinkGenerator.Setup(g => g.CommitmentsLink(CohortDetailsUrl)).Returns(CohortDetailsUrl);
        }

        public Task<IActionResult> Get()
        {
            return Controller.AddDraftApprenticeship(Request);
        }

        public Task<IActionResult> Post()
        {
            return Controller.AddDraftApprenticeship(ViewModel);
        }

        public AddDraftApprenticeshipTestsFixture SetInvalidModelState()
        {
            Controller.ModelState.AddModelError("Foo", "Bar");
            
            return this;
        }

        public AddDraftApprenticeshipTestsFixture SetCohortWithParty(Party party)
        {
            Cohort.WithParty = party;
            
            return this;
        }

        public AddDraftApprenticeshipTestsFixture SetCohortFundedByTransfer()
        {
            Cohort.IsFundedByTransfer = true;
            
            return this;
        }

        public AddDraftApprenticeshipTestsFixture SetCommitmentsApiModelException()
        {
            CommitmentsService.Setup(c => c.AddDraftApprenticeshipToCohort(It.IsAny<long>(), It.IsAny<CommitmentsV2.Api.Types.Requests.AddDraftApprenticeshipRequest>()))
                .ThrowsAsync(CommitmentsApiModelException);
            
            return this;
        }
    }
}