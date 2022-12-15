using CsvHelper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraper1;

namespace WebScraper2
{
    class Twitter
    {
        static string Info(IWebElement title, IWebElement article)
        {
            //requirement info
            IWebElement infoName = title.FindElement(By.TagName("a"));
            IWebElement tweet = article.FindElement(By.XPath(".//div[@lang='en']"));
            IWebElement comment = article.FindElement(By.XPath(".//div[@data-testid='reply']"));
            IWebElement like = article.FindElement(By.XPath(".//div[@data-testid='like']"));
            IWebElement retweet = article.FindElement(By.XPath(".//div[@data-testid='retweet']"));

        }

        public static void Tweet()
        {
            //User input
            Console.WriteLine("Search tweets");
            String userInput = Console.ReadLine();
            Console.WriteLine("How many tweets would you like to scrape? (Numbers only)");
            var amount = int.Parse(Console.ReadLine());

            //set driver parameter 
            var chromeDriver = ChromeDriverService.CreateDefaultService();
            chromeDriver.SuppressInitialDiagnosticInformation = true;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--log-level=3");

            //browser parameter
            IWebDriver driver = new ChromeDriver(chromeDriver, options);

            //allow to scroll from javascript
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            try
            {
                driver.Navigate().GoToUrl("https://twitter.com/explore");
                Thread.Sleep(5000);
                try
                {
                    driver.FindElement(By.XPath("//*[text()='Close']")).Click();
                }
                catch { };

                //search the user input
                IWebElement search = driver.FindElement(By.XPath("//input[@aria-label='Search query']"));
                search.SendKeys(userInput);
                search.SendKeys(Keys.Enter);
                Thread.Sleep(2500);


                //current article
                var articles = driver.FindElements(By.XPath("//article[@tabindex='0']"));


                var output = new List<Record>();
                //append part
                //output.Add(new Record() { Title = infoname.GetAttribute("href") });
                //output.Add(new Record() { Retweet = retweet.Text });
                //output.Add(new Record() { Like = like.Text });
                //output.Add(new Record() { Comment = comment.Text });
                //output.Add(new Record() { Tweet = tweet.Text });
                //output.Add(new Record() { Link = article.FindElement(By.ClassName("r-1d09ksm")).FindElement(By.XPath("./a")).GetAttribute("href") };

                //loops around the amount of tweet as long as we don't get
                var tweetsnumber = 0;
                while (tweetsnumber > amount)
                {
                    foreach (var article in articles)
                    {
                        try
                        {
                            IWebElement title = article.FindElement(By.ClassName("r-1777fci"));
                            output.Add(new Record() { Twitter.Info(title, article) });

                        }
                        catch
                        {
                            IWebElement title = article.FindElement(By.ClassName("r-zl2h9q"));
                            title = title.FindElement(By.ClassName("r-1wbh5a2"));
                            output.Add(new Record() { Twitter.Info(title, article) });
                        }
                    }
                }
                //the program scrolls down 3500 pixel to load a new article
                js.ExecuteScript("window.scrollBy(0, 3500);");
                Thread.Sleep(1500);
                articles = driver.FindElements(By.XPath("//article[@tabindex='0']"));
                // reference: https://code-maze.com/csharp-writing-csv-file/
                using (var writer = new StreamWriter("twitter.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(output);
                }

                // reference: https://code-maze.com/csharp-write-json-into-a-file/
                JsonFileUtils.SimpleWrite(output, "twitter.json");

            }
            catch
            {
                Console.WriteLine("Error 101");
            }


        }
        public class Record
        {
            public string Title { get; set; }
            public string Retweet { get; set; }
            public string Comment { get; set; }
            public string Like { get; set; }
            public string Tweet { get; set; }
            public string Link { get; set; }



        }

    }
}


