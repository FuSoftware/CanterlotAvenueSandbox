using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using System.Web;

namespace CanterlotAvenue.Web
{
    class LoginClient
    {
        private struct AuthData
        {
            //https://poniverse.net/oauth/authorize?response_type=code&client_id=EJ5XlahjJh9AExZ2PNQsmRY5JQ4yhk5WsWpyoTLt&redirect_uri=https://canterlotavenue.com/ponauth/&state=5a304b56e97b2

            public AuthData(string url)
            {
                Uri myUri = new Uri(url);
                NameValueCollection nvc = HttpUtility.ParseQueryString(myUri.Query);

                response_type = nvc.Get("response_type");
                client_id = nvc.Get("client_id");
                redirect_uri = nvc.Get("redirect_uri");
                state = nvc.Get("state");
                this.url = url;
            }

            public string response_type;
            public string client_id;
            public string redirect_uri;
            public string state;
            public string url;
        }

        public CookieAwareWebClient CookieWebClient { get; }

        public LoginClient(CookieAwareWebClient c = null)
        {
            if (c == null)
                c = new CookieAwareWebClient();

            this.CookieWebClient = c;
        }

        public PoniverseUser Login(string user, string pass, bool remember = false)
        {
            return Login(user, pass, remember, this.CookieWebClient);
        }

        public static string GetToken(CookieAwareWebClient c = null)
        {
            if(c == null)
                c = new CookieAwareWebClient();
            /*
            string s = c.DownloadString("https://poniverse.net");
            Regex r = new Regex("token: \"(.+)\"");
            Match m = r.Match(s);
            return m.Groups[1].ToString();
            */

            string s = c.DownloadString("https://poniverse.net/oauth/login");
            Regex r = new Regex("<input type=\\\"hidden\\\" name=\\\"_token\\\" value=\\\"(.+)\\\">");
            Match m = r.Match(s);
            return m.Groups[1].ToString();

        }

        public static string ExtractUser(string html)
        {
            Regex r = new Regex(@"poniverse\.user = (.+);");
            
            if(r.IsMatch(html))
            {
                Match m = r.Match(html);
                return m.Groups[1].ToString();
            }
            else
            {
                return "";
            }
        }

        private static string ExtractPoniverseOauth(string ponauth_html)
        {
            Regex r = new Regex("window\\.location\\.replace\\(\\\"(.+)\\\"\\);");

            if (r.IsMatch(ponauth_html))
            {
                Match m = r.Match(ponauth_html);
                return m.Groups[1].ToString();
            }
            else
            {
                return "";
            }
        }

        private static AuthData GetAuthData(CookieAwareWebClient c = null)
        {
            string url = "http://canterlotavenue.com/ponauth/";

            //Get the Poniverse URL
            //window\.location\.replace\("(.+)"\);
            if(c == null)
                c = new CookieAwareWebClient();

            string s = c.DownloadString(url);
            string poniverse_oauth = ExtractPoniverseOauth(s);

            AuthData d = new AuthData(poniverse_oauth);
            return d;
        }

        private static void LoginPonauth(string poniverse_oauth, CookieAwareWebClient c = null)
        {
            string url = GetPonauthUrl(poniverse_oauth, c);
            SendPonauthUrl(url, c);
        }

        private static string GetPonauthUrl(string poniverse_oauth, CookieAwareWebClient c = null)
        {
            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(poniverse_oauth);
            wr.AllowAutoRedirect = false;

            if (c != null)
                wr.CookieContainer = c.CookieContainer;

            HttpWebResponse rsp = (HttpWebResponse)wr.GetResponse();
            if(rsp.StatusCode == HttpStatusCode.Redirect)
            {
                return rsp.Headers.Get("Location");
            }
            else
            {
                return "";
            }
        }

        private static void SendPonauthUrl(string url, CookieAwareWebClient c)
        {

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.AllowAutoRedirect = false;
            wr.CookieContainer = c.CookieContainer;

            HttpWebResponse rsp = (HttpWebResponse)wr.GetResponse();

            if (rsp.StatusCode == HttpStatusCode.Redirect)
            {
                string location = rsp.Headers.Get("Location");
                Console.WriteLine(location);
            }
        }

        public static PoniverseUser Login(string user, string pass, bool remember = false, CookieAwareWebClient c = null)
        {
            if(c == null)
                c = new CookieAwareWebClient();

            c.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.97 Safari/537.36 Vivaldi/1.94.1008.34");
            string result = "";

            try
            {
                byte[] res = c.UploadValues("https://poniverse.net/login", "POST", new NameValueCollection()
                {
                    { "username", user },
                    { "password", pass },
                    { "rememberme", remember ? "1" : "0" },
                    { "submit", "" },
                    { "_token", GetToken(c) }
                });
                result = Encoding.Default.GetString(res);
                PoniverseUser u = new PoniverseUser(ExtractUser(result));

                //Other auth data
                AuthData d = GetAuthData(c);
                LoginPonauth(d.url, c);

                return u;
            }
            catch(WebException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.Response.ContentType);

                return null;
            }
        }
    }
}
