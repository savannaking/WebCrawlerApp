using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebCrawlerApp
{
    public class Crawler
    {
        private Queue<string> linksToTraverse;
        private List<string> linksTraversed;
        private List<string> phoneNumbers;
        private string phoneRegex = @"\d{3}[\s.-]\d{3}[\s.-]\d{4}";
        private int crawlDepth;
        private int crawlDepthCounter;
        private bool stayWithinDomain;

        public Crawler(int crawlDepth, bool stayWithinDomain = false)
        {
            linksTraversed = new List<string>();
            linksToTraverse = new Queue<string>();
            phoneNumbers = new List<string>();
            this.crawlDepth = crawlDepth;
        }

        public void Crawl(string startingUrl)
        {
            if (crawlDepthCounter > crawlDepth)
                return;

            crawlDepthCounter++;

            var uri = new Uri(startingUrl);
            HtmlWeb web = new HtmlWeb();
            var htmlDoc = web.Load(startingUrl);

            ScrapeLinks(htmlDoc, startingUrl, stayWithinDomain);

            ScrapePhoneNumbers(htmlDoc);

            linksTraversed.Add(startingUrl);
            if (linksToTraverse.Count > 0)
            {
                Crawl(linksToTraverse.Dequeue());
            }
        }

        private void ScrapePhoneNumbers(HtmlDocument htmlDoc)
        {
            foreach (var node in htmlDoc.DocumentNode.SelectNodes("//text()[normalize-space(.) != '']"))
            {
                Match m = Regex.Match(node.InnerText, phoneRegex);
                if (m.Success && !phoneNumbers.Contains(m.Value))
                {
                    phoneNumbers.Add(m.Value);
                    Console.WriteLine(m.Value);
                }
            }
        }

        private void ScrapeLinks(HtmlDocument htmlDoc, string startingUrl, bool stayWithinDomain)
        {
            var links = htmlDoc.DocumentNode.SelectNodes("//a[@href]");
            if (links != null)
            {
                foreach (HtmlNode link in links)
                {
                    var url = link.GetAttributeValue("href", null);
                    url = GetAbsoluteUrlFromScrapedLink(url, startingUrl);
                    var startingUri = new Uri(startingUrl);
                    var uri = new Uri(url);

                    if (stayWithinDomain)
                    {
                        if (uri.Host != startingUri.Host)
                        {
                            continue;
                        }
                    }
                    if (url != null && !linksTraversed.Contains(url) && !linksToTraverse.Contains(url))
                        linksToTraverse.Enqueue(url);
                }
            }
        }

        private string GetAbsoluteUrlFromScrapedLink(string url, string startingUrl)
        {
            Uri inputUri = new Uri(startingUrl);

            //if it's already an absolute URL, just add it to the queue
            Uri result;
            if (Uri.TryCreate(url, UriKind.Absolute, out result))
            {
                return url;
            }
            else
            {
                if (url.StartsWith("/"))
                    return inputUri.Scheme + @"://" + inputUri.Host + url;
                else
                {

                    var fullUrl = String.Format(
                        "{0}{1}{2}",
                        inputUri.GetLeftPart(UriPartial.Authority),
                        String.Join(string.Empty, inputUri.Segments.Take(inputUri.Segments.Length - 1)), url);
                    return fullUrl;
                }
            }
        }
    }
}
