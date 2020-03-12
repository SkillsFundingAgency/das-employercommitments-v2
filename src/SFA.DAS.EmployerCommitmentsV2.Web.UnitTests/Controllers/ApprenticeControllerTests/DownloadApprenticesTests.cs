using System.IO;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.ActionResults;
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
            string expectedFileName,
            [Frozen] Mock<IModelMapper> csvMapper,
            ApprenticeController controller)
        {
            //Arrange
            var expectedCsvContent = new DownloadViewModel
            {
                Name = expectedFileName,
                Content = new MemoryStream()
            };
            csvMapper.Setup(x =>
                    x.Map<DownloadViewModel>(request))
                .ReturnsAsync(expectedCsvContent);

            //Act
            var actual = await controller.Download(request);

            var actualFileResult = actual as FileResult;

            //Assert
            Assert.IsNotNull(actualFileResult);
            Assert.AreEqual(expectedCsvContent.Name, actualFileResult.FileDownloadName);
            Assert.AreEqual(expectedCsvContent.ContentType, actualFileResult.ContentType);
        }
    }
}