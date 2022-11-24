using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.Edit;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class WhenSelectingDeliveryModelOnEditApprenticeship
    {
        [Test]
        public async Task GettingDeliveryModel_ForProviderAndCourse_WithOnlyOneOption_ShouldRedirectToEditDraftApprenticeship()
        {
            var fixture = new WhenSelectingDeliveryModelOnEditApprenticeshipFixture()
                .WithTempViewModel()
                .WithDeliveryModels(new List<DeliveryModel> { DeliveryModel.Regular });

            var result = await fixture.Sut.SelectDeliveryModelForEdit(fixture.Request) as RedirectToActionResult;
            result.ActionName.Should().Be("EditApprenticeship");
        }

        [Test]
        public async Task GettingDeliveryModel_ForProviderAndCourse_WithMultipleOptions_ShouldRedirectToSelectDeliveryModel()
        {
            var fixture = new WhenSelectingDeliveryModelOnEditApprenticeshipFixture()
                .WithTempViewModel()
                .WithDeliveryModels(new List<DeliveryModel> { DeliveryModel.Regular, DeliveryModel.PortableFlexiJob });

            var result = await fixture.Sut.SelectDeliveryModelForEdit(fixture.Request) as ViewResult;
            result.ViewName.Should().Be("SelectDeliveryModel");
        }

        [Test]
        public void WhenSettingDeliveryModel_AndNoOptionSet_ShouldThrowException()
        {
            var fixture = new WhenSelectingDeliveryModelOnEditApprenticeshipFixture();

            fixture.ViewModel.DeliveryModel = null;

            try
            {
                fixture.Sut.SetDeliveryModelForEdit(fixture.ViewModel);
                Assert.Fail("Should have had exception thrown");
            }
            catch (CommitmentsApiModelException e)
            {
                e.Errors[0].Field.Should().Be("DeliveryModel");
                e.Errors[0].Message.Should().Be("You must select the apprenticeship delivery model");
            }
        }

        [Test]
        public void WhenSettingDeliveryModel_AndOptionSet_ShouldRedirectToAddDraftApprenticeship()
        {
            var fixture = new WhenSelectingDeliveryModelOnEditApprenticeshipFixture()
                .WithTempViewModel()
                .WithDeliveryModels(new List<DeliveryModel> { DeliveryModel.Regular, DeliveryModel.PortableFlexiJob });

            fixture.ViewModel.DeliveryModel =
                EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel.PortableFlexiJob;

            var result = fixture.Sut.SetDeliveryModelForEdit(fixture.ViewModel) as RedirectToActionResult;
            result.ActionName.Should().Be("EditApprenticeship");
        }
    }

    public class WhenSelectingDeliveryModelOnEditApprenticeshipFixture
    {
        public ApprenticeController Sut { get; set; }

        public string RedirectUrl;
        public Mock<IModelMapper> ModelMapperMock;
        public Mock<IAuthorizationService> AuthorizationServiceMock;
        public Mock<ITempDataDictionary> TempDataMock;
        public EditApprenticeshipDeliveryModelViewModel ViewModel;
        public EditApprenticeshipRequest Request;
        public EditApprenticeshipRequestViewModel Apprenticeship;

        public WhenSelectingDeliveryModelOnEditApprenticeshipFixture()
        {
            var fixture = new Fixture();
            ViewModel = fixture.Create<EditApprenticeshipDeliveryModelViewModel>();
            Request = fixture.Create<EditApprenticeshipRequest>();
            Apprenticeship = fixture.Build<EditApprenticeshipRequestViewModel>().Without(x => x.BirthDay).Without(x => x.BirthMonth).Without(x => x.BirthYear)
                .Without(x => x.StartMonth).Without(x => x.StartYear).Without(x => x.StartDate)
                .Without(x => x.EndDate).Without(x => x.EndMonth).Without(x => x.EndYear)
                .Without(x => x.EmploymentEndDate).Without(x => x.EmploymentEndMonth).Without(x => x.EmploymentEndYear)
                .Create();

            ModelMapperMock = new Mock<IModelMapper>();
            TempDataMock = new Mock<ITempDataDictionary>();
            AuthorizationServiceMock = new Mock<IAuthorizationService>();

            Sut = new ApprenticeController(ModelMapperMock.Object, Mock.Of<ICookieStorageService<IndexRequest>>(), Mock.Of<ICommitmentsApiClient>(), Mock.Of<ILogger<ApprenticeController>>());
            Sut.TempData = TempDataMock.Object;
        }

        public WhenSelectingDeliveryModelOnEditApprenticeshipFixture WithDeliveryModels(List<EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel> list)
        {
            ModelMapperMock.Setup(x => x.Map<EditApprenticeshipDeliveryModelViewModel>(It.IsAny<EditApprenticeshipRequestViewModel>()))
                .ReturnsAsync(new EditApprenticeshipDeliveryModelViewModel { DeliveryModels = list });
            return this;
        }

        public WhenSelectingDeliveryModelOnEditApprenticeshipFixture WithTempViewModel()
        {
            object asString = JsonConvert.SerializeObject(Apprenticeship);
            TempDataMock.Setup(x => x.Peek(It.IsAny<string>())).Returns(asString);
            return this;
        }
    }
}
