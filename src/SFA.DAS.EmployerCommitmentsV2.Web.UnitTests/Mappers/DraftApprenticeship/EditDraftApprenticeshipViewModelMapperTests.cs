using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Extensions;
using SFA.DAS.Encoding;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship;

[TestFixture]
public class EditDraftApprenticeshipViewModelMapperTests
{
    private EditDraftApprenticeshipViewModelMapper _mapper;
    private EditDraftApprenticeshipRequest _source;
    private EditDraftApprenticeshipViewModel _result;

    private Mock<ICommitmentsApiClient> _commitmentsApiClient;
    private Mock<IApprovalsApiClient> _apiClient;
    private GetEditDraftApprenticeshipResponse _draftApprenticeshipResponse;
    private Mock<IEncodingService> _encodingService;
    private string _encodedApprenticeshipId;
    private string _cohortReference;
    private string _aleHashedId;
    private GetCohortResponse _cohort;
    private List<TrainingProgramme> _allTrainingProgrammes;
    private List<TrainingProgramme> _standardTrainingProgrammes;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();

        _allTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
        _standardTrainingProgrammes = autoFixture.CreateMany<TrainingProgramme>().ToList();
        _encodedApprenticeshipId = autoFixture.Create<string>();
        _cohortReference = autoFixture.Create<string>();
        _aleHashedId = autoFixture.Create<string>();

        _encodingService = new Mock<IEncodingService>();
        _encodingService
            .Setup(x => x.Encode(It.IsAny<long>(), It.Is<EncodingType>(e => e == EncodingType.ApprenticeshipId)))
            .Returns(_encodedApprenticeshipId);
        _encodingService
            .Setup(x => x.Encode(It.IsAny<long>(), It.Is<EncodingType>(e => e == EncodingType.CohortReference)))
            .Returns(_cohortReference);
        _encodingService
            .Setup(x => x.Encode(It.IsAny<long>(), EncodingType.PublicAccountLegalEntityId))
            .Returns(_aleHashedId);

        _draftApprenticeshipResponse = autoFixture.Create<GetEditDraftApprenticeshipResponse>();
        _draftApprenticeshipResponse.IsContinuation = false;
        _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            
        _commitmentsApiClient
            .Setup(x => x.GetAllTrainingProgrammeStandards(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAllTrainingProgrammeStandardsResponse
            {
                TrainingProgrammes = _standardTrainingProgrammes
            });
            
        _commitmentsApiClient
            .Setup(x => x.GetAllTrainingProgrammes(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new GetAllTrainingProgrammesResponse
            {
                TrainingProgrammes = _allTrainingProgrammes
            });

        _apiClient = new Mock<IApprovalsApiClient>();
        _apiClient.Setup(x =>
                x.GetEditDraftApprenticeship(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_draftApprenticeshipResponse);

        _cohort = autoFixture.Create<GetCohortResponse>();
        _cohort.WithParty = Party.Employer;
        _cohort.ChangeOfPartyRequestId = null;
        _commitmentsApiClient.Setup(x => x.GetCohort(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_cohort);

        _source = autoFixture.Create<EditDraftApprenticeshipRequest>();
        _source.Cohort = _cohort;
        _mapper = new EditDraftApprenticeshipViewModelMapper(_commitmentsApiClient.Object, _encodingService.Object, _apiClient.Object);

        _result = await _mapper.Map(TestHelper.Clone(_source)) as EditDraftApprenticeshipViewModel;
    }

    [Test]
    public void DraftApprenticeshipIdIsMappedCorrectly()
    {
        Assert.That(_result.DraftApprenticeshipId, Is.EqualTo(_source.Request.DraftApprenticeshipId));
    }

    [Test]
    public void DraftApprenticeshipHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.DraftApprenticeshipHashedId, Is.EqualTo(_encodedApprenticeshipId));
    }

    [Test]
    public void CohortIdIsMappedCorrectly()
    {
        Assert.That(_result.CohortId, Is.EqualTo(_source.Request.CohortId));
    }

    [Test]
    public void CohortReferenceIsMappedCorrectly()
    {
        Assert.That(_result.CohortReference, Is.EqualTo(_cohortReference));
    }

    [Test]
    public void ReservationIdIsMappedCorrectly()
    {
        Assert.That(_result.ReservationId, Is.EqualTo(_draftApprenticeshipResponse.ReservationId));
    }

    [Test]
    public void FirstNameIsMappedCorrectly()
    {
        Assert.That(_result.FirstName, Is.EqualTo(_draftApprenticeshipResponse.FirstName));
    }

    [Test]
    public void LastNameIsMappedCorrectly()
    {
        Assert.That(_result.LastName, Is.EqualTo(_draftApprenticeshipResponse.LastName));
    }

    [Test]
    public void EmailIsMappedCorrectly()
    {
        Assert.That(_result.Email, Is.EqualTo(_draftApprenticeshipResponse.Email));
    }

    [Test]
    public void EmailAddressConfirmedIsMappedCorrectly()
    {
        Assert.That(_result.EmailAddressConfirmed, Is.EqualTo(_draftApprenticeshipResponse.EmailAddressConfirmed));
    }

    [Test]
    public void DateOfBirthIsMappedCorrectly()
    {
        Assert.That(_result.DateOfBirth.Date, Is.EqualTo(_draftApprenticeshipResponse.DateOfBirth));
    }

    [Test]
    public void UniqueLearnerNumberIsMappedCorrectly()
    {
        Assert.That(_result.Uln, Is.EqualTo(_draftApprenticeshipResponse.Uln));
    }

    [Test]
    public void DeliveryModelIsMappedCorrectly()
    {
        Assert.That(_result.DeliveryModel, Is.EqualTo((DeliveryModel)_draftApprenticeshipResponse.DeliveryModel));
    }

    [Test]
    public void CourseCodeIsMappedCorrectly()
    {
        Assert.That(_result.CourseCode, Is.EqualTo(_draftApprenticeshipResponse.CourseCode));
    }

    [Test]
    public void CostIsMappedCorrectly()
    {
        Assert.That(_result.Cost, Is.EqualTo(_draftApprenticeshipResponse.Cost));
    }

    [Test]
    public void EmploymentPriceIsMappedCorrectly()
    {
        Assert.That(_result.EmploymentPrice, Is.EqualTo(_draftApprenticeshipResponse.EmploymentPrice));
    }

    [Test]
    public void StartDateIsMappedCorrectly()
    {
        Assert.That(_result.StartDate.Date.ToMonthYearString(), Is.EqualTo(_draftApprenticeshipResponse.StartDate.ToMonthYearString()));
    }

    [Test]
    public void EndDateIsMappedCorrectly()
    {
        Assert.That(_result.EndDate.Date.ToMonthYearString(), Is.EqualTo(_draftApprenticeshipResponse.EndDate.ToMonthYearString()));
    }

    [Test]
    public void EmploymentEndDateIsMappedCorrectly()
    {
        Assert.That(_result.EmploymentEndDate.Date.ToMonthYearString(), Is.EqualTo(_draftApprenticeshipResponse.EmploymentEndDate.ToMonthYearString()));
    }

    [Test]
    public void ReferenceIsMappedCorrectly()
    {
        Assert.That(_result.Reference, Is.EqualTo(_draftApprenticeshipResponse.EmployerReference));
    }

    [Test]
    public void AccountHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountHashedId, Is.EqualTo(_source.Request.AccountHashedId));
    }

    [Test]
    public void ProviderNameIsMappedCorrectly()
    {
        Assert.That(_result.ProviderName, Is.EqualTo(_cohort.ProviderName));
    }

    [Test]
    public void ProviderIdsMappedCorrectly()
    {
        Assert.That(_result.ProviderId, Is.EqualTo(_cohort.ProviderId));
    }

    [Test]
    public void LegalEntityNameIsMappedCorrectly()
    {
        Assert.That(_result.LegalEntityName, Is.EqualTo(_source.Cohort.LegalEntityName));
    }

    [TestCase(123, true)]
    [TestCase(null, false)]
    public async Task CoursesAreMappedCorrectlyWhenAccountIsLevy(long? transferSenderId, bool fundedByTransfer)
    {
        _cohort.LevyStatus = ApprenticeshipEmployerType.Levy;
        _cohort.TransferSenderId = transferSenderId;

        _result = await _mapper.Map(_source) as EditDraftApprenticeshipViewModel;

        Assert.That(_result.Courses, Is.EqualTo(fundedByTransfer
            ? _standardTrainingProgrammes
            : _allTrainingProgrammes));
    }

    [TestCase(123)]
    [TestCase(null)]
    public async Task CoursesAreMappedCorrectlyWhenAccountIsNonLevy(long? transferSenderId)
    {
        _cohort.LevyStatus = ApprenticeshipEmployerType.NonLevy;
        _cohort.TransferSenderId = transferSenderId;

        _result = await _mapper.Map(_source) as EditDraftApprenticeshipViewModel;

        Assert.That(_result.Courses, Is.EqualTo(_standardTrainingProgrammes));
    }

    [Test]
    public async Task CoursesAreMappedCorrectlyWhenCohortIsChangeOfParty()
    {
        _cohort.LevyStatus = ApprenticeshipEmployerType.NonLevy;
        _draftApprenticeshipResponse.IsContinuation = true;

        _result = await _mapper.Map(_source) as EditDraftApprenticeshipViewModel;

        Assert.That(_result.Courses, Is.EqualTo(_allTrainingProgrammes));
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task ThenIsContinuationIsMappedCorrectly(bool isContinuation)
    {
        _draftApprenticeshipResponse.IsContinuation = isContinuation;
        _result = await _mapper.Map(_source) as EditDraftApprenticeshipViewModel;
        Assert.That(_result.IsContinuation, Is.EqualTo(_draftApprenticeshipResponse.IsContinuation));
    }

    [Test]
    public void AccountLegalEntityIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountLegalEntityId, Is.EqualTo(_source.Cohort.AccountLegalEntityId));
    }

    [Test]
    public void AccountLegalEntityHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountLegalEntityHashedId, Is.EqualTo(_aleHashedId));
    }

    [Test]
    public void HasMultipleDeliveryModelOptionsIsMappedCorrectly()
    {
        Assert.That(_result.HasMultipleDeliveryModelOptions, Is.EqualTo(_draftApprenticeshipResponse.HasMultipleDeliveryModelOptions));
    }

    [Test]
    public void IsOnFlexiPaymentsPilotIsMappedCorrectly()
    {
        Assert.That(_result.IsOnFlexiPaymentPilot, Is.EqualTo(_draftApprenticeshipResponse.IsOnFlexiPaymentPilot));
    }

    [Test]
    public void ActualStartDateIsMappedCorrectly()
    {
        Assert.That(_result.ActualStartDate, Is.EqualTo(_draftApprenticeshipResponse.ActualStartDate));
    }

    [Test]
    public void ActualEndDateIsMappedCorrectly()
    {
        Assert.That(_result.ActualEndDate, Is.EqualTo(null));
    }

    [Test]
    public void RecognisePriorLearningIsMappedCorrectly()
    {
        Assert.That(_result.RecognisePriorLearning, Is.EqualTo(_draftApprenticeshipResponse.RecognisePriorLearning));
    }

    [Test]
    public void TrainingTotalHoursIsMappedCorrectly()
    {
        Assert.That(_result.TrainingTotalHours, Is.EqualTo(_draftApprenticeshipResponse.TrainingTotalHours));
    }

    [Test]
    public void DurationReducedByHoursIsMappedCorrectly()
    {
        Assert.That(_result.DurationReducedByHours, Is.EqualTo(_draftApprenticeshipResponse.DurationReducedByHours));
    }

    [Test]
    public void DurationReducedByIsMappedCorrectly()
    {
        Assert.That(_result.DurationReducedBy, Is.EqualTo(_draftApprenticeshipResponse.DurationReducedBy));
    }

    [Test]
    public void PriceReducedByIsMappedCorrectly()
    {
        Assert.That(_result.PriceReducedBy, Is.EqualTo(_draftApprenticeshipResponse.PriceReducedBy));
    }

}