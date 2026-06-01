using FluentAssertions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests;

[TestFixture]
public class WhenCallingFinished
{
    [Test, MoqAutoData]
    public async Task ThenMapsTheRequestToViewModel(
        AddApprenticeshipCacheModel cacheModel,
        FinishedRequest request,
        FinishedViewModel viewModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheModel.ApprenticeshipSessionKey = (Guid)request.ApprenticeshipSessionKey;
        cacheStorageService
          .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
          .ReturnsAsync(cacheModel);

        mockMapper
            .Setup(x => x.Map<FinishedViewModel>(It.Is<FinishedRequest>(t => t == request)))
            .ReturnsAsync(viewModel);

        await controller.Finished(request);
        mockMapper.Verify(x => x.Map<FinishedViewModel>(It.Is<FinishedRequest>(t => t == request)), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task ThenReturnsView(
        FinishedViewModel viewModel,
        FinishedRequest request,
        AddApprenticeshipCacheModel cacheModel,
        [Frozen] Mock<IModelMapper> mockMapper,
        [Frozen] Mock<ICacheStorageService> cacheStorageService,
        [Greedy] CohortController controller)
    {
        cacheModel.ApprenticeshipSessionKey = (Guid)request.ApprenticeshipSessionKey;
        cacheStorageService
           .Setup(x => x.RetrieveFromCache<AddApprenticeshipCacheModel>(cacheModel.ApprenticeshipSessionKey))
           .ReturnsAsync(cacheModel);

        mockMapper
            .Setup(mapper => mapper.Map<FinishedViewModel>(request))
            .ReturnsAsync(viewModel);

        var result = await controller.Finished(request) as ViewResult;

        result.Should().NotBeNull();
        result.ViewName.Should().BeNull();
        result.Model.Should().BeEquivalentTo(viewModel);
    }

    [TestCase(FundingType.DirectTransfers, "Transfer funds from a connection")]
    [TestCase(FundingType.UnallocatedReservations, "Reserved funds")]
    [TestCase(FundingType.AdditionalReservations, "New reserved funds")]
    [TestCase(FundingType.CurrentLevy, "Current levy funds")]
    [TestCase(FundingType.LtmTransfers, "Levy transfer funds")]
    public void FundingType_Should_Return_Description(FundingType type , string description)
    {       
            var fundingDescription = type.GetEnumDescription();

            fundingDescription.Should().NotBeNullOrEmpty($"Description for {type} should not be null or empty.");
            fundingDescription.Should().Be(description);
        
    }
}