﻿using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.EmployerUrlHelper;
using SFA.DAS.Encoding;

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
        private string OrganisationAgreementsUrl;
        private string AgreementUrl;

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

            OrganisationAgreementsUrl = $"accounts/{_viewEmployerAgreementRequest.AccountHashedId}/agreements/";
            AgreementUrl = $"accounts/{_viewEmployerAgreementRequest.AccountHashedId}/agreements/{_viewEmployerAgreementRequest.AgreementHashedId}/about-your-agreement";

            _controller = new CohortController(Mock.Of<ICommitmentsApiClient>(),
              Mock.Of<ILogger<CohortController>>(),
              _linkGenerator.Object,
              _modelMapper.Object,
              Mock.Of<IEncodingService>(),
              Mock.Of<IApprovalsApiClient>());

            _controller.TempData = new TempDataDictionary(new Mock<HttpContext>().Object, new Mock<ITempDataProvider>().Object);
        }
        
        [TearDown]
        public void TearDown() => _controller?.Dispose();

        [Test]
        public async Task Then_User_Is_Redirected_To_View_Organisations_Agreements_When_NoTempData()
        {
            // Arrange
            _linkGenerator.Setup(linkGen => linkGen.AccountsLink(OrganisationAgreementsUrl))
              .Returns(OrganisationAgreementsUrl);

            //Act
            var result = await _controller.ViewAgreement(_viewEmployerAgreementRequest.AccountHashedId) as RedirectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Url, Is.EqualTo(OrganisationAgreementsUrl));
        }

        [Test]
        public async Task Then_User_Is_Redirected_To_View_Agreement_If_AgreementID_In_TempData()
        {
            // Arrange         
            _controller.TempData.Put(nameof(ViewEmployerAgreementModel), _viewEmployerAgreementModel);

            _linkGenerator.Setup(linkGen => 
                linkGen.AccountsLink(AgreementUrl))
              .Returns(AgreementUrl);

            //Act
            var result = await _controller.ViewAgreement(_viewEmployerAgreementModel.AccountHashedId) as RedirectResult;

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Url, Is.EqualTo(AgreementUrl));
        }
    }
}
