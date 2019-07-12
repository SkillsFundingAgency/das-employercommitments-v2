using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerCommitmentsV2.Application.Queries.Providers;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.CreateCohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.AddDraftApprenticeshipToNewCohortControllerTests
{
    public class WhenGettingConfirmProvider
    {
        [Test, MoqAutoData]
        public async Task Then_The_View_Is_Returned(
            int ukPrn,
            ConfirmProviderRequest confirmProviderRequest,
            GetProviderResponse getProviderResponse,
            [Frozen]Mock<IMediator> mediator,
            CreateCohortController controller)
        {
            confirmProviderRequest.ProviderId = ukPrn;
            mediator.Setup(x => x.Send(It.Is<GetProviderRequest>(c => c.UkPrn.Equals(ukPrn)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(getProviderResponse);

            var result = await controller.ConfirmProvider(confirmProviderRequest) as ViewResult;

            result.ViewName.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_The_Provider_Details_Are_Populated_From_The_UkPrn(
            int ukPrn,
            ConfirmProviderRequest confirmProviderRequest,
            GetProviderResponse getProviderResponse,
            [Frozen]Mock<IMediator> mediator,
            CreateCohortController controller)
        {
            confirmProviderRequest.ProviderId = ukPrn;
            mediator.Setup(x => x.Send(It.Is<GetProviderRequest>(c => c.UkPrn.Equals(ukPrn)),It.IsAny<CancellationToken>()))
                .ReturnsAsync(getProviderResponse);

            var result = await controller.ConfirmProvider(confirmProviderRequest) as ViewResult;

            var actualModel = result.Model as ConfirmProviderViewModel;
            actualModel.ProviderId.Should().Be(getProviderResponse.ProviderId);
            actualModel.ProviderName.Should().Be(getProviderResponse.ProviderName);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Result_Is_Not_Returned_A_Bad_Request_Is_Returned(
            int ukPrn,
            ConfirmProviderRequest confirmProviderRequest,
            GetProviderResponse getProviderResponse,
            [Frozen]Mock<IMediator> mediator,
            CreateCohortController controller)
        {
            //var result = await controller.ConfirmProvider(confirmProviderRequest) as BadRequestResult;
        }
    }
}
