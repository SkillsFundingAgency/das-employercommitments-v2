using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingDataLockRequestChangesTests
    {
        WhenCallingDataLockRequestChangesTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingDataLockRequestChangesTestsFixture();
        }

        public async Task ThenTheCorrectViewIsReturned()
        {
            var result = await _fixture.DataLockRequestChanges();

            _fixture.VerifyViewModel(result as ViewResult);
        }
    }

    public class WhenCallingDataLockRequestChangesTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly DataLockRequestChangesRequest _request;
        private readonly DataLockRequestChangesViewModel _viewModel;

        public WhenCallingDataLockRequestChangesTestsFixture() : base()
        {
            _request = _autoFixture.Create<DataLockRequestChangesRequest>();
            _viewModel = _autoFixture.Create<DataLockRequestChangesViewModel>();
            

            _mockMapper.Setup(m => m.Map<DataLockRequestChangesViewModel>(_request))
                .ReturnsAsync(_viewModel);
        }

        public async Task<IActionResult> DataLockRequestChanges()
        {
            return await _controller.DataLockRequestChanges(_request);
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as DataLockRequestChangesViewModel;

            Assert.IsInstanceOf<DataLockRequestChangesViewModel>(viewModel);
            Assert.AreEqual(_viewModel, viewModel);
        }
    }
}
