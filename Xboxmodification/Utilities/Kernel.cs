namespace Xboxmodification
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;

    public class Kernel
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(out Point lpPoint);
    }
}
