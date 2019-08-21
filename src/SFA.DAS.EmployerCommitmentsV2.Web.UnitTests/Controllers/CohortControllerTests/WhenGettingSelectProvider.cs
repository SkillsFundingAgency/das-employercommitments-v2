using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    [TestFixture]
    public class WhenGettingSelectProvider
    {
        [Test, MoqAutoData]
        public async Task ThenMapsTheRequestToViewModel(
            SelectProviderRequest request,
            SelectProviderViewModel viewModel,
            [Frozen] Mock<IModelMapper> mockMapper,
            CohortController controller)
        {
            await controller.SelectProvider(request);

            mockMapper.Verify(x => x.Map<SelectProviderViewModel>(It.IsAny<SelectProviderRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task ThenReturnsView(
            SelectProviderRequest request,
            SelectProviderViewModel viewModel,
            [Frozen] Mock<IModelMapper> mockMapper,
            CohortController controller)
        {
            mockMapper
                .Setup(mapper => mapper.Map<SelectProviderViewModel>(request))
                .ReturnsAsync(viewModel);

            var result = await controller.SelectProvider(request) as ViewResult;

            Assert.Null(result.ViewName);
            Assert.AreSame(viewModel, result.Model);
        }
    }
}
