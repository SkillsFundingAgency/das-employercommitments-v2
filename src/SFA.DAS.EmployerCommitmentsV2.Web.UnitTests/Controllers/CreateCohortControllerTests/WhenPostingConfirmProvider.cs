using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CreateCohortControllerTests
{
    public class WhenPostingConfirmProvider
    {
        [Test, MoqAutoData]
        public async Task And_The_ViewModel_Is_Invalid_Then_Returns_View(
            ConfirmProviderViewModel viewModel,
            ValidationResult validationResult,
            ValidationFailure error,
            [Frozen] Mock<IValidator<ConfirmProviderViewModel>> mockValidator,
            CreateCohortController controller)
        {
            validationResult.Errors.Add(error);
            mockValidator
                .Setup(x => x.Validate(viewModel))
                .Returns(validationResult);

            var result = controller.ConfirmProvider(viewModel) as ViewResult;

            Assert.IsNotNull(result);
            Assert.Null(result.ViewName);
            Assert.AreSame(viewModel, result.ViewData.Model);

        }

        [Test, MoqAutoData]
        public async Task And_The_ViewModel_Is_Valid_And_Set_To_Use_Provider_Then_Redirects_To_Assign_Action_And_The_Model_Mapped_To_The_Assign(
            ConfirmProviderViewModel viewModel,
            ValidationResult validationResult,
            ValidationFailure error,
            [Frozen] Mock<IValidator<ConfirmProviderViewModel>> mockValidator,
            CreateCohortController controller)
        {
            viewModel.UseThisProvider = true;

            var result = controller.ConfirmProvider(viewModel) as RedirectToActionResult;

            result.ActionName.Should().Be("assign");
        }


        [Test, MoqAutoData]
        public async Task And_The_ViewModel_Is_Valid_And_Set_To_Not_Use_Provider_Then_Redirects_To_SelectProvider_Action_And_The_Model_Mapped_To_The_Assign(
            ConfirmProviderViewModel viewModel,
            ValidationResult validationResult,
            ValidationFailure error,
            [Frozen] Mock<IValidator<ConfirmProviderViewModel>> mockValidator,
            IMapper<ConfirmProviderViewModel, SelectProviderViewModel> mockMapper,
            CreateCohortController controller)
        {
            viewModel.UseThisProvider = false;

            var result = controller.ConfirmProvider(viewModel) as RedirectToActionResult;

            result.ActionName.Should().Be("SelectProvider");
            result.RouteValues.Should().NotBeEmpty();
            result.RouteValues.Should().NotContainKey("ProviderName");
            result.RouteValues.Should().NotContainKey("UseThisProvider");
        }
    }
}