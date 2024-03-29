﻿using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship;

[TestFixture]
public class AddDraftApprenticeshipRequestToSelectDeliveryModelViewModelMapperTests
{
    private AddDraftApprenticeshipRequestToSelectDeliveryModelViewModelMapper _mapper;
    private AddDraftApprenticeshipRequest _source;
    private Mock<ICommitmentsApiClient> _commitmentsApiClient;
    private GetCohortResponse _getCohortResponse;
    private Mock<IApprovalsApiClient> _approvalsApiClient;
    private ProviderCourseDeliveryModels _providerCourseDeliveryModels;
    private long _providerId;
    private string _courseCode;
    private long _cohortId;
    private long _accountLegalEntityId;
    private SelectDeliveryModelViewModel _result;

    [SetUp]
    public async Task Arrange()
    {
        var autoFixture = new Fixture();

        _providerId = autoFixture.Create<long>();
        _courseCode = autoFixture.Create<string>();
        _cohortId = autoFixture.Create<long>();
        _accountLegalEntityId = autoFixture.Create<long>();

        _source = autoFixture.Build<AddDraftApprenticeshipRequest>()
            .With(x => x.StartMonthYear, "062020")
            .With(x => x.CourseCode, "Course1")
            .With(x => x.ProviderId, _providerId)
            .With(x => x.CourseCode, _courseCode)
            .With(x => x.AccountLegalEntityId, _accountLegalEntityId)
            .With(x => x.CohortId, _cohortId)
            .With(x => x.DeliveryModel, DeliveryModel.PortableFlexiJob)
            .Create();

        _getCohortResponse = autoFixture.Build<GetCohortResponse>()
            .With(x => x.LevyStatus, ApprenticeshipEmployerType.Levy)
            .With(x => x.WithParty, Party.Employer)
            .With(x => x.ProviderId, _providerId)
            .Without(x => x.TransferSenderId)
            .Create();

        _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _commitmentsApiClient.Setup(x => x.GetCohort(_source.CohortId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getCohortResponse);

        _providerCourseDeliveryModels = autoFixture.Create<ProviderCourseDeliveryModels>();

        _approvalsApiClient = new Mock<IApprovalsApiClient>();
        _approvalsApiClient.Setup(x => x.GetProviderCourseDeliveryModels(_providerId, _courseCode, _accountLegalEntityId, It.IsAny<CancellationToken>())).ReturnsAsync(_providerCourseDeliveryModels);

        _mapper = new AddDraftApprenticeshipRequestToSelectDeliveryModelViewModelMapper(_commitmentsApiClient.Object, _approvalsApiClient.Object);

        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void AccountHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountHashedId, Is.EqualTo(_source.AccountHashedId));
    }

    [Test]
    public void AccountLegalEntityIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountLegalEntityId, Is.EqualTo(_source.AccountLegalEntityId));
    }

    [Test]
    public void AccountLegalEntityHashedIdIsMappedCorrectly()
    {
        Assert.That(_result.AccountLegalEntityHashedId, Is.EqualTo(_source.AccountLegalEntityHashedId));
    }

    [Test]
    public void CohortIdIsMappedCorrectly()
    {
        Assert.That(_result.CohortId, Is.EqualTo(_source.CohortId));
    }

    [Test]
    public void CohortReferenceIsMappedCorrectly()
    {
        Assert.That(_result.CohortReference, Is.EqualTo(_source.CohortReference));
    }

    [Test]
    public void CourseCodeIsMappedCorrectly()
    {
        Assert.That(_result.CourseCode, Is.EqualTo(_source.CourseCode));
    }

    [Test]
    public void DeliveryModelIsMappedCorrectly()
    {
        Assert.That(_result.DeliveryModel, Is.EqualTo(_source.DeliveryModel));
    }

    [Test]
    public void DeliveryModelsAreMappedCorrectly()
    {
        Assert.That(_result.DeliveryModels, Is.EqualTo(_providerCourseDeliveryModels.DeliveryModels));
    }

    [Test]
    public void ProviderIdIsMappedCorrectly()
    {
        Assert.That(_result.ProviderId, Is.EqualTo(_source.ProviderId));
    }

    [Test]
    public void ReservationIdIsMappedCorrectly()
    {
        Assert.That(_result.ReservationId, Is.EqualTo(_source.ReservationId));
    }

    [Test]
    public void StartDateIsMappedCorrectly()
    {
        Assert.That(_result.StartMonthYear, Is.EqualTo(_source.StartMonthYear));
    }
}