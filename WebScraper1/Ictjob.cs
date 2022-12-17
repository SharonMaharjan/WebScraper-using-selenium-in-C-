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
    public class Ictjob
    {
        //public static Record Info(IWebElement job, IWebElement jobInfo)
        //{
        //    IWebElement title = job.FindElement(By.XPath("/html/body/section/div[1]/div/div[2]/div/div/form/div[2]/div/div/div[2]/section/div/div[2]/div[1]/div/ul/li[1]/span[2]/a/h2"));
        //    IWebElement company = jobInfo.FindElement(By.XPath("/html/body/section/div[1]/div/div[2]/div/div/form/div[2]/div/div/div[2]/section/div/div[2]/div[1]/div/ul/li[1]/span[2]/span[1]"));
        //    IWebElement location = jobInfo.FindElement(By.ClassName("job-location"));
        //    IWebElement keyword = jobInfo.FindElement(By.ClassName("tag selection-item"));
        //    IWebElement link = jobInfo.FindElement(By.ClassName("href"));

        //    return new Record()
        //    {
        //      Title = title.Text;
        //    Company = company.Text;
        //    Location = location.Text;
        //    Keywords = keyword.Text;
        //    Link = link.Text;
        //};
        

           

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
            Thread.Sleep(7000);

            IWebElement datebutton =  driver.FindElement(By.CssSelector("#sort-by-date"));
            IJavaScriptExecutor javascriptExecutor = (IJavaScriptExecutor)driver;
            javascriptExecutor.ExecuteScript("arguments[0].click();", datebutton);
            

            //for the privacy cookie
            try
            {
                
                driver.FindElement(By.XPath("/html/body/div[2]/a")).Click();


            }
            catch { }


            int counter = 0;
            var T = true;
            var output = new List<Record>();
            //while (T == true)
            //{
                //var jobs = driver.FindElements(By.ClassName("search-item"));
                var jobs = driver.FindElements(By.XPath("/html/body/section/div[1]/div/div[2]/div/div/form/div[2]/div/div/div[2]/section/div/div[2]/div[1]/div/ul/li[2]"));

                for (int i = 0; i < 5; i++)
                {
                    Console.WriteLine("5 records of ictjobs");
                    output.Add(new Record()
                    {
                        Title = jobs[i].FindElement(By.XPath("/html/body/section/div[1]/div/div[2]/div/div/form/div[2]/div/div/div[2]/section/div/div[2]/div[1]/div/ul/li[1]/span[2]/a/h2")).Text
                    });
                    output.Add(new Record()
                    {
                        Company = jobs[i].FindElement(By.XPath("/html/body/section/div[1]/div/div[2]/div/div/form/div[2]/div/div/div[2]/section/div/div[2]/div[1]/div/ul/li[1]/span[2]/span[1]")).Text
                    });
                    output.Add(new Record() { Location = jobs[i].FindElement(By.XPath("/html/body/section/div[1]/div/div[2]/div/div/form/div[2]/div/div/div[2]/section/div/div[2]/div[1]/div/ul/li[1]/span[2]/span[2]/span[2]/span/span")).Text });
                    output.Add(new Record() { Keywords = jobs[i].FindElement(By.XPath("/html/body/section/div[1]/div/div[2]/div/div/form/div[2]/div/div/div[2]/section/div/div[2]/div[1]/div/ul/li[1]/span[2]/span[3]")).Text });
                    output.Add(new Record() { Link = jobs[i].GetAttribute("href") });

                }
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
