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
        [Test, MoqAutoData]
        public async Task ThenValidatesViewModel(
            SelectProviderViewModel viewModel,
            CreateCohortController controller)
        {
            throw new NotImplementedException();
        }

        [Test, MoqAutoData]
        public async Task ThenCallsApiWithCorrectProviderId(
            SelectProviderViewModel viewModel,
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            GetProviderResponse apiResponse,
            CreateCohortController controller)
        {
            mockApiClient
                .Setup(x => x.GetProvider(viewModel.ProviderId, CancellationToken.None))
                .ReturnsAsync(apiResponse);

            var result = await controller.SelectProvider(viewModel);

            mockApiClient.Verify(x => x.GetProvider(viewModel.ProviderId, CancellationToken.None), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task ThenMapsConfirmProviderRequest(
            SelectProviderViewModel viewModel,
            ICommitmentsApiClient apiClient,
            GetProviderResponse apiResponse,
            CreateCohortController controller)
        {

        }

    }
}
