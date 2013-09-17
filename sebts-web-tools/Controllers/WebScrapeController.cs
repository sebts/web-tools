using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using HtmlAgilityPack;
using WebAPI.OutputCache;
using System.Configuration;

namespace sebts_web_tools.Controllers
{
    public class WebScrapeController : ApiController
    {
        // GET api/webscrape
        [CacheOutput(ClientTimeSpan = 3600, ServerTimeSpan = 3600)]
        public string Get(string url)
        {
            Uri uri = new Uri(url);
            HtmlDocument doc = Scrape(uri);

            string baseUrl = uri.AbsoluteUri.Remove(uri.AbsoluteUri.LastIndexOf(uri.AbsolutePath));
            FixRelativeUrls(doc, baseUrl);

            return doc.DocumentNode.WriteTo();
        }

        HtmlDocument Scrape(Uri uri)
        {
            WebClient web = new WebClient();
            try
            {
                string html = web.DownloadString(uri);
                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);
                return doc;
            }
            catch (Exception)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(string.Format("Unable to find url {0}", uri.AbsoluteUri)),
                    ReasonPhrase = "Url Not Found"
                });
            }
        }

        void FixRelativeUrls(HtmlDocument doc, string baseUrl)
        {
            Uri baseUri = new Uri(baseUrl);
            FixRelativeUrl(doc, "a", "href", baseUri);
            FixRelativeUrl(doc, "img", "src", baseUri);
            FixRelativeUrl(doc, "script", "src", baseUri);
        }

        void FixRelativeUrl(HtmlDocument doc, string findNode, string findAttr, Uri baseUri)
        {
            string xpath = String.Format("//{0}[@{1}]", findNode, findAttr);
            var nodes = doc.DocumentNode.SelectNodes(xpath);
            if (nodes != null)
            {
                foreach (HtmlNode node in nodes)
                {
                    var attr = node.Attributes[findAttr];
                    Uri uri = new Uri(attr.Value, UriKind.RelativeOrAbsolute);
                    if (!uri.IsAbsoluteUri)
                        attr.Value = new Uri(baseUri, uri).ToString();
                }
            }
        }
    }
}
