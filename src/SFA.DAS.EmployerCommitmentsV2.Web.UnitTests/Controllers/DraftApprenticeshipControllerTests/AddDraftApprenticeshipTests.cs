using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Exceptions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.Testing;

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
                    .And.Match<AddDraftApprenticeshipViewModel>(m => m == f.ViewModel));
        }

        [Test]
        public async Task WhenGettingActionAndCohortIsNotWithEmployer_ThenShouldRedirectToCohortDetailsUrl()
        {
            await TestAsync(
                f => f.SetCohortWithOtherParty(),
                f => f.Get(),
                (f, r) => r.Should().NotBeNull()
                    .And.BeEquivalentTo(new {ActionName = "Details", ControllerName = "Cohort"}, op => op.ExcludingMissingMembers()));
        }

        [Test]
        public async Task WhenPostingAction_ThenShouldAddDraftApprenticeship()
        {
            await TestAsync(
                f => f.Post(),
                f => f.CommitmentsApiClient.Verify(c => c.AddDraftApprenticeship(f.ViewModel.CohortId.Value, f.AddDraftApprenticeshipRequest, It.IsAny<CancellationToken>())));
        }
        
        [Test]
        public async Task WhenPostingAction_WithEnhancedApproval_ThenShouldRedirectToCohortDetailsV2()
        {
            await TestAsync(
                f => f.Post(),
                (f, r) => r.Should().NotBeNull()
                    .And.BeOfType<RedirectToActionResult>().Which.ActionName.Should().Be("Details"));
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
        public Mock<ICommitmentsApiClient> CommitmentsApiClient { get; set; }
        public Mock<ITrainingProgrammeApiClient> TrainingProgrammeApiClient { get; set; }
        public Mock<IModelMapper> ModelMapper { get; set; }
        public Mock<IAuthorizationService> AuthorizationService { get; set; }
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
            CohortDetailsUrl = $"{Request.AccountHashedId}/unapproved/{Request.CohortReference}";
            CommitmentsApiModelException = new CommitmentsApiModelException(new List<ErrorDetail> { new ErrorDetail("Foo", "Bar") });
            CommitmentsService = new Mock<ICommitmentsService>();
            CommitmentsApiClient = new Mock<ICommitmentsApiClient>();
            TrainingProgrammeApiClient = new Mock<ITrainingProgrammeApiClient>();
            ModelMapper = new Mock<IModelMapper>();
            
            Controller = new DraftApprenticeshipController(
                CommitmentsService.Object, 
                ModelMapper.Object,
                CommitmentsApiClient.Object);

            CommitmentsService.Setup(c => c.GetCohortDetail(Cohort.CohortId)).ReturnsAsync(Cohort);
            TrainingProgrammeApiClient.Setup(c => c.GetAllTrainingProgrammes()).ReturnsAsync(Courses);
            TrainingProgrammeApiClient.Setup(c => c.GetStandardTrainingProgrammes()).ReturnsAsync(StandardCourses);
            ModelMapper.Setup(m => m.Map<CommitmentsV2.Api.Types.Requests.AddDraftApprenticeshipRequest>(ViewModel)).Returns(Task.FromResult(AddDraftApprenticeshipRequest));

            ModelMapper.Setup(m => m.Map<AddDraftApprenticeshipViewModel>(It.IsAny<AddDraftApprenticeshipRequest>())).ReturnsAsync(ViewModel);
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

        public AddDraftApprenticeshipTestsFixture SetCohortWithOtherParty()
        {
            ModelMapper.Setup(x => x.Map<AddDraftApprenticeshipViewModel>(It.IsAny<AddDraftApprenticeshipRequest>()))
                .Throws(new CohortEmployerUpdateDeniedException("Cohort With Other Party"));
            
            return this;
        }
    }
}