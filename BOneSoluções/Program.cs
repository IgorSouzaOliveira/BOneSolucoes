using BOneSolucoes.Comonn;
using BOneSolucoes.Entities;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace BOneSolucoes
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static SAPbobsCOM.Company oCompany = null;
        public static SAPbouiCOM.Application Sbo_App = null;

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application oApp = null;
                if (args.Length < 1)
                {
                    oApp = new Application();
                }
                else
                {
                    //If you want to use an add-on identifier for the development license, you can specify an add-on identifier string as the second parameter.
                    //oApp = new Application(args[0], "XXXXX");
                    oApp = new Application(args[0]);
                }
                oCompany = (SAPbobsCOM.Company)Application.SBO_Application.Company.GetDICompany();
                Sbo_App = Application.SBO_Application;
                                
                Menu MyMenu = new Menu();
                MyMenu.RemoveMenu();


                Application.SBO_Application.SetStatusBarMessage("Add-on BOne Soluções conectado com sucesso", SAPbouiCOM.BoMessageTime.bmt_Short, false);

                MyMenu.CreateMenu();

                
                Application.SBO_Application.ItemEvent += EventClass.SBO_Application_ItemEvent;                
                Application.SBO_Application.FormDataEvent += EventClass.GetDocEntryPed;

                oApp.RegisterMenuEventHandler(MyMenu.SBO_Application_MenuEvent);
                Application.SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
                oApp.Run();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            Menu menu = new Menu();

            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:

                    System.Windows.Forms.Application.Exit();
                    menu.RemoveMenu();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                    menu.RemoveMenu();
                    menu.CreateMenu();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                    break;
                default:
                    break;
            }
        }
    }
}
