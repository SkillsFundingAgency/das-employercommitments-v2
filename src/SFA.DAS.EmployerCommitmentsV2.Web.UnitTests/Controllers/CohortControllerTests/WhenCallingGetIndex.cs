﻿using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

public class WhenCallingGetIndex
{
    [Test, MoqAutoData]
    public async Task Then_Returns_View_With_Correct_ViewModel(
        IndexRequest request,
        IndexViewModel viewModel,
        string organisationsLink,
        string schemesLink,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ILinkGenerator> mockLinkGenerator,
        [Greedy] CohortController controller)
    {
        mockMapper
            .Setup(mapper => mapper.Map<IndexViewModel>(request))
            .ReturnsAsync(viewModel);
        mockLinkGenerator
            .Setup(generator => generator.AccountsLink($"accounts/{request.AccountHashedId}/agreements"))
            .Returns(organisationsLink);
        mockLinkGenerator
            .Setup(generator => generator.AccountsLink($"accounts/{request.AccountHashedId}/schemes"))
            .Returns(schemesLink);

        var result = await controller.Index(request) as ViewResult;

        result.ViewName.Should().BeNull();
        var model = result.Model as IndexViewModel;
        model.Should().BeSameAs(viewModel);
    }
}