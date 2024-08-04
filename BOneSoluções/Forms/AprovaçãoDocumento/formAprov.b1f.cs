using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOneSolucoes.Forms.Compras
{
    [FormAttribute("formAprov", "Forms/AprovaçãoDocumento/formAprov.b1f")]
    class formAprov : UserFormBase
    {

        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.EditText EditText2;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.ComboBox cVendComp;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.ComboBox cFilial;
        private SAPbouiCOM.StaticText StaticText5;
        private SAPbouiCOM.ComboBox ComboBox2;
        private SAPbouiCOM.Button bFiltrar;
        private SAPbouiCOM.Matrix mtxAprov;
        SAPbobsCOM.Recordset oRst = null;

        public formAprov()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtxAprov = ((SAPbouiCOM.Matrix)(this.GetItem("mtxAprov").Specific));
            this.mtxAprov.LinkPressedBefore += new SAPbouiCOM._IMatrixEvents_LinkPressedBeforeEventHandler(this.mtxAprov_LinkPressedBefore);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("bProcessar").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("eDocNum").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("eDataDe").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_7").Specific));
            this.EditText2 = ((SAPbouiCOM.EditText)(this.GetItem("eDataAte").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_9").Specific));
            this.cVendComp = ((SAPbouiCOM.ComboBox)(this.GetItem("cVendComp").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_12").Specific));
            this.cFilial = ((SAPbouiCOM.ComboBox)(this.GetItem("cFilial").Specific));
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_14").Specific));
            this.ComboBox2 = ((SAPbouiCOM.ComboBox)(this.GetItem("cTipDoc").Specific));
            this.bFiltrar = ((SAPbouiCOM.Button)(this.GetItem("bFiltrar").Specific));
            this.bFiltrar.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.bFiltrar_PressedAfter);
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
            LoadVendComp();
            LoadFilial();
            LoadMatrix();

        }

        /*Metodo para carregar os Compradores e Vendedores*/
        private void LoadVendComp()
        {
            oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            try
            {
                oRst.DoQuery(@"SELECT T0.""SlpCode"", T0.""SlpName"" FROM OSLP T0 WHERE T0.""Active"" = 'Y'");

                if (oRst.RecordCount > 0)
                {
                    oRst.MoveFirst();
                    for (int i = 0; i < oRst.RecordCount; i++)
                    {
                        cVendComp.ValidValues.Add(oRst.Fields.Item("SlpCode").Value.ToString(), oRst.Fields.Item("SlpName").Value.ToString());
                        oRst.MoveNext();
                    }

                }

            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                if (oRst != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRst);
                }
            }
        }

        /*Metodo para carregar a Filial*/
        private void LoadFilial()
        {
            oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            try
            {
                oRst.DoQuery(@"SELECT T0.""BPLId"", T0.""BPLName"" FROM OBPL T0 WHERE T0.""Disabled"" = 'N'");

                if (oRst.RecordCount > 0)
                {
                    oRst.MoveFirst();
                    for (int i = 0; i < oRst.RecordCount; i++)
                    {
                        cFilial.ValidValues.Add(oRst.Fields.Item("BPLId").Value.ToString(), oRst.Fields.Item("BPLName").Value.ToString());
                        oRst.MoveNext();
                    }

                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                if (oRst != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRst);
                }
            }


        }

        /* Metodo para carregar Matrix */
        private void LoadMatrix()
        {
            this.UIAPIRawForm.Freeze(true);
            try
            {
                string query = string.Format(Resources.Resource.BONE_ExecAprov, Program.oCompany.UserSignature);

                this.UIAPIRawForm.DataSources.DataTables.Item("mtxAprov").ExecuteQuery(query);
                mtxAprov.Columns.Item("colSel").DataBind.Bind("mtxAprov", "Sel");
                mtxAprov.Columns.Item("colDocDate").DataBind.Bind("mtxAprov", "DocDate");
                mtxAprov.Columns.Item("colTipoDoc").DataBind.Bind("mtxAprov", "TipoDoc");
                mtxAprov.Columns.Item("colNumDoc").DataBind.Bind("mtxAprov", "DocEntry");
                mtxAprov.Columns.Item("colCodPn").DataBind.Bind("mtxAprov", "CardCode");
                mtxAprov.Columns.Item("colNamePn").DataBind.Bind("mtxAprov", "CardName");
                mtxAprov.Columns.Item("colFilial").DataBind.Bind("mtxAprov", "BplName");
                mtxAprov.Columns.Item("colComVen").DataBind.Bind("mtxAprov", "SlpName");
                mtxAprov.Columns.Item("colEtapa").DataBind.Bind("mtxAprov", "NameEtapa");
                mtxAprov.Columns.Item("colModAut").DataBind.Bind("mtxAprov", "ModeloAut");
                mtxAprov.Columns.Item("colCondP").DataBind.Bind("mtxAprov", "PaymentName");
                mtxAprov.Columns.Item("colFmPag").DataBind.Bind("mtxAprov", "PaymentMethod");
                mtxAprov.Columns.Item("colDocT").DataBind.Bind("mtxAprov", "DocTotal");
                mtxAprov.Columns.Item("colStatus").DataBind.Bind("mtxAprov", "Status");

                mtxAprov.LoadFromDataSource();
                mtxAprov.AutoResizeColumns();
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

        /* Metodo para LinkedButton com base no tipo de documento */
        private void mtxAprov_LinkPressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            SAPbouiCOM.LinkedButton oLink = null;
            SAPbouiCOM.Column oColumn = null;
            SAPbouiCOM.Matrix oMatrix = null;

            try
            {
                oMatrix = (SAPbouiCOM.Matrix)this.UIAPIRawForm.Items.Item("mtxAprov").Specific;
                oColumn = oMatrix.Columns.Item("colNumDoc");
                oLink = (SAPbouiCOM.LinkedButton)oColumn.ExtendedObject;
                string tipoDoc;

                tipoDoc = ((SAPbouiCOM.EditText)mtxAprov.Columns.Item("colTipoDoc").Cells.Item(pVal.Row).Specific).Value;

                switch (tipoDoc)
                {
                    case "Pedido de venda":
                        oLink.LinkedObjectType = "17";
                        break;

                    case "Pedido de compra":
                        oLink.LinkedObjectType = "22";
                        break;

                    case "Oferta de compra":
                        oLink.LinkedObjectType = "540000006";
                        break;
                }

            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                if (oLink != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oLink);
                }
                if (oColumn != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oColumn);
                }

                if (oMatrix != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oMatrix);
                }
            }

        }

        /* Metodo para filtrar Matrix */
        private void bFiltrar_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {

                this.UIAPIRawForm.Freeze(true);

                var docNum = this.UIAPIRawForm.DataSources.UserDataSources.Item("udDocNum").Value;
                var tipoDoc = this.UIAPIRawForm.DataSources.UserDataSources.Item("udTipDoc").ValueEx == "Todos" ? "" : this.UIAPIRawForm.DataSources.UserDataSources.Item("udTipDoc").ValueEx;
                var dataDe = this.UIAPIRawForm.DataSources.UserDataSources.Item("udDataDe").ValueEx;
                var dataAte = this.UIAPIRawForm.DataSources.UserDataSources.Item("udDataAte").ValueEx;
                var slpCode = this.UIAPIRawForm.DataSources.UserDataSources.Item("cbVendComp").ValueEx == "-1" ? "" : this.UIAPIRawForm.DataSources.UserDataSources.Item("cbVendComp").ValueEx;
                var filial = this.UIAPIRawForm.DataSources.UserDataSources.Item("cbFilial").ValueEx == "Todas" ? "" : this.UIAPIRawForm.DataSources.UserDataSources.Item("cbFilial").ValueEx;
                var userAprove = Program.oCompany.UserSignature;

                string sqlQuery = string.Format(Resources.Resource.LoadDocAprove,
                                                                                  docNum,
                                                                                  tipoDoc,
                                                                                  dataDe,
                                                                                  dataAte,
                                                                                  slpCode,
                                                                                  filial,
                                                                                  userAprove);

                this.UIAPIRawForm.DataSources.DataTables.Item("mtxAprov").ExecuteQuery(sqlQuery);

                mtxAprov.LoadFromDataSource();
                mtxAprov.AutoResizeColumns();
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

        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            SAPbouiCOM.DataTable oDT = this.UIAPIRawForm.DataSources.DataTables.Item("mtxAprov");
            mtxAprov.FlushToDataSource();

            try
            {
                Dictionary<string, string> docsAprov = new Dictionary<string, string>();
                Dictionary<string, string> docsReprov = new Dictionary<string, string>();



                for (int i = 0; i < oDT.Rows.Count; i++)
                {
                    var tipoDoc = oDT.GetValue("TipoDoc", i).ToString();
                    var selected = oDT.GetValue("Sel", i).ToString();
                    var docEntry = oDT.GetValue("DocEntry", i).ToString();
                    var status = oDT.GetValue("Status", i).ToString();

                    if (selected == "N" || selected == "")
                        continue;

                    switch (status)
                    {
                        case "Y":
                            docsAprov.Add(docEntry, tipoDoc);
                            break;

                        case "N":
                            docsReprov.Add(docEntry, tipoDoc);
                            break;
                    }
                }

                foreach (var dAprov in docsAprov)
                {
                    ProcessDocAprov(Convert.ToInt32(dAprov.Key), dAprov.Value, "Y");
                }

                foreach (var dReprov in docsReprov)
                {
                    ProcessTableAprov(Convert.ToInt32(dReprov.Key), dReprov.Value, "N");
                }

            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.Message, 1, "Ok");
            }
            finally
            {
                if (oDT != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oDT);
                }
            }
        }

        /* Metodo para processar documentos selecionados - Aprovado */
        private void ProcessDocAprov(int docentry, string tipoDoc, string status)
        {
            SAPbobsCOM.Documents oOrder = null;
            SAPbobsCOM.Documents oPurchaseQuotations = null;
            SAPbobsCOM.Documents oPurchaseOrders = null;
            try
            {
                switch (tipoDoc)
                {
                    case "Pedido de venda":
                        oOrder = (SAPbobsCOM.Documents)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                        if (oOrder.GetByKey(docentry))
                        {
                            oOrder.Confirmed = SAPbobsCOM.BoYesNoEnum.tYES;

                            Int32 lRet = oOrder.Update();

                            if (lRet != 0)
                            {
                                throw new Exception(Program.oCompany.GetLastErrorDescription());
                            }

                            ProcessTableAprov(docentry, oOrder.DocObjectCodeEx, status);

                        }
                        break;

                    case "Oferta de compra":
                        oPurchaseQuotations = (SAPbobsCOM.Documents)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseQuotations);
                        if (oPurchaseQuotations.GetByKey(docentry))
                        {
                            oPurchaseQuotations.Confirmed = SAPbobsCOM.BoYesNoEnum.tYES;

                            Int32 lRet = oPurchaseQuotations.Update();

                            if (lRet != 0)
                            {
                                throw new Exception(Program.oCompany.GetLastErrorDescription());
                            }

                            ProcessTableAprov(docentry, oPurchaseQuotations.DocObjectCodeEx, status);
                        }
                        break;

                    case "Pedido de compra":
                        oPurchaseOrders = (SAPbobsCOM.Documents)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);
                        if (oPurchaseOrders.GetByKey(docentry))
                        {
                            oPurchaseOrders.Confirmed = SAPbobsCOM.BoYesNoEnum.tYES;

                            Int32 lRet = oPurchaseOrders.Update();

                            if (lRet != 0)
                            {
                                throw new Exception(Program.oCompany.GetLastErrorDescription());
                            }

                            ProcessTableAprov(docentry, oPurchaseOrders.DocObjectCodeEx, status);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.Message, 1, "Ok");
            }
            finally
            {
                if (oOrder != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oOrder);
                }
                if (oPurchaseQuotations != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oPurchaseQuotations);
                }
                if (oPurchaseOrders != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oPurchaseOrders);
                }
            }
        }

        /*Metodo para popular tabela: @BONEAPROV*/
        private void ProcessTableAprov(int docEntry, string tipoDoc, string status)
        {
            SAPbobsCOM.UserTable oTable = Program.oCompany.UserTables.Item("BONEAPROV");
            SAPbobsCOM.Recordset oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            try
            {
                switch (tipoDoc)
                {
                    case "Pedido de venda":
                        tipoDoc = "17";
                        break;

                    case "Oferta de compra":
                        tipoDoc = "540000006";
                        break;

                    case "Pedido de compra":
                        tipoDoc = "22";
                        break;
                }


                oRst.DoQuery($@"SELECT T0.""Code"" FROM [@BONEAPROV] T0 WHERE T0.""U_BOneNumDoc"" = {docEntry} AND T0.""U_BOneTipoDoc"" = '{tipoDoc}'");
                var code = oRst.Fields.Item("Code").Value.ToString();

                switch (status)
                {
                    case "Y":

                        if (oTable.GetByKey(code))
                        {
                            oTable.UserFields.Fields.Item("U_BOneAutorizado").Value = "TRUE";
                            oTable.UserFields.Fields.Item("U_BOneProcessado").Value = 1;
                            Int32 lRet = oTable.Update();

                            if (lRet != 0)
                            {
                                throw new Exception(Program.oCompany.GetLastErrorDescription());
                            }
                        }
                        break;

                    case "N":
                        if (oTable.GetByKey(code))
                        {
                            oTable.UserFields.Fields.Item("U_BOneProcessado").Value = 1;
                            Int32 lRet = oTable.Update();

                            if (lRet != 0)
                            {
                                throw new Exception(Program.oCompany.GetLastErrorDescription());
                            }
                        }

                        break;
                }


                Application.SBO_Application.StatusBar.SetText("Processo finalizado com sucesso", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);


            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.Message, 1, "Ok");
            }
            finally
            {
                if (oTable != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oTable);
                }
                if (oRst != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRst);
                }

                LoadMatrix();
            }
        }

    }
}