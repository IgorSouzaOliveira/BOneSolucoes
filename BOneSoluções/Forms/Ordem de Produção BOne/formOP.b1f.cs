using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOneSolucoes.Forms.Ordem_de_Produção_BOne
{
    [FormAttribute("BOneSolucoes.Forms.Ordem_de_Produção_BOne.formOP", "Forms/Ordem de Produção BOne/formOP.b1f")]
    class formOP : UserFormBase
    {
        private SAPbouiCOM.Matrix mtxOrdemP;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.EditText colCode;
        private SAPbouiCOM.Button Button3;

        public formOP()
        {
            Application.SBO_Application.MenuEvent += SBO_Application_MenuEvent;

        }




        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtxOrdemP = ((SAPbouiCOM.Matrix)(this.GetItem("mtxOrdemP").Specific));
            this.mtxOrdemP.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.mtxOrdemP_ChooseFromListAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.Button0_PressedBefore);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("btnAddL").Specific));
            this.Button2.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button2_PressedAfter);
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.colCode = ((SAPbouiCOM.EditText)(this.GetItem("colCode").Specific));
            this.Button3 = ((SAPbouiCOM.Button)(this.GetItem("btnExcluL").Specific));
            this.Button3.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button3_PressedAfter);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.ComboBox0 = ((SAPbouiCOM.ComboBox)(this.GetItem("cbxFilial").Specific));
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
            mtxOrdemP.AutoResizeColumns();

        }

        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Button Button2;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.ComboBox ComboBox0;          
     

        private void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            SAPbobsCOM.Recordset oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            try
            {
                switch (pVal.MenuUID)
                {
                    case "1282":
                        if (pVal.BeforeAction == false && this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                        {

                            oRst.DoQuery("SELECT COUNT('A') FROM [@BONEOPT]");
                            var code = oRst.Fields.Item(0).Value;

                            if (Convert.ToInt32(code) == 0)
                            {
                                oRst.DoQuery("SELECT CAST((ISNULL(MAX(CAST([CODE] AS NUMERIC)),0) + 1) AS NVARCHAR(MAX)) FROM [@BONEOPT]");
                                string NextSerial = oRst.Fields.Item(0).Value.ToString();
                                this.UIAPIRawForm.DataSources.DBDataSources.Item("@BONEOPT").SetValue("Code", 0, NextSerial.ToString());
                                this.UIAPIRawForm.DataSources.DBDataSources.Item("@BONEOPT").SetValue("DocEntry", 0, NextSerial.ToString());
                            }
                            else
                            {
                                oRst.DoQuery("SELECT CAST((ISNULL(MAX(CAST([CODE] AS NUMERIC)),0) + 1) AS NVARCHAR(MAX)) FROM [@BONEOPT]");
                                string NextSerial = oRst.Fields.Item(0).Value.ToString();
                                this.UIAPIRawForm.DataSources.DBDataSources.Item("@BONEOPT").SetValue("Code", 0, NextSerial.ToString());
                                this.UIAPIRawForm.DataSources.DBDataSources.Item("@BONEOPT").SetValue("DocEntry", 0, NextSerial.ToString());
                            }

                            this.UIAPIRawForm.Items.Item("colCode").Enabled = false;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.Message, 1, "Ok", "Cancelar");
            }
            finally
            {
                if (oRst != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRst);
                }
            }

        }
        private void Button2_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                mtxOrdemP.FlushToDataSource();

                if (mtxOrdemP.RowCount > 0)
                {
                    this.UIAPIRawForm.DataSources.DBDataSources.Item("@BONEOPTL").InsertRecord(mtxOrdemP.RowCount);
                }

                this.UIAPIRawForm.DataSources.DBDataSources.Item("@BONEOPTL").SetValue("LineId", mtxOrdemP.RowCount, (mtxOrdemP.RowCount + 1).ToString());

                mtxOrdemP.LoadFromDataSourceEx();

                if (this.UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
                {
                    this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                }

            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.Message, 1, "Ok", "Cancelar");
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }

        }
        private void Button3_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            int iRow = 0;

            try
            {
                this.UIAPIRawForm.Freeze(true);
                mtxOrdemP.FlushToDataSource();

                iRow = mtxOrdemP.GetNextSelectedRow(0, SAPbouiCOM.BoOrderType.ot_RowOrder);

                if (iRow > -1)
                {
                    mtxOrdemP.DeleteRow(iRow);
                    mtxOrdemP.FlushToDataSource();

                    for (int i = iRow; i < mtxOrdemP.RowCount; i++)
                    {
                        this.UIAPIRawForm.DataSources.DBDataSources.Item("@BONEOPTL").SetValue("LineId", i - 1, i.ToString());
                    }
                }

                mtxOrdemP.LoadFromDataSourceEx();

                if (this.UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
                {
                    this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                }

            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.Message, 1, "Ok", "Cancelar");
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }

        }
        private void mtxOrdemP_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                SAPbouiCOM.ISBOChooseFromListEventArg oCFLEvent = null;
                oCFLEvent = (SAPbouiCOM.ISBOChooseFromListEventArg)pVal;
                SAPbouiCOM.DataTable oDataTable = oCFLEvent.SelectedObjects;

                if (oDataTable == null)
                    return;

                if (oCFLEvent.SelectedObjects.UniqueID == "cflItemCode")
                {
                    String itemCode = oDataTable.GetValue("ItemCode", 0).ToString();
                    String ItemName = oDataTable.GetValue("ItemName", 0).ToString();
                    this.UIAPIRawForm.DataSources.DBDataSources.Item("@BONEOPTL").SetValue("U_ItemCode", pVal.Row - 1, itemCode);
                    this.UIAPIRawForm.DataSources.DBDataSources.Item("@BONEOPTL").SetValue("U_ItemName", pVal.Row - 1, ItemName);
                }

                if (oCFLEvent.SelectedObjects.UniqueID == "cflDeposito")
                {
                    String qDeposito = oDataTable.GetValue("WhsCode", 0).ToString();
                    this.UIAPIRawForm.DataSources.DBDataSources.Item("@BONEOPTL").SetValue("U_Deposito", pVal.Row - 1, qDeposito);
                }



                if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_OK_MODE)
                {
                    this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.Message, 1, "Ok", "Cancelar");
            }
            finally
            {
                mtxOrdemP.LoadFromDataSource();
                this.UIAPIRawForm.Freeze(false);
            }

        }
        private void Button0_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            try
            {
                ValidaNull();   
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                BubbleEvent = false;
            }

        }      
        private void ValidaNull()
        {
            for (int i = 1; i <= mtxOrdemP.RowCount; i++)
            {
                var itemCode = ((SAPbouiCOM.EditText)mtxOrdemP.Columns.Item("colItemC").Cells.Item(i).Specific).Value;
                var itemName = ((SAPbouiCOM.EditText)mtxOrdemP.Columns.Item("colItemN").Cells.Item(i).Specific).Value;
                var eDeposito = ((SAPbouiCOM.EditText)mtxOrdemP.Columns.Item("colDep").Cells.Item(i).Specific).Value;

                if (string.IsNullOrEmpty(itemCode))
                {                   
                    throw new Exception($"BOne - Campo: Código do Item em Branco. Linha: {i.ToString()}");
                }

                if (string.IsNullOrEmpty(itemName))
                {                  
                    throw new Exception($"BOne - Campo: Descrição do Item em Branco. Linha: {i.ToString()}");
                }

                if (string.IsNullOrEmpty(eDeposito))
                {
                    throw new Exception($"BOne - Campo: Depósito do Item em Branco. Linha: {i.ToString()}");
                }
            }

            
        }

    }
}
