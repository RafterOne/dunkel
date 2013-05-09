using System;
using System.Web.UI;

namespace PixelMEDIA.SitecoreCMS.Controls.Controls
{
	/// <summary>
	/// HTML5 text box that supports any new specialized type.
	/// </summary>
	public class HtmlFiveInput : System.Web.UI.WebControls.TextBox
	{
		/// <summary>
		/// The text box type to render as.
		/// </summary>
		public string Type
		{
			get;
			set;
		}

		/// <summary>
		/// HtmlFileInput default constructor.
		/// </summary>
		public HtmlFiveInput()
			: base()
		{
		}

		/// <summary>
		/// Overriden method that changes the default text box type to whatever is set at runtime.
		/// </summary>
		/// <param name="writer"></param>
		protected override void AddAttributesToRender(HtmlTextWriter writer)
		{
			writer.AddAttribute(HtmlTextWriterAttribute.Type, this.Type);
			base.AddAttributesToRender(writer);
		}
	}
}