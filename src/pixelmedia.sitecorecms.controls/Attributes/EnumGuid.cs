using System;

namespace PixelMEDIA.SitecoreCMS.Controls.Attributes
{
	public class EnumGuid : Attribute
	{
		public Guid Guid { get; set; }

		public EnumGuid(string guid)
		{
			this.Guid = new Guid(guid);
		}
	}
}