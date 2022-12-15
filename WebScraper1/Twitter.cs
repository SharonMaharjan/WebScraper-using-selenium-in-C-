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
        public static void Tweet() {
            //User input
            Console.WriteLine("Search tweets");
            String userInput = Console.ReadLine();
            Console.WriteLine("How many tweets would you like to scrape? (Numbers only)");
            var amount = int.Parse(Console.ReadLine());

            var chromeDriver = ChromeDriverService.CreateDefaultService();
            chromeDriver.SuppressInitialDiagnosticInformation = true;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--log-level=3");

            IWebDriver driver = new ChromeDriver(chromeDriver, options);

            driver.Navigate().GoToUrl("https://twitter.com/explore");
            try
            {
                driver.FindElement(By.XPath("//*[text()='Close']")).Click();
            }
            catch  { }

            IWebElement search = driver.FindElement(By.XPath("//input[@aria-label='Search query']"));
            search.SendKeys(userInput);
            search.SendKeys(Keys.Enter);
            Thread.Sleep(2500);

            var articles = driver.FindElements(By.XPath("//article[@tabindex='0']"));
            var tweetsnumber = 0;
            var output = new List<Record>();
            while (tweetsnumber> amount)
            {
                foreach(var item in articles)
                {
                    try
                    {
                        IWebElement title = item.FindElement(By.ClassName("r-1777fci"));
                        output.Add(
                            new Record()
                        { Title = title});
                    }
                    catch 
                    {
                        IWebElement Title = item.FindElement(By.ClassName("r-zl2h9q"));
                        Title = Title.FindElement(By.ClassName("r-1wbh5a2"));
                    }
                }
            }

            // reference: https://code-maze.com/csharp-writing-csv-file/
            using (var writer = new StreamWriter("twitter.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(output);
            }

            // reference: https://code-maze.com/csharp-write-json-into-a-file/
            JsonFileUtils.SimpleWrite(output, "twitter.json");

        }
    }
    public class Record
    {
        public string Title { get; set; }
    }
}
