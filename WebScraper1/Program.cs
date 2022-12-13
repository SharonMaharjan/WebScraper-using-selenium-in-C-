using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

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

             foreach( var item in titles)
             {
                Console.WriteLine(item.Text);
             }

            //var firstResult = driver.FindElement(By.ClassName("yuRUbf"));
            //var link = firstResult.FindElement(By.TagName("a"));

            //driver.Navigate().GoToUrl(link.GetAttribute("href"));
        }
    }
}