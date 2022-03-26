using System.Drawing;

namespace ScreenRec2
{
    public interface IScreenRecord
    {
        Rectangle Bounds { get; set; }
        string OutputPath { get; set; }
        string TempPath { get; set; }
        void RecordVideo();
        void RecordAudio();
        void SaveVideo(int width, int height, int frameRate);
        void SaveAudio();
        void CombineVideoAndAudio(string video, string audio);
        void Stop();
    }
}
