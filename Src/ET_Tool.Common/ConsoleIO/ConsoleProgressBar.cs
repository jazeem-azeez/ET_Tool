using System;

namespace ET_Tool.Common.ConsoleIO
{
    public class ConsoleProgressBar
    {
        public ConsoleProgressBar()
        {
        }
        public void DrawTextProgressBar(int progress, int total, int level = 0, int startCursorLeft = 0, int size = 32)
        {
            Console.CursorVisible = false;
            int currTop = Console.CursorTop;
            Console.CursorTop = Console.LargestWindowHeight - (2*(level+1));


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
            Console.Write(progress.ToString() + " of " + total.ToString() + $" {this.Turn(progress)} "); //blanks at the end remove any excess
            Console.CursorVisible = true;
            //Console.CursorTop = currTop;


        }
        private string Turn(int progress)
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
}
