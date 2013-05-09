using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Configuration;
using Sitecore.Shell.Applications.ContentEditor.Gutters;

namespace PixelMEDIA.SitecoreCMS.Controls.GutterRenderers
{

    
    /// <summary>
    /// JP - I stole this from http://firebreaksice.com/sitecore-gutter-icon-to-indicate-an-item-is-a-page/
    /// 
    /// This class is used to show content editors which items in the tree are pages (and thus have URLs represented by their path) vs. 
    /// items that are merely there to store data. The code is quite simple, and the icon and tooltip are overridable via two custom Sitecore settings:
    ///   To change the icon used in the gutter, create a Sitecore setting named PageGutter.Icon with the path to a Sitecore icon (e.g. “People/32×32/monitor2.png”).
    ///   To change the tooltip on the icon, create a Sitecore setting named PageGutter.Tooltip with the desired text (e.g. “The item is a page.”).
    /// 
    ///  In Core, add an item of type /sitecore/templates/Sitecore Client/Content editor/Gutter Renderer to the location:
    ///  /sitecore/content/Applications/Content Editor/Gutters.  The only fields you need to set are "Header", the text to appear in the gutter 
    ///  context-menu toggle control, and "Type", which contains a reference to the class above, in standard "Full.Class.Name, Assembly" format, with no ".DLL" 
    ///  after the assembly. Save, and you're set.
    /// </summary>
    public class PageGutterRenderer : GutterRenderer
    {

        protected override GutterIconDescriptor GetIconDescriptor(Item item)
        {
            if (string.IsNullOrEmpty(item[FieldIDs.LayoutField]))
            {
                return null;
            }

            string iconPath = Settings.GetSetting("PageGutter.Icon");

            if (string.IsNullOrEmpty(iconPath))
            {
                iconPath = "People/32x32/monitor2.png";
            }

            string iconTooltip = Settings.GetSetting("PageGutter.Tooltip");

            if (string.IsNullOrEmpty(iconTooltip))
            {
                iconTooltip = "The item is a page.";
            }

            GutterIconDescriptor gutterIconDescriptor = new GutterIconDescriptor
            {
                Icon = iconPath,
                Tooltip = iconTooltip,
                Click = string.Format("item:setlayoutdetails(id={0})", item.ID)
            };

            return gutterIconDescriptor;
        }

    }
}
