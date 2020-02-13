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
        public void Then_Maps_Status(
            GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source)
        {
            ApprenticeshipDetailsCsvModel result = source;

            result.Status.Should().Be(source.ApprenticeshipStatus.GetDescription());
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