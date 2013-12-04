using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Web.UI;
using System;
using System.Web.UI;

namespace PixelMEDIA.SitecoreCMS.Controls.Controls
{
    public class CanonicalUrl : WebControl
    {
        public bool AlwaysIncludeServerUrl = true;
        public LanguageEmbedding LanguageEmbedding = LanguageEmbedding.Never;
        public bool SiteResolving = true;
        public bool UseDisplayName = false;

        /// <summary>
        ///     A server control that outputs a Canonical URL tag such as <link rel="canonical" href="hostname.com/foo/bar" />
        /// </summary>
        /// <param name="output"></param>
        /// <code><pxl:CanonicalUrl runat="server" AlwaysIncludeServerUrl="True" SiteResolving="" LanguageEmbedding="Never"
        ///     UseDisplayName="True" /></code>
        protected override void DoRender(HtmlTextWriter output)
        {
            Assert.ArgumentNotNull(output, "output");

            //
            // --- UseDisplayName - Defaults to false
            // --- AlwaysIncludeServerUrl - Defaults to true
            // --- LanguageEmbedding - Defaults to LanguageEmbedding.Never
            // --- SiteResolving - Defaults to true
            //

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