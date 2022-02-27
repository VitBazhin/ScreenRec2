using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

using Accord.Video.FFMPEG;

namespace ScreenRec2
{
    internal class ScreenRecord
    {
        private Rectangle bounds;
        private string outPath;
        private string tempPath;
        private int fileCount = 1;
        private List<string> inputImagesSequence = new List<string>();

        private string audioName = "mic.wav";
        private string videoName = "video.mp4";
        internal string FinalName { private get; set; } =  "FinalVideo.mp4";

        private readonly Stopwatch watch = new Stopwatch();
        
        public static class NativeMethods
        {
            [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            public static extern int Record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);
        }

        internal ScreenRecord(Rectangle bounds, string outPath)
        {
            this.bounds = bounds;
            this.outPath = outPath;
        }

        private void CreateTempDirectory(string name)
        {
            string pathName = $"C://{name}";
            Directory.CreateDirectory(pathName);
            tempPath = pathName;
        }

        private void DeletePath(string targetDirName)
        {
            foreach (var file in Directory.GetFiles(targetDirName))
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
            foreach (var dir in Directory.GetDirectories(targetDirName))
            {
                DeletePath(dir);
            }
            Directory.Delete(targetDirName, false);
        }

        //public string GetElapsed()
        //{
        //    return string.Format("{0:D2}:{1:D2}:{2:D2}", watch.Elapsed.Hours, watch.Elapsed.Minutes, watch.Elapsed.Seconds);
        //}

        public void RecordVideo()
        {
            watch.Start();
            using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(new Point(bounds.Left, bounds.Right), Point.Empty, bounds.Size);
                    string name = $"{tempPath}//screenshot_{fileCount++}.png";
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
                videoFileWriter.Open($"{outPath}//{videoName}", width, height, frameRate, VideoCodec.MPEG4);
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
            NativeMethods.Record($"save recsound {outPath}//{audioName}", "", 0, 0);
            NativeMethods.Record("close recsound", "", 0, 0);
        }

        private void CombineVideoAndAudio(string video, string audio)
        {
            string command = $"/c ffmpeg -i \"{video}\" -i \"{audio}\" -shortest {FinalName}";
            ProcessStartInfo processStartInfo = new ProcessStartInfo 
            {
                CreateNoWindow = false,
                FileName = "cmd.exe",
                WorkingDirectory = outPath,
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
            int width = bounds.Width;
            int height = bounds.Height;
            int frameRate = 10;

            SaveAudio();
            SaveVideo(width, height, frameRate);

            CombineVideoAndAudio(videoName, audioName);

            DeletePath(tempPath);

            //TODO
            //DeleteFileExcept(outPath, $"{outPath}//{finalName}");
        }

    }
}
