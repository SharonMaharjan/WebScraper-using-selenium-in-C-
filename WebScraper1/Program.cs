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
            //user input to choose program
            Console.WriteLine("Youtube, Ictjob, Twitter(Which one do you want to scrape?) Press the number of your choice:");
            Console.WriteLine("1: Youtube");
            Console.WriteLine("2: Ictjob");
            Console.WriteLine("3: Twitter");
            Console.WriteLine("Only numbers!");

            switch (int.Parse(Console.ReadLine()))
            {
                case 1:
                    WebScraper0.Youtube.Yt();
                    break;
                case 2:
                    WebScraper1.Ictjob.Job();
                    break;
                case 3:
                    WebScraper2.Twitter.Tweet();
                    break;
            }

            
        }
    }

   
}