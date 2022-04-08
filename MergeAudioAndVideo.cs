using System.Diagnostics;

namespace ScreenRec2
{
    internal static class MergeAudioAndVideo
    {
        private static readonly string _finalName=$"Screenrecording_{FileShell.UniqueName()}.mp4";
        public static void Mergefile(string inputPath, string outputPath,string videoName,string audioName)
        {
            #region lastMethod
            //var startInfo = new ProcessStartInfo()
            //{
            //    CreateNoWindow = true,
            //    FileName="ffmpeg.exe",
            //    UseShellExecute = false,
            //    RedirectStandardOutput = true,
            //    RedirectStandardError = true,
            //    Arguments = string.Format(" -i {0} -i {1} -shortest {2} -y", $"{inputPath }//video.mp4", $"{inputPath }//audio.wav", $"{outputPath}//{_finalName}")
            //};

            //using (var exeProcess = Process.Start(startInfo))
            //{
            //    string StdOutVideo = exeProcess.StandardOutput.ReadToEnd();
            //    string StdErrVideo = exeProcess.StandardError.ReadToEnd();
            //    exeProcess.WaitForExit(3000);
            //    exeProcess.Close();
            //}
            #endregion

            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = "ffmpeg.exe",//экзешник ffmpeg лежит в bin'е приложения
                Arguments = string.Format(" -i {0} -i {1} -shortest {2} -y", $"{inputPath }//{videoName}", $"{inputPath }//{audioName}", $"{outputPath}//{_finalName}")
            };
            using (var exeProcess = Process.Start(startInfo))
            {
                exeProcess.WaitForExit();
            }
        }
    }
}
