using BOneSolucoes.Forms;
using BOneSolucoes.Forms.Compras;
using BOneSolucoes.Forms.ImportacaoXML;
using BOneSolucoes.Forms.ParceiroDeNegocios;
using BOneSolucoes.Forms.Vendas;
using SAPbouiCOM.Framework;
using System;

namespace BOneSolucoes
{
    class Menu
    {
        public void CreateMenu()
        {
            String xmlMenu;

            try
            {
                RemoveMenu();

                xmlMenu = Resources.Resource.menuAdd.ToString().Replace("%path%", Environment.CurrentDirectory);
                Application.SBO_Application.LoadBatchActions(ref xmlMenu);
            }
            catch
            {
                throw;
            }
        }

        public void RemoveMenu()
        {
            String xmlMenu;

            try
            {
                xmlMenu = Resources.Resource.menuRemove.ToString();
                Application.SBO_Application.LoadBatchActions(ref xmlMenu);
            }
            catch
            {
                throw;
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.BeforeAction && pVal.MenuUID == "mnu_mnuParam")
                {
                    formMain activeForm = new formMain();
                    activeForm.Show();
                }

                if (pVal.BeforeAction && pVal.MenuUID == "mnuAssisGer")
                {
                    formAssis formAssis = new formAssis();
                    formAssis.Show();
                }

                if (pVal.BeforeAction && pVal.MenuUID == "mnuBP")
                {
                    formPDN formPDN = new formPDN();
                    formPDN.Show();
                }

                if (pVal.BeforeAction && pVal.MenuUID == "mnuAprovDeDoc")
                {
                    formAprov formAprov = new formAprov();
                    formAprov.Show();
                }

                if (pVal.BeforeAction && pVal.MenuUID == "mnuImportXml")
                {
                    formImpXML formImpXML = new formImpXML();
                    formImpXML.Show();
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

    }
}
