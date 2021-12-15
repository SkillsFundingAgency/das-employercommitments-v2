using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    public class WhenGettingConfirmProvider
    {
        [Test, MoqAutoData]
        public async Task Then_The_View_Is_Returned(
            int providerId,
            ConfirmProviderRequest confirmProviderRequest,
            GetProviderResponse getProviderResponse,
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            [Greedy] CohortController controller)
        {
            confirmProviderRequest.ProviderId = providerId;
            mockApiClient
                .Setup(x => x.GetProvider(providerId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(getProviderResponse);

            var result = await controller.ConfirmProvider(confirmProviderRequest) as ViewResult;

            result.ViewName.Should().BeNull();
        }
    }
}
