﻿using Accord.Video.FFMPEG;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace ScreenRec2
{
    class Video: IVideo
    {
        private readonly Stopwatch _watch = new Stopwatch();

        public Video(Rectangle bounds,string videoPath,string tempPath)
        {
            _bounds = bounds;
            _outputPath = videoPath;
            _tempPath= tempPath;
        }

        private Rectangle _bounds;
        private readonly string _outputPath;
        private readonly string _tempPath; 

        private int _fileCount = 1;
        private readonly List<string> inputImagesSequence = new List<string>() { };
        private readonly string _videoName = "video.mp4";

        public void RecordVideo()
        {
            _watch.Start();
            using (var bitmap = new Bitmap(_bounds.Width, _bounds.Height))
            {
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.CopyFromScreen(new Point(_bounds.Left, _bounds.Top), Point.Empty, _bounds.Size);
                    string name = $"{_tempPath}//screenshot_{_fileCount++}.png";
                    bitmap.Save(name, ImageFormat.Png);
                    inputImagesSequence.Add(name);
                }
            }
        }

        public void SaveVideo()
        {
            int width = _bounds.Width;
            int height = _bounds.Height;
            int frameRate = 30;

            using (var videoFileWriter = new VideoFileWriter())
            {
                videoFileWriter.Open($"{_outputPath}//{_videoName}", width, height, frameRate, VideoCodec.MPEG4);
                foreach (var imagePath in inputImagesSequence)
                {
                    using (Bitmap bitmap = Image.FromFile(imagePath) as Bitmap)
                    {
                        videoFileWriter.WriteVideoFrame(bitmap);
                    }
                }
                videoFileWriter.Close();
                videoFileWriter.Dispose();
            }
        }

        public void StopRecordVideo()
        {
            _watch.Stop();
            Thread.Sleep(1000);
            SaveVideo();
        }
    }
}