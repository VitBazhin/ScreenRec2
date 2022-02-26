using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

using Accord.Video.FFMPEG;
//using Accord;

namespace ScreenRec
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
        private string finalName = "FinalVideo.mp4";

        Stopwatch watch = new Stopwatch();

        public static class NativeMethods
        {
            [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
            public static extern int Record(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

            public static bool IsRunning { get; private set; }
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

        public string GetElapsed()
        {
            return string.Format("{0:D2}:{1:D2}:{2:D2}", watch.Elapsed.Hours, watch.Elapsed.Minutes, watch.Elapsed.Seconds);
        }

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
                bitmap.Dispose();//? using already dispose
            }
        }

        public void RecordAudio()
        {
            _ = NativeMethods.Record("open new Type waveaudio Alias recsound", "", 0, 0);
            _ = NativeMethods.Record("record recsound", "", 0, 0);
        }

        //Timer timer = new Timer(new TimerCallback(new ));
        internal void SaveVideo(int width, int height, int frameRate)
        {


            using (VideoFileWriter videoFileWriter = new VideoFileWriter())
            {

            }


        }
    }
}
