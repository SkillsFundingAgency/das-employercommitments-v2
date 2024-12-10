using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenPostingConfirmProvider
{
    [Test, MoqAutoData]
    public static void And_The_ViewModel_Is_Valid_And_Set_To_Use_Provider_Then_Redirects_To_Assign_Action_And_The_Model_Mapped_To_The_Assign(
        ConfirmProviderViewModel viewModel,
        [Greedy] CohortController controller)
    {
        viewModel.UseThisProvider = true;
        var result = controller.ConfirmProvider(viewModel) as RedirectToActionResult;

        result.ActionName.Should().Be("assign");
        result.RouteValues.Should().NotBeEmpty();
        result.RouteValues["AccountHashedId"].Should().Be(viewModel.AccountHashedId);
        result.RouteValues["AddApprenticeshipCacheKey"].Should().Be(viewModel.AddApprenticeshipCacheKey);
    }


    [Test, MoqAutoData]
    public static void And_The_ViewModel_Is_Valid_And_Set_To_Not_Use_Provider_Then_Redirects_To_SelectProvider_Action_And_The_Model_Mapped_To_The_Assign(
        ConfirmProviderViewModel viewModel,
        [Greedy] CohortController controller)
    {
        viewModel.UseThisProvider = false;

        var result = controller.ConfirmProvider(viewModel) as RedirectToActionResult;

        result.ActionName.Should().Be("SelectProvider");
        result.RouteValues.Should().NotBeEmpty();
        result.RouteValues["AccountHashedId"].Should().Be(viewModel.AccountHashedId);
        result.RouteValues["AddApprenticeshipCacheKey"].Should().Be(viewModel.AddApprenticeshipCacheKey);
    }
}