using System;
using System.Configuration;
using System.IO;

namespace ScreenRec2
{
    internal static class CommonLogic
    {
        public static bool TryGetSetting(string key, out string result)
        {
            result = "";
            try
            {
                result = ConfigurationManager.AppSettings[key];
                if (string.IsNullOrWhiteSpace(result))
                {
                    Console.WriteLine($"Not Found {key}");
                    return false;
                }
                Console.WriteLine($"{key} = {result}");
                return true;
            }
            catch (ConfigurationErrorsException ex)
            {
                Console.WriteLine($"Error reading app settings {ex}");
                return false;
            }
        }

        public static void DeleteFiles(string targetDirName) => Directory.Delete(targetDirName, true);
        public static string CreateUniqueName() => DateTime.Now.ToString("yyyyMMdd_HHmmss");
        public static string CreateTempPath() => Path.GetTempPath() + "//TempDirectory";
    }
}
