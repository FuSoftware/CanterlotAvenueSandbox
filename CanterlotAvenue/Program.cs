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
        static void Main()
        {
            /*
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            */
            string user = Console.ReadLine();
            string pass = Console.ReadLine();

            PoniverseUser u = LoginClient.Login(user, pass);

            Console.WriteLine(u.Username);
        }
    }
}
