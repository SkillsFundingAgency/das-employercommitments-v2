using AutoFixture;
using NUnit.Framework;
using AutoFixture.Kernel;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.DraftApprenticeship;
using SFA.DAS.CommitmentsV2.Shared.Models;
using System.Reflection;
using System;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models
{
    [TestFixture]
    public class DraftApprenticeshipViewModelTests
    {
        private DraftApprenticeshipViewModel _draftApprenticeshipViewModel;

        [SetUp]
        public void Setup()
        {
            // Arrange
            var fixture = new Fixture();
            var baseDate = DateTime.Now;
            var startDate = baseDate;
            var endDate = baseDate.AddYears(2);
            var dateOfBirth = baseDate.AddYears(-18);
            var employmentEndDate = baseDate.AddYears(1);

            _draftApprenticeshipViewModel = fixture.Build<DraftApprenticeshipViewModel>()
                .Do(x =>
                {
                    var constructorInfo = x.GetType().GetConstructor(new[] { typeof(DateTime?), typeof(DateTime?), typeof(DateTime?), typeof(DateTime?) });
                    if (constructorInfo != null)
                    {
                        constructorInfo.Invoke(x, new object[] { dateOfBirth, startDate, endDate, employmentEndDate });
                    }
                })
                .Without(x => x.StartDate)
                .Create();
        }

        [Test]
        public void CloneBaseValues_Should_Not_Include_Excluded_Properties()
        {                    
            // Act
            var clonedViewModel = _draftApprenticeshipViewModel.CloneBaseValues();

            // Assert
            Assert.IsNotNull(clonedViewModel);
            Assert.AreEqual(_draftApprenticeshipViewModel.ProviderId, clonedViewModel.ProviderId);
            Assert.AreEqual(_draftApprenticeshipViewModel.ProviderName, clonedViewModel.ProviderName);
            Assert.That(clonedViewModel, Does.Not.Property("AccountLegalEntityId"));
            Assert.That(clonedViewModel, Does.Not.Property("CohortId"));
        }   
    }
}
