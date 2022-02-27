using System;
using System.Drawing;
using System.Management;
using System.Timers;
using System.Configuration;
using System.Linq;

namespace ScreenRec2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Start");
            var appSettings = ConfigurationManager.AppSettings;

            bool TryGetSetting(string key, out string result)
            {
                result = "";
                try
                {
                    result = appSettings[key];
                    if (string.IsNullOrEmpty(result))
                    {
                        Console.WriteLine($"Not Found {key}");
                        return false;
                    }
                    Console.WriteLine($"Found {result}");
                    return true;
                }
                catch (ConfigurationErrorsException ex)
                {
                    Console.WriteLine($"Error reading app settings {ex}");
                    return false;
                }
            }
            string outputPath;
            int timerInterval = 10;
            string timerIntervalSetting;
            string finalName;

            if (!(TryGetSetting(nameof(outputPath), out outputPath)
                && TryGetSetting(nameof(timerIntervalSetting), out timerIntervalSetting)
                && int.TryParse(timerIntervalSetting, out timerInterval)
                && TryGetSetting(nameof(finalName), out finalName)
                ))
            {
                Console.WriteLine("Exit");
                Console.ReadLine();
                return;
            }
            

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

            var screenRecord = new ScreenRecord(new Rectangle(0, 0, width, height), outputPath)
            {
                FinalName = finalName
            };

            Timer timer = new Timer
            {
                Interval = timerInterval
            };

            timer.Elapsed += (s, e) => 
            {
                screenRecord.RecordVideo();
                screenRecord.RecordAudio();
            };
            // Timer_Elapsed;
            //screenRecord.RecordVideo();
            //ConsoleKeyInfo consoleKeykey = Console.ReadKey();
            //Console.Read();
            //ConsoleKey[] consoleKeys = new ConsoleKey[] { ConsoleKey.Z, ConsoleKey.C };
            //Console.WriteLine(Console.ReadKey().Key);
            //Console.WriteLine(consoleKeys.Any(k => k == Console.ReadKey().Key));
            //while (consoleKeys.Any(k => k == Console.ReadKey().Key))
            //{
            //    Console.WriteLine("While");
            //}
            Console.WriteLine("End");
            Console.ReadLine();
        }

        //private static void Timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
