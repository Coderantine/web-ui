using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Coderantine.TagHelpers
{
    [HtmlTargetElement(Attributes = "is-active-route")]
    public class ActiveRouteTagHelper : TagHelper
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public ActiveRouteTagHelper(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        private IDictionary<string, string> _routeValues;


        [HtmlAttributeName("asp-page")]
        public string Page { get; set; }

        [HtmlAttributeName("asp-page-handler")]
        public string Handler { get; set; }

        /// <summary>Additional parameters for the route.</summary>
        [HtmlAttributeName("asp-all-route-data", DictionaryAttributePrefix = "asp-route-")]
        public IDictionary<string, string> RouteValues
        {
            get
            {
                if (this._routeValues == null)
                    this._routeValues = (IDictionary<string, string>)new Dictionary<string, string>((IEqualityComparer<string>)StringComparer.OrdinalIgnoreCase);
                return this._routeValues;
            }
            set
            {
                this._routeValues = value;
            }
        }


        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            if (ShouldBeActive())
            {
                MakeActive(output);
            }

            output.Attributes.RemoveAll("is-active-route");
        }

        private bool ShouldBeActive()
        {

            if (Page != null)
            {
                if (Page.ToLower() == "index" && _contextAccessor.HttpContext.Request.Path.Value.ToLower() == "/")
                {
                    return true;
                }
                else if (!string.IsNullOrWhiteSpace(Page) && $"/{Page.ToLower()}" != _contextAccessor.HttpContext.Request.Path.Value.ToLower())
                {
                    return false;
                }
            }

            foreach (KeyValuePair<string, string> routeValue in RouteValues)
            {
                if (!ViewContext.RouteData.Values.ContainsKey(routeValue.Key) ||
                    ViewContext.RouteData.Values[routeValue.Key].ToString() != routeValue.Value)
                {
                    return false;
                }
            }

            return true;
        }

        private void MakeActive(TagHelperOutput output)
        {
            var classAttr = output.Attributes.FirstOrDefault(a => a.Name == "class");
            if (classAttr == null)
            {
                classAttr = new TagHelperAttribute("class", "active");
                output.Attributes.Add(classAttr);
            }
            else if (classAttr.Value == null || classAttr.Value.ToString().IndexOf("active") < 0)
            {
                output.Attributes.SetAttribute("class", classAttr.Value == null
                    ? "active"
                    : classAttr.Value.ToString() + " active");
            }
        }
    }
}