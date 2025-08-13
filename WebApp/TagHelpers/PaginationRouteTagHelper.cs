using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace WebApp.TagHelpers
{
    [HtmlTargetElement("pagination-route")]
    public class PaginationRouteTagHelper : TagHelper
    {
        [HtmlAttributeName("page-number")]
        public int PageNumber { get; set; }

        [HtmlAttributeName("view-context")]
        [ViewContext]
        public ViewContext ViewContext { get; set; } = default!;

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.Attributes.SetAttribute("class", "page-link");

            var queryParams = ViewContext.HttpContext.Request.Query
                .Where(q => q.Key != "pageNumber")
                .ToDictionary(q => q.Key, q => q.Value.ToString());

            var urlBuilder = new StringBuilder($"?pageNumber={PageNumber}");
            foreach (var param in queryParams)
            {
                urlBuilder.Append($"&{param.Key}={param.Value}");
            }

            output.Attributes.SetAttribute("href", urlBuilder.ToString());
        }
    }
}