using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOneSolucoes.Forms.Configuração
{
    [FormAttribute("formConfigAprov", "Forms/Configuração/formConfigAprov.b1f")]
    class formConfigAprov : UserFormBase
    {

        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.ComboBox ComboBox0;
        private SAPbouiCOM.Button Button2;
        private SAPbouiCOM.Button Button3;
        private SAPbouiCOM.Matrix mtxConf;
        public formConfigAprov()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtxConf = ((SAPbouiCOM.Matrix)(this.GetItem("mtxConf").Specific));
            this.mtxConf.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.mtxConf_ChooseFromListAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.ComboBox0 = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_4").Specific));
            this.ComboBox0.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.ComboBox0_ComboSelectAfter);
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("Item_5").Specific));
            this.Button2.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button2_PressedAfter);
            this.Button3 = ((SAPbouiCOM.Button)(this.GetItem("Item_0").Specific));
            this.Button3.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button3_PressedAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
        }
        private void OnCustomInitialize()
        {
            LoadMatrix();
        }
        private void LoadMatrix()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                this.UIAPIRawForm.DataSources.DataTables.Item("mtxConf").ExecuteQuery(Resources.Resource.LoadConfAprov);
                mtxConf.Columns.Item("#").DataBind.Bind("mtxConf", "Code");
                mtxConf.Columns.Item("colSel").DataBind.Bind("mtxConf", "Sel");
                mtxConf.Columns.Item("colObj").DataBind.Bind("mtxConf", "U_BOne_ObjectType");
                mtxConf.Columns.Item("colName").DataBind.Bind("mtxConf", "U_BOne_NomeConsulta");
                mtxConf.Columns.Item("colQuery").DataBind.Bind("mtxConf", "U_BOne_Query");
                mtxConf.Columns.Item("colCodE").DataBind.Bind("mtxConf", "U_BOne_CodeEtapa");
                mtxConf.Columns.Item("colEtap").DataBind.Bind("mtxConf", "U_BOne_EtapaAut");
                mtxConf.Columns.Item("colAtivo").DataBind.Bind("mtxConf", "U_BOne_Ativo");

                mtxConf.LoadFromDataSource();
                mtxConf.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }
        private void Button2_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                int row = mtxConf.RowCount;
                mtxConf.AddRow();

                if (row == 0)
                {

                    ((SAPbouiCOM.EditText)mtxConf.Columns.Item("#").Cells.Item(1).Specific).String = "001";
                }
                else
                {
                    int newRow = row + 1;
                    ((SAPbouiCOM.EditText)mtxConf.Columns.Item("#").Cells.Item(newRow).Specific).String = $"00{newRow.ToString()}";
                    mtxConf.ClearRowData(newRow);
                }


            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }
        private void mtxConf_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            SAPbouiCOM.ISBOChooseFromListEventArg oCFLEvent = null;
            oCFLEvent = (SAPbouiCOM.ISBOChooseFromListEventArg)pVal;
            SAPbouiCOM.DataTable oDataTable = oCFLEvent.SelectedObjects;

            if (oDataTable == null)
                return;

            if (oCFLEvent.SelectedObjects.UniqueID == "OUQR")
            {
                var qName = oDataTable.GetValue("QName", 0).ToString();
                var qString = oDataTable.GetValue("QString", 0).ToString();
                ((SAPbouiCOM.EditText)mtxConf.Columns.Item("colName").Cells.Item(pVal.Row).Specific).String = qName;
                ((SAPbouiCOM.EditText)mtxConf.Columns.Item("colQuery").Cells.Item(pVal.Row).Specific).String = qString;
            }

            if (oCFLEvent.SelectedObjects.UniqueID == "OWST")
            {

                var qCodEtapa = oDataTable.GetValue("WstCode", 0).ToString();
                var qEtapa = oDataTable.GetValue("Remarks", 0).ToString();
                ((SAPbouiCOM.EditText)mtxConf.Columns.Item("colCodE").Cells.Item(pVal.Row).Specific).String = qCodEtapa;
                ((SAPbouiCOM.EditText)mtxConf.Columns.Item("colEtap").Cells.Item(pVal.Row).Specific).String = qEtapa;
            }

        }
        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            SAPbouiCOM.DataTable oDT = this.UIAPIRawForm.DataSources.DataTables.Item("mtxConf");
            SAPbobsCOM.UserTable oTable = Program.oCompany.UserTables.Item("BONMODAPROV");
            mtxConf.FlushToDataSource();

            try
            {
                for (int i = 0; i < oDT.Rows.Count; i++)
                {
                    if (oDT.GetValue("Code", i).ToString() == "")
                        return;

                    if (oTable.GetByKey(oDT.GetValue("Code", i).ToString()))
                    {
                        oTable.Code = oDT.GetValue("Code", i).ToString();
                        oTable.Name = oDT.GetValue("Code", i).ToString();
                        oTable.UserFields.Fields.Item("U_BONE_ObjectType").Value = oDT.GetValue(2, i).ToString();
                        oTable.UserFields.Fields.Item("U_BOne_NomeConsulta").Value = oDT.GetValue(3, i).ToString();
                        oTable.UserFields.Fields.Item("U_BOne_Query").Value = oDT.GetValue(4, i).ToString();
                        oTable.UserFields.Fields.Item("U_BOne_CodeEtapa").Value = oDT.GetValue(5, i).ToString();
                        oTable.UserFields.Fields.Item("U_BOne_EtapaAut").Value = oDT.GetValue(6, i).ToString();
                        oTable.UserFields.Fields.Item("U_BOne_Ativo").Value = oDT.GetValue(7, i).ToString();

                        Int32 lRetU = oTable.Update();

                        if (lRetU != 0)
                        {
                            throw new Exception(Program.oCompany.GetLastErrorDescription());
                        }
                    }
                    else
                    {       
                        oTable.Code = oDT.GetValue("Code", i).ToString();
                        oTable.Name = oDT.GetValue("Code", i).ToString();
                        oTable.UserFields.Fields.Item("U_BONE_ObjectType").Value = oDT.GetValue(2, i).ToString();
                        oTable.UserFields.Fields.Item("U_BOne_NomeConsulta").Value = oDT.GetValue(3, i).ToString();
                        oTable.UserFields.Fields.Item("U_BOne_Query").Value = oDT.GetValue(4, i).ToString();
                        oTable.UserFields.Fields.Item("U_BOne_CodeEtapa").Value = oDT.GetValue(5, i).ToString();
                        oTable.UserFields.Fields.Item("U_BOne_EtapaAut").Value = oDT.GetValue(6, i).ToString();
                        oTable.UserFields.Fields.Item("U_BOne_Ativo").Value = oDT.GetValue(7, i).ToString();

                        Int32 lRetA = oTable.Add();

                        if (lRetA != 0)
                        {
                            throw new Exception(Program.oCompany.GetLastErrorDescription());
                        }
                    }

                }
                Application.SBO_Application.StatusBar.SetText("Operação completada com sucesso", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);

            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                if (oDT != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oDT);
                }
                if (oTable != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oTable);
                }

            }

        }
        private void ComboBox0_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Freeze(true);
            try
            {
                var objType = this.UIAPIRawForm.DataSources.UserDataSources.Item("udTela").ValueEx;

                string query = $@"{Resources.Resource.LoadConfAprov} WHERE T0.""U_BONE_ObjectType"" = {objType}";

                this.UIAPIRawForm.DataSources.DataTables.Item("mtxConf").ExecuteQuery(query);

                mtxConf.LoadFromDataSource();
                mtxConf.AutoResizeColumns();

            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }
        private void Button3_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            SAPbobsCOM.UserTable oTable = Program.oCompany.UserTables.Item("BONMODAPROV");

            try
            {
                this.UIAPIRawForm.Freeze(true);

                if (Application.SBO_Application.MessageBox("A linha selecionada será excluida." + Environment.NewLine + "Deseja continuar ?", 1, "Sim", "Não") != 1)
                    return;

                for (int i = mtxConf.RowCount; i >= 1; i--)
                {
                    var selected = ((SAPbouiCOM.CheckBox)mtxConf.Columns.Item("colSel").Cells.Item(i).Specific).Checked;

                    if (selected == false)
                        continue;                    

                    var getKey = ((SAPbouiCOM.EditText)mtxConf.Columns.Item("#").Cells.Item(i).Specific).Value;
                    if (oTable.GetByKey(getKey))
                    {
                        Int32 lRet = oTable.Remove();
                        mtxConf.DeleteRow(i);
                        mtxConf.FlushToDataSource();

                        if (lRet != 0)
                        {
                            throw new Exception(Program.oCompany.GetLastErrorDescription());
                        }
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

                this.UIAPIRawForm.Freeze(false);
            }
        }

    }
}
