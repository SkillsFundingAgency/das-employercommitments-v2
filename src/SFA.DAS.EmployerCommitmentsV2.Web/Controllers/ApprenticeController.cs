using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.EmployerUserRoles.Options;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.CommitmentsV2.Shared.ActionResults;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.Employer.Shared.UI;
using SFA.DAS.Employer.Shared.UI.Attributes;
using SFA.DAS.EmployerCommitmentsV2.Features;
using SFA.DAS.EmployerCommitmentsV2.Web.Cookies;
using SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;
using SFA.DAS.EmployerCommitmentsV2.Web.RouteValues;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Controllers
{
    [Route("{accountHashedId}/apprentices")]
    [SetNavigationSection(NavigationSection.ApprenticesHome)]
    [DasAuthorize(EmployerUserRole.OwnerOrTransactor)]
    public class ApprenticeController : Controller
    {
        private readonly IModelMapper _modelMapper;
        private readonly ICookieStorageService<IndexRequest> _cookieStorage;

        public ApprenticeController(IModelMapper modelMapper, ICookieStorageService<IndexRequest> cookieStorage)
        {
            _modelMapper = modelMapper;
            _cookieStorage = cookieStorage;
        }

        [Route("", Name = RouteNames.ApprenticesIndex)]
        [DasAuthorize(EmployerFeature.ManageApprenticesV2)]
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
        [DasAuthorize(EmployerFeature.ManageApprenticesV2)]
        public async Task<IActionResult> Download(DownloadRequest request)
        {
            var downloadViewModel = await _modelMapper.Map<DownloadViewModel>(request);

            return new FileCallbackResult(downloadViewModel.ContentType, async (outputStream, _) =>
            {
                var moreData = true;
                while (moreData)
                {
                    using (var stream2 = await downloadViewModel.GetAndCreateContent(downloadViewModel.Request))
                    {
                        if (stream2.Length == 0)
                        {
                            moreData = false;
                        }

                        downloadViewModel.Request.PageNumber += 1;

                        stream2.CopyTo(outputStream);
                    }

                    downloadViewModel.Dispose();
                }

            })
            { FileDownloadName = downloadViewModel.Name };
        }
    }
}