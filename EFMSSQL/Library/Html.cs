using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Library
{
	public class Html
	{
		public string GetHtmlWeb(string url)
		{
			HtmlWeb web = new HtmlWeb();
			HtmlDocument doc = web.Load("https://bing.ioliu.cn/");
			HtmlNodeCollection hrefList = doc.DocumentNode.SelectNodes(".//a[@src]");
			if (hrefList != null)
			{
				foreach (HtmlNode href in hrefList)
				{
					HtmlAttribute att = href.Attributes["href"];
					string ss = att.Value;
				}
			}
			return "145s";
		}
	}
}