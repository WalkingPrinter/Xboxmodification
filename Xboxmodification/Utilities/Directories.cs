namespace Xboxmodification
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    public class Directories
    {
        private static Dictionary<ePaths, string> Paths = new Dictionary<ePaths, string>();
        public static void Initialize()
        {
            Paths.Add(ePaths.PATH_APPDATA, Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            Paths.Add(ePaths.PATH_XBOXMODIFICATION, Path.Combine(GetPath(ePaths.PATH_APPDATA), "Xboxmodification"));
            Paths.Add(ePaths.PATH_TMP, Path.Combine(GetPath(ePaths.PATH_XBOXMODIFICATION), "TMP"));
            Paths.Add(ePaths.PATH_SCREENSHOTS, Path.Combine(GetPath(ePaths.PATH_XBOXMODIFICATION), "Screenshots"));
            Paths.Add(ePaths.PATH_LOGS, Path.Combine(GetPath(ePaths.PATH_XBOXMODIFICATION), "Logs"));

            Directory.CreateDirectory(GetPath(ePaths.PATH_XBOXMODIFICATION));
            Directory.CreateDirectory(GetPath(ePaths.PATH_SCREENSHOTS));
            Directory.CreateDirectory(GetPath(ePaths.PATH_TMP));
        }

        public static string GetPath(ePaths path)
        {
            return Paths[path];
        }
    }

    public enum ePaths
    {
        PATH_APPDATA,
        PATH_TMP,
        PATH_XBOXMODIFICATION,
        PATH_SCREENSHOTS,
        PATH_LOGS
    }
}
