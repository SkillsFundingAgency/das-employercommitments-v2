namespace SFA.DAS.EmployerCommitmentsV2.Web.Models.Shared;

public class PageLink
{
    public string Label { get; set; }
    public string AriaLabel { get; set; }
    public bool? IsCurrent { get; set; }
    public Dictionary<string, string> RouteData { get; set; }
}