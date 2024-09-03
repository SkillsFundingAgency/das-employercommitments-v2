﻿using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Responses;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.DraftApprenticeship;

[TestFixture]
public class EditDetailsRequestToSelectDeliveryModelForEditViewModelMapperTests
{
    private EditDetailsRequestToSelectDeliveryModelForEditViewModelMapper _mapper;
    private Mock<IApprovalsApiClient> _approvalsApiClient;
    private GetEditDraftApprenticeshipSelectDeliveryModelResponse _response;
    private EditDetailsRequest _source;
    private GetCohortDetailsResponse _cohortDetail;

    [SetUp]
    public void Setup()
    {
        _source = new EditDetailsRequest
        {
            CohortId = 1
        };

        _cohortDetail = new GetCohortDetailsResponse
        {
            ProviderId = 1060
        };

        _response = new GetEditDraftApprenticeshipSelectDeliveryModelResponse
        {
            DeliveryModel = DeliveryModel.FlexiJobAgency,
            DeliveryModels = new List<DeliveryModel>() { DeliveryModel.Regular },
            EmployerName = "test",
            HasUnavailableDeliveryModel = false
        };

        _approvalsApiClient = new Mock<IApprovalsApiClient>();
        _approvalsApiClient.Setup(x => x.GetEditDraftApprenticeshipSelectDeliveryModel(_cohortDetail.ProviderId,
                _source.CohortId, It.IsAny<long>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_response);
        _approvalsApiClient.Setup(x => x.GetCohortDetails(It.IsAny<long>(), _source.CohortId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(_cohortDetail);

        _mapper = new EditDetailsRequestToSelectDeliveryModelForEditViewModelMapper(_approvalsApiClient.Object);
    }


    [TestCase(DeliveryModel.FlexiJobAgency, true, false, true)]
    [TestCase(DeliveryModel.PortableFlexiJob, true, false, false)]
    [TestCase(DeliveryModel.FlexiJobAgency, false, false, false)]
    [TestCase(DeliveryModel.FlexiJobAgency, true, true, false)]
    public async Task ShowFlexiJobAgencyDeliveryModelConfirmation_Is_Mapped_Correctly(CommitmentsV2.Types.DeliveryModel deliveryModel, bool deliveryModelIsUnavailable, bool hasOtherOptions, bool expectShowConfirmation)
    {
        _source.DeliveryModel = deliveryModel;
        _response.HasUnavailableDeliveryModel = deliveryModelIsUnavailable;

        if (hasOtherOptions)
        {
            _response.DeliveryModels.Add(DeliveryModel.PortableFlexiJob);
        }

        var result = await _mapper.Map(_source);

        result.ShowFlexiJobAgencyDeliveryModelConfirmation.Should().Be(expectShowConfirmation);
    }
}