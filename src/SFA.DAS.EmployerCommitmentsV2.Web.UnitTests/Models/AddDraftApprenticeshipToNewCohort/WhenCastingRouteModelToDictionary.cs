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
            StartRequest startRequest)
        {
            var dictionary = startRequest.ToDictionary();

            dictionary.Should().ContainKey(nameof(StartRequest.AccountId))
                .WhichValue.Should().Be(startRequest.AccountId);
        }

        [Test, AutoData]
        public void Then_Adds_ReservationId_To_Dictionary(
            StartRequest startRequest)
        {
            var dictionary = startRequest.ToDictionary();

            dictionary.Should().ContainKey(nameof(StartRequest.ReservationId))
                .WhichValue.Should().Be(startRequest.ReservationId);
        }

        [Test, AutoData]
        public void Then_Adds_AleId_To_Dictionary(
            StartRequest startRequest)
        {
            var dictionary = startRequest.ToDictionary();

            dictionary.Should().ContainKey(nameof(StartRequest.EmployerAccountLegalEntityPublicHashedId))
                .WhichValue.Should().Be(startRequest.EmployerAccountLegalEntityPublicHashedId);
        }

        [Test, AutoData]
        public void Then_Adds_StartMonthYear_To_Dictionary(
            StartRequest startRequest)
        {
            var dictionary = startRequest.ToDictionary();

            dictionary.Should().ContainKey(nameof(StartRequest.StartMonthYear))
                .WhichValue.Should().Be(startRequest.StartMonthYear);
        }

        [Test, AutoData]
        public void Then_Adds_CourseCode_To_Dictionary(
            StartRequest startRequest)
        {
            var dictionary = startRequest.ToDictionary();

            dictionary.Should().ContainKey(nameof(StartRequest.CourseCode))
                .WhichValue.Should().Be(startRequest.CourseCode);
        }

        // missing value cases

        [Test, AutoData]
        public void And_No_ReservationId_Then_Not_Add_ReservationId_To_Dictionary(
            StartRequest startRequest)
        {
            startRequest.ReservationId = null;
            var dictionary = startRequest.ToDictionary();

            dictionary.Should().NotContainKey(nameof(StartRequest.ReservationId));
        }

        [Test, AutoData]
        public void And_No_StartMonthYear_Then_Not_Add_StartMonthYear_To_Dictionary(
            StartRequest startRequest)
        {
            startRequest.StartMonthYear = null;
            var dictionary = startRequest.ToDictionary();

            dictionary.Should().NotContainKey(nameof(StartRequest.StartMonthYear));
        }

        [Test, AutoData]
        public void And_No_CourseCode_Then_Not_Add_CourseCode_To_Dictionary(
            StartRequest startRequest)
        {
            startRequest.CourseCode = null;
            var dictionary = startRequest.ToDictionary();

            dictionary.Should().NotContainKey(nameof(StartRequest.CourseCode));
        }
    }
}