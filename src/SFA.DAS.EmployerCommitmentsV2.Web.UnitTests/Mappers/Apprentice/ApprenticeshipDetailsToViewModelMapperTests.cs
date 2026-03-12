using FluentAssertions;
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
    public async Task Then_Maps_EmploymentStatus_NullStatus_ReturnsEmpty(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        source.EmployerVerificationStatus = null;

        var result = await mapper.Map(source);

        result.EmploymentStatus.Should().Be(string.Empty);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_EmploymentStatus_Pending_ReturnsCheckPending(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        source.EmployerVerificationStatus = 0;

        var result = await mapper.Map(source);

        result.EmploymentStatus.Should().Be("Check Pending");
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_EmploymentStatus_Passed_ReturnsEmployed(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        source.EmployerVerificationStatus = 2;

        var result = await mapper.Map(source);

        result.EmploymentStatus.Should().Be("Employed");
    }

    [TestCase("HmrcFailure", "Not Verified")]
    [TestCase("NinoFailure", "Not Verified - missing or invalid NINO")]
    [TestCase("NinoInvalid", "Not Verified - missing or invalid NINO")]
    [TestCase("NinoNotFound", "Not verified - invalid NINO")]
    [TestCase("NinoAndPAYENotFound", "Not verified - missing PAYE scheme and invalid NINO")]
    [TestCase("PAYENotFound", "Not verified - missing PAYE scheme")]
    public async Task Then_Maps_EmploymentStatus_ErrorWithCode_ReturnsCorrectNotVerifiedString(string errorCode, string expected)
    {
        var source = new GetApprenticeshipsResponse.ApprenticeshipDetailsResponse
        {
            EmployerVerificationStatus = 4,
            EmployerVerificationNotes = errorCode,
            Alerts = []
        };
        var mapper = new ApprenticeshipDetailsToViewModelMapper(Mock.Of<IEncodingService>());

        var result = await mapper.Map(source);

        result.EmploymentStatus.Should().Be(expected);
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_EmploymentStatus_FailedNoNotes_ReturnsNotVerified(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        source.EmployerVerificationStatus = 3;
        source.EmployerVerificationNotes = null;

        var result = await mapper.Map(source);

        result.EmploymentStatus.Should().Be("Not Verified");
    }

    [Test, MoqAutoData]
    public async Task Then_Maps_EmploymentStatus_UnknownErrorCode_ReturnsNotVerified(
        GetApprenticeshipsResponse.ApprenticeshipDetailsResponse source,
        ApprenticeshipDetailsToViewModelMapper mapper)
    {
        source.EmployerVerificationStatus = 4;
        source.EmployerVerificationNotes = "SomeUnknownCode";

        var result = await mapper.Map(source);

        result.EmploymentStatus.Should().Be("Not Verified");
    }
}