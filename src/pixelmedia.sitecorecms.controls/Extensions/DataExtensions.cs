#region USINGS

using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Web.Script.Serialization;

#endregion

namespace PixelMEDIA.SitecoreCMS.Controls.Extensions
{
	public static class DataExtensions
	{
		public static void RemoveNamespace(this DataTable table)
		{
			table.Namespace = table.Prefix = String.Empty;
		}

		public static void RemoveNamespace(this DataSet ds)
		{
			ds.Namespace = ds.Prefix = String.Empty;
			foreach (DataTable tbl in ds.Tables)
			{
				tbl.RemoveNamespace();
			}
		}

		public static string ToJson(this Object o)
		{
			string result;

			using (var ms = new MemoryStream())
			{
				o.ToJson(ms);
				ms.Position = 0;
				using (var sr = new StreamReader(ms))
				{
					result = sr.ReadToEnd();
				}
			}
			return result;
		}

		public static string ToJavaScript(this Object o)
		{
			var serializer = new JavaScriptSerializer();
			return serializer.Serialize(o);
		}

		public static void ToJson(this Object o, Stream outputStream)
		{
			var ser = new DataContractJsonSerializer(o.GetType());
			ser.WriteObject(outputStream, o);
		}
	}
}