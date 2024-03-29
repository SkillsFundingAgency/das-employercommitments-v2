﻿using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

[TestFixture]
public class EditApprenticeshipRequestViewModelToSelectDeliveryModelViewModelMapperTests
{
    private EditApprenticeshipRequestViewModelToEditApprenticeshipDeliveryModelViewModelMapper _mapper;
    private EditApprenticeshipRequestViewModel _source;
    private Mock<IApprovalsApiClient> _approvalsApiClient;
       
    private GetEditApprenticeshipDeliveryModelResponse _apiResponse;
    private EditApprenticeshipDeliveryModelViewModel _result;
    private Fixture _autoFixture;

    [SetUp]
    public async Task Arrange()
    {
        _autoFixture = new Fixture();

        _apiResponse = _autoFixture.Create<GetEditApprenticeshipDeliveryModelResponse>();

        _source = _autoFixture.Build<EditApprenticeshipRequestViewModel>()
            .With(x => x.DateOfBirth, new DateModel())
            .With(x => x.StartDate, new MonthYearModel(""))
            .With(x => x.EndDate, new MonthYearModel(""))
            .With(x => x.EmploymentEndDate, new MonthYearModel(""))
            .With(x => x.DeliveryModel, DeliveryModel.PortableFlexiJob)
            .Create();

        _approvalsApiClient = new Mock<IApprovalsApiClient>();
        _approvalsApiClient.Setup(x => x.GetEditApprenticeshipDeliveryModel(_source.AccountId, _source.ApprenticeshipId,
            It.IsAny<CancellationToken>())).ReturnsAsync(_apiResponse);

        _mapper = new EditApprenticeshipRequestViewModelToEditApprenticeshipDeliveryModelViewModelMapper(_approvalsApiClient.Object);

        _result = await _mapper.Map(TestHelper.Clone(_source));
    }

    [Test]
    public void DeliveryModelMappedCorrectly()
    {
        Assert.That(_result.DeliveryModel, Is.EqualTo((EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel) _source.DeliveryModel));
    }

    [Test]
    public void DeliveryModelsAreCorrectlyMapped()
    {
        Assert.That(_result.DeliveryModels, Is.EqualTo(_apiResponse.DeliveryModels.ToArray()));
    }
}