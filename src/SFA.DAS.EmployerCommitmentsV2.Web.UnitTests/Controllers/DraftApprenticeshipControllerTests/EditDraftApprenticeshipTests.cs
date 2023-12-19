using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Contracts;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals;
using SFA.DAS.EmployerCommitmentsV2.Services.Approvals.Requests;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;
using SFA.DAS.Encoding;
using AddDraftApprenticeshipRequest = SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship.AddDraftApprenticeshipRequest;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.DraftApprenticeshipControllerTests;

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

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.That((result as ViewResult).ViewName, Is.EqualTo("Edit"));
            Assert.That((result as ViewResult).Model, Is.InstanceOf(typeof(EditDraftApprenticeshipViewModel)));
        });
    }

    [Test]
    [TestCase("", "")]
    [TestCase("Edit", "")]
    [TestCase("", "Edit")]
    public async Task WhenPostingEditDraftApprenticeship(string changeCourse, string changeDeliveryModel)
    {
        var result = await _testFixture.PostEditDraftApprenticeship(changeCourse, changeDeliveryModel, _testFixture._editDraftApprenticeshipViewModel);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));

        if (string.IsNullOrEmpty(changeCourse) && string.IsNullOrEmpty(changeDeliveryModel))
        {
            Assert.Multiple(() =>
            {
                Assert.That((result as RedirectToActionResult).ActionName, Is.EqualTo("SelectOption"));
                Assert.That((result as RedirectToActionResult).ControllerName, Is.EqualTo("DraftApprenticeship"));
            });
        }

        else if (!string.IsNullOrEmpty(changeCourse) && string.IsNullOrEmpty(changeDeliveryModel))
        {
            Assert.That((result as RedirectToActionResult).ActionName, Is.EqualTo("SelectCourseForEdit"));
        }

        else if (string.IsNullOrEmpty(changeCourse) && !string.IsNullOrEmpty(changeDeliveryModel))
        {
            Assert.That((result as RedirectToActionResult).ActionName, Is.EqualTo("SelectDeliveryModelForEdit"));
        }
    }

    [Test]
    public async Task WhenGettingSelectCourseForEdit()
    {
        var result = await _testFixture.GetSelectCourseForEdit(_testFixture._addDraftApprenticeshipRequest);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.That((result as ViewResult).ViewName, Is.EqualTo("SelectCourse"));
            Assert.That((result as ViewResult).Model, Is.InstanceOf(typeof(SelectCourseViewModel)));
        });
    }

    [Test]
    public async Task WhenPostingSetCourseForEdit()
    {
        var result = await _testFixture.PostSetCourseForEdit(_testFixture._selectCourseViewModel);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.That((result as RedirectToActionResult).ActionName, Is.EqualTo("SelectDeliveryModelForEdit"));
        });
    }

    [TestCase(true, true)]
    [TestCase(true, false)]
    [TestCase(false, true)]
    public async Task WhenGettingSelectDeliveryModelForEdit(bool hasDeliveryModels, bool hasUnavailableDeliveryModel)
    {
        var result = await _testFixture.GetSelectDeliveryModelForEdit(_testFixture._addDraftApprenticeshipRequest, hasDeliveryModels, hasUnavailableDeliveryModel);

        if (hasDeliveryModels || hasUnavailableDeliveryModel)
        {
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
                Assert.That((result as ViewResult).Model, Is.InstanceOf(typeof(SelectDeliveryModelForEditViewModel)));
            });
        }
        else
        {
            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
        }
    }

    [Test]
    public void WhenPostingSetDeliveryModelForEdit()
    {
        var result = _testFixture.PostSetDeliveryModelForEdit(_testFixture._selectDeliveryModelViewModel_WithDeliveryModels);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.That((result as RedirectToActionResult).ActionName, Is.EqualTo("EditDraftApprenticeshipDisplay"));
        });
    }
}

public class EditDraftApprenticeshipTestsFixture
{
    private readonly Fixture _autoFixture;
    private readonly DraftApprenticeshipController _controller;
    private readonly Mock<IModelMapper> _modelMapper;
    private readonly Mock<ICommitmentsApiClient> _commitmentsApiClient;
    private readonly Mock<IApprovalsApiClient> _outerApiClient;
    private readonly Mock<ITempDataDictionary> _tempData;

    public UpdateDraftApprenticeshipApimRequest _updateDraftApprenticeshipRequest;
    public EditDraftApprenticeshipViewModel _editDraftApprenticeshipViewModel;
    public AddDraftApprenticeshipRequest _addDraftApprenticeshipRequest;
    public SelectDeliveryModelForEditViewModel _selectDeliveryModelViewModel_WithDeliveryModels;
    public SelectDeliveryModelForEditViewModel _selectDeliveryModelViewModel_WithOutDeliveryModels;
    public SelectCourseViewModel _selectCourseViewModel;

    public EditDraftApprenticeshipTestsFixture()
    {
        _autoFixture = new Fixture();

        _modelMapper = new Mock<IModelMapper>();
        _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
        _outerApiClient = new Mock<IApprovalsApiClient>();
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
            .With(x => x.DeliveryModel, DeliveryModel.Regular)
            .Without(x => x.StartDate)
            .Without(x => x.Courses)
            .Create();


        _updateDraftApprenticeshipRequest = _autoFixture.Build<UpdateDraftApprenticeshipApimRequest>().Create();
        _addDraftApprenticeshipRequest = _autoFixture.Build<AddDraftApprenticeshipRequest>().Create();
        _selectCourseViewModel = _autoFixture.Build<SelectCourseViewModel>().Create();

        var someDms = _autoFixture
            .Create<Generator<EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel>>()
            .Take(2)
            .ToList();

        var noDms = _autoFixture
            .Create<Generator<EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel>>()
            .Take(0)
            .ToList();

        _selectDeliveryModelViewModel_WithDeliveryModels = _autoFixture.Build<SelectDeliveryModelForEditViewModel>()
            .With(x => x.HasUnavailableFlexiJobAgencyDeliveryModel, false)
            .With(x => x.DeliveryModels, someDms)
            .With(x => x.DeliveryModel, EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel.Regular)
            .Create();

        _selectDeliveryModelViewModel_WithOutDeliveryModels = _autoFixture.Build<SelectDeliveryModelForEditViewModel>()
            .With(x => x.HasUnavailableFlexiJobAgencyDeliveryModel, false)
            .With(x => x.DeliveryModels, noDms).Create();

        _modelMapper.Setup(m => m.Map<UpdateDraftApprenticeshipApimRequest>(It.IsAny<EditDraftApprenticeshipViewModel>()))
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
            Mock.Of<IEncodingService>(),
            _outerApiClient.Object
        );

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

    public async Task<IActionResult> GetSelectDeliveryModelForEdit(AddDraftApprenticeshipRequest request, bool hasDeliveryModels, bool hasUnavailableFlexiJobAgencyDeliveryModel)
    {
        if (hasDeliveryModels)
        {
            _modelMapper.Setup(m => m.Map<SelectDeliveryModelForEditViewModel>(It.IsAny<EditDraftApprenticeshipViewModel>()))
                .ReturnsAsync(_selectDeliveryModelViewModel_WithDeliveryModels);

            _modelMapper.Setup(x => x.Map<IDraftApprenticeshipViewModel>(It.IsAny<EditDraftApprenticeshipRequest>()))
                .ReturnsAsync(new EditDraftApprenticeshipViewModel());
        }
        else
        { 
            _modelMapper.Setup(m => m.Map<SelectDeliveryModelForEditViewModel>(It.IsAny<EditDraftApprenticeshipViewModel>()))
                .ReturnsAsync(_selectDeliveryModelViewModel_WithOutDeliveryModels);
        }

        if (hasUnavailableFlexiJobAgencyDeliveryModel)
        {
            _selectDeliveryModelViewModel_WithDeliveryModels.DeliveryModel = EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel.FlexiJobAgency;
            _selectDeliveryModelViewModel_WithOutDeliveryModels.DeliveryModel = EmployerCommitmentsV2.Services.Approvals.Types.DeliveryModel.FlexiJobAgency;
            _selectDeliveryModelViewModel_WithDeliveryModels.HasUnavailableFlexiJobAgencyDeliveryModel = true;
            _selectDeliveryModelViewModel_WithOutDeliveryModels.HasUnavailableFlexiJobAgencyDeliveryModel = true;
        }


        return await _controller.SelectDeliveryModelForEdit(_editDraftApprenticeshipViewModel);
    }

    public IActionResult PostSetDeliveryModelForEdit(SelectDeliveryModelForEditViewModel model)
    {
        return _controller.SetDeliveryModelForEdit(model);
    }
}