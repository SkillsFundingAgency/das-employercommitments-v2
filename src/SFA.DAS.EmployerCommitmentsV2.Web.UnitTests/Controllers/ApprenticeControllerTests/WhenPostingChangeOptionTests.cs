using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests;

public class WhenPostingChangeOptionTests : ApprenticeControllerTestBase
{
    private ChangeOptionViewModel _viewModel;

    [SetUp]
    public void Arrange()
    {
        var fixture = new Fixture();

        _viewModel = fixture.Create<ChangeOptionViewModel>();

        MockModelMapper = new Mock<IModelMapper>();

        Controller = new ApprenticeController(
            MockModelMapper.Object,
            Mock.Of<Interfaces.ICookieStorageService<IndexRequest>>(),
            Mock.Of<ICommitmentsApiClient>(),
            Mock.Of<ILogger<ApprenticeController>>());

        Controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
    }

    [Test]
    public async Task Then_MapperIsCalled()
    {
        await Controller.ChangeOption(_viewModel);

        MockModelMapper.Verify(m => m.Map<EditApprenticeshipRequestViewModel>(_viewModel), Times.Once());
    }

    [Test]
    public async Task Then_RedirectToConfirmChanges()
    {
        var result = await Controller.ChangeOption(_viewModel) as RedirectToActionResult;

        result.ActionName.Should().Be("ConfirmEditApprenticeship");
    }
}