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
        private SAPbouiCOM.CheckBox CheckBox0, cFechaDoc;
        private SAPbouiCOM.Button Button2;
        private SAPbouiCOM.Folder Folder1;
        private SAPbouiCOM.EditText editUrl;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.EditText editPort;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.EditText edServer;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.StaticText StaticText2;
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
            this.Folder1 = ((SAPbouiCOM.Folder)(this.GetItem("Item_2").Specific));
            this.editUrl = ((SAPbouiCOM.EditText)(this.GetItem("editUrl").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_11").Specific));
            this.editPort = ((SAPbouiCOM.EditText)(this.GetItem("editPort").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_13").Specific));
            this.edServer = ((SAPbouiCOM.EditText)(this.GetItem("edServer").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("edUsuario").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("edPass").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_9").Specific));
            this.cFechaDoc = ((SAPbouiCOM.CheckBox)(this.GetItem("cFechaDoc").Specific));
            this.Folder2 = ((SAPbouiCOM.Folder)(this.GetItem("fIndu").Specific));
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
            this.UIAPIRawForm.EnableMenu("1281", false);
            this.UIAPIRawForm.EnableMenu("1282", false);
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
                    //this.UIAPIRawForm.DataSources.UserDataSources.Item("udFechaDoc").ValueEx = (string)oTable.UserFields.Fields.Item("U_FechaDocumento").Value;
                    this.UIAPIRawForm.DataSources.UserDataSources.Item("udUrl").Value = oTable.UserFields.Fields.Item("U_UrlSL").Value.ToString();
                    this.UIAPIRawForm.DataSources.UserDataSources.Item("udPorta").Value = oTable.UserFields.Fields.Item("U_PortaSL").Value.ToString();
                    this.UIAPIRawForm.DataSources.UserDataSources.Item("udServidor").Value = oTable.UserFields.Fields.Item("U_ServidorSL").Value.ToString();
                    this.UIAPIRawForm.DataSources.UserDataSources.Item("udUser").Value = oTable.UserFields.Fields.Item("U_UsuarioSL").Value.ToString();
                    this.UIAPIRawForm.DataSources.UserDataSources.Item("udPass").Value = oTable.UserFields.Fields.Item("U_SenhaSL").Value.ToString();
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
                Boolean existsCode = oTable.GetByKey("1");
                SetUserTableValues(oTable);
                int result = existsCode ? oTable.Update() : oTable.Add();

                if (result != 0)
                {
                    throw new Exception(Program.oCompany.GetLastErrorDescription());
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

        void SetUserTableValues(SAPbobsCOM.UserTable userTable)
        {
            userTable.Code = "1";
            userTable.Name = "1";
            userTable.UserFields.Fields.Item("U_BOne_AtivoAprov").Value = CheckBox0.Checked ? "Y" : "N";
            //oTable.UserFields.Fields.Item("U_FechaDocumento").Value = cFechaDoc.Checked ? "Y" : "N";
            userTable.UserFields.Fields.Item("U_UrlSL").Value = this.UIAPIRawForm.DataSources.UserDataSources.Item("udUrl").Value;
            userTable.UserFields.Fields.Item("U_PortaSL").Value = this.UIAPIRawForm.DataSources.UserDataSources.Item("udPorta").Value;
            userTable.UserFields.Fields.Item("U_ServidorSL").Value = this.UIAPIRawForm.DataSources.UserDataSources.Item("udServidor").Value;
            userTable.UserFields.Fields.Item("U_UsuarioSL").Value = this.UIAPIRawForm.DataSources.UserDataSources.Item("udUser").Value;
            userTable.UserFields.Fields.Item("U_SenhaSL").Value = this.UIAPIRawForm.DataSources.UserDataSources.Item("udPass").Value;
        }

        private SAPbouiCOM.Folder Folder2;
    }
}
