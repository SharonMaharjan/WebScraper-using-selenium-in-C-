using CsvHelper;
using CsvHelper.Configuration;
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

namespace WebScraper0
{
    class Youtube
    {
        public static void Yt()
        {
            //user enters the input
            Console.WriteLine("Find new videos in youtube");
            String userInput = Console.ReadLine();

            //set driver parameter 
            ChromeOptions options = new ChromeOptions();
            var chromeDriver = ChromeDriverService.CreateDefaultService();
            chromeDriver.SuppressInitialDiagnosticInformation = true;
            options.AddArgument("--log-level=3");

            //browser parameter
            IWebDriver driver = new ChromeDriver(chromeDriver, options);

            //it navigates to the url 
            driver.Navigate().GoToUrl("https://www.youtube.com");


            // explicit wait (it allow the code to halt the program until the condition isn't resolve)
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            // visible cookies and accept the cookie
            IWebElement ytCookies = wait.Until(
                driver => driver.FindElement(By.XPath("/html/body/ytd-app/ytd-consent-bump-v2-lightbox/tp-yt-paper-dialog/div[4]/div[2]/div[6]/div[1]/ytd-button-renderer[2]/yt-button-shape/button/yt-touch-feedback-shape/div/div[2]")));
            ytCookies.Click();
            Thread.Sleep(2500);

            //it tries to find the user input 
            IWebElement webElement = driver.FindElement(By.Name("search_query"));
            webElement.SendKeys(userInput);
            webElement.SendKeys(Keys.Enter);
            Thread.Sleep(2500);


            //clicks filter option
            IWebElement filterbutton = wait.Until(
                driver => driver.FindElement
                (By.XPath("//*[@id='container']/ytd-toggle-button-renderer/yt-button-shape/button/yt-touch-feedback-shape/div/div[2]")));
            filterbutton.Click();
            Thread.Sleep(2000);

            //it clicks recent uploaded video date
            driver.FindElement(By.XPath("/html/body/ytd-app/div[1]/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div[2]/div/ytd-section-list-renderer/div[1]/div[2]/ytd-search-sub-menu-renderer/div[1]/iron-collapse/div/ytd-search-filter-group-renderer[5]/ytd-search-filter-renderer[2]/a/div/yt-formatted-string")).Click();
            driver.FindElement(By.XPath("//*[@id='label']/yt-formatted-string")).Click();
            Thread.Sleep(2500);

            //it gets a current list of videos
            var videos = driver.FindElements(By.TagName("ytd-video-renderer"));

            //list of videos
            var output = new List<Record>();

            Console.WriteLine("5 records of youtube");
            //loop through videos
            foreach (var video in videos.Take(5))
            {
                try
                {
                    output.Add(new Record()
                    {
                        Link = video.FindElement(By.TagName("a")).GetAttribute("href"),
                        Title = video.FindElement(By.TagName("h3")).Text,
                        ChannelName = video.FindElement(By.Id("channel-info")).Text,
                        View = video.FindElement(By.Id("metadata-line")).FindElement(By.XPath("./span[1]")).Text
                    });
                }
                catch (Exception)
                {
                }
               
            }

            // reference: https://code-maze.com/csharp-writing-csv-file/
            //writing in csv file
            using (var writer = new StreamWriter("youtube.csv"))

            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(output);
            }

            // reference: https://code-maze.com/csharp-write-json-into-a-file/
            //writing in json file
            JsonFileUtils.SimpleWrite(output, "youtube.json");
        }
    }

    public class Record
    {
        public string Link { get; set; }
        public string Title { get; set; }
        public string ChannelName { get; set; }
        public string View { get; set; }

    }
}
