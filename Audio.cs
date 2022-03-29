using NAudio.Wave;

namespace ScreenRec2
{
    class Audio: IAudio
    {
        private WaveFileWriter _waveFile;
        private WaveInEvent _waveSource;

        private readonly string _audioPath;
        private readonly string _audioName = "audio.wav";
        public Audio(string audioPath)
        {
            _audioPath = audioPath;
        }

        public void RecordAudio()
        {
            //WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(0);
            _waveSource = new WaveInEvent
            {
                //waveSource.DeviceNumber = 0;
                WaveFormat = new WaveFormat(44100, 1)//sample rate - частота дискретизации
            };

            _waveSource.DataAvailable += (s, e) =>
            {
                _waveFile.Write(e.Buffer, 0, e.BytesRecorded);
            };

            _waveFile = new WaveFileWriter($"{_audioPath}//{_audioName}", _waveSource.WaveFormat);
            _waveSource.StartRecording();
        }
        public void StopRecordAudio()
        {
            _waveSource.StopRecording();
            _waveFile.Dispose();
        }
    }
}