using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
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

            var chromeDriver = ChromeDriverService.CreateDefaultService();
            chromeDriver.SuppressInitialDiagnosticInformation = true;
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("--log-level=3");


            IWebDriver driver = new ChromeDriver(chromeDriver, options);
            driver.Navigate().GoToUrl("https://www.ictjob.be/");
        }
    }
}
