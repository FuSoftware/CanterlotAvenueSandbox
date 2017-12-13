using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using System.Collections.Specialized;

namespace CanterlotAvenue.Web
{
    class ServiceClient
    {
        public enum StatusPrivacy
        {
            Everyone = 0,
            Friends,
            FriendsOffriends,
            OnlyMe,
            Custom
        }

        private CookieAwareWebClient CookieClient;
        
        public ServiceClient(CookieAwareWebClient c = null)
        {
            if (c == null)
                this.CookieClient = new CookieAwareWebClient();
            else
                this.CookieClient = c;
        }

        private dynamic GetOCore()
        {
            Regex r = new Regex("var oCore = (.*);");

            string html = this.CookieClient.DownloadString("https://canterlotavenue.com");

            if(r.IsMatch(html))
            {
                Match m = r.Match(html);
                string json = m.Groups[1].ToString();
                return JsonConvert.DeserializeObject(json);
            }
            else
            {
                Console.WriteLine("Error getting oCore, no match");
                return null;
            }
        }

        public void SendStatus(string text, StatusPrivacy privacy)
        {
            SendStatus(text, (int)privacy);
        }

        public void SendStatus(string text, int privacy)
        {
            string token = GetOCore()["log.security_token"];

            NameValueCollection p = new NameValueCollection();
            p.Add("core[ajax]", "true");
            p.Add("core[call]", "user.updateStatus");
            p.Add("val[user_status]", text);
            p.Add("val[group_id]", "");
            p.Add("val[action]", "upload_photo_via_share");
            p.Add("image[]", "");
            p.Add("val[status_info]", "");
            p.Add("val[privacy]", privacy.ToString());
            p.Add("core[security_token]", "0");
            p.Add("core[is_admincp]", "0");
            p.Add("core[is_user_profile]", "0");
            p.Add("core[profile_user_id]", "0");

            this.CookieClient.UploadValues("https://canterlotavenue.com/_ajax/", p);
        }
    }
}
