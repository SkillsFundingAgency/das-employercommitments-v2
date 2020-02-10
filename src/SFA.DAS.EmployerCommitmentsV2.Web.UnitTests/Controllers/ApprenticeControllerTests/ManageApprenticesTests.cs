using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    [TestFixture]
    public class ManageApprenticesTests
    {
        [Test, MoqAutoData]
        public async Task ThenTheMappedViewModelIsReturned(
            IndexRequest request,
            IndexViewModel expectedViewModel,
            [Frozen] Mock<IModelMapper> apprenticeshipMapper,
            ApprenticeController controller)
        {
            //Arrange
            apprenticeshipMapper
                .Setup(mapper => mapper.Map<IndexViewModel>(request))
                .ReturnsAsync(expectedViewModel);

            //Act
            var result = await controller.Index(request) as ViewResult;
            var actualModel = result.Model as IndexViewModel;

            //Assert
            actualModel.Should().BeEquivalentTo(expectedViewModel);
        }

        [Test, MoqAutoData]
        public async Task Then_SortedByHeaderClassName_Set(
            IndexRequest request,
            IndexViewModel expectedViewModel,
            [Frozen] Mock<IModelMapper> apprenticeshipMapper,
            ApprenticeController controller)
        {
            //Arrange
            expectedViewModel.FilterModel.ReverseSort = false;
            apprenticeshipMapper
                .Setup(mapper => mapper.Map<IndexViewModel>(request))
                .ReturnsAsync(expectedViewModel);

            //Act
            var result = await controller.Index(request) as ViewResult;
            var actualModel = result.Model as IndexViewModel;

            //Assert
            actualModel.SortedByHeaderClassName.Should().EndWith("das-table__sort--asc");
        }
    }
}