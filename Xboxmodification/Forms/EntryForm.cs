namespace Xboxmodification.Forms
{
    using System;
    using System.Threading.Tasks;
    using DevExpress.XtraBars.Ribbon;
    using DevExpress.XtraBars;

    using XDevkit;
    using System.Drawing;

    public partial class EntryForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public EntryForm() {
            InitializeComponent();
        }

        private async void EntryForm_Load(object sender, EventArgs e)
        {
            await PrepareConsoleListAsync();
        }

        private async Task PrepareConsoleListAsync()
        {
            await Task.Run(async () =>
            {
                DeviceManager.Instance.Disconnect();  // disconnect from any previous console

                foreach (string ConsoleName in Globals.xbMgr.Consoles)
                {
                    var consoleItem = new BarButtonItem()
                    {
                        Name = ConsoleName,
                        RibbonStyle = RibbonItemStyles.Large
                    };

                    // Set the console name for DeviceManager
                    DeviceManager.Instance.SetName(ConsoleName);

                    // Attempt to connect to the console
                    bool Status = await DeviceManager.Instance.ConnectAsync(DeviceManager.Instance.GetName());

                    if (Status)
                    {
                        switch (Globals.xbCon.ConsoleType)
                        {
                            case XboxConsoleType.ReviewerKit:
                                consoleItem.Caption = string.Format("[Retail] {0}", ConsoleName);
                                consoleItem.ImageOptions.Image = Properties.Resources.ConsoleRetail;
                                break;

                            case XboxConsoleType.TestKit:
                                consoleItem.Caption = string.Format("[Testkit] {0}", ConsoleName);
                                consoleItem.ImageOptions.Image = Properties.Resources.ConsoleTestkit;
                                break;

                            case XboxConsoleType.DevelopmentKit:
                                consoleItem.Caption = string.Format("[XDK] {0}", ConsoleName);
                                consoleItem.ImageOptions.Image = Properties.Resources.ConsoleDevkit;
                                break;
                        }
                    }
                    else
                    {
                        consoleItem.Caption = ConsoleName;
                        consoleItem.ImageOptions.Image = Properties.Resources.ConsoleOffline;
                        consoleItem.Enabled = true;
                    }

                    // Assign the click handler
                    consoleItem.ItemClick += async (sender, e) =>
                    {
                        DeviceManager.Instance.Disconnect(); // disconnect from previous console
                        DeviceManager.Instance.SetName(e.Item.Name); // set the new console

                        // Attempt to connect
                        bool connectionStatus = await DeviceManager.Instance.ConnectAsync(DeviceManager.Instance.GetName());

                        if (!connectionStatus)
                        {
                            lblStatus.ItemAppearance.Normal.ForeColor = Color.Red;
                            lblStatus.Caption = "Connection failed";
                            return;
                        }

                        // Connect as a debugger if successful
                        Globals.xbDebug = Globals.xbCon.DebugTarget;
                        Globals.xbDebug.ConnectAsDebugger("Juggernaut", XboxDebugConnectFlags.Force);

                        lblStatus.ItemAppearance.Normal.ForeColor = Color.Green;
                        lblStatus.Caption = "Connected";
                    };

                    // Add the item to the ribbon page
                    ribbonPageConsoles.ItemLinks.Add(consoleItem);
                }
            });
        }

        #region EVENTS
        private void BarButtonHomebrewScreenshot_ItemClick(object sender, ItemClickEventArgs e) => new Homebrews.ScreenshotForm().ShowDialog();
        #endregion
    }
}