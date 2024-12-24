using FluentAssertions;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

public class WhenMappingConfirmProviderViewModelToAssignRequest
{
    [Test, AutoData]
    public async Task ThenMapsReservationId(
        ConfirmProviderViewModel request,
        AssignRequestMapper mapper)
    {
        var result = await mapper.Map(request);

        result.ReservationId.Should().Be(request.ReservationId);
    }

    [Test, MoqAutoData]
    public async Task ThenMapsAccountHashedId(
        ConfirmProviderViewModel request,
        AssignRequestMapper mapper)
    {
        var result = await mapper.Map(request);

        result.AccountHashedId.Should().Be(request.AccountHashedId);
    }

    [Test, MoqAutoData]
    public async Task ThenMapsCourseCode(
        ConfirmProviderViewModel request,
        AssignRequestMapper mapper)
    {
        var result = await mapper.Map(request);

        result.CourseCode.Should().Be(request.CourseCode);
    }

    [Test, MoqAutoData]
    public async Task ThenMapsStartMonthYear(
        ConfirmProviderViewModel request,
        AssignRequestMapper mapper)
    {
        var result = await mapper.Map(request);

        result.StartMonthYear.Should().Be(request.StartMonthYear);
    }

    [Test, MoqAutoData]
    public async Task ThenMapsEmployerAccountLegalEntityPublicHashedId(
        ConfirmProviderViewModel request,
        AssignRequestMapper mapper)
    {
        var result = await mapper.Map(request);

        result.AccountLegalEntityHashedId.Should().Be(request.AccountLegalEntityHashedId);
    }

    [Test, MoqAutoData]
    public async Task ThenMapsProviderId(
        ConfirmProviderViewModel request,
        AssignRequestMapper mapper)
    {
        var result = await mapper.Map(request);

        result.ProviderId.Should().Be(request.ProviderId);
    }

    [Test, MoqAutoData]
    public async Task ThenMapsTransferSenderId(
        ConfirmProviderViewModel request,
        AssignRequestMapper mapper)
    {
        var result = await mapper.Map(request);

        result.TransferSenderId.Should().Be(request.TransferSenderId);
    }

    [Test, MoqAutoData]
    public async Task ThenMapsFundingType(
        ConfirmProviderViewModel request,
        AssignRequestMapper mapper)
    {
        var result = await mapper.Map(request);

        result.FundingType.Should().Be(request.FundingType);
    }
}