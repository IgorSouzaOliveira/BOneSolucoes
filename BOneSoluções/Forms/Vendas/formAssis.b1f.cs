﻿using BOneSolucoes.Comonn;
using BOneSolucoes.Entities;
using BOneSolucoes.Models;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOneSolucoes.Forms.Vendas
{
    [FormAttribute("Forms.Vendas", "Forms/Vendas/formAssis.b1f")]
    class formAssis : UserFormBase
    {
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Matrix Matrix0;
        private SAPbouiCOM.Button Button2;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.EditText EditText1;
        private SAPbouiCOM.EditText EditText2;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.EditText EditText3;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.EditText EditText4;
        private SAPbouiCOM.StaticText StaticText5;
        private SAPbouiCOM.ComboBox ComboBox0;
        private SAPbouiCOM.StaticText StaticText6;
        private SAPbouiCOM.ComboBox ComboBox1;
        private SAPbouiCOM.LinkedButton LinkedButton0;
        private SAPbouiCOM.Button Button3;
        private SAPbouiCOM.StaticText StaticText7;
        private SAPbouiCOM.EditText edQuant;
        public formAssis()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.Matrix0 = ((SAPbouiCOM.Matrix)(this.GetItem("Item_3").Specific));
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("Item_4").Specific));
            this.Button2.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button2_PressedAfter);
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_5").Specific));
            this.EditText0.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.EditText0_ChooseFromListAfter);
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_7").Specific));
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_8").Specific));
            this.EditText2 = ((SAPbouiCOM.EditText)(this.GetItem("Item_9").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_10").Specific));
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_11").Specific));
            this.EditText3 = ((SAPbouiCOM.EditText)(this.GetItem("Item_12").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_13").Specific));
            this.EditText4 = ((SAPbouiCOM.EditText)(this.GetItem("Item_14").Specific));
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_15").Specific));
            this.ComboBox0 = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_16").Specific));
            this.StaticText6 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_17").Specific));
            this.ComboBox1 = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_18").Specific));
            this.LinkedButton0 = ((SAPbouiCOM.LinkedButton)(this.GetItem("Item_19").Specific));
            this.Button3 = ((SAPbouiCOM.Button)(this.GetItem("Item_20").Specific));
            this.Button3.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button3_PressedAfter);
            this.StaticText7 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.edQuant = ((SAPbouiCOM.EditText)(this.GetItem("edQuant").Specific));
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
            Vendedor();
            Filial();
        }
        public void LoadMatrix()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                this.UIAPIRawForm.DataSources.DataTables.Item("dtAssis").ExecuteQuery(Resources.Resource.LoadPed);
                Matrix0.Columns.Item("colSel").DataBind.Bind("dtAssis", "Selecionar");
                Matrix0.Columns.Item("colPed").DataBind.Bind("dtAssis", "Nº Pedido");
                Matrix0.Columns.Item("colClie").DataBind.Bind("dtAssis", "Cliente");
                Matrix0.Columns.Item("colNome").DataBind.Bind("dtAssis", "Nome");
                Matrix0.Columns.Item("colPCont").DataBind.Bind("dtAssis", "Pessoa de contato");
                Matrix0.Columns.Item("colBplN").DataBind.Bind("dtAssis", "Filial");
                Matrix0.Columns.Item("colDate").DataBind.Bind("dtAssis", "Data de lançamento");
                Matrix0.Columns.Item("colVend").DataBind.Bind("dtAssis", "Vendedor");
                Matrix0.Columns.Item("colCond").DataBind.Bind("dtAssis", "Condição de pagamento");
                Matrix0.Columns.Item("colForm").DataBind.Bind("dtAssis", "Forma de pagamento");
                Matrix0.Columns.Item("colDocT").DataBind.Bind("dtAssis", "Total do documento");
                Matrix0.Columns.Item("colObs").DataBind.Bind("dtAssis", "Observações");

                Matrix0.LoadFromDataSource();
                Matrix0.AutoResizeColumns();

                edQuant.Value = Matrix0.RowCount.ToString();

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
        public void Vendedor()
        {
            SAPbobsCOM.Recordset oRst = null;

            try
            {
                oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRst.DoQuery(@"SELECT ""SlpCode"", ""SlpName"" FROM OSLP WHERE ""Active"" = 'Y'");

                if (oRst.RecordCount > 0)
                {
                    oRst.MoveFirst();
                    for (int i = 0; i < oRst.RecordCount; i++)
                    {
                        ComboBox1.ValidValues.Add(oRst.Fields.Item("SlpCode").Value.ToString(), oRst.Fields.Item("SlpName").Value.ToString());
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
        public void Filial()
        {
            SAPbobsCOM.Recordset oRst = null;

            try
            {
                oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRst.DoQuery(@"SELECT T0.""BPLId"", T0.""BPLName"" FROM OBPL T0 WHERE T0.""Disabled"" = 'N'");

                if (oRst.RecordCount > 0)
                {
                    oRst.MoveFirst();
                    for (int i = 0; i < oRst.RecordCount; i++)
                    {
                        ComboBox0.ValidValues.Add(oRst.Fields.Item("BPLId").Value.ToString(), oRst.Fields.Item("BPLName").Value.ToString());
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
        private void Button3_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                var cardCode = this.UIAPIRawForm.DataSources.UserDataSources.Item("udCardCode").Value;
                var vendedor = this.UIAPIRawForm.DataSources.UserDataSources.Item("udVendedor").ValueEx == "-1" ? "" : this.UIAPIRawForm.DataSources.UserDataSources.Item("udVendedor").ValueEx;
                var dataDe = this.UIAPIRawForm.DataSources.UserDataSources.Item("dtDataDe").ValueEx;
                var dataAte = this.UIAPIRawForm.DataSources.UserDataSources.Item("dtDataAte").ValueEx;
                var docDe = this.UIAPIRawForm.DataSources.UserDataSources.Item("udDocDe").Value;
                var docAte = this.UIAPIRawForm.DataSources.UserDataSources.Item("udDocAte").Value;
                var filial = this.UIAPIRawForm.DataSources.UserDataSources.Item("udFilial").ValueEx;

                String query = string.Format(Resources.Resource.LoadPedFilter, cardCode
                                                                                      , vendedor
                                                                                      , dataDe
                                                                                      , dataAte
                                                                                      , docDe
                                                                                      , docAte
                                                                                      , filial);

                this.UIAPIRawForm.DataSources.DataTables.Item("dtAssis").ExecuteQuery(query);
                Matrix0.LoadFromDataSource();
                Matrix0.AutoResizeColumns();
                edQuant.Value = Matrix0.RowCount.ToString();
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
        private void EditText0_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            SAPbouiCOM.ISBOChooseFromListEventArg oCFLEvent = null;
            oCFLEvent = (SAPbouiCOM.ISBOChooseFromListEventArg)pVal;
            SAPbouiCOM.DataTable oDataTable = oCFLEvent.SelectedObjects;

            if (oDataTable == null)
                return;

            this.UIAPIRawForm.DataSources.UserDataSources.Item("udCardCode").Value = oDataTable.GetValue("CardCode", 0).ToString();


        }
        private void Button2_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {

            if (Application.SBO_Application.MessageBox("Os pedidos selecionados serão faturados." + Environment.NewLine + "Deseja prosseguir ?", 1, "Sim", "Não") != 1)
                return;

            SAPbouiCOM.DataTable oDT = this.UIAPIRawForm.DataSources.DataTables.Item("dtAssis");
            SAPbouiCOM.ProgressBar oProgressBar = null;

            try
            {

                Matrix0.FlushToDataSource();

                List<String> selectedPed = new List<string>();

                for (int i = 0; i < oDT.Rows.Count; i++)
                {
                    var selected = oDT.GetValue("Selecionar", i).ToString();

                    if (selected == "N" || selected == "")
                        continue;

                    selectedPed.Add(oDT.GetValue("Nº Pedido", i).ToString());

                }

                oProgressBar = Application.SBO_Application.StatusBar.CreateProgressBar("", selectedPed.Count, false);
                oProgressBar.Text = "Gerando Nota Fiscal de Saida. Aguarde...";
                foreach (var list in selectedPed)
                {
                    var dataOrder = SAPCommon.GetOrders(list);

                    if (dataOrder == null)
                        return;

                    InvoiceModel invoice = new InvoiceModel();

                    invoice.CardCode = dataOrder.CardCode;
                    invoice.CardName = dataOrder.CardName;
                    invoice.BPL_IDAssignedToInvoice = dataOrder.BPL_IDAssignedToInvoice;
                    invoice.Comments = dataOrder.Comments;
                    invoice.PaymentGroupCode = dataOrder.PaymentGroupCode;
                    invoice.PaymentMethod = dataOrder.PaymentMethod;
                    invoice.SalesPersonCode = dataOrder.SalesPersonCode;

                    invoice.DocumentLines = new List<ItemModelInvoice>();


                    foreach (var docLine in dataOrder.DocumentLines)
                    {
                        ItemModelInvoice item = new ItemModelInvoice();
                        item.BatchNumbers = new List<BatchNumbersInvoiceModel>();


                        item.ItemCode = docLine.ItemCode;
                        item.Quantity = docLine.Quantity;
                        item.Price = docLine.Price;
                        item.Usage = docLine.Usage;
                        item.BaseType = "17";
                        item.BaseEntry = dataOrder.DocEntry;
                        item.BaseLine = docLine.LineNum;

                        if (docLine.BatchNumbers.Count > 0)
                        {
                            foreach (var lotePed in docLine.BatchNumbers)
                            {
                                BatchNumbersInvoiceModel batchNumbers = new BatchNumbersInvoiceModel();

                                batchNumbers.BatchNumber = lotePed.BatchNumber;
                                batchNumbers.AddmisionDate = lotePed.AddmisionDate;
                                batchNumbers.Quantity = lotePed.Quantity;
                                batchNumbers.ItemCode = lotePed.ItemCode;
                                item.BatchNumbers.Add(batchNumbers);
                            }
                        }

                        invoice.DocumentLines.Add(item);

                    }

                    var result = SAPCommon.AddInvoice(invoice);

                    if (result != null)
                    {
                        oProgressBar.Value++;
                    }
                }

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
                if (oProgressBar != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oProgressBar);
                }

                LoadMatrix();
            }

        }

    }
}
