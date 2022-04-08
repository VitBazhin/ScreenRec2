using System.Runtime.InteropServices;

namespace ScreenRec2
{
    class Audio: IAudio
    {
        [DllImport("winmm.dll", EntryPoint = "mciSendStringA", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern int mciSendString(string lpstrCommand, string lpstrReturnString, int uReturnLength, int hwndCallback);

        private readonly string _audioPath;
        private readonly string _audioName;
        public Audio(string audioPath, string audioName)
        {
            _audioPath = audioPath;
            _audioName = audioName;
        }
        public void RecordAudio()
        {
            mciSendString("open new Type waveaudio Alias recsound", "", 0, 0);
            mciSendString("record recsound", "", 0, 0);
        }

        public void StopRecordAudio()
        {
            mciSendString($"save recsound {_audioPath}//{_audioName}", "", 0, 0);
            mciSendString("close recsound ", "", 0, 0);
        }
    }
}