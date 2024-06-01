using BOneSolucoes.Forms.Configuração;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOneSolucoes.Forms
{
    [FormAttribute("formMain", "Forms/Configuração/formMain.b1f")]
    class formMain : UserFormBase
    {
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.CheckBox CheckBox0;
        private SAPbouiCOM.Button Button2;
        public formMain()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Folder0 = ((SAPbouiCOM.Folder)(this.GetItem("Item_1").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.CheckBox0 = ((SAPbouiCOM.CheckBox)(this.GetItem("Item_4").Specific));
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("Item_5").Specific));
            this.Button2.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button2_PressedAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }

        private SAPbouiCOM.Folder Folder0;

        private void OnCustomInitialize()
        {
            LoadForm();
        }

        public void LoadForm()
        {
            SAPbobsCOM.UserTable oTable = Program.oCompany.UserTables.Item("BONECONFMAIN");
            try
            {
                if (oTable.GetByKey("1"))
                {
                    this.UIAPIRawForm.DataSources.UserDataSources.Item("udCheck01").ValueEx = (string)oTable.UserFields.Fields.Item("U_BOne_AtivoAprov").Value;
                }
                
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                if (oTable != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oTable);
                }
            }
        }
        private void Button2_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            formConfigAprov formConfigAprov = new formConfigAprov();
            formConfigAprov.Show();

        }
        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (pVal.FormMode == 1)
            {
                try
                {
                    PanelGeral();
                }
                catch (Exception ex)
                {
                    Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                }

            }

        }
        public void PanelGeral()
        {
            SAPbobsCOM.UserTable oTable = Program.oCompany.UserTables.Item("BONECONFMAIN");

            try
            {
                if (!oTable.GetByKey("1"))
                {
                    oTable.Code = "1";
                    oTable.Name = "1";
                    oTable.UserFields.Fields.Item("U_BOne_AtivoAprov").Value = CheckBox0.Checked ? "Y" : "N";

                    Int32 lRetA = oTable.Add();

                    if (lRetA != 0)
                    {
                        throw new Exception(Program.oCompany.GetLastErrorDescription());
                    }
                }
                else
                {
                    oTable.Code = "1";
                    oTable.Name = "1";
                    oTable.UserFields.Fields.Item("U_BOne_AtivoAprov").Value = CheckBox0.Checked ? "Y" : "N";

                    Int32 lRetU = oTable.Update();

                    if (lRetU != 0)
                    {
                        throw new Exception(Program.oCompany.GetLastErrorDescription());
                    }
                }

            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                if (oTable != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oTable);
                }
            }
        }
    }
}
