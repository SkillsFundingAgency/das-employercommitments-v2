using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingEditStopDateTests
    {
        WhenCallingEditStopDateTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingEditStopDateTestsFixture();
        }

        [Test]
        public async Task ThenTheCorrectViewIsReturned()
        {
            //Act
            var result = await _fixture.EditStopDate();

            //Assert
            _fixture.VerifyViewModel(result as ViewResult);
        }
    }

    public class WhenCallingEditStopDateTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly EditStopDateRequest _request;
        private readonly EditStopDateViewModel _viewModel;

        public WhenCallingEditStopDateTestsFixture() : base()
        {
            _request = _autoFixture.Create<EditStopDateRequest>();           
            _autoFixture.Customize<EditStopDateViewModel>(c => c.Without(x => x.NewStopDate));
            _viewModel = _autoFixture.Create<EditStopDateViewModel>();
            _viewModel.NewStopDate = new CommitmentsV2.Shared.Models.MonthYearModel("062020");           

            _mockMapper.Setup(m => m.Map<EditStopDateViewModel>(_request))
                .ReturnsAsync(_viewModel);
        }

        public async Task<IActionResult> EditStopDate()
        {
            return await _controller.EditStopDate(_request);
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as EditStopDateViewModel;

            Assert.IsInstanceOf<EditStopDateViewModel>(viewModel);
            Assert.AreEqual(_viewModel, viewModel);
        }

    }
}
