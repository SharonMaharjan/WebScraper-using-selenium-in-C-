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



            WebDriverWait wait = new WebDriverWait(driver,TimeSpan.FromSeconds(5));

            // visible cookies
            IWebElement ytCookies = wait.Until(
                driver => driver.FindElement(By.XPath("/html/body/ytd-app/ytd-consent-bump-v2-lightbox/tp-yt-paper-dialog/div[4]/div[2]/div[6]/div[1]/ytd-button-renderer[2]/yt-button-shape/button/yt-touch-feedback-shape/div/div[2]")));
            ytCookies.Click();
            Thread.Sleep(2500);


            IWebElement webElement = driver.FindElement(By.Name("search_query"));
            
            webElement.SendKeys(userInput);
            webElement.SendKeys(Keys.Enter);
            Thread.Sleep(2500);
            

            //clicks filter
            IWebElement filterbutton = wait.Until(
                driver => driver.FindElement
                (By.XPath("/html/body/ytd-app/div[1]/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div[2]/div/ytd-section-list-renderer/div[1]/div[2]/ytd-search-sub-menu-renderer/div[1]/div/ytd-toggle-button-renderer/yt-button-shape/button/yt-touch-feedback-shape/div/div[2]")));
            filterbutton.Click();
            //Thread.Sleep(2000);

            //clicks upload date

            driver.FindElement(By.XPath("/html/body/ytd-app/div[1]/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div[2]/div/ytd-section-list-renderer/div[1]/div[2]/ytd-search-sub-menu-renderer/div[1]/iron-collapse/div/ytd-search-filter-group-renderer[5]/ytd-search-filter-renderer[2]/a/div/yt-formatted-string")).Click();

            driver.FindElement(By.XPath("//*[@id='label']/yt-formatted-string")).Click();
            //Thread.Sleep(2500);

            var videos = driver.FindElements(By.TagName("ytd-video-renderer"));
            var output = new List<Record>();
            for (int i = 0; i < 5; i++)
            {
                
                Console.WriteLine("5 records of youtube");
                output.Add(new Record() { Link = videos[i].FindElement(By.TagName("a")).GetAttribute("href") });
                output.Add(new Record() { Title = videos[i].FindElement(By.TagName("h3")).Text });
                output.Add(new Record() { ChannelName = videos[i].FindElement(By.Id("channel-info")).Text });
                output.Add(new Record() { View = videos[i].FindElement(By.Id("metadata-line")).FindElement(By.XPath("./span[1]")).Text });
                


            }
            using (var writer = new StreamWriter("youtube.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                //csv.Configuration.Delimiter = ","
                //writer.WriteLine("Sep=,");
                csv.WriteRecords(output);
            }
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
