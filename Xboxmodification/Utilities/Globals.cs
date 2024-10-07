namespace Xboxmodification.Utilities {
    using System;
    using XDevkit;

    public class Globals {
        public static IXboxManager xbMgr = new XboxManager();
        public static IXboxConsole xbCon = null;
        public static IXboxDebugTarget xbDebug = null;

        public static bool bConnected = false;


    }
}
