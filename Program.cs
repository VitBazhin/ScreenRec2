﻿using System;
using System.Drawing;
using System.Management;
using System.Timers;
using System.Configuration;
using System.IO;

namespace ScreenRec2
{
    //Добавить уникальное конечное имя, чтобы не было перезаписи файла в случае повторного запуска программы

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
                    Console.WriteLine($"{key} = {result}");
                    return true;
                }
                catch (ConfigurationErrorsException ex)
                {
                    Console.WriteLine($"Error reading app settings {ex}");
                    return false;
                }
            }
            string outputPath;
            string tempPath;
            string timerIntervalSetting;
            string finalName;

            if (!(TryGetSetting(nameof(outputPath), out outputPath)
                && TryGetSetting(nameof(timerIntervalSetting), out timerIntervalSetting)
                && int.TryParse(timerIntervalSetting, out int timerInterval)
                && TryGetSetting(nameof(finalName), out finalName)
                && TryGetSetting(nameof(tempPath), out tempPath)
                ))
            {
                Console.WriteLine("GetSettingError. Press Enter for exit and fix app.config");
                Console.ReadLine();
                return;
            }

            Directory.CreateDirectory(tempPath);
            Directory.CreateDirectory(outputPath);

            var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
            int width = 1920;
            int height = 1080;

            var screenRecord = new ScreenRecord()
            {
                Bounds=new Rectangle(0, 0, width, height),
                OutputPath = outputPath,
                TempPath = tempPath,
                FinalName = finalName
            };


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


            var timer = new Timer
            {
                Interval = timerInterval
            };

            timer.Elapsed += (sender, e) => 
            {
                screenRecord.RecordAudio();
                screenRecord.RecordVideo();
            };
            

            bool isExit = false;
            do
            {
                var readKey = Console.ReadKey();
                if ((readKey.Modifiers == ConsoleModifiers.Control
                    && readKey.Key == ConsoleKey.Z)
                    || 
                    (
                    (readKey.Modifiers & ConsoleModifiers.Control) != 0
                    && (readKey.Modifiers & ConsoleModifiers.Shift) != 0
                    && readKey.Key == ConsoleKey.Z))
                {
                    Console.WriteLine("Exit");
                    isExit = true;
                }
                else if (readKey.Modifiers == ConsoleModifiers.Control
                    && readKey.Key == ConsoleKey.R)
                {
                    Console.WriteLine("Start rec");
                    timer.Start();
                }
                else if (readKey.Modifiers == ConsoleModifiers.Control
                    && readKey.Key == ConsoleKey.W)
                {
                    Console.WriteLine("Stop rec");
                    timer.Stop();
                    screenRecord.Stop();
                }
            } while (!isExit);

            Console.WriteLine("End. Press Enter");
            Console.ReadLine();

        }
    }
}
