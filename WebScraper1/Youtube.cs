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

namespace WebScraper0
{
    class Youtube
    {
        public static void Yt()
        {
            //User input
            Console.WriteLine("Find new videos in youtube");
            String userInput = Console.ReadLine();

            ChromeOptions options = new ChromeOptions();
            var chromeDriver = ChromeDriverService.CreateDefaultService();
            chromeDriver.SuppressInitialDiagnosticInformation = true;
            options.AddArgument("--log-level=3");

            IWebDriver driver = new ChromeDriver(chromeDriver,options);
            driver.Navigate().GoToUrl("https://www.youtube.com");

            IWebElement webElement = driver.FindElement(By.Name("search_query"));
            webElement.SendKeys(userInput);
            webElement.SendKeys(Keys.Enter);
            Thread.Sleep(2500);

            //driver.FindElement(By.XPath("//*[text()='Filters']")).Click();
            //driver.FindElement(By.XPath("//*[text()='Filters']")).Click();
            //Thread.Sleep(2000);

            driver.FindElement(By.XPath("//*[@id='label']/yt-formatted-string")).Click();
            Thread.Sleep(2500);

            var videos = driver.FindElements(By.TagName("ytd-video-renderer"));
            var output = new List<Record>();
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine(videos[i].FindElement(By.TagName("a")).GetAttribute("href"));
                output.Add(new Record() { Item = videos[i].FindElement(By.TagName("a")).GetAttribute("href") });
            }
            using (var writer = new StreamWriter("youtube.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(output);
            }
           
            //var titles = driver.FindElements(By.XPath("//div[@role='heading']"));
            //var output = new List<Record>();

            //foreach (var item in titles)
            //{
            //    Console.WriteLine(item.Text);
            //    output.Add(new Record() { Item = item.Text });
            //}


            //var firstResult = driver.FindElement(By.ClassName("yuRUbf"));
            //var link = firstResult.FindElement(By.TagName("a"));

            //driver.Navigate().GoToUrl(link.GetAttribute("href"));

            //// reference: https://code-maze.com/csharp-writing-csv-file/
            //using (var writer = new StreamWriter("output.csv"))
            //using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            //{
            //    csv.WriteRecords(output);
            //}

            //// reference: https://code-maze.com/csharp-write-json-into-a-file/
            //JsonFileUtils.SimpleWrite(output, "output.json");
        }
    }
    
    public class Record
    {
        public string Item { get; set; }
    }

}
