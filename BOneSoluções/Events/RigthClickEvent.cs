using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOneSolucoes.Events
{
    class RigthClickEvent
    {
        public static void SBO_Application_RigthClickEvent(ref SAPbouiCOM.ContextMenuInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            SAPbouiCOM.Form oForm = null;
            SAPbouiCOM.MenuItem oMenuItem = null;
            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuCreationParams oMenuCreationParams = null;

            try
            {
                oForm = Application.SBO_Application.Forms.ActiveForm;

                if (oForm.TypeEx == "139" && pVal.BeforeAction == true && oForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                {

                    if (!Application.SBO_Application.Menus.Exists("formListAprov"))
                    {
                        oMenus = Application.SBO_Application.Menus;
                        oMenuCreationParams = (SAPbouiCOM.MenuCreationParams)Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams);
                        oMenuItem = Application.SBO_Application.Menus.Item("1280");

                        oMenuCreationParams = (SAPbouiCOM.MenuCreationParams)Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams);
                        oMenuCreationParams.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                        oMenuCreationParams.UniqueID = "formListAprov";
                        oMenuCreationParams.String = "Disponivel para desenvolver";
                        oMenuCreationParams.Enabled = true;
                        oMenuCreationParams.Position = -1;
                        oForm.Menu.AddEx(oMenuCreationParams);
                    }
                   
                }
             
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message,SAPbouiCOM.BoMessageTime.bmt_Short,SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                BubbleEvent = false;
            }
            finally
            {      
                if (oForm != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oForm);
                }
                if (oMenuItem != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oMenuItem);
                }
                if (oMenus != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oMenus);
                }
                if (oMenuCreationParams != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oMenuCreationParams);
                }
            }
        }
    }
}
