using AutoFixture.Kernel;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using System.Linq;
using System.Reflection;
using SFA.DAS.CommitmentsV2.Types;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetApprenticeshipUpdatesResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ReviewApprenticeshipUpdatesRequestToViewModelMapperTests
{
    ReviewApprenticeshipUpdatesRequestToViewModelMapperTestsFixture fixture;
    [SetUp]
    public void Arrange()
    {
        fixture = new ReviewApprenticeshipUpdatesRequestToViewModelMapperTestsFixture();
    }

    [Test]
    public async Task ApprenticeshipHashedId_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipHashedId, Is.EqualTo(fixture.Source.ApprenticeshipHashedId));
    }

    [Test]
    public async Task AccountHashedId_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.AccountHashedId, Is.EqualTo(fixture.Source.AccountHashedId));
    }

    [Test]
    public async Task FirstName_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.FirstName, Is.EqualTo(fixture.ApprenticeshipUpdate.FirstName));
    }

    [Test]
    public async Task LastName_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.LastName, Is.EqualTo(fixture.ApprenticeshipUpdate.LastName));
    }

    [Test]
    public async Task DateOfBirth_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.DateOfBirth, Is.EqualTo(fixture.ApprenticeshipUpdate.DateOfBirth));
    }

    [Test]
    public async Task Cost_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.Cost, Is.EqualTo(fixture.ApprenticeshipUpdate.Cost));
    }

    [Test]
    public async Task StartDate_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.StartDate, Is.EqualTo(fixture.ApprenticeshipUpdate.StartDate));
    }

    [Test]
    public async Task EndDate_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.EndDate, Is.EqualTo(fixture.ApprenticeshipUpdate.EndDate));
    }

    [Test]
    public async Task CourseCode_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.CourseCode, Is.EqualTo(fixture.ApprenticeshipUpdate.TrainingCode));
    }

    [TestCase(DeliveryModel.Regular)]
    [TestCase(DeliveryModel.PortableFlexiJob)]
    public async Task DeliveryModel_IsMapped(DeliveryModel dm)
    {
        fixture.ApprenticeshipUpdate.DeliveryModel = dm;
        var viewModel = await fixture.Map();
        Assert.That(viewModel.ApprenticeshipUpdates.DeliveryModel, Is.EqualTo(fixture.ApprenticeshipUpdate.DeliveryModel));
    }
    [Test]
    public async Task EmploymentEndDate_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.EmploymentEndDate, Is.EqualTo(fixture.ApprenticeshipUpdate.EmploymentEndDate));
    }

    [Test]
    public async Task EmploymentPrice_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.EmploymentPrice, Is.EqualTo(fixture.ApprenticeshipUpdate.EmploymentPrice));
    }

    [TestCase(DeliveryModel.Regular)]
    [TestCase(DeliveryModel.PortableFlexiJob)]
    public async Task DeliveryModelOnOriginal_IsMapped(DeliveryModel dm)
    {
        fixture.GetApprenticeshipResponse.DeliveryModel = dm;
        var viewModel = await fixture.Map();
        Assert.That(viewModel.OriginalApprenticeship.DeliveryModel, Is.EqualTo(fixture.GetApprenticeshipResponse.DeliveryModel));
    }

    [Test]
    public async Task OriginalApprenticeship_EmploymentEndDate_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.EmploymentEndDate, Is.EqualTo(fixture.ApprenticeshipUpdate.EmploymentEndDate));
    }

    [Test]
    public async Task OriginalApprenticeship_EmploymentPrice_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.EmploymentPrice, Is.EqualTo(fixture.ApprenticeshipUpdate.EmploymentPrice));
    }

    [Test]
    public async Task Version_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.Version, Is.EqualTo(fixture.ApprenticeshipUpdate.Version));
    }

    [Test]
    public async Task Option_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.Option, Is.EqualTo(fixture.ApprenticeshipUpdate.Option));
    }

    [Test]
    public async Task Email_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.Email, Is.EqualTo(fixture.ApprenticeshipUpdate.Email));
    }

    [Test]
    public async Task TrainingName_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.CourseName, Is.EqualTo(fixture.ApprenticeshipUpdate.TrainingName));
    }

    [Test]
    public async Task ProviderName_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ProviderName, Is.EqualTo(fixture.GetApprenticeshipResponse.ProviderName));
    }

    [Test]
    public async Task OriginalApprenticeship_FirstName_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.FirstName, Is.EqualTo(fixture.ApprenticeshipUpdate.FirstName));
    }

    [Test]
    public async Task OriginalApprenticeship_LastName_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.LastName, Is.EqualTo(fixture.ApprenticeshipUpdate.LastName));
    }

    [Test]
    public async Task OriginalApprenticeship_DateOfBirth_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.DateOfBirth, Is.EqualTo(fixture.ApprenticeshipUpdate.DateOfBirth));
    }

    [Test]
    public async Task OriginalApprenticeship_Cost_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.Cost, Is.EqualTo(fixture.ApprenticeshipUpdate.Cost));
    }

    [Test]
    public async Task OriginalApprenticeship_StartDate_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.StartDate, Is.EqualTo(fixture.ApprenticeshipUpdate.StartDate));
    }

    [Test]
    public async Task OriginalApprenticeship_EndDate_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.EndDate, Is.EqualTo(fixture.ApprenticeshipUpdate.EndDate));
    }

    [Test]
    public async Task OriginalApprenticeship_CourseCode_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.CourseCode, Is.EqualTo(fixture.ApprenticeshipUpdate.TrainingCode));
    }

    [Test]
    public async Task OriginalApprenticeship_TrainingName_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.CourseName, Is.EqualTo(fixture.ApprenticeshipUpdate.TrainingName));
    }

    [Test]
    public async Task If_FirstName_Only_Updated_Map_FirstName_From_OriginalApprenticeship()
    {
        fixture.GetApprenticeshipUpdatesResponses.ApprenticeshipUpdates.First().LastName = null;
            
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.FirstName, Is.EqualTo(fixture.ApprenticeshipUpdate.FirstName));
        Assert.That(viewModel.ApprenticeshipUpdates.LastName, Is.EqualTo(fixture.GetApprenticeshipResponse.LastName));
    }

    [Test]
    public async Task If_LastName_Only_Updated_Map_FirstName_From_OriginalApprenticeship()
    {
        fixture.GetApprenticeshipUpdatesResponses.ApprenticeshipUpdates.First().FirstName = null;
            
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.LastName, Is.EqualTo(fixture.ApprenticeshipUpdate.LastName));
        Assert.That(viewModel.ApprenticeshipUpdates.FirstName, Is.EqualTo(fixture.GetApprenticeshipResponse.FirstName));
    }

    [Test]
    public async Task If_BothNames_Updated_Map_BothNames_From_Update()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.FirstName, Is.EqualTo(fixture.ApprenticeshipUpdate.FirstName));
        Assert.That(viewModel.ApprenticeshipUpdates.LastName, Is.EqualTo(fixture.ApprenticeshipUpdate.LastName));
    }

    [Test]
    public async Task OriginalApprenticeship_Email_IsMapped()
    {
        var viewModel = await fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.Email, Is.EqualTo(fixture.ApprenticeshipUpdate.Email));
    }

    public class ReviewApprenticeshipUpdatesRequestToViewModelMapperTestsFixture
    {
        public ReviewApprenticeshipUpdatesRequestToViewModelMapper Mapper;
        public ReviewApprenticeshipUpdatesRequest Source;
        public Mock<ICommitmentsApiClient> CommitmentApiClient;
        public GetApprenticeshipResponse GetApprenticeshipResponse;
        public GetApprenticeshipUpdatesResponse GetApprenticeshipUpdatesResponses;
        public GetApprenticeshipUpdatesRequest GetApprenticeshipUpdatesRequest;
        public GetTrainingProgrammeResponse GetTrainingProgrammeResponse;
        public ApprenticeshipUpdate ApprenticeshipUpdate;

        public long ApprenticeshipId = 1;

        public ReviewApprenticeshipUpdatesRequestToViewModelMapperTestsFixture()
        {
            var autoFixture = new Fixture();
            autoFixture.Customizations.Add(new DateTimeSpecimenBuilder());
            CommitmentApiClient = new Mock<ICommitmentsApiClient>();

            Source = new ReviewApprenticeshipUpdatesRequest { ApprenticeshipId = ApprenticeshipId, AccountId = 22, ApprenticeshipHashedId = "XXX", AccountHashedId = "YYY" };
            GetApprenticeshipResponse = autoFixture.Create<GetApprenticeshipResponse>();
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


            Mapper = new ReviewApprenticeshipUpdatesRequestToViewModelMapper(CommitmentApiClient.Object);
        }

        internal async Task<ReviewApprenticeshipUpdatesViewModel> Map()
        {
            return await Mapper.Map(Source);
        }
    }

    public class DateTimeSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;
            if (pi == null || pi.PropertyType != typeof(DateTime?))
                return new NoSpecimen();

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