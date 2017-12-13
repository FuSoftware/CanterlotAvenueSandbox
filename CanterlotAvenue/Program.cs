using CanterlotAvenue.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CanterlotAvenue
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            */

            string user = "";
            string pass = "";

            if (args.Length == 2)
            {
                user = args[0];
                pass = args[1];
                Console.WriteLine("Starting with user {0} and password {1}", user, pass);
            }

            LoginClient l = new LoginClient();
            PoniverseUser u = l.Login(user, pass, false);
            ServiceClient s = new ServiceClient(l.CookieWebClient);
            s.SendStatus("If you can see this status, it means I've successfully automated both the login and status sending process ^^", ServiceClient.StatusPrivacy.Everyone);

            Console.WriteLine(u.Username);
        }
    }
}
