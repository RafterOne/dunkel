using Sitecore.Configuration;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Web.UI;
using System;
using System.Web.UI;

namespace PixelMEDIA.SitecoreCMS.Controls.Controls
{
    public class CanonicalUrl : WebControl
    {
        protected static readonly ProviderHelper<LinkProvider, LinkProviderCollection> Helper =
            new ProviderHelper<LinkProvider, LinkProviderCollection>("linkManager");

        private bool _addAspxExtension = Helper.Provider.AddAspxExtension;
        private bool _alwaysIncludeServerUrl = Helper.Provider.AlwaysIncludeServerUrl;
        private bool _encodeNames = Helper.Provider.EncodeNames;
        private LanguageEmbedding _languageEmbedding = Helper.Provider.LanguageEmbedding;
        private LanguageLocation _languageLocation = Helper.Provider.LanguageLocation;
        private bool _shortenUrls = Helper.Provider.ShortenUrls;
        private bool _siteResolving = Settings.Rendering.SiteResolving;
        private bool _useDisplayName = Helper.Provider.UseDisplayName;

        public bool AddAspxExtension
        {
            get { return _addAspxExtension; }
            set { _addAspxExtension = value; }
        }

        public bool AlwaysIncludeServerUrl
        {
            get { return this._alwaysIncludeServerUrl; }
            set { this._alwaysIncludeServerUrl = value; }
        }

        public bool EncodeNames
        {
            get { return _encodeNames; }
            set { _encodeNames = value; }
        }

        public LanguageEmbedding LanguageEmbedding
        {
            get { return _languageEmbedding; }
            set { _languageEmbedding = value; }
        }

        public LanguageLocation LanguageLocation
        {
            get { return _languageLocation; }
            set { _languageLocation = value; }
        }

        public bool SiteResolving
        {
            get { return _siteResolving; }
            set { _siteResolving = value; }
        }

        public bool ShortenUrls
        {
            get { return _shortenUrls; }
            set { _shortenUrls = value; }
        }

        public bool UseDisplayName
        {
            get { return this._useDisplayName; }
            set { this._useDisplayName = value; }
        }

        /// <summary>
        ///     A server control that outputs a Canonical URL tag such as <link rel="canonical" href="hostname.com/foo/bar" />
        /// </summary>
        /// <param name="output"></param>
        /// <code><pxl:CanonicalUrl AddAspxExtension="True" AlwaysIncludeServerUrl="True" EncodeNames="True" LowercaseUrls="True"
        ///     SiteResolving="" ShortenUrls="True" UseDisplayName="True" runat="server" /></code>
        protected override void DoRender(HtmlTextWriter output)
        {
            Assert.ArgumentNotNull(output, "output");
            if (Sitecore.Context.Item == null)
            {
                return;
            }
            var options = new UrlOptions
                {
                    AddAspxExtension = AddAspxExtension,
                    AlwaysIncludeServerUrl = AlwaysIncludeServerUrl,
                    EncodeNames = EncodeNames,
                    LanguageEmbedding = LanguageEmbedding,
                    LanguageLocation = LanguageLocation,
                    SiteResolving = SiteResolving,
                    ShortenUrls = ShortenUrls,
                    UseDisplayName = UseDisplayName
                };

            output.AddAttribute(HtmlTextWriterAttribute.Rel, "canonical");
            output.AddAttribute(HtmlTextWriterAttribute.Href, LinkManager.GetItemUrl(Sitecore.Context.Item, options));
            output.RenderBeginTag(HtmlTextWriterTag.Link);
            output.RenderEndTag();
        }
    }
}