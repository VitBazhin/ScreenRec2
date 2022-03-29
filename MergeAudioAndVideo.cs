using System.Diagnostics;

namespace ScreenRec2
{
    internal class MergeAudioAndVideo
    {
        private readonly string _finalName=$"Screenrecording_{Background.UniqName()}.mp4";
        public void Mergefile(string audioPath, string videoPath,string outputPath)
        {
            var startInfo = new ProcessStartInfo()
            {
                CreateNoWindow = false,
                //FileName = @"C:\Users\ilya_\source\repos\ScreenRec3\bin\Debug\ffmpeg.exe",
                FileName="ffmpeg.exe",//экзешник ffmpeg лежит в bin'е приложения
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = string.Format(" -i {0} -i {1} -shortest {2} -y", $"{videoPath}//video.mp4", $@"{audioPath}//audio.wav", $"{outputPath}//{_finalName}")
            };

            using (var exeProcess = Process.Start(startInfo))
            {
                //exeProcess.WaitForExit();
                exeProcess.Close();
            }
        }

        
    }
}
