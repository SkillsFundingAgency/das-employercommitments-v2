using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models.CreateCohort
{
    public class WhenCastingIndexRequestToDictionary
    {
        [Test, AutoData]
        public void Then_Adds_AccountId_To_Dictionary(
            IndexRequest indexRequest)
        {
            var dictionary = indexRequest.ToDictionary();

            dictionary.Should().ContainKey(nameof(IndexRequest.AccountId))
                .WhichValue.Should().Be(indexRequest.AccountId);
        }

        [Test, AutoData]
        public void Then_Adds_ReservationId_To_Dictionary(
            IndexRequest indexRequest)
        {
            var dictionary = indexRequest.ToDictionary();

            dictionary.Should().ContainKey(nameof(IndexRequest.ReservationId))
                .WhichValue.Should().Be(indexRequest.ReservationId);
        }

        [Test, AutoData]
        public void Then_Adds_AleId_To_Dictionary(
            IndexRequest indexRequest)
        {
            var dictionary = indexRequest.ToDictionary();

            dictionary.Should().ContainKey(nameof(IndexRequest.EmployerAccountLegalEntityPublicHashedId))
                .WhichValue.Should().Be(indexRequest.EmployerAccountLegalEntityPublicHashedId);
        }

        [Test, AutoData]
        public void Then_Adds_StartMonthYear_To_Dictionary(
            IndexRequest indexRequest)
        {
            var dictionary = indexRequest.ToDictionary();

            dictionary.Should().ContainKey(nameof(IndexRequest.StartMonthYear))
                .WhichValue.Should().Be(indexRequest.StartMonthYear);
        }

        [Test, AutoData]
        public void Then_Adds_CourseCode_To_Dictionary(
            IndexRequest indexRequest)
        {
            var dictionary = indexRequest.ToDictionary();

            dictionary.Should().ContainKey(nameof(IndexRequest.CourseCode))
                .WhichValue.Should().Be(indexRequest.CourseCode);
        }

        // missing value cases

        [Test, AutoData]
        public void And_No_ReservationId_Then_Not_Add_ReservationId_To_Dictionary(
            IndexRequest indexRequest)
        {
            indexRequest.ReservationId = null;
            var dictionary = indexRequest.ToDictionary();

            dictionary.Should().NotContainKey(nameof(IndexRequest.ReservationId));
        }

        [Test, AutoData]
        public void And_No_StartMonthYear_Then_Not_Add_StartMonthYear_To_Dictionary(
            IndexRequest indexRequest)
        {
            indexRequest.StartMonthYear = null;
            var dictionary = indexRequest.ToDictionary();

            dictionary.Should().NotContainKey(nameof(IndexRequest.StartMonthYear));
        }

        [Test, AutoData]
        public void And_No_CourseCode_Then_Not_Add_CourseCode_To_Dictionary(
            IndexRequest indexRequest)
        {
            indexRequest.CourseCode = null;
            var dictionary = indexRequest.ToDictionary();

            dictionary.Should().NotContainKey(nameof(IndexRequest.CourseCode));
        }
    }
}