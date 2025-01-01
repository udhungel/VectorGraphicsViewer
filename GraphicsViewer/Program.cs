using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;
using WebApplication1.Interface;
using WebApplication1.Services;

namespace DXApplication
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var container = new UnityContainer();
            container.RegisterType<IInputReader, JsonInputReader>(new SingletonLifetimeManager());
            container.RegisterType<Form1>(new SingletonLifetimeManager());
            var form = container.Resolve<Form1>();
            Application.Run(form);
        }
    }
}
