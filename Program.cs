using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;

namespace ScreenRec2
{
    public class Program
    {
        private const string AUDIO_NAME = "audio.wav";
        private const string VIDEO_NAME = "video.mp4";
        public static void Main(string[] args)
        {
            Console.WriteLine("Info:");
            string outputPath;
            string timerIntervalSetting;
            string inputPath = CommonLogic.CreateTempPath();
            string tempPath = inputPath + "//Screenshots";
            Directory.CreateDirectory(tempPath);
            Directory.CreateDirectory(inputPath);
            if (!(CommonLogic.TryGetSetting(nameof(outputPath), out outputPath)
                && CommonLogic.TryGetSetting(nameof(timerIntervalSetting), out timerIntervalSetting)
                && int.TryParse(timerIntervalSetting, out int timerInterval)
                ))
            {
                Console.WriteLine("GetSettingError. Press 'Enter' for exit and fix app.config");
                Console.ReadLine();
                return;
            }
            var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
            int width = 0;
            int height = 0;
            foreach (var queryObj in searcher.Get().Cast<ManagementObject>())
            {
                if (queryObj["CurrentHorizontalResolution"] != null)
                {
                    width = int.Parse(queryObj["CurrentHorizontalResolution"].ToString());
                    height = int.Parse(queryObj["CurrentVerticalResolution"].ToString());
                    Console.WriteLine($"{width}x{height}");
                    break;
                }
            }
            var video = new Video(new Rectangle(0, 0, width, height), inputPath, tempPath, VIDEO_NAME);
            var audio = new Audio(inputPath, AUDIO_NAME);
            var timer = new System.Timers.Timer
            {
                Interval = timerInterval
            };
            timer.Elapsed += (sender, e) =>
            {
                video.RecordVideo();
            };
            timer.Disposed += (sender, e) =>
            {
                audio.StopRecordAudio();
                video.StopRecordVideo();
            };
            bool isExit = false;
            do
            {
                var readKey = Console.ReadKey();
                if (readKey.Modifiers == ConsoleModifiers.Control
                    && readKey.Key == ConsoleKey.R)
                {
                    Console.WriteLine("Start record.");
                    audio.RecordAudio();
                    timer.Start();
                }
                else if (readKey.Modifiers == ConsoleModifiers.Control
                    && readKey.Key == ConsoleKey.W)
                {
                    Console.WriteLine("Stop record.");
                    timer.Dispose();

                    MergeAudioAndVideo.Mergefile(inputPath, outputPath, VIDEO_NAME, AUDIO_NAME);

                    isExit = true;
                    Console.WriteLine("The video and the audio was saved successfully. Press 'Enter'.");
                }
            } while (!isExit);
            CommonLogic.DeleteFiles(inputPath);
            Console.ReadLine();
        }
    }
}