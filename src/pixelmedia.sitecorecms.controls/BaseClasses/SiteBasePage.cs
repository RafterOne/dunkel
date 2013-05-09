using System;

namespace PixelMEDIA.SitecoreCMS.Controls.BaseClasses
{
    public class SiteBasePage : PixelMEDIA.Controls.BasePage
    {

		private string _subhead;
		private string _bodyClass;

		/// <summary>
		/// Get or set a class to apply to the <body> tag
		/// </summary>
		public virtual string BodyClass
		{
			get
			{
				if (null == this._bodyClass)
				{
					this._bodyClass = String.Empty;
				}
				return this._bodyClass;
			}
			set
			{
				this._bodyClass = value;
			}
		}


		public virtual string Subhead
		{
			get
			{
				string subhead = "";
				if (this._subhead != null)
				{
					subhead = this._subhead;
				}
				return subhead;
			}
			set
			{
				this._subhead = value;
			}
		}

		/// <summary>
		/// Get the title of the page
		/// </summary>
		/// <returns></returns>
        public new string GetPageTitle()
        {
            Sitecore.Data.Items.Item currentItem = Sitecore.Context.Item;

            try
            {
                string title = "";
                if (null != currentItem.Fields["pageTitle"] && (!String.IsNullOrEmpty(currentItem.Fields["pageTitle"].Value)))
                {
                    title = currentItem.Fields["pageTitle"].Value;
                }
                else
                {
                    title = currentItem.DisplayName;
                }
                return title;
            }
            catch { }
            
            return GetAppSetting("Name.Company", String.Empty);
        }
    }
}
