using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.CommitmentsV2.Types;
using static SFA.DAS.CommitmentsV2.Api.Types.Responses.GetPriceEpisodesResponse;
using SFA.DAS.Encoding;
using SFA.DAS.CommitmentsV2.Shared.Models;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapperTests
{
    private ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapperTestsFixture _fixture;

    [SetUp]
    public void Setup()
    {
        _fixture = new ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapperTestsFixture();
    }

    [Test]
    public async Task CommitmentApiToGetApprenticeshipIsCalled()
    {
        await _fixture.Map();

        _fixture.VerifyCommitmentApiIsCalled();
    }

    [Test]
    public async Task CommitmentApiToGetPriceEpisodeIsCalled()
    {
        await _fixture.Map();

        _fixture.VerifyPriceEpisodeIsCalled();
    }

    [Test]
    public async Task WhenOnlyEmployerReferenceIsChanged()
    {
        _fixture.source.EmployerReference = "EmployerRef";

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(_fixture.ApprenticeshipResponse.EmployerReference, Is.Not.EqualTo(_fixture.source.EmployerReference));
            Assert.That(result.EmployerReference, Is.EqualTo(_fixture.source.EmployerReference));
            Assert.That(result.OriginalApprenticeship.EmployerReference, Is.EqualTo(_fixture.ApprenticeshipResponse.EmployerReference));
        });
    }

    [Test]
    public async Task WhenFirstNameIsChanged()
    {
        _fixture.source.FirstName = "FirstName";

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(_fixture.ApprenticeshipResponse.FirstName, Is.Not.EqualTo(_fixture.source.FirstName));
            Assert.That(result.FirstName, Is.EqualTo(_fixture.source.FirstName));
            Assert.That(result.OriginalApprenticeship.FirstName, Is.EqualTo(_fixture.ApprenticeshipResponse.FirstName));
        });
    }

    [Test]
    public async Task WhenLastNameIsChanged()
    {
        _fixture.source.LastName = "LastName";

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(_fixture.ApprenticeshipResponse.LastName, Is.Not.EqualTo(_fixture.source.LastName));
            Assert.That(result.LastName, Is.EqualTo(_fixture.source.LastName));
            Assert.That(result.OriginalApprenticeship.LastName, Is.EqualTo(_fixture.ApprenticeshipResponse.LastName));
        });
    }

    [Test]
    public async Task WhenEmailIsChanged()
    {
        _fixture.source.Email = "Email";

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(_fixture.ApprenticeshipResponse.Email, Is.Not.EqualTo(_fixture.source.Email));
            Assert.That(result.Email, Is.EqualTo(_fixture.source.Email));
            Assert.That(result.OriginalApprenticeship.Email, Is.EqualTo(_fixture.ApprenticeshipResponse.Email));
        });
    }

    [Test]
    public async Task WhenDobIsChanged()
    {
        _fixture.source.DateOfBirth = new DateModel(new DateTime(2000,12,31));

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(_fixture.ApprenticeshipResponse.DateOfBirth.Day, Is.Not.EqualTo(_fixture.source.DateOfBirth.Day));
            Assert.That(result.DateOfBirth, Is.EqualTo(_fixture.source.DateOfBirth.Date));
            Assert.That(result.OriginalApprenticeship.DateOfBirth, Is.EqualTo(_fixture.ApprenticeshipResponse.DateOfBirth));
        });
    }

    [TestCase(DeliveryModel.Regular, DeliveryModel.PortableFlexiJob)]
    [TestCase(DeliveryModel.PortableFlexiJob, DeliveryModel.Regular)]
    public async Task WhenDeliveryModelIsChanged(DeliveryModel original, DeliveryModel changedTo)
    {
        _fixture.ApprenticeshipResponse.DeliveryModel = original;
        _fixture.source.DeliveryModel = changedTo;

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(_fixture.ApprenticeshipResponse.DeliveryModel, Is.Not.EqualTo(_fixture.source.DeliveryModel));
            Assert.That(result.DeliveryModel, Is.EqualTo(_fixture.source.DeliveryModel));
            Assert.That(result.OriginalApprenticeship.DeliveryModel, Is.EqualTo(_fixture.ApprenticeshipResponse.DeliveryModel));
        });
    }

    [Test]
    public async Task WhenEmploymentEndDateIsChanged()
    {
        var newDate = _fixture.ApprenticeshipResponse.EmploymentEndDate.Value.AddMonths(-1);
        _fixture.source.EmploymentEndDate = new MonthYearModel(newDate.Month.ToString() + newDate.Year);

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(_fixture.ApprenticeshipResponse.EmploymentEndDate, Is.Not.EqualTo(_fixture.source.EmploymentEndDate.Date));
            Assert.That(result.EmploymentEndDate, Is.EqualTo(_fixture.source.EmploymentEndDate.Date));
            Assert.That(result.OriginalApprenticeship.EmploymentEndDate, Is.EqualTo(_fixture.ApprenticeshipResponse.EmploymentEndDate));
        });
    }

    [Test]
    public async Task WhenEmploymentPriceIsChanged()
    {
        _fixture.source.EmploymentPrice = 1234;
        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(_fixture.ApprenticeshipResponse.EmploymentPrice, Is.Not.EqualTo(_fixture.source.EmploymentPrice));
            Assert.That(result.EmploymentPrice, Is.EqualTo(_fixture.source.EmploymentPrice));
            Assert.That(result.OriginalApprenticeship.EmploymentPrice, Is.EqualTo(_fixture.ApprenticeshipResponse.EmploymentPrice));
        });
    }

    [Test]
    public async Task WhenStartDateIsChanged()
    {
        var newStartDate = _fixture.ApprenticeshipResponse.StartDate.Value.AddMonths(-1);
        _fixture.source.StartDate = new MonthYearModel(newStartDate.Month.ToString() + newStartDate.Year);

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(_fixture.ApprenticeshipResponse.StartDate, Is.Not.EqualTo(_fixture.source.StartDate.Date));
            Assert.That(result.StartDate, Is.EqualTo(_fixture.source.StartDate.Date));
            Assert.That(result.OriginalApprenticeship.StartDate, Is.EqualTo(_fixture.ApprenticeshipResponse.StartDate));
        });
    }

    [Test]
    public async Task WhenEndDateIsChanged()
    {
        var newEndDate = _fixture.ApprenticeshipResponse.EndDate.AddMonths(-1);
        _fixture.source.EndDate = new MonthYearModel(newEndDate.Month.ToString() + newEndDate.Year);

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(_fixture.ApprenticeshipResponse.EndDate, Is.Not.EqualTo(_fixture.source.EndDate.Date));
            Assert.That(result.EndDate, Is.EqualTo(_fixture.source.EndDate.Date));
            Assert.That(result.OriginalApprenticeship.EndDate, Is.EqualTo(_fixture.ApprenticeshipResponse.EndDate));
        });
    }

    [Test]
    public async Task WhenCourseIsChanged()
    {
        _fixture.source.CourseCode = "Abc";

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(_fixture.ApprenticeshipResponse.CourseCode, Is.Not.EqualTo(_fixture.source.CourseCode));
            Assert.That(result.CourseCode, Is.EqualTo(_fixture.source.CourseCode));
            Assert.That(result.OriginalApprenticeship.CourseCode, Is.EqualTo(_fixture.ApprenticeshipResponse.CourseCode));
        });
    }

    [Test]
    public async Task WhenVersionIsChanged()
    {
        _fixture.source.Version = "1.1";

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(_fixture.ApprenticeshipResponse.Version, Is.Not.EqualTo(_fixture.source.Version));
            Assert.That(result.Version, Is.EqualTo(_fixture.source.Version));
        });
    }

    [Test]
    public async Task WhenCourseCodeIsChangeButVersionIsNotChanged_ThenVersionIsMapped()
    {
        _fixture.source.CourseCode = "123";

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(_fixture.ApprenticeshipResponse.Version, Is.Not.EqualTo(_fixture.source.Version));
            Assert.That(result.Version, Is.EqualTo(_fixture.source.Version));
        });
    }

    [Test]
    public async Task WhenOptionIsChanged()
    {
        _fixture.source.Option = "NewOption";

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(_fixture.ApprenticeshipResponse.Option, Is.Not.EqualTo(_fixture.source.Option));
            Assert.That(result.Option, Is.EqualTo(_fixture.source.Option));
        });
    }

    [Test]
    public async Task When_VersionHasOptions_Then_ReturnToChangeOptionsIsTrue()
    {
        _fixture.source.HasOptions = true;

        var result = await _fixture.Map();

        Assert.That(result.ReturnToChangeOption, Is.True);
    }

    [Test]
    public async Task When_VersionIsChangedDirectly_Then_ReturnToChangeVersionIsTrue()
    {
        _fixture.source.Version = "NewVersion";

        var result = await _fixture.Map();

        Assert.That(result.ReturnToChangeVersion, Is.True);
    }

    [Test]
    public async Task When_VersionIsChangedByEditCourse_Then_ReturnToChangeVersionAndOptionAreFalse()
    {
        _fixture.source.Version = "NewVersion";
        _fixture.source.CourseCode = "NewCourseCode";

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(result.ReturnToChangeVersion, Is.False);
            Assert.That(result.ReturnToChangeOption, Is.False);
        });
    }

    [Test]
    public async Task When_VersionIsChangedByEditStartDate_Then_ReturnToChangeVersionAndOptionAreFalse()
    {
        _fixture.source.Version = "NewVersion";
        _fixture.source.StartDate = new MonthYearModel(DateTime.Now.ToString("MMyyyy"));

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(result.ReturnToChangeVersion, Is.False);
            Assert.That(result.ReturnToChangeOption, Is.False);
        });
    }

    [Test]
    public async Task WhenMultipleFieldsAreChanged_TheyAreChanged()
    {
        _fixture.source.CourseCode = "NewCourse";
        _fixture.source.LastName = "NewLastName";

        var result = await _fixture.Map();

        Assert.Multiple(() =>
        {
            Assert.That(result.LastName, Is.EqualTo(_fixture.source.LastName));
            Assert.That(result.CourseCode, Is.EqualTo(_fixture.source.CourseCode));
        });
    }

    [Test]
    public async Task UnchangedFieldsAreNull()
    {
        _fixture.source.CourseCode = "Course";

        var result = await _fixture.Map();
        
        Assert.Multiple(() =>
        {
            Assert.That(result.FirstName, Is.Null);
            Assert.That(result.LastName, Is.Null);
            Assert.That(result.EndMonth, Is.Null);
            Assert.That(result.StartMonth, Is.Null);
            Assert.That(result.BirthMonth, Is.Null);
        });
    }
}

public class ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapperTestsFixture
{
    private readonly Mock<ICommitmentsApiClient> _mockCommitmentsApiClient;

    internal readonly GetApprenticeshipResponse ApprenticeshipResponse;
    private readonly GetPriceEpisodesResponse _priceEpisodeResponse;

    private readonly ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapper _mapper;
    private TrainingProgramme _standardSummary;
    private Mock<IEncodingService> _encodingService;
    public EditApprenticeshipRequestViewModel source;
    private ConfirmEditApprenticeshipViewModel _resultViewModl;

    public ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapperTestsFixture()
    {
        var autoFixture = new Fixture();

        ApprenticeshipResponse = autoFixture.Build<GetApprenticeshipResponse>()
            .With(x => x.CourseCode, "ABC")
            .With(x => x.Version, "1.0")
            .With(x => x.StartDate, new DateTime(2020, 1, 1))
            .With(x => x.EndDate, new DateTime(2021, 1, 1))
            .With(x => x.EmploymentEndDate, new DateTime(2020, 9, 1))
            .With(x => x.DateOfBirth, new DateTime(1990,1,1))
            .Create();

        source = new EditApprenticeshipRequestViewModel
        {
            ApprenticeshipId = ApprenticeshipResponse.Id,
            CourseCode = ApprenticeshipResponse.CourseCode,
            FirstName = ApprenticeshipResponse.FirstName,
            LastName = ApprenticeshipResponse.LastName,
            Email = ApprenticeshipResponse.Email,
            DateOfBirth = new DateModel(ApprenticeshipResponse.DateOfBirth),
            Cost = 1000,
            EmployerReference = ApprenticeshipResponse.EmployerReference,
            StartDate = new MonthYearModel(ApprenticeshipResponse.StartDate.Value.Month.ToString() + ApprenticeshipResponse.StartDate.Value.Year),
            EndDate = new MonthYearModel(ApprenticeshipResponse.EndDate.Month.ToString() + ApprenticeshipResponse.EndDate.Year)
        };

        _priceEpisodeResponse = autoFixture.Build<GetPriceEpisodesResponse>()
            .With(x => x.PriceEpisodes, new List<PriceEpisode> {
                new() { Cost = 1000, FromDate = DateTime.Now.AddMonths(-1), ToDate = null}})
            .Create();

        _standardSummary = autoFixture.Create<TrainingProgramme>();
        _standardSummary.EffectiveFrom = new DateTime(2018, 1, 1);
        _standardSummary.EffectiveTo = new DateTime(2022, 1, 1);
        _standardSummary.FundingPeriods = SetPriceBand(1000);

        _mockCommitmentsApiClient = new Mock<ICommitmentsApiClient>();

        _mockCommitmentsApiClient.Setup(c => c.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(ApprenticeshipResponse);
        _mockCommitmentsApiClient.Setup(c => c.GetPriceEpisodes(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_priceEpisodeResponse);
        _mockCommitmentsApiClient.Setup(t => t.GetTrainingProgrammeVersionByCourseCodeAndVersion(source.CourseCode, source.Version, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetTrainingProgrammeResponse
            {
                TrainingProgramme = _standardSummary
            });

        _encodingService = new Mock<IEncodingService>();
        _encodingService.Setup(x => x.Decode(It.IsAny<string>(), EncodingType.ApprenticeshipId)).Returns(ApprenticeshipResponse.Id);
        _encodingService.Setup(x => x.Decode(It.IsAny<string>(), EncodingType.AccountId)).Returns(ApprenticeshipResponse.EmployerAccountId);

        _mapper = new ConfirmEditApprenticeshipRequestToConfirmEditViewModelMapper(_mockCommitmentsApiClient.Object, _encodingService.Object);
    }

    private static List<TrainingProgrammeFundingPeriod> SetPriceBand(int fundingCap)
    {
        return new List<TrainingProgrammeFundingPeriod>
        {
            new()
            {
                EffectiveFrom = new DateTime(2019, 1, 1),
                EffectiveTo = DateTime.Now.AddMonths(1),
                FundingCap = fundingCap
            }
        };
    }

    public async Task<ConfirmEditApprenticeshipViewModel> Map()
    {
        _resultViewModl =  await _mapper.Map(source);
        return _resultViewModl;
    }

    internal void VerifyCommitmentApiIsCalled()
    {
        _mockCommitmentsApiClient.Verify(c => c.GetApprenticeship(ApprenticeshipResponse.Id, It.IsAny<CancellationToken>()), Times.Once());
    }

    internal void VerifyPriceEpisodeIsCalled()
    {
        _mockCommitmentsApiClient.Verify(c => c.GetPriceEpisodes(ApprenticeshipResponse.Id, It.IsAny<CancellationToken>()), Times.Once());
    }
}