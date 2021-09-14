using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingChangeOption
    {
        WhenCallingChangeOptionTestsFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenCallingChangeOptionTestsFixture();
        }

        [Test]
        public async Task Then_ReturnView()
        {
            var result = await _fixture.ChangeOption();

            _fixture.VerifyViewModel(result as ViewResult);
        }
    }

    public class WhenCallingChangeOptionTestsFixture : ApprenticeControllerTestFixtureBase
    {
        private readonly EditApprenticeshipRequestViewModel _editViewModel;
        private readonly ChangeOptionViewModel _viewModel;

        public WhenCallingChangeOptionTestsFixture() : base()
        {
            var baseDate = DateTime.Now;
            var startDate = new MonthYearModel(baseDate.ToString("MMyyyy"));
            var endDate = new MonthYearModel(baseDate.AddYears(2).ToString("MMyyyy"));
            var dateOfBirth = new MonthYearModel(baseDate.AddYears(-18).ToString("MMyyyy"));

            _editViewModel = _autoFixture.Build<EditApprenticeshipRequestViewModel>()
                    .With(x => x.StartDate, startDate)
                    .With(x => x.EndDate, endDate)
                    .With(x => x.DateOfBirth, dateOfBirth)
                .Create();

            _viewModel = _autoFixture.Create<ChangeOptionViewModel>();

            object serializedModel = JsonConvert.SerializeObject(_editViewModel);
            _tempDataDictionary.Setup(s => s.TryGetValue("EditApprenticeshipRequestViewModel", out serializedModel)).Returns(true);

            _mockMapper.Setup(m => m.Map<ChangeOptionViewModel>(It.IsAny<EditApprenticeshipRequestViewModel>()))
                .ReturnsAsync(_viewModel);
        }

        public async Task<IActionResult> ChangeOption()
        {
            return await _controller.ChangeOption();
        }

        public void VerifyViewModel(ViewResult viewResult)
        {
            var viewModel = viewResult.Model as ChangeOptionViewModel;

            viewModel.Should().BeEquivalentTo(_viewModel);
        }
    }
}
