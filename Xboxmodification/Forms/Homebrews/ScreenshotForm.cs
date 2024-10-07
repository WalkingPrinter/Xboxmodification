namespace Xboxmodification.Forms.Homebrews {
    using System;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using DevExpress.XtraBars.Ribbon;
    using DevExpress.XtraEditors;
    public partial class ScreenshotForm : XtraForm {
        private GalleryItem SelectedGalleryItem { get; set; }
        public ScreenshotForm() => InitializeComponent();
        private async void ScreenshotForm_Load(object sender, EventArgs e) {
            await PopulateGalleryControlFromFoldersAsync(galleryControl1, Directories.GetPath(ePaths.PATH_SCREENSHOTS));

            // Select the last item in the gallery
            galleryControl1.Gallery.Groups.Last().Items.Last().Checked = true;
        }

        private async void BtnCaptureScreenshot_Click(object sender, EventArgs e) {
            var folderName = Utilities.GetFolderFriendlyDateString();
            var screenshotFolderPath = Path.Combine(Directories.GetPath(ePaths.PATH_SCREENSHOTS), folderName);

            if (!Directory.Exists(screenshotFolderPath))
                Directory.CreateDirectory(screenshotFolderPath);

            var tempScreenshotPath = Path.Combine(Directories.GetPath(ePaths.PATH_TMP), "Screenshot.bmp");

            await Task.Run(() =>
            {
                Globals.xbCon.ScreenShot(tempScreenshotPath);
            });

            // convert the bmp to png
            var screenshotName = $"{Utilities.GetCurrentTime()}.png";
            var screenshotPath = Path.Combine(screenshotFolderPath, screenshotName);

            try
            {
                using (var bmp = new Bitmap(tempScreenshotPath))
                {
                    bmp.Save(screenshotPath, System.Drawing.Imaging.ImageFormat.Png);
                }

                // delete the temp bmp
                File.Delete(tempScreenshotPath);
            }
            catch (Exception ex)
            {
                Log.LogException("Screen Capture", ex);
            }

            // Refresh the gallery control
            await PopulateGalleryControlFromFoldersAsync(galleryControl1, Directories.GetPath(ePaths.PATH_SCREENSHOTS));

            // Select the last item in the gallery
            galleryControl1.Gallery.Groups.Last().Items.Last().Checked = true;
        }

        public async Task PopulateGalleryControlFromFoldersAsync(GalleryControl galleryControl, string rootFolderPath)
        {
            // Ensure the gallery control is clear before populating
            galleryControl.Gallery.Groups.Clear();

            // Get all directories in the root folder asynchronously
            string[] directories = await Task.Run(() => Directory.GetDirectories(rootFolderPath));

            foreach (var directory in directories)
            {
                // Use the directory name as the group caption (assuming it is a date)
                string groupCaption = Path.GetFileName(directory);

                // Create a new GalleryItemGroup
                GalleryItemGroup group = new GalleryItemGroup();
                group.Caption = groupCaption;

                // Get all image files in the directory asynchronously
                string[] imageFiles = await Task.Run(() =>
                    Directory.GetFiles(directory, "*.*", SearchOption.TopDirectoryOnly)
                        .Where(f => f.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                                    f.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                                    f.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                                    f.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase))
                        .ToArray()
                );

                if (imageFiles.Length > 0)
                {
                    foreach (var imageFile in imageFiles)
                    {
                        // Use the file name (without extension) as the item caption
                        string itemCaption = Path.GetFileNameWithoutExtension(imageFile);

                        // Load image asynchronously
                        Image image = await Task.Run(() => Image.FromFile(imageFile));

                        // Create a new GalleryItem
                        GalleryItem item = new GalleryItem
                        {
                            Caption = itemCaption,
                            Image = image, // Set the image
                            Tag = imageFile
                        };

                        // Add the item to the group
                        group.Items.Add(item);
                    }

                    galleryControl.Gallery.ImageSize = new Size(200, 100);
                    // Add the group to the gallery
                    galleryControl.Gallery.Groups.Add(group);
                }
            }
        }

        private void galleryControl1_Gallery_ItemRightClick(object sender, GalleryItemClickEventArgs e)
        {

        }
    }
}