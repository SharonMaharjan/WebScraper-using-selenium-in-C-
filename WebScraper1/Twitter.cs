using CsvHelper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebScraper1;

namespace WebScraper2
{
    public class Twitter
    {
        public static Record Info(IWebElement username, IWebElement article)
        {
            
            //requirement information for twitter
            IWebElement infoName = username.FindElement(By.TagName("a"));
            IWebElement tweet = article.FindElement(By.XPath(".//div[@lang='en']"));
            IWebElement comment = article.FindElement(By.XPath(".//div[@data-testid='reply']"));
            IWebElement like = article.FindElement(By.XPath(".//div[@data-testid='like']"));
            IWebElement retweet = article.FindElement(By.XPath(".//div[@data-testid='retweet']"));
            return new Record()
            {
                UserName = infoName.Text,
                Like = like.Text,
                Comment = comment.Text,
                Retweet = retweet.Text,
                Tweet = tweet.Text
            };
        }

        public static void Tweet()
        {
            //user enters the input
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

            // explicit wait (it allow the code to halt the program until the condition isn't resolve)
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            //allow to scroll from javascript
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            try
            {
                driver.Navigate().GoToUrl("https://twitter.com/explore");
                Thread.Sleep(5000);

                try
                {

                    // visible cookies and accept the cookie
                    IWebElement tweetCookies = wait.Until(
                        driver => driver.FindElement(By.XPath("//*[@id='layers']/div/div[2]/div/div/div/div[2]/div[1]")));
                        tweetCookies.Click();
                    

                }
                catch { };

                try
                {
                    //notification
                    IWebElement tweetnotification = wait.Until(
                       driver =>
                       driver.FindElement(By.XPath("//*[@id='layers']/div[2]/div/div/div/div/div/div[2]/div[2]/div/div[2]/div/div[2]/div[2]/div[2]")));

                    tweetnotification.Click();

                    //Thread.Sleep(2500);
                }
                catch { }

             //search the user input
                IWebElement search = driver.FindElement(By.XPath("//input[@aria-label='Search query']"));
             search.SendKeys(userInput);
             search.SendKeys(Keys.Enter);
             Thread.Sleep(2500);

            
             //current article
             var articles = driver.FindElements(By.XPath("//article[@tabindex='0']"));

            //list of data
             var output = new List<Record>();
                
             //loops around the amount of tweet as long as we don't get
             var tweetsnumber = 0;
                foreach (var article in articles.Take(amount))
                    {
                        try
                        {
                            IWebElement title = article.FindElement(By.ClassName("r-1777fci"));
                            output.Add( Twitter.Info(title, article) );

                        }
                        catch
                        {
                            IWebElement title = article.FindElement(By.ClassName("r-zl2h9q"));
                            title = title.FindElement(By.ClassName("r-1wbh5a2"));
                            output.Add( Twitter.Info(title, article) );
                        }
                    }

             //the program scrolls down 3500 pixel to load a new article
                js.ExecuteScript("window.scrollBy(0, 3500);");
                Thread.Sleep(1500);
                articles = driver.FindElements(By.XPath("//article[@tabindex='0']"));


             // reference: https://code-maze.com/csharp-writing-csv-file/
             // write in csv file
                using (var writer = new StreamWriter("twitter.csv"))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(output);
                }

             // reference: https://code-maze.com/csharp-write-json-into-a-file/
             //write in json file
                JsonFileUtils.SimpleWrite(output, "twitter.json");

            }
            catch
            {
                Console.WriteLine("Error 101");
            }


        }
        public class Record
        {
            public string UserName { get; set; }
            public string Retweet { get; set; }
            public string Comment { get; set; }
            public string Like { get; set; }
            public string Tweet { get; set; }
         
        }

    }
}
   

