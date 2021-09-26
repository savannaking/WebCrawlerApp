using System;

namespace WebCrawlerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a starting URL.");
            string url = Console.ReadLine();
            Console.WriteLine("Enter a crawl depth as an integer.");
            int crawlDepth = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Do you want to stay within the domain of your starting URL? Type Y/N");
            bool stayWithinDomain = Console.ReadLine().ToUpper() == "Y" ? true : false;

            var crawler = new Crawler(crawlDepth, stayWithinDomain);
            crawler.Crawl(url);
        }
    }
}
