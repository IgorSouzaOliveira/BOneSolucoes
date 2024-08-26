using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOneSolucoes.Events
{
    class MenuEvent
    {
        public MenuEvent()
        {

        }

        public static void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            SAPbobsCOM.Recordset oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            SAPbouiCOM.Form oForm = null;

            oForm = Application.SBO_Application.Forms.ActiveForm;
            oForm.Freeze(true);

            try
            {
                if (pVal.BeforeAction == false && oForm.UniqueID == "formOPBone")
                {
                    switch (oForm.Mode)
                    {
                        case SAPbouiCOM.BoFormMode.fm_FIND_MODE:
                            oForm.Items.Item("colCode").Enabled = true;
                            break;
                        case SAPbouiCOM.BoFormMode.fm_OK_MODE:
                            oForm.Items.Item("colCode").Enabled = false;
                            break;

                        case SAPbouiCOM.BoFormMode.fm_ADD_MODE:
                            oRst.DoQuery("SELECT COUNT('A') FROM [@BONEOPT]");
                            var code = oRst.Fields.Item(0).Value;

                            if (Convert.ToInt32(code) == 0)
                            {
                                oRst.DoQuery("SELECT CAST((ISNULL(MAX(CAST([CODE] AS NUMERIC)),0) + 1) AS NVARCHAR(MAX)) FROM [@BONEOPT]");
                                string NextSerial = oRst.Fields.Item(0).Value.ToString();
                                oForm.DataSources.DBDataSources.Item("@BONEOPT").SetValue("Code", 0, NextSerial.ToString());
                                oForm.DataSources.DBDataSources.Item("@BONEOPT").SetValue("DocEntry", 0, NextSerial.ToString());
                            }
                            else
                            {
                                oRst.DoQuery("SELECT CAST((ISNULL(MAX(CAST([CODE] AS NUMERIC)),0) + 1) AS NVARCHAR(MAX)) FROM [@BONEOPT]");
                                string NextSerial = oRst.Fields.Item(0).Value.ToString();
                                oForm.DataSources.DBDataSources.Item("@BONEOPT").SetValue("Code", 0, NextSerial.ToString());
                                oForm.DataSources.DBDataSources.Item("@BONEOPT").SetValue("DocEntry", 0, NextSerial.ToString());
                            }

                            oForm.Items.Item("colCode").Enabled = false;
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                BubbleEvent = false;
            }
            finally
            {
                oForm.Freeze(false);

                if (oForm != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oForm);
                }
                if (oRst != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRst);
                }
            }

        }


    }
}
