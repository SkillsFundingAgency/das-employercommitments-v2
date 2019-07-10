using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.AddDraftApprenticeshipToNewCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models.AddDraftApprenticeshipToNewCohort
{
    public class WhenCastingRouteModelToDictionary
    {
        [Test, AutoData]
        public void Then_Adds_AccountId_To_Dictionary(
            RouteModel routeModel)
        {
            var dictionary = routeModel.ToDictionary();

            dictionary.Should().ContainKey(nameof(RouteModel.AccountId))
                .WhichValue.Should().Be(routeModel.AccountId);
        }

        [Test, AutoData]
        public void Then_Adds_ReservationId_To_Dictionary(
            RouteModel routeModel)
        {
            var dictionary = routeModel.ToDictionary();

            dictionary.Should().ContainKey(nameof(RouteModel.ReservationId))
                .WhichValue.Should().Be(routeModel.ReservationId);
        }

        [Test, AutoData]
        public void Then_Adds_AleId_To_Dictionary(
            RouteModel routeModel)
        {
            var dictionary = routeModel.ToDictionary();

            dictionary.Should().ContainKey(nameof(RouteModel.EmployerAccountLegalEntityPublicHashedId))
                .WhichValue.Should().Be(routeModel.EmployerAccountLegalEntityPublicHashedId);
        }

        [Test, AutoData]
        public void Then_Adds_StartMonthYear_To_Dictionary(
            RouteModel routeModel)
        {
            var dictionary = routeModel.ToDictionary();

            dictionary.Should().ContainKey(nameof(RouteModel.StartMonthYear))
                .WhichValue.Should().Be(routeModel.StartMonthYear);
        }

        [Test, AutoData]
        public void Then_Adds_CourseCode_To_Dictionary(
            RouteModel routeModel)
        {
            var dictionary = routeModel.ToDictionary();

            dictionary.Should().ContainKey(nameof(RouteModel.CourseCode))
                .WhichValue.Should().Be(routeModel.CourseCode);
        }

        // missing value cases

        [Test, AutoData]
        public void And_No_ReservationId_Then_Not_Add_ReservationId_To_Dictionary(
            RouteModel routeModel)
        {
            routeModel.ReservationId = null;
            var dictionary = routeModel.ToDictionary();

            dictionary.Should().NotContainKey(nameof(RouteModel.ReservationId));
        }

        [Test, AutoData]
        public void And_No_StartMonthYear_Then_Not_Add_StartMonthYear_To_Dictionary(
            RouteModel routeModel)
        {
            routeModel.StartMonthYear = null;
            var dictionary = routeModel.ToDictionary();

            dictionary.Should().NotContainKey(nameof(RouteModel.StartMonthYear));
        }

        [Test, AutoData]
        public void And_No_CourseCode_Then_Not_Add_CourseCode_To_Dictionary(
            RouteModel routeModel)
        {
            routeModel.CourseCode = null;
            var dictionary = routeModel.ToDictionary();

            dictionary.Should().NotContainKey(nameof(RouteModel.CourseCode));
        }
    }
}