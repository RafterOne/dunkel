using System;

namespace PixelMEDIA.SitecoreCMS.Controls.Controls
{
	/// <summary>
	/// An extension of the standard ASP.NET Panel control, but renders without a surrounding
	/// div tag to prevent issues with padding in Firefox.
	/// </summary>
	public class Panel : System.Web.UI.WebControls.Panel
	{
		public override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer) { }
		public override void RenderEndTag(System.Web.UI.HtmlTextWriter writer) { }
	}
}