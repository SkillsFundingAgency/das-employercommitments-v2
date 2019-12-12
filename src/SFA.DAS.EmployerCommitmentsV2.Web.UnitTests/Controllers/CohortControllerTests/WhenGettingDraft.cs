using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Cohort;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.CohortControllerTests
{
    [TestFixture]
    public class WhenGettingDraft
    {
        [Test, MoqAutoData]
        public async Task ThenMapperIsCalled(
            [Frozen] Mock<IModelMapper> modelMapper,
            CohortsByAccountRequest request,
            CohortController controller)
        {
            await controller.Draft(request);

            modelMapper.Verify(x => x.Map<DraftViewModel>(request), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task ThenViewIsReturned(
            [Frozen] Mock<IModelMapper> modelMapper,
            CohortsByAccountRequest request,
            DraftViewModel viewModel,
            CohortController controller)
        {
            modelMapper
                .Setup(x => x.Map<DraftViewModel>(request))
                .ReturnsAsync(viewModel);

            var result = await controller.Draft(request) as ViewResult;

            Assert.NotNull(result);
            Assert.AreEqual(result.Model, viewModel);
        }
    }
}
