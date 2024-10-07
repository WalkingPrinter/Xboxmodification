namespace Xboxmodification
{
    using System;
    using System.IO;
    using Xboxmodification;

    public class Log
    {
        public enum LogLevel
        {
            Error,
            Warning,
            Info
        }

        public static void Initialize()
        {
            if (!Directory.Exists(Directories.GetPath(ePaths.PATH_LOGS)))
                Directory.CreateDirectory(Directories.GetPath(ePaths.PATH_LOGS));


        }

        public static void LogException(string Type, Exception exception)
        {
            var logMessage = new string('-', 20) + Environment.NewLine +
                      DateTime.Now.ToString() + Environment.NewLine +
                      "Message: " + exception.Message + Environment.NewLine +
                      "StackTrace: " + exception.StackTrace + Environment.NewLine +
                      new string('-', 20);

            File.AppendAllText(Path.Combine(Directories.GetPath(ePaths.PATH_LOGS), "Exceptions.log"), logMessage);
        }

        public static void LogPrint(string message, LogLevel level)
        {
            var logMessage = $"{DateTime.Now}: {level} - {message}" + Environment.NewLine;
            File.AppendAllText(Path.Combine(Directories.GetPath(ePaths.PATH_LOGS), "Exceptions.log"), logMessage);
        }
    }
}
