using FluentAssertions;
using Moq;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Infrastructure;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
public class WhenGettingSelectFundingOptions
{
    [Test, MoqAutoData]
    public async Task ThenMapsTheRequestToViewModel(
        SelectFundingViewModel viewModel,
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        cacheModel.EncodedPledgeApplicationId = null;
        cacheModel.TransferSenderId = null;

        viewModel.IsLevyAccount = false;
        viewModel.HasDirectTransfersAvailable = true;

        cacheStorageService
          .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
          .ReturnsAsync(cacheModel);
    
        mockMapper
            .Setup(mapper => mapper.Map<SelectFundingViewModel>(cacheModel))
            .ReturnsAsync(viewModel);

        await controller.SelectFunding(cacheModel.ApprenticeshipSessionKey);

        mockMapper.Verify(x => x.Map<SelectFundingViewModel>(cacheModel), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task ThenReturnsView(
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        SelectFundingViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        cacheModel.EncodedPledgeApplicationId = null;
        cacheModel.TransferSenderId = null;
        viewModel.IsLevyAccount = false;
        viewModel.HasDirectTransfersAvailable = true;

        cacheStorageService
        .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
        .ReturnsAsync(cacheModel);

        mockMapper
            .Setup(mapper => mapper.Map<SelectFundingViewModel>(cacheModel))
            .ReturnsAsync(viewModel);

        var result = await controller.SelectFunding(cacheModel.ApprenticeshipSessionKey) as ViewResult;

        result.ViewName.Should().BeNull();
        result.Model.Should().Be(viewModel); 
    }

    [Test, MoqAutoData]
    public async Task AndEncodedPledgeIsSetThenRedirectsToSelectProvider(
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheModel.EncodedPledgeApplicationId = "XXXXXXX";
        cacheModel.TransferSenderId = null;

        cacheStorageService
       .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
       .ReturnsAsync(cacheModel);
     
        var result = await controller.SelectFunding(cacheModel.ApprenticeshipSessionKey) as RedirectToActionResult;

        result.ActionName.Should().Be("SelectProvider");
        result.RouteValues["AccountHashedId"].Should().Be(cacheModel.AccountHashedId);
        result.RouteValues["ApprenticeshipSessionKey"].Should().Be(cacheModel.ApprenticeshipSessionKey);     
    }

    [Test, MoqAutoData]
    public async Task AndTransferSenderIdIsSetThenRedirectsToSelectProvider(
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheModel.EncodedPledgeApplicationId = null;
        cacheModel.TransferSenderId = "XXXXX";

        cacheStorageService
          .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
          .ReturnsAsync(cacheModel);

        var result = await controller.SelectFunding(cacheModel.ApprenticeshipSessionKey) as RedirectToActionResult;

        result.ActionName.Should().Be("SelectProvider");
        result.RouteValues["AccountHashedId"].Should().Be(cacheModel.AccountHashedId);
        result.RouteValues["ApprenticeshipSessionKey"].Should().Be(cacheModel.ApprenticeshipSessionKey);
    }

    [Test, MoqAutoData]
    public async Task AndNoFundsAvailableThenRedirectsToSelectProvider(
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        SelectFundingViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Greedy] CohortController controller)
    {
        cacheModel.EncodedPledgeApplicationId = null;
        cacheModel.TransferSenderId = null;
        viewModel.IsLevyAccount = false;
        viewModel.HasDirectTransfersAvailable = false;
        viewModel.HasUnallocatedReservationsAvailable = false;
        viewModel.HasAdditionalReservationFundsAvailable = false;

        cacheStorageService
        .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
        .ReturnsAsync(cacheModel);

        mockMapper.Setup(mapper => mapper.Map<SelectFundingViewModel>(cacheModel)).ReturnsAsync(viewModel);

        var result = await controller.SelectFunding(cacheModel.ApprenticeshipSessionKey) as RedirectToActionResult;

        result.ActionName.Should().Be("SelectProvider");
        result.RouteValues["AccountHashedId"].Should().Be(cacheModel.AccountHashedId);
        result.RouteValues["ApprenticeshipSessionKey"].Should().Be(cacheModel.ApprenticeshipSessionKey);
    }
}