using NAudio.Wave;
using System.Threading;

namespace ScreenRec2
{
    class Audio//private
    {
        WaveFileWriter waveFile;
        WaveInEvent waveSource;

        

        //public string AudioPath { get; set; }

        public void RecordAudio()
        {
            //WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(0);
            waveSource = new WaveInEvent
            {
                //waveSource.DeviceNumber = 0;
                WaveFormat = new WaveFormat(44100, 1)//sample rate - частота дискретизации
            };

            waveSource.DataAvailable += (s, e) =>
            {
                waveFile.WriteData(e.Buffer, 0, e.BytesRecorded);
            };

            string tempFile = (@"C:\Temp\Audio\test1.wav");
            waveFile = new WaveFileWriter(tempFile, waveSource.WaveFormat);
            waveSource.StartRecording();
        }
        public void StopRecordAudio()
        {
            waveSource.StopRecording();
            waveFile.Dispose();
        }
    }
}

