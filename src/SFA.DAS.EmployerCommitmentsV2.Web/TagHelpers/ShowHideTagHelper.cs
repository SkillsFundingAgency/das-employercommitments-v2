using Microsoft.AspNetCore.Razor.TagHelpers;

namespace SFA.DAS.EmployerCommitmentsV2.Web.TagHelpers;

[HtmlTargetElement(Attributes = "asp-hide")]
public class HideTagHelper : TagHelper
{
    public bool? AspHide { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (AspHide ?? false) output.SuppressOutput();
    }
}

[HtmlTargetElement(Attributes = "asp-show")]
public class ShowTagHelper : TagHelper
{
    public bool? AspShow { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (!(AspShow ?? false)) output.SuppressOutput();
    }
}