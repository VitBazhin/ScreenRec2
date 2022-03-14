using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using Accord.Video.FFMPEG;

namespace ScreenRec2
{
    public class ScreenRecord:IScreenRecord
    {
        public Rectangle Bounds { get; set; }
        public string OutputPath { get; set; }
        public string TempPath { get; set; }
        
        private int fileCount = 1;
        private List<string> inputImagesSequence = new List<string>() { };

        private readonly string audioName = "mic.wav";
        private readonly string videoName = "video.mp4";
        public string FinalName { get; set; } = "FinalVideo.mp4";

        private readonly Stopwatch watch = new Stopwatch();
        

        //public ScreenRecord()
        //{
        //    создать папку для скриншотов, если ее нет
        //    Directory.CreateDirectory(TempPath);
        //    Directory.CreateDirectory(OutputPath);
        //}
                
        private void DeleteFiles(string targetDirName, string exceptFileName = "")
        {
            bool notExistsException = string.IsNullOrEmpty(exceptFileName);
            foreach (var fileName in Directory.GetFiles(targetDirName))
            {
                if (notExistsException || exceptFileName != fileName)
                {
                    File.SetAttributes(fileName, FileAttributes.Normal);
                    File.Delete(fileName);
                }
            }
            foreach (var dir in Directory.GetDirectories(targetDirName))
            {
                DeleteFiles(dir);
            }
            if (notExistsException)
            {
                Directory.Delete(targetDirName, false);
            }
        }

        //public string GetElapsed()
        //{
        //    return string.Format("{0:D2}:{1:D2}:{2:D2}", watch.Elapsed.Hours, watch.Elapsed.Minutes, watch.Elapsed.Seconds);
        //}

        public void RecordVideo()
        {
            watch.Start();
            using (Bitmap bitmap = new Bitmap(Bounds.Width, Bounds.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(new Point(Bounds.Left, Bounds.Top), Point.Empty, Bounds.Size);
                    string name = $"{TempPath}//screenshot_{fileCount++}.png";
                    bitmap.Save(name, ImageFormat.Png);
                    inputImagesSequence.Add(name);
                }
            }
        }

        public void RecordAudio()
        {
            _ = NativeMethods.Record("open new Type waveaudio Alias recsound", "", 0, 0);
            _ = NativeMethods.Record("record recsound", "", 0, 0);
        }

        private void SaveVideo(int width, int height, int frameRate)
        {
            using (VideoFileWriter videoFileWriter = new VideoFileWriter())
            {
                videoFileWriter.Open($"{OutputPath}//{videoName}", width, height, frameRate, VideoCodec.MPEG4);
                foreach (var imagePath in inputImagesSequence)
                {
                    using (Bitmap bitmap = Image.FromFile(imagePath) as Bitmap)
                    {
                        videoFileWriter.WriteVideoFrame(bitmap);
                    }
                }
                videoFileWriter.Close();
            }
        }

        private void SaveAudio()
        {
            NativeMethods.Record($"save recsound {OutputPath}//{audioName}", "", 0, 0);
            NativeMethods.Record("close recsound", "", 0, 0);
        }

        private void CombineVideoAndAudio(string video, string audio)
        {
            string command = $"/c ffmpeg -i \"{video}\" -i \"{audio}\" -shortest {FinalName}";
            ProcessStartInfo processStartInfo = new ProcessStartInfo 
            {
                CreateNoWindow = false,
                FileName = "cmd.exe",
                WorkingDirectory = OutputPath,
                Arguments = command 
            };
            using (Process execProcess = Process.Start(processStartInfo))
            {
                execProcess.WaitForExit();
            }
        }

        public void Stop()
        {
            watch.Stop();
            int width = Bounds.Width;
            int height = Bounds.Height;
            int frameRate = 10;

            Thread.Sleep(1000);
            SaveAudio();
            
            Thread.Sleep(1000);
            SaveVideo(width, height, frameRate);

            CombineVideoAndAudio(videoName, audioName);

            DeleteFiles(TempPath);
            //DeleteFiles(outPath, $"{outPath}//{finalName}");
        }

    }

    public static class NativeMethods
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int Record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
    }
}
