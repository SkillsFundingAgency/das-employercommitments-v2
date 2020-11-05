using AutoFixture;
using Moq;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class WhenCallingTheViewChangesPageTests
    {
    }

    public class WhenCallingTheViewChangesPageTestsFixture
    {
        private Mock<IModelMapper> _mockMapper;

        private ViewChangesRequest _request;
        private ViewChangesViewModel _viewModel;

        private ApprenticeController _controller;

        public WhenCallingTheViewChangesPageTestsFixture()
        {
            var autoFixture = new Fixture();
            _request = autoFixture.Create<ViewChangesRequest>();
            _viewModel = autoFixture.Create<ViewChangesViewModel>();

            _mockMapper = new Mock<IModelMapper>();
        }
    }
}
