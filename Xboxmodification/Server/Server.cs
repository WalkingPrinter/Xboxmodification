
namespace Xboxmodification.Server
{
    using DevExpress.XtraBars.Ribbon.ViewInfo;
    using DevExpress.XtraEditors;
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    public class Server
    {
        private static string URL = "http://localhost:8080/api";
        public static async Task<bool> IsPresent() {
            using(HttpClient client = new HttpClient()) {
                HttpResponseMessage content = await client.GetAsync(string.Format("{0}{1}", URL, "/up"));

                if (await content.Content.ReadAsStringAsync() == "we are up!") return true;

                return false;
            }
        }
    }
}
