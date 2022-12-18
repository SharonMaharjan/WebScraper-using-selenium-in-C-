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

namespace WebScraper1
{
    public class Ictjob
    {
        public static void Job()
        {
            //User input
            Console.WriteLine("What kind of job are you looking at");
            String userInput = Console.ReadLine();

            //driver parameter
            var chromeDriver = ChromeDriverService.CreateDefaultService();
            chromeDriver.SuppressInitialDiagnosticInformation = true;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--log-level=3");

            //browser parameter
            IWebDriver driver = new ChromeDriver(chromeDriver, options);

            //javascript parameter
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            //navigate to the url
            driver.Navigate().GoToUrl("https://www.ictjob.be/");
            Thread.Sleep(3000);

            //search for the job
            IWebElement webElement = driver.FindElement(By.Id("keywords-input"));
            webElement.SendKeys(userInput);
            webElement.SendKeys(Keys.Enter);
            Thread.Sleep(10000);

            //it sort out with date 
            IWebElement datebutton = driver.FindElement(By.CssSelector("#sort-by-date"));
            IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)driver;
            javascriptExecutor.ExecuteScript("arguments[0].click();", datebutton);


            //closing  the privacy cookie
            try
            {

                driver.FindElement(By.XPath("/html/body/div[2]/a")).Click();


            }
            catch { }


            //explicit wait (it allow the code to halt the program until the condition isn't resolve)
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));


            //it search
            var jobs = driver.FindElements(By.ClassName("search-item"));

            //tries to wait 
            //IWebElement titleinfo = wait.Until(
            //        driver => driver.FindElement(By.ClassName("job-title")));

            //creating a variable for list
            var output = new List<Record>();

            Console.WriteLine("5 records of ictjobs");

            //loop through job
            foreach (var job in jobs.Take(5))
            {
                try
                {
                    output.Add(new Record()
                    {
                        Title = job.FindElement(By.ClassName("job-title")).Text,
                        Company = job.FindElement(By.ClassName("job-company")).Text,
                        Location = job.FindElement(By.XPath("//*[@id='search-result-body']/div/ul/li[1]/span[2]/span[2]/span[2]/span/span")).Text,
                        Keywords = job.FindElement(By.XPath("//*[@id='search-result-body']/div/ul/li[1]/span[2]/span[3]")).Text,
                        Link = job.FindElement(By.XPath("//*[@id='search-result-body']/div/ul/li[1]/span[2]/a")).GetAttribute("href")

                    });
                }
                catch (Exception)
                {
                }
            }


            // // reference: https://code-maze.com/csharp-writing-csv-file/
            // write in csv file
            using (var writer = new StreamWriter("Ictjobs.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(output);
            }

            //// reference: https://code-maze.com/csharp-write-json-into-a-file/
            //write in json file
            JsonFileUtils.SimpleWrite(output, "Ictjobs.json");

        }
        public class Record
        {
            public string Title { get; set; }
            public string Company { get; set; }
            public string Location { get; set; }
            public string Keywords { get; set; }
            public string Link { get; set; }
        }

    }
}
