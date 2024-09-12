using FluentAssertions;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Cohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Cohort;

[TestFixture]
public class WhenMappingInformRequestToInformViewModelTests
{
    [Test, MoqAutoData]
    public async Task Then_AccountHashedId_Is_Mapped(
        AccountResponse accountResponse,
        InformRequest informRequest,
        [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient,
        InformRequestToInformViewModelMapper mapper)
    {
        // Arrange
        commitmentsApiClient
            .Setup(x => x.GetAccount(informRequest.AccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountResponse);
        
        //Act
        var result = await mapper.Map(informRequest);

        //Assert           
        result.AccountHashedId.Should().Be(informRequest.AccountHashedId);
    }

    [MoqInlineAutoData(ApprenticeshipEmployerType.Levy, true)]
    [MoqInlineAutoData(ApprenticeshipEmployerType.NonLevy, false)]
    public async Task Then_LevyStatus_Is_Mapped(
        ApprenticeshipEmployerType levyStatus,
        bool expectedIsLevy,
        [Frozen] Mock<ICommitmentsApiClient> commitmentsApiClient,
        AccountResponse accountResponse,
        InformRequest indexRequest,
        InformRequestToInformViewModelMapper mapper)
    {
        // Arrange
        accountResponse.LevyStatus = levyStatus;
        commitmentsApiClient
            .Setup(x => x.GetAccount(indexRequest.AccountId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountResponse);
        
        //Act
        var result = await mapper.Map(indexRequest);

        //Assert           
        result.IsLevyFunded.Should().Be(expectedIsLevy);
    }
}