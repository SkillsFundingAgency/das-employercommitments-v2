using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.EmployerCommitmentsV2.Web.Validators;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CreateCohortControllerTests
{
    [TestFixture]
    public class WhenPostingSelectProvider
    {
        [Test, MoqAutoData, Ignore("for now")]
        public async Task ThenValidatesViewModel(
            SelectProviderViewModel viewModel,
            CreateCohortController controller)
        {
            throw new NotImplementedException();
        }

        [Test, MoqAutoData]
        public async Task ThenCallsApiWithCorrectProviderId(
            SelectProviderViewModel viewModel,
            long providerId,
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            GetProviderResponse apiResponse,
            CreateCohortController controller)
        {
            viewModel.ProviderId = providerId.ToString();
            
            await controller.SelectProvider(viewModel);

            mockApiClient.Verify(x => x.GetProvider(providerId, CancellationToken.None), Times.Once);
        }

        [Test, MoqAutoData, Ignore("for now")]
        public async Task ThenMapsConfirmProviderRequest(
            SelectProviderViewModel viewModel,
            long providerId,
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            GetProviderResponse apiResponse,
            CreateCohortController controller)
        {
            viewModel.ProviderId = providerId.ToString();
            mockApiClient
                .Setup(x => x.GetProvider(providerId, CancellationToken.None))
                .ReturnsAsync(apiResponse);

        }

    }
}
