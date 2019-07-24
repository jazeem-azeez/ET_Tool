using System;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using ET_Tool.Common.ConsoleIO;

namespace ET_Tool
{
    internal class Program
    {
  

        private static void Main(string[] args)
        {
            Console.WriteLine("ET_Tool");
            ConsoleProgressBar progressBar = new ConsoleProgressBar();
            Console.Write("Working....");
            int i = 0;
            while (i <= 100)
            {
                progressBar.DrawTextProgressBar(i, 100);
                i++;
                Task.Delay(100).GetAwaiter().GetResult();
            }

            using (var reader = new StreamReader("path\\to\\file.csv"))
            using (var csv = new CsvReader(reader))
            {
                var records = csv.GetRecords<dynamic>();
            }
            Console.ReadLine();
            /*
             * 1. Load Configurations
             *      Get Emit Configuration
             *          [
             *          destination type
             *          destination colums
             *          ]
             *      Get Ingest Configuration
             *          [
             *          Load Main Data Source 
             *          Load Lookups
             *          Load Transformation Lookups
             *          ]
             *  
             * 2. Run Extraction Phase
             * 3. Run Transformation Phase
             * 4. Emit OutPut
             * 5. CheckSum Output , Input
             * 
             */
        }
    }
}
