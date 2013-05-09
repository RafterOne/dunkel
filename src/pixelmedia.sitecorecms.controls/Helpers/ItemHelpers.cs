using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Fields;
using Sitecore.Shell.Framework;
using Sitecore.Data.Items;
using Sitecore.Collections;
using Sitecore.Data;
using Sitecore.Resources.Media;
using Sitecore.Web.UI.WebControls;
using Sitecore.Layouts;

namespace PixelMEDIA.SitecoreCMS.Controls.Helpers
{
	/// <summary>
	/// A set of helper methods for dealing with Sitecore items
	/// </summary>
	public class ItemHelpers
	{
		private static MediaUrlOptions _mediaUrlOptions;

		/// <summary>
		/// The standard options used when retrieving a media item's URL.
		/// </summary>
		public static MediaUrlOptions MediaUrlOptions
		{
			get
			{
				if (null == _mediaUrlOptions)
				{
					_mediaUrlOptions = new MediaUrlOptions();

					// TODO: populate any additional options here
				}

				return _mediaUrlOptions;
			}
		}

		private static Sitecore.Links.UrlOptions _linkUrlOptions;

        // Default UrlOptions to pass to LinkManager
        public static Sitecore.Links.UrlOptions UrlOptions
        {
            get
            {
                if (null == _linkUrlOptions)
                {
                    _linkUrlOptions = Sitecore.Links.UrlOptions.DefaultOptions.Clone() as Sitecore.Links.UrlOptions;
                    _linkUrlOptions.ShortenUrls = true;
                    _linkUrlOptions.SiteResolving = true;
                    _linkUrlOptions.AlwaysIncludeServerUrl = false;
                    _linkUrlOptions.LanguageEmbedding = Sitecore.Links.LanguageEmbedding.Never;
                }

                return _linkUrlOptions;
            }
        }
		
		#region mediahelpers
        
		/// <summary>
        /// Retrieves a URL for the specified media item
        /// </summary>
        /// <param name="itemId">The ID of the media item to retrieve</param>
        /// <returns>A URL, or an empty string if it could not be found</returns> 
        public static String GetMediaUrl(Sitecore.Data.ID itemId)
        {
            Sitecore.Data.Items.Item item = Sitecore.Context.Database.GetItem(itemId);
            return GetMediaUrl(item);
        }

        /// <summary>
        /// Retrieves a URL for the specified media item
        /// </summary>
        /// <param name="itemId">The the media item to retrieve</param>
        /// <returns>A URL, or an empty string if it could not be found</returns> 
        public static String GetMediaUrl(Sitecore.Data.Items.Item item)
        {
            if (null != item)
            {
                var media = new Sitecore.Data.Items.MediaItem(item);
                return MediaManager.GetMediaUrl(media, MediaUrlOptions);
            }

            return String.Empty;
        }

		/// <summary>
		/// Helper method to get the path to an image from an ImageField
		/// </summary>
		/// <param name="field"></param>
		/// <returns></returns>
		public static String GetImageSrcFromImageField(Field field)
		{
			// TODO: Check that it's an ImageField
			if (null != field)
			{
				var item = ((Sitecore.Data.Fields.ImageField)field).MediaItem;
				return GetMediaUrl(item);
			}

			return String.Empty;
		}

		#endregion mediahelpers

		#region templatehelpers

		/// <summary>
		/// Is Template (Compares Template ID to Item.Template.ID)
		/// </summary>
		/// <param name="item">Source Item</param>
		/// <param name="template">Source Template</param>
		/// <returns>bool</returns>
        public static bool IsTemplate(Sitecore.Data.Items.Item item, ID templateId)
		{
			return (item.Template.ID.Equals(templateId));
		}

		/// <summary>
		/// Is Template (Compares a List of Templates IDs to Item.Template.ID)
		/// </summary>
		/// <param name="item">Source Item</param>
		/// <param name="templates">Source Template List</param>
		/// <returns>bool</returns>
        public static bool IsTemplate(Sitecore.Data.Items.Item item, List<ID> templateIds)
		{
			foreach (ID id in templateIds)
			{
				if (item.Template.ID.Equals(id))
				{ return true; }
			}
			return false;
		}

		/// <summary>
		/// Determines whether the specified item uses the specified base template.  Note that this only checks
		/// the immediate base templates; it does not recursively check their base templates.
		/// </summary>
		/// <param name="x"></param>
		/// <returns></returns>
        public static bool UsesBaseTemplate(Item item, ID templateId)
		{
			if (null != item)
			{
				if (item.TemplateID.Equals(templateId)) return true;

				var baseTemplate = item.Template.BaseTemplates.Where(t => t.ID.Equals(templateId));
				return (null != baseTemplate && baseTemplate.Any());
			}

			return false;
		}

		#endregion templatehelpers

		#region languagehelpers

	    /// <summary>
        /// Determines whether the specified item exists for (has a version in) the user's current language.
        /// Taken from Sean Kearney, http://stackoverflow.com/questions/8231987/check-if-items-exist-in-the-current-language
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <returns>True if a version in the current language exists; otherwise, false</returns>
        public static bool HasLanguageVersion(Sitecore.Data.Items.Item item)
        {
            return HasLanguageVersion(item, Sitecore.Context.Language.Name);
        }

        /// <summary>
        /// Determines whether the specified item exists for (has a version in) the specified language
        /// Taken from Sean Kearney, http://stackoverflow.com/questions/8231987/check-if-items-exist-in-the-current-language
        /// </summary>
        /// <param name="item">The item to check</param>
        /// <param name="languageName">The language to check - can include language and country information (e.g., "en-CA")</param>
        /// <returns>True if a version in this language exists; otherwise, false</returns>
        public static bool HasLanguageVersion(Sitecore.Data.Items.Item item, String languageName)
        {
            if (null != item)
            {
                if (null == languageName) languageName = Sitecore.Context.Language.Name;

                var language = item.Languages.FirstOrDefault(l => l.Name == languageName);
                if (language != null)
                {
                    var languageSpecificItem = Sitecore.Context.Database.GetItem(item.ID, language);
                    if (languageSpecificItem != null && languageSpecificItem.Versions.Count > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

		/// <summary>
        /// A helper method that filters a ChildList for items that support the user's current language.  This is 
        /// used in navigation controls or on landing or directory pages, to ignore all items that are not available in the 
        /// current language
        /// </summary>
        /// <param name="item">The set of children to filter</param>
        /// <returns>A collection of children that have versions in the current language, or null if none were available</returns>
        public static IEnumerable<Sitecore.Data.Items.Item> FilterChildListByLanguage(Sitecore.Collections.ChildList list)
        {
			return FilterChildListByLanguage(list, Sitecore.Context.Language.Name);
        }

        /// <summary>
        /// A helper method that filters a ChildList for items that support the specified language.  This is 
        /// used in navigation controls or on landing or directory pages, to ignore all items that are not available in the 
        /// current language
        /// </summary>
        /// <param name="item">The set of children to filter</param>
        /// <param name="language">The language for which to filter</param>
        /// <returns>A collection of children that have versions in the current language, or null if none were available</returns>
        public static IEnumerable<Sitecore.Data.Items.Item> FilterChildListByLanguage(Sitecore.Collections.ChildList list, String language)
        {
            if (null != list && null != language)
            {
				var items = list.Where(i => HasLanguageVersion(i, language)).ToList();
				if (null != items && items.Any()) return items;
            }

            return null;
        }

        /// <summary>
        /// A helper method that filters a List for items that support the user's current language.  This is 
        /// used in navigation controls or on landing or directory pages, to ignore all items that are not available in the 
        /// current language
        /// </summary>
        /// <param name="item">The list to filter</param>
        /// <returns>A collection of children that have versions in the current language, or null if none were available</returns>
        public static IEnumerable<Sitecore.Data.Items.Item> FilterItemsByLanguage(List<Sitecore.Data.Items.Item> list)
        {
            return FilterItemsByLanguage(list, Sitecore.Context.Language.Name);
        }

        /// <summary>
        /// A helper method that filters a List for items that support the specified language.  This is 
        /// used in navigation controls or on landing or directory pages, to ignore all items that are not available in the 
        /// current language
        /// </summary>
        /// <param name="item">The list to filter</param>
        /// <param name="language">The language for which to filter</param>
        /// <returns>A collection of children that have versions in the current language, or null if none were available</returns>
        public static IEnumerable<Sitecore.Data.Items.Item> FilterItemsByLanguage(List<Sitecore.Data.Items.Item> list, String language)
        {
            if (null != list && null != language)
            {
				var items = list.Where(i => HasLanguageVersion(i, language)).ToList();
				if (null != items && items.Any()) return items;
            }

            return null;
        }

		#endregion languagehelpers

		/// <summary>
		/// Helper method to convert a pipe-delimited string of guids (for example, from a 
		/// Sitecore multilist field) to a list of Sitecore items
		/// </summary>
		/// <param name="guidList">The pipe-delimited list of item IDs</param>
		/// <returns>A List containing the items matching the specified ID guids, or null if none were found</returns>
		public static List<Sitecore.Data.Items.Item> GetItemsFromStringOfGuids(String guidList)
		{
			if (!String.IsNullOrEmpty(guidList))
			{
				var itemList = new List<Sitecore.Data.Items.Item>();
				String[] guids = guidList.Split('|');
				foreach (String g in guids)
				{
					Guid guid;
					if (Guid.TryParse(g, out guid))
					{
						Sitecore.Data.Items.Item item = Sitecore.Context.Database.GetItem(new Sitecore.Data.ID(guid));
						if (null != item)
						{
							itemList.Add(item);
						}
					}
				}

				// The method returns null instead of an empty list, in the expectation that it will be 
				// hooked up to a Repeater
				if (itemList.Count > 0) return itemList;
			}

			return null;
		}


		/// <summary>
		/// Find the best name to display for the link's destination
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static String GetLinkDestinationName(Sitecore.Data.Items.Item item)
		{
			if (null != item)
			{
				// TODO: Move these field names to a config file

				// The navigation page item field, if there is one, overrides the page header
				if (!String.IsNullOrEmpty(item["navigationPageTitle"]))
				{
					return item["navigationPageTitle"];
				}
				else if (!String.IsNullOrEmpty(item["pageHeader"]))
				{
					return item["pageHeader"];
				}
				else if (!String.IsNullOrEmpty(item["folderName"]))
				{
					return item["folderName"];
				}
				else if (!String.IsNullOrEmpty(item.DisplayName))
				{
					return item.DisplayName;
				}
				else return item.Name;
			}

			return String.Empty;
		}

		/// <summary>
		/// Find the most appropriate teaser or summary text on the specified item.  This may be a topic summary,
		/// or a page teaser, depending on the item's template.  This method also handles the case where the method is
		/// wrapped in paragraph tags, and needs a "More" link inserted within those tags.
		/// </summary>
		/// <param name="item">The item from which to get the summary</param>
		/// <param name="urlMore">The "More" link (as in, "read more ... ") to tack at the end of the summary, 
		/// before any closing paragraph tag</param>
		/// <returns>The summary tag</returns>
		public static String GetSummaryText(Sitecore.Data.Items.Item item)
		{
			String summary = String.Empty;
			if (null != item)
			{
				// The navigation page item field, if there is one, overrides the page header
				if (!String.IsNullOrEmpty(item["topicSummary"]))
				{
					summary = item["topicSummary"];
				}
				else if (!String.IsNullOrEmpty(item["pageSummary"]))
				{
					summary = item["pageSummary"];
				}
				else if (!String.IsNullOrEmpty(item["pageTeaser"]))
				{
					summary = item["pageTeaser"];
				}
				else if (!String.IsNullOrEmpty(item["metaDescription"]))
				{
					summary = item["metaDescription"];
				}
			}

			return summary;
		}

		/// <summary>
		/// Get the URL to an Item as a String, using the default UrlOptions
		/// </summary>
		/// <param name="item">The item</param>
		/// <returns></returns>
		public static String GetItemUrl(Sitecore.Data.Items.Item item)
		{
			if (null == item) return String.Empty;

			return Sitecore.Links.LinkManager.GetItemUrl(item, UrlOptions);
		}

		/// <summary>
		/// Determines whether the specified item has a valid layout.  This separates a viewable content item from,
		/// say, a folder
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public static bool HasLayout(Sitecore.Data.Items.Item item)
		{
			if (null != item && null != item.Visualization)
			{
				Sitecore.Data.ID layoutId = item.Visualization.GetLayoutID(Sitecore.Context.Device);
				return Guid.Empty != layoutId.Guid;
			}

			return false;
		}

		/// <summary>
		/// Creates an entire image tag for the specified ImageField, or returns an empty String if no image was found.  This is a substitute for
		/// sc:Image; unlike that control, this one will not plug in hard width and height values, which cause problems in responsive designs.
		/// </summary>
        /// <param name="imageField">The ImageField to use</param>
		/// <param name="cssClass">An optional cssClass.  If specified, this will override any class already attached to the ImageField</param>
		/// <returns></returns>
		public static String GetImageTagForImageField(ImageField imageField, String cssClass = "")
		{
			if (null != imageField && null != imageField.MediaItem)
			{
				var image = new Sitecore.Data.Items.MediaItem(imageField.MediaItem);
				String src = MediaManager.GetMediaUrl(image, new MediaUrlOptions());
				String alt = imageField.Alt;
				String css = (String.IsNullOrEmpty(cssClass) ? imageField.Class : cssClass);

				return String.Format("<img src=\"{0}\" alt=\"{1}\" {2}>",
					src,
					alt,
					String.IsNullOrEmpty(css) ? String.Empty : String.Format("class=\"{0}\"", css));
			}

			return String.Empty;
		}

        /// <summary>
        /// Create a Sublayout that supports the Rendering for the specified item
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static Sublayout GetSublayoutForItem(Sitecore.Data.Items.Item item)
        {
            RenderingReference rendering =
                item.Visualization.GetRenderings(Sitecore.Context.Device, false).FirstOrDefault();
            if (null != rendering)
            {
                var sublayout = new Sublayout();
                
                sublayout.DataSource = item.ID.ToString();
                sublayout.Path = rendering.RenderingItem.InnerItem["Path"];
                sublayout.Cacheable = rendering.RenderingItem.Caching.Cacheable;

                // Copy cache settings
                if (rendering.RenderingItem.Caching.Cacheable)
                {
                    sublayout.VaryByData = rendering.RenderingItem.Caching.VaryByData;
                    sublayout.VaryByDevice = rendering.RenderingItem.Caching.VaryByDevice;
                    sublayout.VaryByLogin = rendering.RenderingItem.Caching.VaryByLogin;
                    sublayout.VaryByParm = rendering.RenderingItem.Caching.VaryByParm;
                    sublayout.VaryByQueryString = rendering.RenderingItem.Caching.VaryByQueryString;
                    sublayout.VaryByUser = rendering.RenderingItem.Caching.VaryByUser;
                }

                return sublayout;
            }

            return null;
        }

        /// <summary>
        /// Gets a List of items from a multilistfield list suitable for populating a repeater.
        /// </summary>
        /// <param name="item">Item which has the multilist</param>
        /// <param name="MaxToReturn"/>max or -1 for all;</param>
        /// <param name="fieldName">The fieldname of the multilistlist field</param>
        /// <returns>A List of Sitecore.DataItems.Item</returns>
        public static List<Sitecore.Data.Items.Item> GetItemsFromMultilist(Sitecore.Data.Items.Item item, string fieldName, int MaxToReturn)
        {
            List<Sitecore.Data.Items.Item> items = new List<Item>();

            if (null != item)
            {
                Sitecore.Data.Fields.MultilistField mulitlistField = item.Fields[fieldName];
                if (null != mulitlistField && null != mulitlistField.Items && mulitlistField.Items.Length > 0)
                {
                    items = mulitlistField.GetItems().ToList<Sitecore.Data.Items.Item>();
                    MaxToReturn = (MaxToReturn == -1 ? items.Count : MaxToReturn);
                    if (items.Count > MaxToReturn)
                    {
                        for (int i = items.Count - 1; i > MaxToReturn; i--)
                        {
                            items.RemoveAt(i);
                        }
                    }
                }
            }
            return items;
        }

        /// <summary>
        /// Gets a List of items from a multilistfield list suitable for populating a repeater.
        /// </summary>
        /// <param name="item">The id of the item which has the multilist</param>
        /// <param name="fieldName">The fieldname of the multilistlist field</param> 
        /// <param name="MaxToReturn"/>max or -1 for all;</param>
        /// <returns>A List of Sitecore.DataItems.Item</returns>
        public static List<Sitecore.Data.Items.Item> GetItemsFromMultilist(string itemId, string fieldName, int MaxToReturn)
        {
            List<Sitecore.Data.Items.Item> items = new List<Item>();

            Sitecore.Data.Items.Item item = Sitecore.Context.Item;
            Sitecore.Data.Items.Item home = Sitecore.Context.Database.GetItem(Sitecore.Context.Site.StartPath);

            string query = String.Format("/*/content//*[@@id='{0}']", itemId);
            Sitecore.Data.Items.Item child = home.Axes.SelectSingleItem(query);
            if (null == child)
            {
                return items;
            }
            items = GetItemsFromMultilist(child, fieldName, MaxToReturn);
            return items;
        }

        /// <summary>
        /// Get the url of the image located in the specified field
        /// </summary>
        /// <param name="fieldName">The name of the image field</param>
        /// <param name="item">The item holding the image</param>
        /// <returns></returns>
        public static string GetImageUrl(string fieldName, Item item)
        {
            string retVal = "";

            ImageField imageField = ((ImageField)item.Fields[fieldName]);

            if (null != imageField && !String.IsNullOrEmpty(imageField.Value))
            {
                Item imageItem = imageField.MediaItem;
                if (null != imageItem)
                {
                    retVal = ItemHelpers.GetMediaUrl(imageItem);
                }
            }
            return retVal;
        }

        /// <summary>
        /// Get the url of the image located in the specified field on the current context item
        /// </summary>
        /// <param name="fieldName">The name of the image field</param>
        /// <returns></returns>
        public static string GetImageUrl(string fieldName)
        {
            Item item = Sitecore.Context.Item;
            return GetImageUrl(fieldName, item);
        }

	}
}
