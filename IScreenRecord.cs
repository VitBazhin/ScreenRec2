namespace ScreenRec2
{
    public interface IScreenRecord
    {
        void RecordVideo();
        void RecordAudio();
        void SaveVideo(int width, int height, int frameRate);
        void SaveAudio();
        void CombineVideoAndAudio(string video, string audio);
        void Stop();
    }
}
