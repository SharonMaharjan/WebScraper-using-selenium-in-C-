using CsvHelper;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Globalization;

namespace WebScraper1
{
    class Program
    {
        static void Main(string[] args)
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.google.com");

            //var searchInput =  driver.FindElement(By.Name("q"));
             var searchInput =  driver.FindElement(By.CssSelector(".gLFyf"));

             searchInput.SendKeys("resturant");
             searchInput.Submit();

             var titles = driver.FindElements(By.XPath("//div[@role='heading']"));
            var output = new List<Record>();

            foreach ( var item in titles)
             {
                Console.WriteLine(item.Text);
                output.Add(new Record() { Item= item.Text});
             }


            //var firstResult = driver.FindElement(By.ClassName("yuRUbf"));
            //var link = firstResult.FindElement(By.TagName("a"));

            //driver.Navigate().GoToUrl(link.GetAttribute("href"));

            // reference: https://code-maze.com/csharp-writing-csv-file/
            using (var writer = new StreamWriter("output.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(output);
            }

            // reference: https://code-maze.com/csharp-write-json-into-a-file/
            JsonFileUtils.SimpleWrite(output, "output.json");
        }
    }

    public class Record
    {
        public string Item { get; set; }
    }

}