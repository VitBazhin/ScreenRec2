using Accord.Video.FFMPEG;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace ScreenRec2
{
    class Video
    {
        private readonly Stopwatch watch = new Stopwatch();

        public Rectangle Bounds { get; /*private*/ set; }
        public string OutputPath { get; /*private*/ set; }
        public string TempPath { get; /*private*/ set; }

        private int fileCount = 1;
        private readonly List<string> inputImagesSequence = new List<string>() { };
        private readonly string videoName = $"video_{CreateGuid()}.mp4";

        public void RecordVideo()
        {
            watch.Start();
            using (var bitmap = new Bitmap(Bounds.Width, Bounds.Height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(new Point(Bounds.Left, Bounds.Top), Point.Empty, Bounds.Size);
                    string name = $"{TempPath}//screenshot_{fileCount++}.png";
                    bitmap.Save(name, ImageFormat.Png);
                    inputImagesSequence.Add(name);
                }
            }
        }

        public void SaveVideo()
        {
            int width = Bounds.Width;
            int height = Bounds.Height;
            int frameRate = 10;

            using (var videoFileWriter = new VideoFileWriter())
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

        public void StopRecordVideo()
        {
            watch.Stop();
            Thread.Sleep(1000);
            SaveVideo();
            Thread.Sleep(1000);
            DeleteFiles(TempPath);
        }

        public void DeleteFiles(string targetDirName)
        {
            foreach (var fileName in Directory.GetFiles(targetDirName))
            {
                if (fileName != null)
                {
                    File.SetAttributes(fileName, FileAttributes.Normal);
                    File.Delete(fileName);
                }
            }
            foreach (var dir in Directory.GetDirectories(targetDirName))
            {
                DeleteFiles(dir);
            }
            if (string.IsNullOrEmpty(""))
            {
                Directory.Delete(targetDirName, false);
            }
        }

        #region Comments
        /// <summary>
        /// Creates a unique Guid of 5 characters
        /// </summary>
        /// <returns></returns>
        #endregion
        public static string CreateGuid()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 5);
        }
    }
}