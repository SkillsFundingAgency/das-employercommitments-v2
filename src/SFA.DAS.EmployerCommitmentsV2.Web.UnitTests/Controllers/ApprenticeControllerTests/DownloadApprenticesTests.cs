using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Controllers;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Controllers.ApprenticeControllerTests
{
    public class DownloadApprenticesTests
    {
        [Test, MoqAutoData]
        public async Task ThenTheFileContentIsSetCorrectly(
            DownloadRequest request,
            [Frozen] DownloadViewModel expectedCsvContent,
            [Frozen] Mock<IModelMapper> csvMapper,
            ApprenticeController controller)
        {
            //Arrange
            csvMapper.Setup(x =>
                    x.Map<DownloadViewModel>(request))
                .ReturnsAsync(expectedCsvContent);

            //Act
            var actual = await controller.Download(request);

            var actualFileResult = actual as FileContentResult;

            //Assert
            Assert.IsNotNull(actualFileResult);
            Assert.AreEqual(expectedCsvContent.Content, actualFileResult.FileContents);
            Assert.AreEqual(expectedCsvContent.Name, actualFileResult.FileDownloadName);
            Assert.AreEqual(Constants.ApprenticesSearch.DownloadContentType, actualFileResult.ContentType);
        }
    }
}