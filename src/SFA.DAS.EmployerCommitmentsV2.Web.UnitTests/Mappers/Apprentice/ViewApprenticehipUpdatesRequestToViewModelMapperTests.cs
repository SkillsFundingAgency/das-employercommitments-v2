using AutoFixture;
using AutoFixture.Kernel;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetApprenticeshipUpdatesResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class ViewApprenticehipUpdatesRequestToViewModelMapperTests
    {
        ViewApprenticehipUpdatesRequestToViewModelMapperTestsFixture fixture;
        [SetUp]
        public void Arrange()
        {
            fixture = new ViewApprenticehipUpdatesRequestToViewModelMapperTestsFixture();
        }

        [Test]
        public async Task ApprenticeshipHashedId_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.Source.ApprenticeshipHashedId, viewModel.ApprenticeshipHashedId);
        }

        [Test]
        public async Task AccountHashedId_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.Source.AccountHashedId, viewModel.AccountHashedId);
        }

        [Test]
        public async Task FirstName_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.FirstName, viewModel.ApprenticeshipUpdates.FirstName);
        }

        [Test]
        public async Task LastName_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.LastName, viewModel.ApprenticeshipUpdates.LastName);
        }

        [Test]
        public async Task Email_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.Email, viewModel.ApprenticeshipUpdates.Email);
        }

        [Test]
        public async Task DateOfBirth_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.DateOfBirth, viewModel.ApprenticeshipUpdates.DateOfBirth);
        }

        [Test]
        public async Task Cost_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.Cost, viewModel.ApprenticeshipUpdates.Cost);
        }

        [Test]
        public async Task StartDate_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.StartDate, viewModel.ApprenticeshipUpdates.StartDate);
        }

        [Test]
        public async Task EndDate_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.EndDate, viewModel.ApprenticeshipUpdates.EndDate);
        }

        [Test]
        public async Task CourseCode_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.TrainingCode, viewModel.ApprenticeshipUpdates.CourseCode);
        }

        [Test]
        public async Task TrainingName_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.TrainingName, viewModel.ApprenticeshipUpdates.CourseName);
        }

        [Test]
        public async Task ProviderName_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.GetApprenticeshipResponse.ProviderName, viewModel.ProviderName);
        }

        [Test]
        public async Task OriginalApprenticeship_FirstName_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.FirstName, viewModel.ApprenticeshipUpdates.FirstName);
        }

        [Test]
        public async Task OriginalApprenticeship_LastName_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.LastName, viewModel.ApprenticeshipUpdates.LastName);
        }

        [Test]
        public async Task OriginalApprenticeship_DateOfBirth_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.DateOfBirth, viewModel.ApprenticeshipUpdates.DateOfBirth);
        }

        [Test]
        public async Task OriginalApprenticeship_Cost_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.Cost, viewModel.ApprenticeshipUpdates.Cost);
        }

        [Test]
        public async Task OriginalApprenticeship_StartDate_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.StartDate, viewModel.ApprenticeshipUpdates.StartDate);
        }

        [Test]
        public async Task OriginalApprenticeship_EndDate_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.EndDate, viewModel.ApprenticeshipUpdates.EndDate);
        }

        [Test]
        public async Task OriginalApprenticeship_CourseCode_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.TrainingCode, viewModel.ApprenticeshipUpdates.CourseCode);
        }

        [Test]
        public async Task OriginalApprenticeship_TrainingName_IsMapped()
        {
            var viewModel = await fixture.Map();

            Assert.AreEqual(fixture.ApprenticeshipUpdate.TrainingName, viewModel.ApprenticeshipUpdates.CourseName);
        }

        public class ViewApprenticehipUpdatesRequestToViewModelMapperTestsFixture
        {
            public ViewApprenticehipUpdatesRequestToViewModelMapper Mapper;
            public ViewApprenticeshipUpdatesRequest Source;
            public Mock<ICommitmentsApiClient> CommitmentApiClient;
            public GetApprenticeshipResponse GetApprenticeshipResponse;
            public GetApprenticeshipUpdatesResponse GetApprenticeshipUpdatesResponses;
            public GetApprenticeshipUpdatesRequest GetApprenticeshipUpdatesRequest;
            public GetTrainingProgrammeResponse GetTrainingProgrammeResponse;
            public ApprenticeshipUpdate ApprenticeshipUpdate;
            public long ApprenticeshipId = 1;

            public ViewApprenticehipUpdatesRequestToViewModelMapperTestsFixture()
            {
                var autoFixture = new Fixture();
                autoFixture.Customizations.Add(new DateTimeSpecimenBuilder());
                CommitmentApiClient = new Mock<ICommitmentsApiClient>();

                Source = new ViewApprenticeshipUpdatesRequest { ApprenticeshipId = ApprenticeshipId, AccountId = 22, ApprenticeshipHashedId = "XXX", AccountHashedId = "YYY" };
                GetApprenticeshipResponse = autoFixture.Create<GetApprenticeshipResponse>();
                GetApprenticeshipResponse.Id = ApprenticeshipId;
                autoFixture.RepeatCount = 1;
                GetApprenticeshipUpdatesResponses = autoFixture.Create<GetApprenticeshipUpdatesResponse>();
                ApprenticeshipUpdate = GetApprenticeshipUpdatesResponses.ApprenticeshipUpdates.First();
                GetTrainingProgrammeResponse = autoFixture.Create<GetTrainingProgrammeResponse>();

                var priceEpisode = new GetPriceEpisodesResponse
                {
                    PriceEpisodes = new List<GetPriceEpisodesResponse.PriceEpisode>(){ new GetPriceEpisodesResponse.PriceEpisode
                {
                    FromDate = DateTime.UtcNow.AddDays(-10),
                    ToDate = null,
                    Cost = 100
                    } }
                };

                CommitmentApiClient.Setup(x => x.GetApprenticeship(ApprenticeshipId, It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(GetApprenticeshipResponse));
                CommitmentApiClient.Setup(x => x.GetApprenticeshipUpdates(ApprenticeshipId, It.IsAny<GetApprenticeshipUpdatesRequest>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(GetApprenticeshipUpdatesResponses));
                CommitmentApiClient.Setup(x => x.GetPriceEpisodes(ApprenticeshipId, It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(priceEpisode));
                CommitmentApiClient.Setup(x => x.GetTrainingProgramme(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(GetTrainingProgrammeResponse));

                Mapper = new ViewApprenticehipUpdatesRequestToViewModelMapper(CommitmentApiClient.Object);
            }

            internal async Task<ViewApprenticeshipUpdatesViewModel> Map()
            {
                return await Mapper.Map(Source) as ViewApprenticeshipUpdatesViewModel;
            }
        }

        public class DateTimeSpecimenBuilder : ISpecimenBuilder
        {
            public object Create(object request, ISpecimenContext context)
            {
                var pi = request as PropertyInfo;
                if (pi == null || pi.PropertyType != typeof(DateTime?))
                {
                    return new NoSpecimen();
                }
                else
                {
                    DateTime dt;
                    var randomDateTime = context.Create<DateTime>();

                    if (pi.Name == "DateOfBirth")
                    {
                        dt = new DateTime(randomDateTime.Year, randomDateTime.Month, randomDateTime.Day);
                    }
                    else
                    {
                        dt = new DateTime(randomDateTime.Year, randomDateTime.Month, 1);
                    }

                    return dt;
                }
            }
        }
    }
}
