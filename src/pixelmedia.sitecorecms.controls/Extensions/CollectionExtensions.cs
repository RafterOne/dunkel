#region USINGS

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

#endregion

namespace PixelMEDIA.SitecoreCMS.Controls.Extensions
{
	public static class CollectionExtensions
	{
		public static bool IsEmpty<T>(this IEnumerable<T> e)
		{
			return e == null || !e.Any();
		}

		public static bool IsNotEmpty<T>(this IEnumerable<T> e)
		{
			return !e.IsEmpty();
		}

		public static string ToQueryString(this NameValueCollection nvc)
		{
			var list = (from k in nvc.AllKeys
			            from v in nvc.GetValues(k)
			            select string.Format("{0}={1}", HttpUtility.UrlEncode(k), HttpUtility.UrlEncode(v))).ToList();

			return string.Join("&", list.ToArray());
		}

		public static IDictionary<string, string> ToDictionary(this NameValueCollection nvc)
		{
			var dict = new Dictionary<string, string>();
			nvc.AllKeys.Where(k => !String.IsNullOrEmpty(k)).ToList().ForEach(k => dict.Add(k, nvc[k]));
			return dict;
		}

		public static string ToQueryString(this IDictionary<string, string> nvc)
		{
			return string.Join("&",
			                   Array.ConvertAll(nvc.Keys.ToArray(),
			                                    key =>
			                                    string.Format("{0}={1}", HttpUtility.UrlEncode(key),
			                                                  HttpUtility.UrlEncode(nvc[key]))));
		}

		public static NameValueCollection GetQueryString(this Uri uri)
		{
			if (uri.Query.Length == 0)
			{
				return null;
			}

			return HttpUtility.ParseQueryString(uri.Query.Substring(1));
		}

		public static void MoveItem<K, V>(this IDictionary<K, V> dict, K key, int index)
		{
			if (!dict.Keys.Contains(key))
			{
				return;
			}

			if (dict.Keys.ToList().IndexOf(key) == index)
			{
				return;
			}

			var kvp = new KeyValuePair<K, V>(key, dict[key]);
			dict.Remove(key);

			var items = new List<KeyValuePair<K, V>>(dict.ToList());
			dict.Clear();

			if (index > items.Count())
			{
				items.Add(kvp);
			}
			else
			{
				items.Insert(index, kvp);
			}

			items.ForEach(dict.Add);
		}

		public static IDictionary<K, V> Clone<K, V>(this IDictionary<K, V> dict)
		{
			return new Dictionary<K, V>(dict);
		}

		public static IEnumerable<T> Clone<T>(this IEnumerable<T> list)
		{
			return new List<T>(list);
		}
	}
}