using BOneSoluções.Forms;
using BOneSoluções.Forms.ParceiroDeNegocios;
using BOneSoluções.Forms.Vendas;
using SAPbouiCOM.Framework;
using System;

namespace BOneSoluções
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
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }

    }
}
