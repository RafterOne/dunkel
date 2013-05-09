using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Web;
using Sitecore.Sites;

namespace PixelMEDIA.SitecoreCMS.Controls.Helpers
{
	/// <summary>
	/// A set of helper methods for dealing with Sitecore sites (as defined in the SiteDefinition.config file)
	/// </summary>
	public class SiteHelpers
	{
		/// <summary>
		/// Retrieve the site for the specified language.
		/// Taken from http://briancaos.wordpress.com/2011/06/10/sitecore-language-selector-creating-a-country-selector/
		/// </summary>
		/// <param name="language">The language/country code to retrieve</param>
		/// <returns>The site for that language, as determined from the SiteDefinition.config file</returns>
		public static SiteInfo GetSite(String language)
		{
			try
			{
				return SiteContextFactory.Sites.First(s => s.Language == language && s.HostName != string.Empty);
			}
			catch (Exception e)
			{
				Sitecore.Diagnostics.Log.Error(e.Message, typeof(SiteHelpers));
			}

			return null;
		}

		/// <summary>
		/// Get the path to the home page of this site, taking into account the virtual folder that was set for it
		/// </summary>
		/// <returns>The path to use, for example, in a hyperlink to the home page</returns>
		public static String GetHomeUrl()
		{
			var site = Sitecore.Context.Site;
			if (null != site) return site.VirtualFolder;
			return "/";
		}
	}
}
