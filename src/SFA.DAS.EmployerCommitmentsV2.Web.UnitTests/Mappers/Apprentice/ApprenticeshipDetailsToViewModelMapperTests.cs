using AutoFixture.NUnit3;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.Encoding;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice;

public class ApprenticeshipDetailsToViewModelMapperTests
{
    [Test, MoqAutoData]
    public async Task Then_Maps_ApprenticeshipId(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        string encodedApprenticeshipId,
        [Frozen] Mock<IEncodingService> mockEncodingService,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        mockEncodingService
            .Setup(service => service.Encode(source.Id, EncodingType.ApprenticeshipId))
            .Returns(encodedApprenticeshipId);

        var result = await mapper.Map(source);

        result.EncodedApprenticeshipId.Should().Be(encodedApprenticeshipId);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_ApprenticeName(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        var result = await mapper.Map(source);

        result.ApprenticeName.Should().Be($"{source.FirstName} {source.LastName}");
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_ProviderName(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        var result = await mapper.Map(source);

        result.ProviderName.Should().Be(source.ProviderName);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_CourseName(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        var result = await mapper.Map(source);

        result.CourseName.Should().Be(source.CourseName);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_PlannedStartDate(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        var result = await mapper.Map(source);

        result.PlannedStartDate.Should().Be(source.StartDate);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_PlannedEndDate(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        var result = await mapper.Map(source);

        result.PlannedEndDate.Should().Be(source.EndDate);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_ApprenticeshipStatus(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        var result = await mapper.Map(source);

        result.Status.Should().Be(source.ApprenticeshipStatus);
    }


    [Test, MoqAutoData]
    public async Task Then_Maps_ConfirmationStatus(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        var result = await mapper.Map(source);

        result.ConfirmationStatus.Should().Be(source.ConfirmationStatus);
    }


    [Test, MoqAutoData]
    public async Task Then_Maps_Alerts(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        var alertStrings = source.Alerts.Select(x => x.GetDescription());

        var result = await mapper.Map(source);

        result.Alerts.Should().BeEquivalentTo(alertStrings);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_ActualStartDate(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        var result = await mapper.Map(source);

        result.ActualStartDate.Should().Be(source.ActualStartDate);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_IsOnFlexiPaymentPilot(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        var result = await mapper.Map(source);

        result.IsOnFlexiPaymentPilot.Should().Be(source.IsOnFlexiPaymentPilot);
    }
}