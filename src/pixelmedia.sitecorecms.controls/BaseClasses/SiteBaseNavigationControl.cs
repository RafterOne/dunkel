using System;
using System.Linq;
using PixelMEDIA.SitecoreCMS.Controls.Helpers;

namespace PixelMEDIA.SitecoreCMS.Controls.BaseClasses
{
    public class SiteBaseNavigationControl : SiteBaseUserControl
    {
        /// <summary>
        /// Checks whether the specified item is the current item, or (optionally) is the parent of the current item
        /// </summary>
        /// <param name="navItem">The item to check</param>
        /// <param name="acceptParent">If this is "true," then the item will be marked as selected if it
        /// is the current item, or if it is the parent of the current item.  For example, a top nav item is
        /// selected if the user is viewing any item under it.  This parameter defaults to "true."</param>
        /// <returns>True if the item should be marked as selected; otherwise, false</returns>
        protected bool IsSelected(Sitecore.Data.Items.Item navItem, bool acceptParent = true)
        {
            if (null != navItem)
            {
                Sitecore.Data.Items.Item item = Sitecore.Context.Item;
                if (navItem.ID.Equals(item.ID)) return true;
                if (acceptParent)
                {
                    while (null != item.Parent && !item.TemplateName.Equals("Root"))
                    {
                        if (navItem.ID.Equals(item.Parent.ID)) return true;
                        item = item.Parent;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Get the URL to the first child item under the specified item.  For example, this is used when a folder is
        /// collapsed, and the link attached to that folder takes the user to the first item inside that folder.  This
        /// does not handle complex (and unlikely) cases where the item we want is actually inside multiple levels of folders
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected String GetFirstChildItemUrl(Sitecore.Data.Items.Item item)
        {
            if (null != item && null != item.Children && item.Children.Any())
            {
                return ItemHelpers.GetItemUrl(item.Children.FirstOrDefault());
            }

            return String.Empty;
        }
    }
}
