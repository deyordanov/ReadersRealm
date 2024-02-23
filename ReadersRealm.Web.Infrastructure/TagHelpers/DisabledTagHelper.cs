namespace ReadersRealm.Web.Infrastructure.TagHelpers;

using Microsoft.AspNetCore.Razor.TagHelpers;

[HtmlTargetElement("input", Attributes = "asp-is-disabled")]
public class DisabledTagHelper : TagHelper
{
    [HtmlAttributeName("asp-is-disabled")]
    public bool IsDisabled { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (this.IsDisabled)
        {
            output.Attributes.SetAttribute("disabled", "disabled");
        }
    }
}