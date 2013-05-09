using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PixelMEDIA.SitecoreCMS.Controls.Controls
{
	/// <summary>
	/// A subclass of the Image handler where the height and width can be suppressed.  This offers better
	/// support in responsive designs than the standard Image field renderer.
	/// </summary>
	public class Image : Sitecore.Web.UI.WebControls.Image
	{
		/// <summary>
		/// Setting this to true will suppress the width and height params in the rendered tag
		/// </summary>
		public bool RemoveWidthHeight { get; set; }

		public Image()
		{ }
		protected override void Render(System.Web.UI.HtmlTextWriter output)
		{
			if (RemoveWidthHeight)
			{
				output.Write(this.StripHeight(this.StripWidth(base.RenderAsText())));
			}
			else
			{
				base.Render(output);
			}
		}
		protected string StripWidth(string input)
		{
			Regex regex = new Regex(
				  "width=\\\"\\d*\\\"",
				RegexOptions.IgnoreCase
				| RegexOptions.CultureInvariant
				| RegexOptions.IgnorePatternWhitespace
				| RegexOptions.Compiled
				);
			return regex.Replace(input, String.Empty);
		}
		protected string StripHeight(string input)
		{
			Regex regex = new Regex(
				"height=\\\"\\d*\\\"",
				RegexOptions.IgnoreCase
				| RegexOptions.CultureInvariant
				| RegexOptions.IgnorePatternWhitespace
				| RegexOptions.Compiled
				);
			return regex.Replace(input, String.Empty);
		}
	}
}
