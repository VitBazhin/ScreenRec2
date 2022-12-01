using System.Diagnostics;

namespace ScreenRec2
{
    internal static class MergeAudioAndVideo
    {
        private static readonly string _finalName = $"Screenrecording_{CommonLogic.CreateUniqueName()}.mp4";
        public static void Mergefile(string inputPath, string outputPath, string videoName, string audioName)
        {
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = "ffmpeg.exe",
                Arguments = string.Format(" -i {0} -i {1} -shortest {2} -y", $"{inputPath}//{videoName}", $"{inputPath}//{audioName}", $"{outputPath}//{_finalName}")
            };
            using (var exeProcess = Process.Start(startInfo))
            {
                exeProcess.WaitForExit();
            }
        }
    }
}
