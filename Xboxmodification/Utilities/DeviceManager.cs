namespace Xboxmodification
{
    using System;
    using System.Threading.Tasks;
    using Xboxmodification.Utilities;
    using XDevkit;

    public class DeviceManager
    {
        private static DeviceManager instance;
        private string selectedConsole;
        private Exception lastException;

        /// <summary>
        /// Connect to the console
        /// </summary>
        /// <param name="consoleName"></param>
        /// <returns></returns>
        public async Task<bool> ConnectAsync(string consoleName)
        {
            try
            {
                Globals.xbCon = new XboxManager().OpenConsole(consoleName);

                await Task.Run(() => Globals.xbCon.FindConsole(1, 1000));

                Globals.bConnected = true;

                lastException = null;
                return true;
            }
            catch (Exception exception)
            {
                Globals.bConnected = false;
                lastException = exception;
                return false;
            }
        }

        /// <summary>
        /// Set the console name
        /// </summary>
        /// <param name="ConsoleName"></param>
        public void SetName(string ConsoleName)
        {
            this.selectedConsole = ConsoleName;
        }

        /// <summary>
        /// Get the console name
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return this.selectedConsole;
        }

        /// <summary>
        /// Disconnect from the console
        /// </summary>
        public void Disconnect()
        {
            Globals.xbCon = null;
            this.lastException = null;
            this.selectedConsole = null;
        }

        /// <summary>
        /// Get the last exception
        /// </summary>
        public Exception LastException => lastException;

        /// <summary>
        /// Get the console instance
        /// </summary>
        public static DeviceManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DeviceManager();
                }
                return instance;
            }
        }
    }

}
