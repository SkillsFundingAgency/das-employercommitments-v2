using AutoFixture;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using System;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models
{
    [TestFixture]
    public class EditDraftApprenticeshipViewModelTests
    {
        private EditDraftApprenticeshipViewModel _editDraftApprenticeshipViewModel;

        [SetUp]
        public void Setup()
        {
            // Arrange
            var fixture = new Fixture();
            var baseDate = DateTime.Now;
            var startDate = baseDate;
            var endDate = baseDate.AddYears(2);
            var dateOfBirth = baseDate.AddYears(-18);

            _editDraftApprenticeshipViewModel = fixture.Build<EditDraftApprenticeshipViewModel>()
                .Do(x =>
                {
                    var constructorInfo = x.GetType().GetConstructor(new[] { typeof(DateTime?), typeof(DateTime?), typeof(DateTime?) });
                    if (constructorInfo != null)
                    {
                        constructorInfo.Invoke(x, new object[] { dateOfBirth, startDate, endDate });
                    }
                })
                .Without(x => x.StartDate)
                .Create();
        }

        [Test]
        public void CloneBaseValues_Should_Not_Include_Excluded_Properties()
        {           
            // Act
            var clonedViewModel = _editDraftApprenticeshipViewModel.CloneBaseValues();

            // Assert
            Assert.IsNotNull(clonedViewModel);
            Assert.AreEqual(_editDraftApprenticeshipViewModel.ProviderId, clonedViewModel.ProviderId);
            Assert.AreEqual(_editDraftApprenticeshipViewModel.ProviderName, clonedViewModel.ProviderName);
            Assert.That(clonedViewModel, Does.Not.Property("AccountLegalEntityId"));
            Assert.That(clonedViewModel, Does.Not.Property("CohortId"));
        }   
    }
}
