using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenPostingConfirmProvider
{
    [Test, MoqAutoData]
    public async Task And_The_ViewModel_Is_Valid_And_Set_To_Use_Provider_Then_Redirects_To_Assign_Action_And_The_Model_Mapped_To_The_Assign(
        ConfirmProviderViewModel viewModel,
        ValidationResult validationResult,
        ValidationFailure error,
        [Frozen] Mock<IValidator<ConfirmProviderViewModel>> mockValidator,
        [Frozen] Mock<IModelMapper> mockMapper,
        AssignRequest mapperResult,
        [Greedy] CohortController controller)
    {
        mockMapper.Setup(x => x.Map<AssignRequest>(It.IsAny<ConfirmProviderViewModel>()))
            .ReturnsAsync(mapperResult);
        viewModel.UseThisProvider = true;

        var result = await controller.ConfirmProvider(viewModel) as RedirectToActionResult;

        result.ActionName.Should().Be("assign");
        result.RouteValues.Should().NotBeEmpty();
        mockMapper.Verify(x=>x.Map<AssignRequest>(viewModel), Times.Once);
    }


    [Test, MoqAutoData]
    public async Task And_The_ViewModel_Is_Valid_And_Set_To_Not_Use_Provider_Then_Redirects_To_SelectProvider_Action_And_The_Model_Mapped_To_The_Assign(
        ConfirmProviderViewModel viewModel,
        ValidationResult validationResult,
        ValidationFailure error,
        [Frozen] Mock<IValidator<ConfirmProviderViewModel>> mockValidator,
        [Frozen] Mock<IModelMapper> mockMapper,
        SelectProviderViewModel mapperResult,
        [Greedy] CohortController controller)
    {
        mockMapper.Setup(x => x.Map<SelectProviderViewModel>(It.IsAny<ConfirmProviderViewModel>()))
            .ReturnsAsync(mapperResult);

        viewModel.UseThisProvider = false;

        var result = await controller.ConfirmProvider(viewModel) as RedirectToActionResult;

        result.ActionName.Should().Be("SelectProvider");
        result.RouteValues.Should().NotBeEmpty();
        result.RouteValues.Should().NotContainKey("ProviderName");
        result.RouteValues.Should().NotContainKey("UseThisProvider");
    }
}