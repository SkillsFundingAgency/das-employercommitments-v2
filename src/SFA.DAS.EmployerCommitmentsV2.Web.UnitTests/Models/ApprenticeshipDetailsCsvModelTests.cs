using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Models
{
    public class ApprenticeshipDetailsCsvModelTests
    {
        [Test, AutoData]
        public void Then_Maps_ApprenticeName(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            ApprenticeshipDetailsCsvModel result = source;

            result.ApprenticeName.Should().Be($"{source.FirstName} {source.LastName}");
        }

        [Test, AutoData]
        public void Then_Maps_ProviderName(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            ApprenticeshipDetailsCsvModel result = source;

            result.Provider.Should().Be(source.ProviderName);
        }

        [Test, AutoData]
        public void Then_Maps_CourseName(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            ApprenticeshipDetailsCsvModel result = source;

            result.CourseName.Should().Be(source.CourseName);
        }

        [Test, AutoData]
        public void Then_Maps_PlannedEndDate(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            ApprenticeshipDetailsCsvModel result = source;

            result.PlannedEndDate.Should().Be(source.EndDate.ToString("MMM yyyy"));
        }


        [Test, AutoData]
        public void Then_Maps_PlannedStartDate(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            ApprenticeshipDetailsCsvModel result = source;

            result.PlannedStartDate.Should().Be(source.StartDate.ToString("MMM yyyy"));
        }

        [Test, AutoData]
        public void Then_Maps_PausedDate(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            ApprenticeshipDetailsCsvModel result = source;

            result.PausedDate.Should().Be(source.PauseDate.ToString("MMM yyyy"));
        }

        [Test, AutoData]
        public void Then_Maps_DateOfBirth(GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            ApprenticeshipDetailsCsvModel result = source;

            result.DateOfBirth.Should().Be(source.DateOfBirth.ToString("d MMM yyyy"));
        }

        [Test, AutoData]
        public void Then_Maps_Empty_If_No_PausedDate(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            source.PauseDate = DateTime.MinValue;
            ApprenticeshipDetailsCsvModel result = source;

            result.PausedDate.Should().BeEmpty();
        }

        [Test, AutoData]
        public void Then_Maps_ApprenticeConfirmation(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            ApprenticeshipDetailsCsvModel result = source;

            result.ApprenticeConfirmation.Should().Be(source.ConfirmationStatus.GetDescription());
        }

        [Test, AutoData]
        public void Then_Maps_Null_ApprenticeConfirmation(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            source.ConfirmationStatus = null;

            ApprenticeshipDetailsCsvModel result = source;

            result.ApprenticeConfirmation.Should().Be("N/A");
        }

        [Test, AutoData]
        public void Then_Maps_Status(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            ApprenticeshipDetailsCsvModel result = source;

            result.Status.Should().Be(source.ApprenticeshipStatus.GetDescription());
        }
        
        [Test, AutoData]
        public void Then_Maps_Reference(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            ApprenticeshipDetailsCsvModel result = source;

            result.CohortReference.Should().Be(source.CohortReference);
        }

        [Test, AutoData]
        public void Then_Maps_Your_Reference(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            ApprenticeshipDetailsCsvModel result = source;

            result.EmployerRef.Should().Be(source.EmployerRef);
        }

        [Test, AutoData]
        public void Then_Maps_Uln(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            ApprenticeshipDetailsCsvModel result = source;

            result.Uln.Should().Be(source.Uln);
        }

        [Test, AutoData]
        public void Then_Maps_TotalAgreedPrice(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            ApprenticeshipDetailsCsvModel result = source;

            result.TotalAgreedPrice.Should().Be($"{source.TotalAgreedPrice.Value as object:n0}");
        }

        [Test, MoqAutoData]
        public void Then_Maps_Alerts(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            var expectedAlertString = string.Empty;

            foreach (var alert in source.Alerts)
            {
                expectedAlertString += alert.GetDescription() + "|";
            }
            expectedAlertString = expectedAlertString.TrimEnd('|');

            ApprenticeshipDetailsCsvModel result = source;

            result.Alerts.Should().Be(expectedAlertString);
        }
    }
}