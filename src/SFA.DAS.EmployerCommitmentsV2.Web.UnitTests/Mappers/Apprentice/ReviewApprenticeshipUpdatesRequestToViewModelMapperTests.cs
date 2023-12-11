using AutoFixture.Kernel;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using System.Reflection;
using SFA.DAS.CommitmentsV2.Types;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetApprenticeshipUpdatesResponse;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ReviewApprenticeshipUpdatesRequestToViewModelMapperTests
{
    private ReviewApprenticeshipUpdatesRequestToViewModelMapperTestsFixture _fixture;
    
    [SetUp]
    public void Arrange()
    {
        _fixture = new ReviewApprenticeshipUpdatesRequestToViewModelMapperTestsFixture();
    }

    [Test]
    public async Task ApprenticeshipHashedId_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipHashedId, Is.EqualTo(_fixture.Source.ApprenticeshipHashedId));
    }

    [Test]
    public async Task AccountHashedId_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.AccountHashedId, Is.EqualTo(_fixture.Source.AccountHashedId));
    }

    [Test]
    public async Task FirstName_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.FirstName, Is.EqualTo(_fixture.ApprenticeshipUpdate.FirstName));
    }

    [Test]
    public async Task LastName_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.LastName, Is.EqualTo(_fixture.ApprenticeshipUpdate.LastName));
    }

    [Test]
    public async Task DateOfBirth_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.DateOfBirth, Is.EqualTo(_fixture.ApprenticeshipUpdate.DateOfBirth));
    }

    [Test]
    public async Task Cost_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.Cost, Is.EqualTo(_fixture.ApprenticeshipUpdate.Cost));
    }

    [Test]
    public async Task StartDate_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.StartDate, Is.EqualTo(_fixture.ApprenticeshipUpdate.StartDate));
    }

    [Test]
    public async Task EndDate_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.EndDate, Is.EqualTo(_fixture.ApprenticeshipUpdate.EndDate));
    }

    [Test]
    public async Task CourseCode_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.CourseCode, Is.EqualTo(_fixture.ApprenticeshipUpdate.TrainingCode));
    }

    [TestCase(DeliveryModel.Regular)]
    [TestCase(DeliveryModel.PortableFlexiJob)]
    public async Task DeliveryModel_IsMapped(DeliveryModel dm)
    {
        _fixture.ApprenticeshipUpdate.DeliveryModel = dm;
        var viewModel = await _fixture.Map();
        Assert.That(viewModel.ApprenticeshipUpdates.DeliveryModel, Is.EqualTo(_fixture.ApprenticeshipUpdate.DeliveryModel));
    }
    [Test]
    public async Task EmploymentEndDate_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.EmploymentEndDate, Is.EqualTo(_fixture.ApprenticeshipUpdate.EmploymentEndDate));
    }

    [Test]
    public async Task EmploymentPrice_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.EmploymentPrice, Is.EqualTo(_fixture.ApprenticeshipUpdate.EmploymentPrice));
    }

    [TestCase(DeliveryModel.Regular)]
    [TestCase(DeliveryModel.PortableFlexiJob)]
    public async Task DeliveryModelOnOriginal_IsMapped(DeliveryModel dm)
    {
        _fixture.GetApprenticeshipResponse.DeliveryModel = dm;
        var viewModel = await _fixture.Map();
        Assert.That(viewModel.OriginalApprenticeship.DeliveryModel, Is.EqualTo(_fixture.GetApprenticeshipResponse.DeliveryModel));
    }

    [Test]
    public async Task OriginalApprenticeship_EmploymentEndDate_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.EmploymentEndDate, Is.EqualTo(_fixture.ApprenticeshipUpdate.EmploymentEndDate));
    }

    [Test]
    public async Task OriginalApprenticeship_EmploymentPrice_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.EmploymentPrice, Is.EqualTo(_fixture.ApprenticeshipUpdate.EmploymentPrice));
    }

    [Test]
    public async Task Version_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.Version, Is.EqualTo(_fixture.ApprenticeshipUpdate.Version));
    }

    [Test]
    public async Task Option_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.Option, Is.EqualTo(_fixture.ApprenticeshipUpdate.Option));
    }

    [Test]
    public async Task Email_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.Email, Is.EqualTo(_fixture.ApprenticeshipUpdate.Email));
    }

    [Test]
    public async Task TrainingName_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.CourseName, Is.EqualTo(_fixture.ApprenticeshipUpdate.TrainingName));
    }

    [Test]
    public async Task ProviderName_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ProviderName, Is.EqualTo(_fixture.GetApprenticeshipResponse.ProviderName));
    }

    [Test]
    public async Task OriginalApprenticeship_FirstName_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.FirstName, Is.EqualTo(_fixture.ApprenticeshipUpdate.FirstName));
    }

    [Test]
    public async Task OriginalApprenticeship_LastName_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.LastName, Is.EqualTo(_fixture.ApprenticeshipUpdate.LastName));
    }

    [Test]
    public async Task OriginalApprenticeship_DateOfBirth_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.DateOfBirth, Is.EqualTo(_fixture.ApprenticeshipUpdate.DateOfBirth));
    }

    [Test]
    public async Task OriginalApprenticeship_Cost_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.Cost, Is.EqualTo(_fixture.ApprenticeshipUpdate.Cost));
    }

    [Test]
    public async Task OriginalApprenticeship_StartDate_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.StartDate, Is.EqualTo(_fixture.ApprenticeshipUpdate.StartDate));
    }

    [Test]
    public async Task OriginalApprenticeship_EndDate_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.EndDate, Is.EqualTo(_fixture.ApprenticeshipUpdate.EndDate));
    }

    [Test]
    public async Task OriginalApprenticeship_CourseCode_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.CourseCode, Is.EqualTo(_fixture.ApprenticeshipUpdate.TrainingCode));
    }

    [Test]
    public async Task OriginalApprenticeship_TrainingName_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.CourseName, Is.EqualTo(_fixture.ApprenticeshipUpdate.TrainingName));
    }

    [Test]
    public async Task If_FirstName_Only_Updated_Map_FirstName_From_OriginalApprenticeship()
    {
        _fixture.GetApprenticeshipUpdatesResponses.ApprenticeshipUpdates.First().LastName = null;
            
        var viewModel = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.ApprenticeshipUpdates.FirstName, Is.EqualTo(_fixture.ApprenticeshipUpdate.FirstName));
            Assert.That(viewModel.ApprenticeshipUpdates.LastName, Is.EqualTo(_fixture.GetApprenticeshipResponse.LastName));
        });
    }

    [Test]
    public async Task If_LastName_Only_Updated_Map_FirstName_From_OriginalApprenticeship()
    {
        _fixture.GetApprenticeshipUpdatesResponses.ApprenticeshipUpdates.First().FirstName = null;
            
        var viewModel = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.ApprenticeshipUpdates.LastName, Is.EqualTo(_fixture.ApprenticeshipUpdate.LastName));
            Assert.That(viewModel.ApprenticeshipUpdates.FirstName, Is.EqualTo(_fixture.GetApprenticeshipResponse.FirstName));
        });
    }

    [Test]
    public async Task If_BothNames_Updated_Map_BothNames_From_Update()
    {
        var viewModel = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(viewModel.ApprenticeshipUpdates.FirstName, Is.EqualTo(_fixture.ApprenticeshipUpdate.FirstName));
            Assert.That(viewModel.ApprenticeshipUpdates.LastName, Is.EqualTo(_fixture.ApprenticeshipUpdate.LastName));
        });
    }

    [Test]
    public async Task OriginalApprenticeship_Email_IsMapped()
    {
        var viewModel = await _fixture.Map();

        Assert.That(viewModel.ApprenticeshipUpdates.Email, Is.EqualTo(_fixture.ApprenticeshipUpdate.Email));
    }

    private class ReviewApprenticeshipUpdatesRequestToViewModelMapperTestsFixture
    {
        private readonly ReviewApprenticeshipUpdatesRequestToViewModelMapper _mapper;
        public readonly ReviewApprenticeshipUpdatesRequest Source;
        public readonly GetApprenticeshipResponse GetApprenticeshipResponse;
        public readonly GetApprenticeshipUpdatesResponse GetApprenticeshipUpdatesResponses;
        public readonly ApprenticeshipUpdate ApprenticeshipUpdate;

        private const long ApprenticeshipId = 1;

        public ReviewApprenticeshipUpdatesRequestToViewModelMapperTestsFixture()
        {
            var autoFixture = new Fixture();
            autoFixture.Customizations.Add(new DateTimeSpecimenBuilder());
            var commitmentApiClient = new Mock<ICommitmentsApiClient>();

            Source = new ReviewApprenticeshipUpdatesRequest { ApprenticeshipId = ApprenticeshipId, AccountId = 22, ApprenticeshipHashedId = "XXX", AccountHashedId = "YYY" };
            GetApprenticeshipResponse = autoFixture.Create<GetApprenticeshipResponse>();
            autoFixture.RepeatCount = 1;
            GetApprenticeshipUpdatesResponses = autoFixture.Create<GetApprenticeshipUpdatesResponse>();
            ApprenticeshipUpdate = GetApprenticeshipUpdatesResponses.ApprenticeshipUpdates.First();
            var getTrainingProgrammeResponse = autoFixture.Create<GetTrainingProgrammeResponse>();

            var priceEpisode = new GetPriceEpisodesResponse
            {
                PriceEpisodes = new List<GetPriceEpisodesResponse.PriceEpisode>(){ new GetPriceEpisodesResponse.PriceEpisode
                {
                    FromDate = DateTime.UtcNow.AddDays(-10),
                    ToDate = null,
                    Cost = 100
                } }
            };

            commitmentApiClient.Setup(x => x.GetApprenticeship(ApprenticeshipId, It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(GetApprenticeshipResponse));
            commitmentApiClient.Setup(x => x.GetApprenticeshipUpdates(ApprenticeshipId, It.IsAny<GetApprenticeshipUpdatesRequest>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(GetApprenticeshipUpdatesResponses));
            commitmentApiClient.Setup(x => x.GetPriceEpisodes(ApprenticeshipId, It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(priceEpisode));
            commitmentApiClient.Setup(x => x.GetTrainingProgramme(It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(() => Task.FromResult(getTrainingProgrammeResponse));


            _mapper = new ReviewApprenticeshipUpdatesRequestToViewModelMapper(commitmentApiClient.Object);
        }

        internal async Task<ReviewApprenticeshipUpdatesViewModel> Map()
        {
            return await _mapper.Map(Source);
        }
    }

    private class DateTimeSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;
            if (pi == null || pi.PropertyType != typeof(DateTime?))
                return new NoSpecimen();

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