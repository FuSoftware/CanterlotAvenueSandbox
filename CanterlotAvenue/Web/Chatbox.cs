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
    }
}
