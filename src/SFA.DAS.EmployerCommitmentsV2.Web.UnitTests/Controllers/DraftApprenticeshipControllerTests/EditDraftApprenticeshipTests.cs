using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using NUnit.Framework;
using SFA.DAS.Authorization.Services;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using AddDraftApprenticeshipRequest = SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship.AddDraftApprenticeshipRequest;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Encoding;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.DraftApprenticeshipControllerTests
{
    [TestFixture]
    public class EditDraftApprenticeshipTests
    {
        private EditDraftApprenticeshipTestsFixture _testFixture;

        [SetUp]
        public void Arrange()
        {
            _testFixture = new EditDraftApprenticeshipTestsFixture();
        }

        [Test]
        public async Task WhenGettingEditDraftApprenticeshipDisplay()
        {
            var result = await _testFixture.GetEditDraftApprenticeshipDisplay(_testFixture._editDraftApprenticeshipViewModel);

            Assert.NotNull(result);
            Assert.IsInstanceOf(typeof(ViewResult), result);
            Assert.AreEqual("Edit", (result as ViewResult).ViewName);
            Assert.IsInstanceOf(typeof(EditDraftApprenticeshipViewModel), (result as ViewResult).Model);
        }

        [Test]
        [TestCase("", "")]
        [TestCase("Edit", "")]
        [TestCase("", "Edit")]
        public async Task WhenPostingEditDraftApprenticeship(string changeCourse, string changeDeliveryModel)
        {
            var result = await _testFixture.PostEditDraftApprenticeship(changeCourse, changeDeliveryModel, _testFixture._editDraftApprenticeshipViewModel);

            Assert.NotNull(result);
            Assert.IsInstanceOf(typeof(RedirectToActionResult), result);

            if (string.IsNullOrEmpty(changeCourse) && string.IsNullOrEmpty(changeDeliveryModel))
            {                
                Assert.AreEqual("SelectOption", (result as RedirectToActionResult).ActionName);
                Assert.AreEqual("DraftApprenticeship", (result as RedirectToActionResult).ControllerName);
            }

            else if (!string.IsNullOrEmpty(changeCourse) && string.IsNullOrEmpty(changeDeliveryModel))
            {
                Assert.AreEqual("SelectCourseForEdit", (result as RedirectToActionResult).ActionName);
            }

            else if (string.IsNullOrEmpty(changeCourse) && !string.IsNullOrEmpty(changeDeliveryModel))
            {
                Assert.AreEqual("SelectDeliveryModelForEdit", (result as RedirectToActionResult).ActionName);
            }
        }

        [Test]
        public async Task WhenGettingSelectCourseForEdit()
        {
            var result = await _testFixture.GetSelectCourseForEdit(_testFixture._addDraftApprenticeshipRequest);

            Assert.NotNull(result);
            Assert.IsInstanceOf(typeof(ViewResult), result);
            Assert.AreEqual("SelectCourse", (result as ViewResult).ViewName);
            Assert.IsInstanceOf(typeof(SelectCourseViewModel), (result as ViewResult).Model);
        }

        [Test]
        public async Task WhenPostingSetCourseForEdit()
        {
            var result = await _testFixture.PostSetCourseForEdit(_testFixture._selectCourseViewModel);

            Assert.NotNull(result);
            Assert.IsInstanceOf(typeof(RedirectToActionResult), result);
            Assert.AreEqual("SelectDeliveryModelForEdit", (result as RedirectToActionResult).ActionName);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public async Task WhenGettingSelectDeliveryModelForEdit(bool hasDeliveryModels)
        {
            var result = await _testFixture.GetSelectDeliveryModelForEdit(_testFixture._addDraftApprenticeshipRequest, hasDeliveryModels);

            if (hasDeliveryModels)
            {
                Assert.NotNull(result);
                Assert.IsInstanceOf(typeof(ViewResult), result);
                Assert.AreEqual("SelectDeliveryModel", (result as ViewResult).ViewName);
                Assert.IsInstanceOf(typeof(SelectDeliveryModelViewModel), (result as ViewResult).Model);
            }
            else
            {
                Assert.IsInstanceOf(typeof(RedirectToActionResult), result);
                Assert.AreEqual("EditDraftApprenticeshipDisplay", (result as RedirectToActionResult).ActionName);
            }
        }

        [Test]
        public async Task WhenPostingSetDeliveryModelForEdit()
        {
            var result = await _testFixture.PostSetDeliveryModelForEdit(_testFixture._selectDeliveryModelViewModel_WithDeliveryModels);

            Assert.NotNull(result);
            Assert.IsInstanceOf(typeof(RedirectToActionResult), result);
            Assert.AreEqual("EditDraftApprenticeshipDisplay", (result as RedirectToActionResult).ActionName);
        }
    }

    public class EditDraftApprenticeshipTestsFixture
    {
        private Fixture _autoFixture;
        private DraftApprenticeshipController _controller;
        private Mock<IModelMapper> _modelMapper;
        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private Mock<IAuthorizationService> _authorizationService;
        private Mock<ITempDataDictionary> _tempData;

        public UpdateDraftApprenticeshipRequest _updateDraftApprenticeshipRequest;
        public EditDraftApprenticeshipViewModel _editDraftApprenticeshipViewModel;
        public AddDraftApprenticeshipRequest _addDraftApprenticeshipRequest;
        public SelectDeliveryModelViewModel _selectDeliveryModelViewModel_WithDeliveryModels;
        public SelectDeliveryModelViewModel _selectDeliveryModelViewModel_WithOutDeliveryModels;
        public SelectCourseViewModel _selectCourseViewModel;

        public EditDraftApprenticeshipTestsFixture()
        {
            _autoFixture = new Fixture();

            _modelMapper = new Mock<IModelMapper>();
            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _authorizationService = new Mock<IAuthorizationService>();
            _tempData = new Mock<ITempDataDictionary>();

            var birthDate = _autoFixture.Create<DateTime?>();
            var startDate = _autoFixture.Create<DateTime?>();
            var endDate = _autoFixture.Create<DateTime?>();

            _editDraftApprenticeshipViewModel = _autoFixture.Build<EditDraftApprenticeshipViewModel>()
                .With(x => x.DeliveryModel, DeliveryModel.PortableFlexiJob)
                .With(x => x.CourseCode, _autoFixture.Create<int>().ToString())
                .With(x => x.BirthDay, birthDate?.Day)
                .With(x => x.BirthMonth, birthDate?.Month)
                .With(x => x.BirthYear, birthDate?.Year)
                .With(x => x.EndMonth, endDate?.Month)
                .With(x => x.EndYear, endDate?.Year)
                .With(x => x.StartMonth, startDate?.Month)
                .With(x => x.StartYear, startDate?.Year)
                .Without(x => x.StartDate)
                .Without(x => x.Courses)
                .Create();

            _updateDraftApprenticeshipRequest = _autoFixture.Build<UpdateDraftApprenticeshipRequest>().Create();
            _addDraftApprenticeshipRequest = _autoFixture.Build<AddDraftApprenticeshipRequest>().Create();
            _selectCourseViewModel = _autoFixture.Build<SelectCourseViewModel>().Create();

            _selectDeliveryModelViewModel_WithDeliveryModels = _autoFixture.Build<SelectDeliveryModelViewModel>()
                    .With(x => x.DeliveryModels, _autoFixture.CreateMany<DeliveryModel>().ToArray()).Create();
            _selectDeliveryModelViewModel_WithOutDeliveryModels = _autoFixture.Build<SelectDeliveryModelViewModel>()
                    .With(x => x.DeliveryModels, new DeliveryModel[0]).Create();

            _modelMapper.Setup(m => m.Map<UpdateDraftApprenticeshipRequest>(It.IsAny<EditDraftApprenticeshipViewModel>()))
                .ReturnsAsync(_updateDraftApprenticeshipRequest);
            _modelMapper.Setup(m => m.Map<AddDraftApprenticeshipRequest>(It.IsAny<EditDraftApprenticeshipViewModel>()))
                .ReturnsAsync(_addDraftApprenticeshipRequest);
            _modelMapper.Setup(m => m.Map<SelectCourseViewModel>(It.IsAny<AddDraftApprenticeshipRequest>()))
                .ReturnsAsync(_selectCourseViewModel);
            _modelMapper.Setup(m => m.Map<AddDraftApprenticeshipRequest>(It.IsAny<SelectCourseViewModel>()))
                .ReturnsAsync(_addDraftApprenticeshipRequest);
            _modelMapper.Setup(m => m.Map<EditDraftApprenticeshipViewModel>(It.IsAny<AddDraftApprenticeshipRequest>()))
                .ReturnsAsync(_editDraftApprenticeshipViewModel);
            _modelMapper.Setup(m => m.Map<EditDraftApprenticeshipViewModel>(It.IsAny<SelectDeliveryModelViewModel>()))
                .ReturnsAsync(_editDraftApprenticeshipViewModel);

            _controller = new DraftApprenticeshipController(
                _modelMapper.Object,
                _commitmentsApiClient.Object,
                _authorizationService.Object,
                Mock.Of<IEncodingService>());

            _controller.TempData = _tempData.Object;
        }

        public async Task<IActionResult> GetEditDraftApprenticeshipDisplay(EditDraftApprenticeshipViewModel model)
        {
            return await Task.Run(() => _controller.EditDraftApprenticeshipDisplay(model));
        }

        public async Task<IActionResult> PostEditDraftApprenticeship(string changeCourse, string changeDeliveryModel, EditDraftApprenticeshipViewModel model)
        {
            return await _controller.EditDraftApprenticeship(changeCourse, changeDeliveryModel, model);
        }

        public async Task<IActionResult> GetSelectCourseForEdit(AddDraftApprenticeshipRequest request)
        {
            return await _controller.SelectCourseForEdit(request);
        }

        public async Task<IActionResult> PostSetCourseForEdit(SelectCourseViewModel model)
        {
            return await _controller.SetCourseForEdit(model);
        }

        public async Task<IActionResult> GetSelectDeliveryModelForEdit(AddDraftApprenticeshipRequest request, bool hasDeliveryModels)
        {
            if (hasDeliveryModels)
                _modelMapper.Setup(m => m.Map<SelectDeliveryModelViewModel>(It.IsAny<AddDraftApprenticeshipRequest>()))
                    .ReturnsAsync(_selectDeliveryModelViewModel_WithDeliveryModels);
            else
                _modelMapper.Setup(m => m.Map<SelectDeliveryModelViewModel>(It.IsAny<AddDraftApprenticeshipRequest>()))
                    .ReturnsAsync(_selectDeliveryModelViewModel_WithOutDeliveryModels);

            return await _controller.SelectDeliveryModelForEdit(request);
        }

        public async Task<IActionResult> PostSetDeliveryModelForEdit(SelectDeliveryModelViewModel model)
        {
            return await _controller.SetDeliveryModelForEdit(model);
        }
    }
}
