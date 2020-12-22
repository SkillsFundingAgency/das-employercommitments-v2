using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Home;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.HomeControllerTests
{
    [TestFixture]
    public class WhenGettingIndex
    {
        private WhenGettingIndexTestFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenGettingIndexTestFixture();
        }

        [Test]
        public async Task ThenViewModelShouldBeMappedFromRequest()
        {
            await _fixture.GetIndex();
            _fixture.VerifyViewModelIsMappedFromRequest();
        }

        private class WhenGettingIndexTestFixture
        {
            public HomeController Controller { get; private set; }
            public Mock<IModelMapper> ModelMapper { get; private set; }
            public IndexRequest Request { get; private set; }
            public IndexViewModel ViewModel { get; private set; }
            public IActionResult Result { get; private set; }

            public WhenGettingIndexTestFixture()
            {
                var autoFixture = new Fixture();
                Request = autoFixture.Create<IndexRequest>();
                ViewModel = autoFixture.Create<IndexViewModel>();

                ModelMapper = new Mock<IModelMapper>();
                ModelMapper.Setup(x => x.Map<IndexViewModel>(It.Is<IndexRequest>(r => r == Request)))
                    .ReturnsAsync(ViewModel);

                Controller = new HomeController(ModelMapper.Object);
            }

            public async Task GetIndex()
            {
                Result = await Controller.Index(Request);
            }

            public void VerifyViewModelIsMappedFromRequest()
            {
                var viewResult = (ViewResult)Result;
                var viewModel = viewResult.Model;

                Assert.IsInstanceOf<IndexViewModel>(viewModel);
                var indexViewModel = (IndexViewModel)viewModel;

                Assert.AreEqual(ViewModel, indexViewModel);
            }
        }
    }
}
