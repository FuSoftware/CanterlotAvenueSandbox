﻿using CanterlotAvenue.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
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
            Console.WriteLine("Logged in as user {0}", u.Id);

            //Service Client
            ServiceClient s = new ServiceClient(l.CookieWebClient);
            //s.UpdateSecurityToken();
            //s.SendStatus("If you can see this status, it means I've successfully automated both the login and status sending process ^^", ServiceClient.StatusPrivacy.Everyone);

            //Chatbox Client
            //Chatbox c = new Chatbox(l.CookieWebClient);
            //c.SendMessage("Mew, I C#");

            //Friends
            //int f = 0;
            //s.SendFriendInvite(f);

            Console.WriteLine(u.Username);
        }
    }
}
