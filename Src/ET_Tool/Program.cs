using System;
using System.Threading.Tasks;

namespace ET_Tool
{
    internal class Program
    {
        public class ConsoleProgressBar
        {
            public ConsoleProgressBar()
            {
            }
            public void DrawTextProgressBar(int progress, int total, int startCursorLeft = 0, int size = 32)
            {
                Console.CursorVisible = false;

                //draw empty progress bar
                int startPos = startCursorLeft + 0;
                Console.CursorLeft = startPos;
                Console.Write("["); //start
                int endPos = startCursorLeft + size;
                Console.CursorLeft = endPos + 2;
                Console.Write("]"); //end
                Console.CursorLeft = 1;
                float onechunk = (float)(size + 1) / total;
                int position = 1;
                for (int i = 0; i <= endPos; i++)
                {
                    if (i < onechunk * progress)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Blue;
                    }
                    Console.CursorLeft = position++;
                    Console.Write(" ");
                }

                //draw totals
                Console.CursorLeft = endPos + 8;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(progress.ToString() + " of " + total.ToString() + $" {Turn(progress)} "); //blanks at the end remove any excess
                Console.CursorVisible = true;


            }
            public string Turn(int progress)
            {
                switch (progress % 5)
                {
                    case 0: return ("|");
                    case 1: return ("/");
                    case 2: return ("-");
                    case 3: return ("\\");
                    default: return ("-");
                }

            }
        }

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
