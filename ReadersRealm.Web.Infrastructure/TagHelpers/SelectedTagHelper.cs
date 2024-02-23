namespace ReadersRealm.Web.Infrastructure.TagHelpers;

using Microsoft.AspNetCore.Razor.TagHelpers;

[HtmlTargetElement("option", Attributes = "asp-is-selected")]
public class SelectedTagHelper : TagHelper
{
    [HtmlAttributeName("asp-is-selected")]
    public bool IsSelected { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (this.IsSelected)
        {
            output.Attributes.SetAttribute("selected", "selected");
        }
    }
}