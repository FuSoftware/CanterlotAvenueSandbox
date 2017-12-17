using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Collections.Specialized;

namespace CanterlotAvenue.Web
{
    class Chatbox
    {
        CookieAwareWebClient CookieClient { get; set; }

        public Chatbox(CookieAwareWebClient c)
        {
            this.CookieClient = c;
        }

        public void SendMessage(string text)
        {
            long time = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            NameValueCollection p = new NameValueCollection();
            p.Add("type", "push");
            p.Add("time_id", time.ToString());
            p.Add("text", text);
            p.Add("parent_module_id", "index");
            p.Add("parent_item_id", "0");

            CookieClient.UploadValues("https://canterlotavenue.com/PF.Site/Apps/core-shoutbox/polling.php", p);
        }

        public dynamic PullMessage(long timestamp)
        {
            //{"timestamp":"1513543299","shoutbox_id":"6475","text":"Hello~","user_avatar":"<a href=\"https:\/\/canterlotavenue.com\/Queen-Chrysalis\/\" title=\"Queen Chrysalis\"><img src=\"https:\/\/store.canterlotavenue.com\/file\/file\/pic\/user\/812b655757d40ab1aa6e71fd552b1cd8_50_square.jpg\"  alt=\"Queen Chrysalis\"  width=\"40\"  height=\"40\"  class=\" _image__50_square image_deferred \" \/><\/a>","user_type":"u","user_profile_link":"https:\/\/canterlotavenue.com\/Queen-Chrysalis\/","user_full_name":"Queen Chrysalis"}

        }
    }
}
