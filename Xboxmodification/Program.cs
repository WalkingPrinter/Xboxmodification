namespace Xboxmodification
{
    using System;
    using System.Windows.Forms;
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Directories.Initialize();

            Application.Run(new Forms.EntryForm());
        }
    }
}
