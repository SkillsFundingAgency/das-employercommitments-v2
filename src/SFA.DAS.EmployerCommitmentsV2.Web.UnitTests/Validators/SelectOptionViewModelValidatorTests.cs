using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Validators
{
    public class SelectOptionViewModelValidatorTests
    {
        private SelectOptionViewModel _viewModel;

        private SelectOptionViewModelValidator _validator;

        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();

            var birthDate = fixture.Create<DateTime?>();
            var startDate = fixture.Create<DateTime?>();
            var endDate = fixture.Create<DateTime?>();

            _viewModel = fixture.Build<SelectOptionViewModel>()
                .With(x => x.BirthDay, birthDate?.Day)
                .With(x => x.BirthMonth, birthDate?.Month)
                .With(x => x.BirthYear, birthDate?.Year)
                .With(x => x.EndMonth, endDate?.Month)
                .With(x => x.EndYear, endDate?.Year)
                .With(x => x.StartMonth, startDate?.Month)
                .With(x => x.StartYear, startDate?.Year)
                .Without(x => x.StartDate)
                .Create();
            _validator = new SelectOptionViewModelValidator();
        }

        [Test]
        public void WhenNoOptionIsSelected_ThenValidatorReturnsInvalid()
        {
            _viewModel.CourseOption = null;

            var result = _validator.Validate(_viewModel);

            Assert.False(result.IsValid);
        }
    }
}
