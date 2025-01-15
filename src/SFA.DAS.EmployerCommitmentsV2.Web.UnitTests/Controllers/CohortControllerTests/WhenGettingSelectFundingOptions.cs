using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
public class WhenGettingSelectFundingOptions
{
    [Test, MoqAutoData]
    public async Task ThenMapsTheRequestToViewModel(
        SelectFundingRequest request,
        SelectFundingViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        request.EncodedPledgeApplicationId = null;
        request.TransferSenderId = null;
        viewModel.IsLevyAccount = false;
        viewModel.HasDirectTransfersAvailable = true;

        await controller.SelectFunding(request);

        mockMapper.Verify(x => x.Map<SelectFundingViewModel>(request), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task ThenReturnsView(
        SelectFundingRequest request,
        SelectFundingViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {

        request.EncodedPledgeApplicationId = null;
        request.TransferSenderId = null;
        viewModel.IsLevyAccount = false;
        viewModel.HasDirectTransfersAvailable = true;

        mockMapper
            .Setup(mapper => mapper.Map<SelectFundingViewModel>(request))
            .ReturnsAsync(viewModel);

        var result = await controller.SelectFunding(request) as ViewResult;

        result.ViewName.Should().BeNull();
        result.Model.Should().Be(viewModel); 
    }

    [Test, MoqAutoData]
    public async Task AndEncodedPledgeIsSetThenRedirectsToSelectProvider(
        SelectFundingRequest request,
        [Greedy] CohortController controller)
    {
        request.EncodedPledgeApplicationId = "XXXXXXX";
        request.TransferSenderId = null;

        var result = await controller.SelectFunding(request) as RedirectToActionResult;

        result.ActionName.Should().Be("SelectProvider");
        result.RouteValues["AccountHashedId"].Should().Be(request.AccountHashedId);
        result.RouteValues["TransferSenderId"].Should().Be(request.TransferSenderId);
        result.RouteValues["AccountLegalEntityHashedId"].Should().Be(request.AccountLegalEntityHashedId);
        result.RouteValues["EncodedPledgeApplicationId"].Should().Be(request.EncodedPledgeApplicationId);
    }

    [Test, MoqAutoData]
    public async Task AndTransferSenderIdIsSetThenRedirectsToSelectProvider(
        SelectFundingRequest request,
        [Greedy] CohortController controller)
    {
        request.EncodedPledgeApplicationId = null;
        request.TransferSenderId = "XXXXX";

        var result = await controller.SelectFunding(request) as RedirectToActionResult;

        result.ActionName.Should().Be("SelectProvider");
        result.RouteValues["AccountHashedId"].Should().Be(request.AccountHashedId);
        result.RouteValues["TransferSenderId"].Should().Be(request.TransferSenderId);
        result.RouteValues["AccountLegalEntityHashedId"].Should().Be(request.AccountLegalEntityHashedId);
        result.RouteValues["EncodedPledgeApplicationId"].Should().Be(request.EncodedPledgeApplicationId);
    }

    [Test, MoqAutoData]
    public async Task AndNoFundsAvailableThenRedirectsToSelectProvider(
        SelectFundingRequest request,
        SelectFundingViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        request.EncodedPledgeApplicationId = null;
        request.TransferSenderId = null;
        viewModel.IsLevyAccount = false;
        viewModel.HasDirectTransfersAvailable = false;
        viewModel.HasUnallocatedReservationsAvailable = false;
        viewModel.HasAdditionalReservationFundsAvailable = false;

        mockMapper.Setup(mapper => mapper.Map<SelectFundingViewModel>(request)).ReturnsAsync(viewModel);

        var result = await controller.SelectFunding(request) as RedirectToActionResult;

        result.ActionName.Should().Be("SelectProvider");
        result.RouteValues["AccountHashedId"].Should().Be(request.AccountHashedId);
        result.RouteValues["TransferSenderId"].Should().Be(request.TransferSenderId);
        result.RouteValues["AccountLegalEntityHashedId"].Should().Be(request.AccountLegalEntityHashedId);
        result.RouteValues["EncodedPledgeApplicationId"].Should().Be(request.EncodedPledgeApplicationId);
    }
}