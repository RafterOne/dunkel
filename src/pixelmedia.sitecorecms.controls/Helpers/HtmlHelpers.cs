using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PixelMEDIA.SitecoreCMS.Controls.Helpers
{
	/// <summary>
	/// A set of helper methods for dealing with html on the page
	/// </summary>
	public class HtmlHelpers
	{
		/// <summary>
		/// Takes any string and replaces all non-alphanumeric characters with the provided separator.
		/// </summary> 
		/// <param name="dirty">String to manipulate.</param>
		/// <param name="replacement">Character to use when replacing non alphanumeric characters.</param>
		/// <returns>A clean and lowercased string.</returns>
		public static string GetCleanedString(string dirty, string replacement)
		{
			if (!String.IsNullOrEmpty(dirty) && !String.IsNullOrEmpty(replacement))
			{
				Regex rgx = new Regex("[^a-zA-Z0-9]+");
				string result = rgx.Replace(dirty, replacement);
				return result.ToLower();
			}

			return dirty;
		}

		/// <summary>
		/// Verifies the input string is in email address format
		/// </summary>
		/// <param name="emailAddress">Email Address input string</param>
		/// <returns>True or False</returns>
		public static bool IsEmail(string emailAddress)
		{
			if (String.IsNullOrEmpty(emailAddress)) return false;

			string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
				  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
				  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
			System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(strRegex);

			return re.IsMatch(emailAddress.Trim());
		}
	}
}
