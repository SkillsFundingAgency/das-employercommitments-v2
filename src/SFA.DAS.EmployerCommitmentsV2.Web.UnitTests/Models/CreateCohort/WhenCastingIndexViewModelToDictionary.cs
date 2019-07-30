using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models.CreateCohort
{
    public class WhenCastingIndexViewModelToDictionary
    {
        [Test, AutoData]
        public void Then_Adds_AccountHashedId_To_Dictionary(
            IndexViewModel indexViewModel)
        {
            var dictionary = indexViewModel.ToDictionary();

            dictionary.Should().ContainKey(nameof(indexViewModel.AccountHashedId))
                .WhichValue.Should().Be(indexViewModel.AccountHashedId);
        }

        [Test, AutoData]
        public void Then_Adds_ReservationId_To_Dictionary(
            IndexViewModel indexViewModel)
        {
            var dictionary = indexViewModel.ToDictionary();

            dictionary.Should().ContainKey(nameof(IndexViewModel.ReservationId))
                .WhichValue.Should().Be(indexViewModel.ReservationId);
        }

        [Test, AutoData]
        public void Then_Adds_AleId_To_Dictionary(
            IndexViewModel indexViewModel)
        {
            var dictionary = indexViewModel.ToDictionary();

            dictionary.Should().ContainKey(nameof(IndexViewModel.AccountLegalEntityHashedId))
                .WhichValue.Should().Be(indexViewModel.AccountLegalEntityHashedId);
        }

        [Test, AutoData]
        public void Then_Adds_StartMonthYear_To_Dictionary(
            IndexViewModel indexViewModel)
        {
            var dictionary = indexViewModel.ToDictionary();

            dictionary.Should().ContainKey(nameof(IndexViewModel.StartMonthYear))
                .WhichValue.Should().Be(indexViewModel.StartMonthYear);
        }

        [Test, AutoData]
        public void Then_Adds_CourseCode_To_Dictionary(
            IndexViewModel indexViewModel)
        {
            var dictionary = indexViewModel.ToDictionary();

            dictionary.Should().ContainKey(nameof(IndexViewModel.CourseCode))
                .WhichValue.Should().Be(indexViewModel.CourseCode);
        }

        // missing value cases

        [Test, AutoData]
        public void And_No_ReservationId_Then_Not_Add_ReservationId_To_Dictionary(
            IndexViewModel indexViewModel)
        {
            indexViewModel.ReservationId = null;
            var dictionary = indexViewModel.ToDictionary();

            dictionary.Should().NotContainKey(nameof(IndexViewModel.ReservationId));
        }

        [Test, AutoData]
        public void And_No_StartMonthYear_Then_Not_Add_StartMonthYear_To_Dictionary(
            IndexViewModel indexViewModel)
        {
            indexViewModel.StartMonthYear = null;
            var dictionary = indexViewModel.ToDictionary();

            dictionary.Should().NotContainKey(nameof(IndexViewModel.StartMonthYear));
        }

        [Test, AutoData]
        public void And_No_CourseCode_Then_Not_Add_CourseCode_To_Dictionary(
            IndexViewModel indexViewModel)
        {
            indexViewModel.CourseCode = null;
            var dictionary = indexViewModel.ToDictionary();

            dictionary.Should().NotContainKey(nameof(IndexViewModel.CourseCode));
        }
    }
}