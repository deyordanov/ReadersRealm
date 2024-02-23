namespace ReadersRealm.Web.Infrastructure.TagHelpers;

using Microsoft.AspNetCore.Razor.TagHelpers;

[HtmlTargetElement("input", Attributes = "asp-is-checked")]
public class CheckedTagHelper : TagHelper
{
    [HtmlAttributeName("asp-is-checked")]
    public bool IsChecked { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (this.IsChecked)
        {
            output.Attributes.SetAttribute("checked", "checked");
        }
    }
}