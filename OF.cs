using System;
using System.Configuration;
using System.IO;

namespace ScreenRec2
{
    public static class OF
    {
        /// <summary>
        /// Return config data.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="result"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete tempary directories.
        /// </summary>
        /// <param name="targetDirName"></param>
        public static void DeleteFiles(string targetDirName)
        {
            Directory.Delete(targetDirName, true);
        }

        /// <summary>
        /// Create a unique video name.
        /// </summary>
        /// <returns></returns>
        public static string UniqueName()
        {
            return DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
        }
    }
}
