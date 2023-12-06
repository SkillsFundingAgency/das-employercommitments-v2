using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class GetApprenticeshipsCsvContentMapperTests
    {
        [Test, MoqAutoData]
        public async Task Then_Passes_Filter_Args_To_Api(
            DownloadRequest csvRequest,
            long decodedAccountId,
            [Frozen] Mock<IEncodingService> mockEncodingService,
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            DownloadApprenticesRequestMapper mapper)
        {
            mockEncodingService
                .Setup(service => service.Decode(csvRequest.AccountHashedId, EncodingType.AccountId))
                .Returns(decodedAccountId);

            await mapper.Map(csvRequest);

            mockApiClient.Verify(client => client.GetApprenticeships(
                It.Is<GetApprenticeshipsRequest>(apiRequest =>
                    apiRequest.AccountId == decodedAccountId &&
                    apiRequest.SearchTerm == csvRequest.SearchTerm && 
                    apiRequest.ProviderName == csvRequest.SelectedProvider &&
                    apiRequest.CourseName == csvRequest.SelectedCourse &&
                    apiRequest.Status == csvRequest.SelectedStatus &&
                    apiRequest.ApprenticeConfirmationStatus == csvRequest.SelectedApprenticeConfirmation &&
                    apiRequest.PageNumber == 0 &&
                    apiRequest.EndDate == csvRequest.SelectedEndDate),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldMapValues()
        {
            //Arrange
            var fixture = new Fixture();
            var clientResponse = fixture.Create<GetApprenticeshipsResponse>();
            var request = fixture.Create<DownloadRequest>();
            var decodedAccountId = fixture.Create<long>();
            var mockEncodingService = new Mock<IEncodingService>();
            var client = new Mock<ICommitmentsApiClient>();
            var csvService = new Mock<ICreateCsvService>();
            var currentDateTime = new Mock<ICurrentDateTime>();
            var expectedCsvContent = new byte[] { 1, 2, 3, 4 };
            var expectedMemoryStream = new MemoryStream(expectedCsvContent);
            currentDateTime.Setup(x => x.UtcNow).Returns(new DateTime(2020, 12, 30));
            var expectedFileName = $"{"Manageyourapprentices"}_{currentDateTime.Object.UtcNow:yyyyMMddhhmmss}.csv";

            mockEncodingService
                .Setup(service => service.Decode(request.AccountHashedId, EncodingType.AccountId))
                .Returns(decodedAccountId);

            var mapper = new DownloadApprenticesRequestMapper(client.Object, csvService.Object, currentDateTime.Object, mockEncodingService.Object);

            client.Setup(x => x.GetApprenticeships(It.Is<GetApprenticeshipsRequest>(r => 
                    r.AccountId.Equals(decodedAccountId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResponse);

            csvService.Setup(x => x.GenerateCsvContent(It.IsAny<IEnumerable<ApprenticeshipDetailsCsvModel>>(), true))
                .Returns(expectedMemoryStream);

            //Act
            var content = await mapper.Map(request);

            //Assert
            Assert.That(content.Name, Is.EqualTo(expectedFileName));
            Assert.That(content.Content, Is.EqualTo(expectedMemoryStream));
        }
    }
}