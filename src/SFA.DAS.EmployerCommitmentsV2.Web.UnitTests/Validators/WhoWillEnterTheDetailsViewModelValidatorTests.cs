using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class WhoWillEnterTheDetailsViewModelValidatorTests
    {
        private const string errorMessage = "Select who will enter the new course dates and price";

        private WhoWillEnterTheDetailsViewModelValidator _validator;
        private Fixture _autoFixture;

        [SetUp]
        public void Arrange()
        {
            _autoFixture = new Fixture();
            _validator = new WhoWillEnterTheDetailsViewModelValidator();
        }

        [Test]
        public void WhenValidatingWhoWillEnterTheDetails_AndSelectionIsNotMade_ThenValidatorReturnsInvalid()
        {
            var viewModel = _autoFixture.Create<WhoWillEnterTheDetailsViewModel>();
            viewModel.EmployerWillAdd = null;

            var result = _validator.Validate(viewModel);

            Assert.False(result.IsValid);
            Assert.AreEqual(errorMessage, result.Errors.First().ErrorMessage);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void WhenValidatingWhoWillEnterTheDetails_AndSelectionIsMade_ThenValidatorReturnsValid(bool employerResponsibility)
        {
            var viewModel = _autoFixture.Build<WhoWillEnterTheDetailsViewModel>()
                .With(vm => vm.EmployerWillAdd, employerResponsibility).Create();

            var result = _validator.Validate(viewModel);

            Assert.True(result.IsValid);
        }
    }
}
