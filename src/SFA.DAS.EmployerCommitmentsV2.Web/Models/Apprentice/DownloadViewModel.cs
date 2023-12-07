using System;
using System.IO;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;

namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Apprentice;

public class DownloadViewModel
{
    public string Name { get; set; }
    public string ContentType => "application/octet-stream";
    public GetApprenticeshipsRequest Request { get; set; }
    public Stream Content { get; set; }
}