using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Layouts;
using Sitecore.Resources.Media;
using Sitecore.Web.UI.WebControls;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.Data.Fields;
using System.Text.RegularExpressions;

namespace PixelMEDIA.SitecoreCMS.Controls.BaseClasses
{
    public class SiteBaseUserControl : PixelMEDIA.Controls.BaseUserControl
    {
		public new SiteBasePage ParentPage
		{
			get { return base.ParentPage as SiteBasePage; }
		}

		private Item _dataSource = null;

		/// <summary>
		/// This property looks up the Item associated with a Sublayout.  This allows you to easily retrieve controls like
		/// focus areas and programmatically add them to a placeholder on a page.
		/// Reference: http://firebreaksice.com/using-the-datasource-field-with-sitecore-sublayouts/
		/// </summary>
		public Item DataSource
		{
			get
			{
				if (_dataSource == null)
					if (Parent is Sublayout)
						_dataSource = Sitecore.Context.Database.GetItem(((Sublayout)Parent).DataSource);

				return _dataSource;
			}
		}

        protected Item ParentItem
        {
            get
            {
                var item = Sitecore.Context.Item;
                return item.Parent;
            }

        }
    }
}
