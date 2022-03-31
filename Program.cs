using System;
using System.Drawing;
using System.Management;
using System.Timers;
using System.IO;

namespace ScreenRec2
{
    //Ideas: Приостановка видео
    //Ideas: save or non-save?

    //fix: останавливать аудио до начала обработки скриншотов видео
    //fix: подобрать нужное время кадров
    //fix: улучшить качество
    //fix: удаление Temp файлов и ненужных библиотек
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Info:");

            string outputPath;
            string timerIntervalSetting;
            string videoPath;
            string tempPath;
            string audioPath;

            if (!(Background.TryGetSetting(nameof(outputPath), out outputPath)
                && Background.TryGetSetting(nameof(audioPath),out audioPath)
                && Background.TryGetSetting(nameof(videoPath),out videoPath)
                && Background.TryGetSetting(nameof(tempPath), out tempPath)
                && Background.TryGetSetting(nameof(timerIntervalSetting), out timerIntervalSetting)
                && int.TryParse(timerIntervalSetting, out int timerInterval)
                ))
            {
                Console.WriteLine("GetSettingError. Press 'Enter' for exit and fix app.config");
                Console.ReadLine();
                return;
            }

            Directory.CreateDirectory(tempPath);
            Directory.CreateDirectory(videoPath);
            Directory.CreateDirectory(audioPath);

            var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
            int width=0;
            int height=0;
            
            foreach (ManagementObject queryObj in searcher.Get())
            {
                if (queryObj["CurrentHorizontalResolution"] != null)
                {
                    width = int.Parse(queryObj["CurrentHorizontalResolution"].ToString());
                    height = int.Parse(queryObj["CurrentVerticalResolution"].ToString());
                    Console.WriteLine($"{width}x{height}");
                    break;
                }
            }

            var video = new Video(new Rectangle(0, 0, width, height), videoPath, tempPath);
            var audio = new Audio(audioPath);
            var timer = new Timer
            {
                Interval = timerInterval
            };

            timer.Elapsed += (sender, e) =>
            {
                video.RecordVideo();
                audio.RecordAudio();
            };

            bool isExit = false;
            do
            {
                var readKey = Console.ReadKey();
                if (readKey.Modifiers == ConsoleModifiers.Control
                    && readKey.Key == ConsoleKey.R)
                {
                    Console.WriteLine("Start record");
                    timer.Start();
                }
                else if (readKey.Modifiers == ConsoleModifiers.Control
                    && readKey.Key == ConsoleKey.W)
                {
                    Console.WriteLine("Stop record");
                    timer.Stop();
                    video.StopRecordVideo();
                    audio.StopRecordAudio();

                    Background.DeleteFiles(tempPath);
                    MergeAudioAndVideo.Mergefile(audioPath, videoPath, outputPath);
                    
                    isExit = true;
                }
            } while (!isExit);

            //Background.DeleteFiles(videoPath);
            //Background.DeleteFiles(audioPath);

            Console.WriteLine("The video and the audio was saved successfully. Press 'Enter'");
            Console.ReadLine();
        }
    }
}