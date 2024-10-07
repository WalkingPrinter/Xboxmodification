namespace Xboxmodification {
    using DevExpress.XtraEditors;
    using System;
    using System.Linq;
    using System.Net.Http.Headers;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using System.Xml.Linq;
    using System.IO;
    public class Utilities {
        public static string GetFolderFriendlyDateString()
        {
            return DateTime.UtcNow.ToString("yyyy-MM-dd");
        }

        public static string GetCurrentTime()
        {
            return DateTime.Now.ToString("HH-mm-ss");
        }

        public static async Task<bool> UploadImageToImgurAsync(string imagePath)
        {
            const string clientId = "e6b362a275ea564";
            const string uploadUrl = "https://api.imgur.com/3/upload.xml";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Client-ID", clientId);

                    var imageBytes = File.ReadAllBytes(imagePath);
                    var base64Image = Convert.ToBase64String(imageBytes);

                    var formContent = new MultipartFormDataContent {
                        { new StringContent(base64Image), "image" }
                    };

                    var response = await httpClient.PostAsync(uploadUrl, formContent);
                    response.EnsureSuccessStatusCode();

                    var responseContent = await response.Content.ReadAsStringAsync();
                    var xdoc = XDocument.Parse(responseContent);
                    var imgurLink = xdoc.Descendants("link").FirstOrDefault()?.Value;

                    if (!string.IsNullOrEmpty(imgurLink))
                    {
                        Clipboard.SetText(imgurLink);

                        XtraMessageBox.Show("Your Screenshot has been uploaded! we have copied it to your Clipboard!");
                        return true;
                    }
                    else
                    {
                        XtraMessageBox.Show("Failed to retrieve the image link from the response.");
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogException("Imgur Upload", ex);
                return false;
            }
        }
    }
}
