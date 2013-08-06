using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Data.Items;
using Sitecore.Events;

namespace PixelMEDIA.SitecoreCMS.Controls.EventHandlers
{
    /// <summary>
    /// Populate the display name with the name that the user originally gave the item.  Typically, a new item's name will be rewrittten to replace
    /// any spaces with dashes - for example, "Home Page" becomes "Home-Page".  This event handler will catch that and create a display name that retains
    /// the spaces - so, in the content tree, users will still the item as "Home Page".  
    /// 
    /// To use this, an entry must be added to EventHandlers.config, like so:
    /// 
    /// <event name="item:added">
    ///     <handler type="PixelMEDIA.SitecoreCMS.Controls.EventHandlers.ItemEventHandler, PixelMEDIA.SitecoreCMS.Controls" method="PopulateDisplayName"/>
    /// </event>
    /// 
    /// Taken from http://www.cognifide.com/blogs/sitecore/sitecore-best-practice-9/.
    /// </summary>
    public class ItemEventHandler
    {
        protected void PopulateDisplayName(object sender, EventArgs args)
        {
            var item = (Item)Event.ExtractParameter(args, 0);
            string processedName = item.Name.ToLower().Replace(' ', '-');

            if (item.Database.Name != "master"
                || !item.Paths.Path.StartsWith("/sitecore/content/Home/")
                || item.Name.Equals(processedName))
            {
                return;
            }

            item.Editing.BeginEdit();
            try
            {
                item.Appearance.DisplayName = item.Name;
                item.Name = processedName;
            }
            finally
            {
                item.Editing.EndEdit();
            }
        }
    }
}
