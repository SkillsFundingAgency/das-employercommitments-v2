using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.Encoding;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Http;
using StructureMap.Query;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    [TestFixture]
    public class WhenViewingEmployerAgreement
    {
        private CohortController _controller;
        private ViewEmployerAgreementRequest _viewEmployerAgreementRequest;        
        private ViewEmployerAgreementModel _viewEmployerAgreementModel;
        private Mock<IModelMapper> _modelMapper;
        private Mock<ILinkGenerator> _linkGenerator;

        [SetUp]
        public void Arrange()
        {
            var autoFixture = new Fixture();
            _modelMapper = new Mock<IModelMapper>();
            _linkGenerator = new Mock<ILinkGenerator>();

            _viewEmployerAgreementModel = autoFixture.Create<ViewEmployerAgreementModel>();
            _viewEmployerAgreementRequest = autoFixture.Create<ViewEmployerAgreementRequest>();
            _modelMapper.Setup(x => x.Map<ViewEmployerAgreementRequest>(It.IsAny<DetailsViewModel>()))
               .ReturnsAsync(_viewEmployerAgreementRequest);


            _controller = new CohortController(Mock.Of<ICommitmentsApiClient>(),
              Mock.Of<ILogger<CohortController>>(),
              _linkGenerator.Object,
              _modelMapper.Object,
              Mock.Of<IAuthorizationService>(),
              Mock.Of<IEncodingService>(),
              Mock.Of<IApprovalsApiClient>());

            _controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
        }

        [Test]
        public async Task Then_User_Is_Redirected_To_View_Organisations_Agreements_If_AgreementId_Is_Null()
        {
            // Arrange
            var expectedUrl = $"accounts/{_viewEmployerAgreementRequest.AccountHashedId}/agreements/";

            _linkGenerator.Setup(linkGen => linkGen.AccountsLink(It.IsAny<string>()))
              .Returns(expectedUrl);

            //Act
            var result = await _controller.ViewAgreement(_viewEmployerAgreementModel.AccountHashedId) as RedirectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedUrl, result.Url);
        }
       
    }
}
