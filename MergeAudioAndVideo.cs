using System.Diagnostics;
using System.Threading;

namespace ScreenRec2
{
    internal class MergeAudioAndVideo
    {
        public void Mergefile(string audioPath, string videoPath)
        {
            #region firstEdition
            //string args = "/c ffmpeg -i \"video.mp4\" -i \"mic.wav\" -shortest outPutFile.mp4";
            //var startInfo = new ProcessStartInfo
            //{
            //    CreateNoWindow = false,
            //    FileName = "cmd.exe",
            //    WorkingDirectory = @"" + outputPath,
            //    Arguments = args
            //};
            //using (Process exeProcess = Process.Start(startInfo))
            //{
            //    exeProcess.WaitForExit();
            //}
            #endregion
            #region secondEdition
            //var startInfo = new ProcessStartInfo()
            //{
            //    CreateNoWindow = false,
            //    FileName = @"C:\Users\ilya_\source\repos\ScreenRec3\bin\Debug\ScreenRec2.exe",
            //    //FileName = @"C:\Users\User\Documents\ffmpeg.exe",
            //    UseShellExecute = false,
            //    RedirectStandardOutput = true,
            //    RedirectStandardError = true,
            //    Arguments = string.Format(" -i {0} -i {1} -shortest {2} -y", @"C:\images\video.avi", @"C:\images\voice.wav", @"C:\images\result.avi")
            //};

            //using (var exeProcess = Process.Start(startInfo))
            //{
            //    //string StdOutVideo = exeProcess.StandardOutput.ReadToEnd();//
            //    //string StdErrVideo = exeProcess.StandardError.ReadToEnd();
            //    exeProcess.WaitForExit();
            //    exeProcess.Close();
            //}
            #endregion
        }
    }
}
