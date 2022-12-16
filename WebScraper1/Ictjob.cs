using CsvHelper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraper1
{
    class Ictjob
    {
        public static void Job()
        {
            //User input
            Console.WriteLine("What kind of job are you looking at");
            String userInput = Console.ReadLine();
            //Console.WriteLine("Where are you looking at");
            //String userInput1 = Console.ReadLine();

            var chromeDriver = ChromeDriverService.CreateDefaultService();
            chromeDriver.SuppressInitialDiagnosticInformation = true;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--log-level=3");


            IWebDriver driver = new ChromeDriver(chromeDriver, options);

            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;

            driver.Navigate().GoToUrl("https://www.ictjob.be/");
            Thread.Sleep(3000);

            //search for the job
            IWebElement webElement = driver.FindElement(By.Id("keywords-input"));
            webElement.SendKeys(userInput);
            webElement.SendKeys(Keys.Enter);
            Thread.Sleep(10000);

            //driver.FindElement(By.Id("main-search-button")).Click();
            //driver.FindElement(By.XPath("/html/body/div[2]/a")).Click();
            driver.FindElement(By.ClassName("sort-by-date-container")).Click();
            Thread.Sleep(10000);
            
            
            
            
            
            //search for location
            //IWebElement webElement1 = driver.FindElement(By.Id("smart-search-location-input"));
            //webElement1.SendKeys(userInput1);
            //webElement1.SendKeys(Keys.Enter);
            //Thread.Sleep(2500);

            //driver.FindElement(By.Id("main-search-button")).Click();


            //for the privacy cookie
            try {
                
                driver.FindElement(By.XPath("/html/body/div[2]/a")).Click();


            }
            catch { }



            var jobs = driver.FindElements(By.ClassName("search-item"));
            var output = new List<Record>();
            for (int i = 0; i < 5; i++)
            {

                output.Add(new Record() { Title = jobs[i].FindElement(By.ClassName("job-title")).FindElement(By.XPath("/html/body/section/div[1]/div/div[2]/div/div/form/div[2]/div/div/div[2]/section/div/div[2]/div[1]/div/ul/li[1]/span[2]/a/h2")).Text });
                output.Add(new Record() { Company = jobs[i].FindElement(By.ClassName("job-company")).Text });
                output.Add(new Record() { Location = jobs[i].FindElement(By.ClassName("job-location")).Text });
                output.Add(new Record() { Keywords = jobs[i].FindElement(By.ClassName("tag selection-item")).Text });
                output.Add(new Record() { Link = jobs[i].GetAttribute("href") });

            }
            //while (t == true)
            //{
            //    var jobs = driver.FindElement(By.Id("search-result"));

            //    foreach (var job in jobs)
            //    {

            //        //append part
            //        output.Add(new Record() { Title = job.FindElement(By.ClassName("job-title")).FindElement(By.XPath("/html/body/section/div[1]/div/div[2]/div/div/form/div[2]/div/div/div[2]/section/div/div[2]/div[1]/div/ul/li[1]/span[2]/a/h2")).Text });
            //        output.Add(new Record() { Company = job.FindElement(By.ClassName("job-company")).Text });
            //        output.Add(new Record() { Location = job.FindElement(By.ClassName("job-location")).Text });
            //        output.Add(new Record() { Keywords = job.FindElement(By.ClassName("tag selection-item")).Text });
            //        output.Add(new Record() { Link = job.GetAttribut("href") });


            //    }

            //}
            // reference: https://code-maze.com/csharp-writing-csv-file/
            using (var writer = new StreamWriter("ictjob.csv"))
                          using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                              {
                                 csv.WriteRecords(output);
                             }

                //                // reference: https://code-maze.com/csharp-write-json-into-a-file/
                               JsonFileUtils.SimpleWrite(output, "ictjob.json");

                         
            

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
