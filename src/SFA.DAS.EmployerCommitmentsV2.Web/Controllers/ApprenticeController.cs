﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.Employer.Shared.UI.Attributes;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Cookies;
using SFA.DAS.EmployerCommitmentsV2.Web.Extensions;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;
using SFA.DAS.EmployerUrlHelper;
using EditEndDateRequest = SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice.EditEndDateRequest;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [Route("{accountHashedId}/apprentices")]
    [SetNavigationSection(NavigationSection.ApprenticesHome)]
    [DasAuthorize(EmployerUserRole.OwnerOrTransactor)]
    public class ApprenticeController : Controller
    {
        private readonly IModelMapper _modelMapper;
        private readonly ICookieStorageService<IndexRequest> _cookieStorage;
        private readonly ICommitmentsApiClient _commitmentsApiClient;
        private readonly ILinkGenerator _linkGenerator;

        public ApprenticeController(IModelMapper modelMapper, ICookieStorageService<IndexRequest> cookieStorage, ICommitmentsApiClient commitmentsApiClient, ILinkGenerator linkGenerator)
        {
            _modelMapper = modelMapper;
            _cookieStorage = cookieStorage;
            _commitmentsApiClient = commitmentsApiClient;
            _linkGenerator = linkGenerator;
        }

        [Route("", Name = RouteNames.ApprenticesIndex)]
        public async Task<IActionResult> Index(IndexRequest request)
        {
            IndexRequest savedRequest = null;

            if (request.FromSearch)
            {
                savedRequest = _cookieStorage.Get(CookieNames.ManageApprentices);

                if (savedRequest != null)
                {
                    request = savedRequest;
                }
            }

            if (savedRequest == null)
            {
                _cookieStorage.Update(CookieNames.ManageApprentices, request);
            }

            var viewModel = await _modelMapper.Map<IndexViewModel>(request);
            viewModel.SortedByHeader();

            return View(viewModel);
        }

        [Route("download", Name = RouteNames.ApprenticesDownload)]
        public async Task<IActionResult> Download(DownloadRequest request)
        {
            var downloadViewModel = await _modelMapper.Map<DownloadViewModel>(request);

            return File(downloadViewModel.Content, downloadViewModel.ContentType, downloadViewModel.Name);
        }

        [Route("{apprenticeshipHashedId}/details/editenddate", Name = RouteNames.ApprenticeEditEndDate)]
        public async Task<IActionResult> EditEndDate(EditEndDateRequest request)
        {
            var viewModel = await _modelMapper.Map<EditEndDateViewModel>(request);
            return View(viewModel);
        }

        [Route("{apprenticeshipHashedId}/details/editenddate", Name = RouteNames.ApprenticeEditEndDate)]
        [HttpPost]
        public async Task<IActionResult> EditEndDate(EditEndDateViewModel viewModel)
        {
            var request = await _modelMapper.Map<CommitmentsV2.Api.Types.Requests.EditEndDateRequest>(viewModel);
            await _commitmentsApiClient.UpdateEndDateOfCompletedRecord(request, CancellationToken.None);
            var url = _linkGenerator.ApprenticeDetails(viewModel.AccountHashedId, viewModel.ApprenticeshipHashedId);
            return Redirect(url);
        }

        [Route("{apprenticeshipHashedId}/details/changing-training-provider", Name = RouteNames.ChangeProviderInform)]
        [DasAuthorize(EmployerFeature.ChangeOfProvider)]
        public async Task<IActionResult> ChangeProviderInform(ChangeProviderInformRequest request)
        {
            var viewModel = await _modelMapper.Map<ChangeProviderInformViewModel>(request);

            return View(viewModel);
        }

        // Placeholder for CON-2516 - url not specified yet
        [Route("{apprenticeshipHashedId}/details/stopped-error", Name = RouteNames.ApprenticeNotStoppedError)]
        [DasAuthorize(EmployerFeature.ChangeOfProvider)]
        public IActionResult ApprenticeNotStoppedError()
        {
            return View();
        }

        // Placeholder for CON-2505
        [Route("{apprenticeshipHashedId}/details/enter-new-training-provider-name-or-reference-number", Name = RouteNames.EnterNewTrainingProvider)]
        [DasAuthorize(EmployerFeature.ChangeOfProvider)]
        public IActionResult EnterNewTrainingProvider()
        {
            return View();
        }
    }
}