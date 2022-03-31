using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace ScreenRec2
{
    public static class Background
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

            #region oldMethod
            //foreach (var fileName in Directory.GetFiles(targetDirName))
            //{
            //    if (fileName != null)
            //    {
            //        File.SetAttributes(fileName, FileAttributes.Normal);
            //        File.Delete(fileName);
            //    }
            //}
            //foreach (var dir in Directory.GetDirectories(targetDirName))
            //{
            //    DeleteFiles(dir);
            //}
            //if (string.IsNullOrEmpty(""))
            //{
            //    Directory.Delete(targetDirName, false);
            //}
            #endregion
        }

        /// <summary>
        /// Create a unique video name.
        /// </summary>
        /// <returns></returns>
        public static string UniqName()
        {
            var dateTime = DateTime.Now;
            return dateTime.Year.ToString()+dateTime.Month+dateTime.Day+"_"+dateTime.Hour+dateTime.Minute+dateTime.Second;
        }
    }
}
