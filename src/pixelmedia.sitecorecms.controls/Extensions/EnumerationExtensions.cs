using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace PixelMEDIA.SitecoreCMS.Controls.Extensions
{
	public static class EnumerationExtensions
	{
		public static Guid GetEnumGuid(this Enum e)
		{
			Type type = e.GetType();

			MemberInfo[] memInfo = type.GetMember(e.ToString());

			if (memInfo.Length > 0)
			{
				object[] attrs = memInfo[0].GetCustomAttributes(typeof(Attributes.EnumGuid), false);
				if (attrs.Length > 0) return ((Attributes.EnumGuid)attrs[0]).Guid;
			}

			throw new ArgumentException("Enum " + e.ToString() + " has no EnumGuid defined!");
		}
	}
}
