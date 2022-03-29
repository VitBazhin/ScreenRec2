using System.Diagnostics;

namespace ScreenRec2
{
    internal class MergeAudioAndVideo
    {
        public void Mergefile(string audioPath, string videoPath)
        {
            var startInfo = new ProcessStartInfo()
            {
                CreateNoWindow = false,
                //FileName = @"C:\Users\ilya_\source\repos\ScreenRec3\bin\Debug\ffmpeg.exe",
                FileName="ffmpeg.exe",//экзешник ffmpeg лежит в bin'е приложения
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = string.Format(" -i {0} -i {1} -shortest {2} -y", @"C:\Temp\Video\video.mp4", @"C:\Temp\Audio\audio.wav", @"C:\Temp\result.mp4")
            };

            using (var exeProcess = Process.Start(startInfo))
            {
                //exeProcess.WaitForExit();
                exeProcess.Close();
            }
        }
    }
}
