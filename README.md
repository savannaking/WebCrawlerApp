# WebCrawlerApp

### Implementation Details
* Console application written in C#.
* Could easily be adapted into a DLL that could be used by any client application. The `Crawler` class was written with a focus on clarity, maintainability, and ease of use by 
client code. It exposes only a constructor and one method, `Crawler.Crawl`. 
* The constructor takes two parameters, an integer `crawlDepth` and an optional boolean `stayWithinDomain` which defaults to `false`. These parameters are settings that
control how the crawler works. 
* `crawlDepth` is meant to limit the amount of links scraped to the selected number. This implementation doesn't actually know the true recursive depth, all this does is 
limit the number of links crawled without knowledge of where they are in the tree. The links are added to a first in, last out `Queue`. 
* `stayWithinDomain` allows the caller to choose to only crawl links within the same domain rather than crawling all links to any domain on the Internet found on the pages. This
could be used if you are only interested in what's on one particular domain.
* `Crawler.Crawl` takes one `url` parameter, which is just a string with the starting URL.
* The `Crawler` implementation does check for duplicate links and duplicate phone numbers to avoid repeatedly crawling the same links or maintaining duplicates in the
list of phone numbers.
* The `Crawl` method prints the phone numbers found to the console. A list of phone numbers is maintained that is currently only used for duplicate checking but
could easily be returned to the calling code for further processing rather than simply printing to the console. 

### Potential Future Enhancements
* Adaptation into a DLL to be used by other code. Adaptation into a web API to allow other applications to easily make a REST call to obtain the scraped phone numbers
from any number of links. Adaptation into a web application with a GUI to allow a user to more easily input the parameters and receive the data without writing any code. 
* Adding a parameter to allow the user to choose what sort of data they want to scrape. Instead of only scraping phone numbers, the user could choose to scrape email addresses,
DOBs, SSNs, etc.
* Adding support for persisting the scraped data to a database to allow more advanced data analysis, reporting, etc.
* There's always the potential to improve the code that detects links and phone numbers. Right now, you are likely to get some output that may not necessarily be a valid 
phone number because I did not focus on making the Regex pattern I used bulletproof. Similarly, the method of finding links on the pages is likely not bulletproof either.
* To that end, the error handling could be much improved. `try/catch` blocks around some of the riskier areas of the code would be a good thing to add, as well as better
feedback to the calling code/user if an error does occur. 
* Better input validation would be required to use this in production, it's possible to give bad input parameters right now and it will crash. 
* It would also be cool to add a better visual feedback mechanism to show what's going on while the crawler is running, which/how many links are being scraped, 
etc.
* This code has not been optimized for performance.
