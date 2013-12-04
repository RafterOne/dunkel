    using Sitecore.Diagnostics;
    using Sitecore.Links;
    using Sitecore.Web.UI;
    using System;
    using System.Web.UI;

namespace PixelMEDIA.SitecoreCMS.Controls.Controls
{
    public class CanonicalUrl : WebControl
    {
        public bool UseDisplayName = false;
        public bool AlwaysIncludeServerUrl = true;
        public LanguageEmbedding LanguageEmbedding = LanguageEmbedding.Never;
        public bool SiteResolving = true;
        /// <summary>
        /// A server control that outputs a Canonical URL tag such as <link rel=href="hostname.com/foo/bar" />
        /// </summary>
        /// <param name="output"></param>
        /// <param name="UseDisplayName">Defaults to false</param>
        /// <param name="AlwaysIncludeServerUrl">Defaults to true</param>
        /// <param name="LanguageEmbedding">Defaults to LanguageEmbedding.Never</param>
        /// <param name="SiteResolving">Defaults to true</param>
        protected override void DoRender(HtmlTextWriter output)
        {
            Assert.ArgumentNotNull(output, "output");
            var options = new UrlOptions
                {
                    AlwaysIncludeServerUrl = AlwaysIncludeServerUrl,
                    LanguageEmbedding = LanguageEmbedding,
                    SiteResolving = SiteResolving,
                    UseDisplayName = UseDisplayName
                };

            output.Write("<link rel=\"canonical\" href=\"{0}\" />", 
                (Sitecore.Context.Item == null
                             ? String.Empty
                             : LinkManager.GetItemUrl(Sitecore.Context.Item, options)));

        }
    }
}
