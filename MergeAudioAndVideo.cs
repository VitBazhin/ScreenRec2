using System.Diagnostics;

namespace ScreenRec2
{
    internal static class MergeAudioAndVideo
    {
        private static readonly string _finalName=$"Screenrecording_{OF.UniqueName()}.mp4";
        public static void Mergefile(string /*audioVideoPath*/ audioPath, string videoPath, string outputPath)
        {
            var startInfo = new ProcessStartInfo()
            {
                CreateNoWindow = false,
                FileName="ffmpeg.exe",//экзешник ffmpeg лежит в bin'е приложения
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = string.Format(" -i {0} -i {1} -shortest {2} -y", $"{videoPath }//video.mp4", $@"{audioPath }//audio.wav", $"{outputPath}//{_finalName}")
            };

            using (var exeProcess = Process.Start(startInfo))
            {
                //exeProcess.WaitForExit();
                exeProcess.Close();
            }
        }

        
    }
}
