using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

using Accord.Video.FFMPEG;

namespace ScreenRec2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start");

            new ScreenRecord(new Rectangle(0, 0, 1000, 800), "");

            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
