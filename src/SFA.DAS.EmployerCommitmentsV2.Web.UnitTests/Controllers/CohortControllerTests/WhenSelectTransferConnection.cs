using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    [TestFixture]
    public class WhenSelectTransferConnection
    {
        private CohortController _controller;     
        private SelectTransferConnectionViewModel _informViewModel;
        private InformRequest _informRequest;
        private string _linkGeneratorResult;
        private Mock<IModelMapper> _modelMapper;
        private Mock<ILinkGenerator> _linkGenerator;

       [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();
            _modelMapper = new Mock<IModelMapper>();
            _linkGenerator = new Mock<ILinkGenerator>();

            _informRequest = autoFixture.Create<InformRequest>();
            _informViewModel = autoFixture.Create<SelectTransferConnectionViewModel>();
            _modelMapper.Setup(x => x.Map<SelectTransferConnectionViewModel>(It.Is<InformRequest>(r => r == _informRequest)))
               .ReturnsAsync(_informViewModel);
           
            _controller = new CohortController(Mock.Of<ICommitmentsApiClient>(),
              Mock.Of<ILogger<CohortController>>(),
              _linkGenerator.Object,
              _modelMapper.Object,
              Mock.Of<IAuthorizationService>());
        }

        [Test]
        public void Then_User_Is_Redirected_To_TransferConnections_Page()
        {
            //Arrange
            _linkGeneratorResult = $"accounts/{_informViewModel.AccountHashedId}/apprentices/legalEntity/create?transferConnectionCode={_informViewModel.TransferConnectionCode}";
            _linkGenerator.Setup(x => x.CommitmentsLink(It.IsAny<string>())).Returns(_linkGeneratorResult);

            //Act
            var result =  _controller.SetTransferConnection(_informViewModel);

           //Assert
            Assert.IsInstanceOf<RedirectResult>(result);
            var redirect = (RedirectResult)result;
            Assert.AreEqual(_linkGeneratorResult, redirect.Url);
        }


        [Test]
        public async Task Then_User_Is_Redirected_To_SelectLegalEntity_Page()
        {
            //Arrange
            _linkGeneratorResult = $"accounts/{_informRequest.AccountHashedId}/apprentices/legalEntity/create";
            _linkGenerator.Setup(x => x.CommitmentsLink(It.IsAny<string>())).Returns(_linkGeneratorResult);
            _informViewModel.TransferConnections = new List<TransferConnection>();          

            //Act
            var result = await _controller.SelectTransferConnection(_informRequest);

            //Assert
            Assert.IsInstanceOf<RedirectResult>(result);
            var redirect = (RedirectResult)result;
            Assert.AreEqual(_linkGeneratorResult, redirect.Url);
        }
    }
}
