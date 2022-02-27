using System;
using System.Drawing;
using System.Management;



namespace ScreenRec2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start");
            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
            int width = 1920;
            int height = 1080;
            foreach (ManagementObject queryObj in searcher.Get())
            {
                if (queryObj["CurrentHorizontalResolution"] != null)
                {
                    width = int.Parse(queryObj["CurrentHorizontalResolution"].ToString());
                    height = int.Parse(queryObj["CurrentVerticalResolution"].ToString());
                    Console.WriteLine(width);
                    Console.WriteLine(height);
                    break;
                }
            }
            //foreach (var screen in System.Windows.Forms.Screen.AllScreens)
            //{
            //    Console.WriteLine(screen.Bounds.Width);
            //    Console.WriteLine(screen.Bounds.Height);
            //}

            var screenRecord = new ScreenRecord(new Rectangle(0, 0, width, height), "_");
            //screenRecord.RecordVideo();
            Console.WriteLine("End");
            Console.ReadLine();
        }
    }
}
