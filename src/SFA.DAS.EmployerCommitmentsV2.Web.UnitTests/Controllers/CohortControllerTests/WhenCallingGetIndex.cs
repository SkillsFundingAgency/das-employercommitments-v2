using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    public class WhenCallingGetIndex
    {
        [Test, MoqAutoData]
        public void Then_Returns_View_With_Correct_ViewModel(
            IndexRequest request,
            IndexViewModel viewModel,
            string organisationsLink,
            string schemesLink,
            [Frozen] Mock<IModelMapper> mockMapper,
            [Frozen] Mock<ILinkGenerator> mockLinkGenerator,
            CohortController controller)
        {
            mockMapper
                .Setup(mapper => mapper.Map<IndexViewModel>(request))
                .Returns(viewModel);
            mockLinkGenerator
                .Setup(generator => generator.AccountsLink($"accounts/{request.AccountHashedId}/agreements"))
                .Returns(organisationsLink);
            mockLinkGenerator
                .Setup(generator => generator.AccountsLink($"accounts/{request.AccountHashedId}/schemes"))
                .Returns(schemesLink);

            var result = controller.Index(request) as ViewResult;

            result.ViewName.Should().BeNull();
            var model = result.Model as IndexViewModel;
            model.Should().BeSameAs(viewModel);
        }
    }
}