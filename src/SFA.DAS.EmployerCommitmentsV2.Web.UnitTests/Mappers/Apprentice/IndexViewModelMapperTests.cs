using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.EmployerCommitmentsV2.Web.Mappers.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.Encoding;
using SFA.DAS.Testing.AutoFixture;
using ApiRequests = SFA.DAS.CommitmentsV2.Api.Types.Requests;

namespace SFA.DAS.EmployerCommitmentsV2.Web.UnitTests.Mappers.Apprentice
{
    public class IndexViewModelMapperTests
    {
        [Test, MoqAutoData]
        public async Task Then_Defaults_To_Page_One_If_Less_Than_One(
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            IndexViewModelMapper mapper)
        {
            var request = new IndexRequest { PageNumber = 0 };

            await mapper.Map(request);

            mockApiClient.Verify(client => client.GetApprenticeships(It.Is<ApiRequests.GetApprenticeshipsRequest>(apiRequest =>
                        apiRequest.PageNumber == 1 &&
                        apiRequest.PageItemCount == Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Should_Pass_Params_To_Api_Call(
            IndexRequest webRequest,
            long decodedAccountId,
            [Frozen] Mock<IEncodingService> mockEncodingService,
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            IndexViewModelMapper mapper)
        {
            mockEncodingService
                .Setup(service => service.Decode(webRequest.AccountHashedId, EncodingType.AccountId))
                .Returns(decodedAccountId);

            await mapper.Map(webRequest);

            mockApiClient.Verify(client => client.GetApprenticeships(It.Is<ApiRequests.GetApprenticeshipsRequest>(apiRequest => 
                        apiRequest.AccountId == decodedAccountId &&
                        apiRequest.PageNumber == webRequest.PageNumber &&
                        apiRequest.PageItemCount == Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage &&
                        apiRequest.SearchTerm == webRequest.SearchTerm && 
                        apiRequest.ProviderName == webRequest.SelectedProvider &&
                        apiRequest.CourseName == webRequest.SelectedCourse &&
                        apiRequest.Status == webRequest.SelectedStatus &&
                        apiRequest.EndDate == webRequest.SelectedEndDate),
                    It.IsAny<CancellationToken>()), 
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_Filter_Values_From_Api(
            IndexRequest webRequest,
            long decodedAccountId,
            [Frozen] Mock<IEncodingService> mockEncodingService,
            GetApprenticeshipsResponse clientResponse,
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            IndexViewModelMapper mapper)
        {
            clientResponse.TotalApprenticeships =
                Constants.ApprenticesSearch.NumberOfApprenticesRequiredForSearch + 1;
            mockApiClient
                .Setup(client => client.GetApprenticeships(
                    It.IsAny<ApiRequests.GetApprenticeshipsRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResponse);
            mockEncodingService
                .Setup(service => service.Decode(webRequest.AccountHashedId, EncodingType.AccountId))
                .Returns(decodedAccountId);
           
            await mapper.Map(webRequest);

            mockApiClient.Verify(client => client.GetApprenticeshipsFilterValues(
                It.Is<ApiRequests.GetApprenticeshipFiltersRequest>(request => request.AccountId == decodedAccountId),
                It.IsAny<CancellationToken>()), 
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task And_TotalApprentices_Less_Than_NumberOfApprenticesRequiredForSearch_Then_Not_Get_Filter_Values_From_Api(
            IndexRequest webRequest,
            GetApprenticeshipsResponse clientResponse,
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            IndexViewModelMapper mapper)
        {
            clientResponse.TotalApprenticeships = Constants.ApprenticesSearch.NumberOfApprenticesRequiredForSearch - 1;
            
            mockApiClient
                .Setup(client => client.GetApprenticeships(
                    It.IsAny<ApiRequests.GetApprenticeshipsRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResponse);

            await mapper.Map(webRequest);

            mockApiClient.Verify(client => client.GetApprenticeshipsFilterValues(
                    It.IsAny<ApiRequests.GetApprenticeshipFiltersRequest>(), 
                    It.IsAny<CancellationToken>()),
                Times.Never); 
        }

        [Test, MoqAutoData]
        public async Task ShouldMapApiValues(
            IndexRequest request,
            GetApprenticeshipsResponse apprenticeshipsResponse,
            GetApprenticeshipsFilterValuesResponse filtersResponse,
            ApprenticeshipDetailsViewModel expectedViewModel,
            [Frozen] Mock<IModelMapper> modelMapper,
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            IndexViewModelMapper mapper)
        {
            //Arrange
            apprenticeshipsResponse.TotalApprenticeships =
                Constants.ApprenticesSearch.NumberOfApprenticesRequiredForSearch + 1;
            
            mockApiClient
                .Setup(x => x.GetApprenticeships(
                    It.IsAny<ApiRequests.GetApprenticeshipsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apprenticeshipsResponse);
            
            mockApiClient
                .Setup(client => client.GetApprenticeshipsFilterValues(
                    It.IsAny<ApiRequests.GetApprenticeshipFiltersRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(filtersResponse);
            
            modelMapper
                .Setup(x => x.Map<ApprenticeshipDetailsViewModel>(It.IsAny<GetApprenticeshipsResponse.ApprenticeshipDetailsResponse>()))
                .ReturnsAsync(expectedViewModel);

            //Act
            var viewModel = await mapper.Map(request);

            //Assert
            Assert.IsNotNull(viewModel);
            Assert.AreEqual(request.AccountHashedId, viewModel.AccountHashedId);
            viewModel.Apprenticeships.Should().AllBeEquivalentTo(expectedViewModel);
            Assert.AreEqual(apprenticeshipsResponse.TotalApprenticeshipsFound, viewModel.FilterModel.TotalNumberOfApprenticeshipsFound);
            Assert.AreEqual(apprenticeshipsResponse.TotalApprenticeshipsWithAlertsFound, viewModel.FilterModel.TotalNumberOfApprenticeshipsWithAlertsFound);
            Assert.AreEqual(apprenticeshipsResponse.TotalApprenticeships, viewModel.FilterModel.TotalNumberOfApprenticeships);
            Assert.AreEqual(apprenticeshipsResponse.PageNumber, viewModel.FilterModel.PageNumber);
            Assert.AreEqual(request.ReverseSort,viewModel.FilterModel.ReverseSort);
            Assert.AreEqual(request.SortField, viewModel.FilterModel.SortField);
            Assert.AreEqual(filtersResponse.ProviderNames, viewModel.FilterModel.ProviderFilters);
            Assert.AreEqual(filtersResponse.CourseNames, viewModel.FilterModel.CourseFilters);
            Assert.AreEqual(filtersResponse.EndDates, viewModel.FilterModel.EndDateFilters);
            Assert.AreEqual(request.SearchTerm, viewModel.FilterModel.SearchTerm);
            Assert.AreEqual(request.SelectedProvider, viewModel.FilterModel.SelectedProvider);
            Assert.AreEqual(request.SelectedCourse, viewModel.FilterModel.SelectedCourse);
            Assert.AreEqual(request.SelectedStatus, viewModel.FilterModel.SelectedStatus);
            Assert.AreEqual(request.SelectedEndDate, viewModel.FilterModel.SelectedEndDate);
        }

        [Test, MoqAutoData]
        public async Task ShouldMapStatusValues(
            IndexRequest request,
            GetApprenticeshipsResponse apprenticeshipsResponse,
            GetApprenticeshipsFilterValuesResponse filtersResponse,
            ApprenticeshipDetailsViewModel expectedViewModel,
            [Frozen]
            Mock<IMapper<GetApprenticeshipsResponse.ApprenticeshipDetailsResponse, ApprenticeshipDetailsViewModel>>
                detailsViewModelMapper,
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            IndexViewModelMapper mapper)
        {
            //Arrange
            apprenticeshipsResponse.TotalApprenticeships =
                Constants.ApprenticesSearch.NumberOfApprenticesRequiredForSearch + 1;

            mockApiClient
                .Setup(x => x.GetApprenticeships(
                    It.IsAny<ApiRequests.GetApprenticeshipsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apprenticeshipsResponse);

            mockApiClient
                .Setup(client => client.GetApprenticeshipsFilterValues(
                    It.IsAny<ApiRequests.GetApprenticeshipFiltersRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(filtersResponse);

            detailsViewModelMapper
                .Setup(x => x.Map(It.IsAny<GetApprenticeshipsResponse.ApprenticeshipDetailsResponse>()))
                .ReturnsAsync(expectedViewModel);

            //Act
            var viewModel = await mapper.Map(request);
            
            Assert.IsTrue(viewModel.FilterModel.StatusFilters.Contains(ApprenticeshipStatus.Live));
            Assert.IsTrue(viewModel.FilterModel.StatusFilters.Contains(ApprenticeshipStatus.Paused));
            Assert.IsTrue(viewModel.FilterModel.StatusFilters.Contains(ApprenticeshipStatus.Stopped));
            Assert.IsTrue(viewModel.FilterModel.StatusFilters.Contains(ApprenticeshipStatus.WaitingToStart));
            Assert.IsFalse(viewModel.FilterModel.StatusFilters.Contains(ApprenticeshipStatus.Unknown));
            Assert.IsFalse(viewModel.FilterModel.StatusFilters.Contains(ApprenticeshipStatus.Completed));
        }

        [Test, MoqAutoData]
        public async Task ThenWillSetPageNumberToLastOneIfRequestPageNumberIsTooHigh(
            IndexRequest request,
            GetApprenticeshipsResponse apprenticeshipsResponse,
            GetApprenticeshipsFilterValuesResponse filtersResponse,
            ApprenticeshipDetailsViewModel expectedViewModel,
            [Frozen]
            Mock<IMapper<GetApprenticeshipsResponse.ApprenticeshipDetailsResponse, ApprenticeshipDetailsViewModel>>
                detailsViewModelMapper,
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            IndexViewModelMapper mapper)
        {
            //Arrange
            apprenticeshipsResponse.PageNumber = (int)Math.Ceiling((double)apprenticeshipsResponse.TotalApprenticeshipsFound / Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage);
            request.PageNumber = apprenticeshipsResponse.PageNumber + 10;

            apprenticeshipsResponse.TotalApprenticeships =
                Constants.ApprenticesSearch.NumberOfApprenticesRequiredForSearch + 1;

            mockApiClient
                .Setup(x => x.GetApprenticeships(
                    It.IsAny<ApiRequests.GetApprenticeshipsRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(apprenticeshipsResponse);

            mockApiClient
                .Setup(client => client.GetApprenticeshipsFilterValues(
                    It.IsAny<ApiRequests.GetApprenticeshipFiltersRequest>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(filtersResponse);

            detailsViewModelMapper
                .Setup(x => x.Map(It.IsAny<GetApprenticeshipsResponse.ApprenticeshipDetailsResponse>()))
                .ReturnsAsync(expectedViewModel);

            //Act
            var viewModel = await mapper.Map(request);

            //Assert
            Assert.AreEqual(1, viewModel.FilterModel.PageLinks.Count(x => x.IsCurrent.HasValue && x.IsCurrent.Value));
            Assert.IsTrue(viewModel.FilterModel.PageLinks.Last().IsCurrent);
        }
    }
}