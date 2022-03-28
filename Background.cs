using System;
using System.Configuration;

namespace ScreenRec2
{
    public static class Background
    {
        //add coments
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
        /// Creates a unique Guid of 5 characters
        /// </summary>
        /// <returns></returns>
        public static string CreateGuid()
        {
            return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 5);
        }
    }
}
