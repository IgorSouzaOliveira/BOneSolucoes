using SAPbouiCOM.Framework;
using System;
using System.Threading;
using System.Windows.Forms;
using Application = SAPbouiCOM.Framework.Application;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using BOneSolucoes.Forms.ImportacaoXML.Entities;
using System.Diagnostics;

namespace BOneSolucoes.Forms.ImportacaoXML
{
    [FormAttribute("formAssisImp", "Forms/ImportacaoXML/formAssisImp.b1f")]
    class formAssisImp : UserFormBase
    {

        private SAPbouiCOM.Matrix mtxImpo;
        private SAPbouiCOM.Button Button2;
        private SAPbouiCOM.Button Button3;
        private SAPbouiCOM.Button Button4;
        private SAPbouiCOM.OptionBtn rdPedC, rdRecM, radNfE;
        private SAPbouiCOM.EditText EditText4;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.ComboBox cbxFilial;
      
        public formAssisImp()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtxImpo = ((SAPbouiCOM.Matrix)(this.GetItem("mtxImpo").Specific));
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.Button3 = ((SAPbouiCOM.Button)(this.GetItem("bArquivo").Specific));
            this.Button3.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.Button3_PressedBefore);
            this.rdPedC = ((SAPbouiCOM.OptionBtn)(this.GetItem("rdPedC").Specific));
            this.rdRecM = ((SAPbouiCOM.OptionBtn)(this.GetItem("rdRecM").Specific));
            this.radNfE = ((SAPbouiCOM.OptionBtn)(this.GetItem("radNfE").Specific));
            this.Button4 = ((SAPbouiCOM.Button)(this.GetItem("Item_12").Specific));
            this.Button4.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button4_PressedAfter);
            this.EditText4 = ((SAPbouiCOM.EditText)(this.GetItem("Item_3").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("btnGerarDo").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_1").Specific));
            this.cbxFilial = ((SAPbouiCOM.ComboBox)(this.GetItem("cbxFilial").Specific));
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
            Utilizacao();
            Filial();
            mtxImpo.AutoResizeColumns();
            radNfE.GroupWith("rdPedC");
            rdRecM.GroupWith("radNfE");
            //rdImpMul.GroupWith("rdImpUni");
        }
        private void ReadXML(string[] arquivos)
        {
            SAPbobsCOM.UserTable oTable = Program.oCompany.UserTables.Item("BONEXMLDATA");
            SAPbobsCOM.Recordset oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            SAPbobsCOM.Recordset oRstCardCode = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            try
            {
                this.UIAPIRawForm.Freeze(true);
                string chaveAcesso = null;

                foreach (var arq in arquivos)
                {
                    string path = arq;

                    if (!File.Exists(path))
                    {
                        Application.SBO_Application.MessageBox("Arquivo não encontrado.", 1, "Ok", "Cancelar");
                    }

                    XmlSerializer ser = new XmlSerializer(typeof(NFeProc));

                    using (TextReader textReader = new StreamReader(path))
                    using (XmlTextReader reader = new XmlTextReader(textReader))
                    {
                        NFeProc nfe = (NFeProc)ser.Deserialize(reader);

                        chaveAcesso = nfe.ProtNFe.InfoProtocolo.chNFe;

                        oRstCardCode.DoQuery($@"SELECT TOP 1 A.CardCode FROM CRD7 A WHERE A.""TaxId0"" = '{FormatarCnpj(nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.CNPJ)}' AND A.CardCode IN (SELECT B.CardCode FROM OSCN B )");
                        if (oRstCardCode.RecordCount == 0)
                            throw new Exception("Necessário vinculo na tela de Números de catálogo de parceiro de negócios.");


                        oRst.DoQuery($"SELECT 'TRUE' FROM [@BONEXMLDATA] WHERE U_ChaveAcesso = '{chaveAcesso}'");
                        if (oRst.RecordCount > 0)
                        {
                            LoadMatrix(chaveAcesso);
                            return;
                        }

                        foreach (var produto in nfe.NotaFiscalEletronica.InformacoesNFe.Produtos)
                        {
                            /* TAG <ide>*/
                            oTable.UserFields.Fields.Item("U_ChaveAcesso").Value = nfe.ProtNFe.InfoProtocolo.chNFe;
                            oTable.UserFields.Fields.Item("U_idecUF").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.cUF.ToString();
                            oTable.UserFields.Fields.Item("U_idecNF").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.cNF;
                            oTable.UserFields.Fields.Item("U_ideMod").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.mod;
                            oTable.UserFields.Fields.Item("U_ideSerie").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.serie.ToString();
                            oTable.UserFields.Fields.Item("U_idenNF").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.nNF;
                            oTable.UserFields.Fields.Item("U_idedhEmi").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Identificacao.dhEmi;


                            /* TAG <emit>*/

                            oTable.UserFields.Fields.Item("U_emitCNPJ").Value = FormatarCnpj(nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.CNPJ);
                            oTable.UserFields.Fields.Item("U_emitxNome").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.xNome;
                            oTable.UserFields.Fields.Item("U_emitxFant").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.xFant;
                            oTable.UserFields.Fields.Item("U_enderEmitxLgr").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.xLgr;
                            oTable.UserFields.Fields.Item("U_enderEmitNro").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.nro;
                            oTable.UserFields.Fields.Item("U_enderEmitxBairro").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.xBairro;
                            oTable.UserFields.Fields.Item("U_enderEmitcMun").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.cMun;
                            oTable.UserFields.Fields.Item("U_enderEmitxMun").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.xMun;
                            oTable.UserFields.Fields.Item("U_enderEmitUF").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.UF;
                            oTable.UserFields.Fields.Item("U_enderEmitCEP").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.CEP;
                            oTable.UserFields.Fields.Item("U_enderEmitcPais").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.cPais.ToString();
                            oTable.UserFields.Fields.Item("U_enderEmitxPais").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.Endereco.xPais;
                            oTable.UserFields.Fields.Item("U_enderEmitIE").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.IE;
                            oTable.UserFields.Fields.Item("U_enderEmitCRT").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.CRT.ToString();

                            /*TAG <det>*/
                            oTable.UserFields.Fields.Item("U_prodcProd").Value = produto.cProd;
                            oTable.UserFields.Fields.Item("U_prodcEAN").Value = produto.cEAN;
                            oTable.UserFields.Fields.Item("U_prodxProd").Value = produto.xProd;
                            oTable.UserFields.Fields.Item("U_prodNCM").Value = produto.NCM;
                            oTable.UserFields.Fields.Item("U_prodCFOP").Value = produto.CFOP;
                            oTable.UserFields.Fields.Item("U_produCom").Value = produto.uCom;
                            oTable.UserFields.Fields.Item("U_prodqCom").Value = produto.qCom;
                            oTable.UserFields.Fields.Item("U_prodvUnCom").Value = produto.vUnCom;




                            ///* TAG <dest>*/
                            //oTable.UserFields.Fields.Item("U_destCNPJ").Value = nfe.NFe.infNFe.dest.CNPJ.ToString();
                            //oTable.UserFields.Fields.Item("U_destxNome").Value = nfe.NFe.infNFe.dest.xNome.ToString();
                            //oTable.UserFields.Fields.Item("U_enderDestxLgr").Value = nfe.NFe.infNFe.dest.enderDest.xLgr.ToString();
                            //oTable.UserFields.Fields.Item("U_enderDestNro").Value = nfe.NFe.infNFe.dest.enderDest.nro.ToString();
                            //oTable.UserFields.Fields.Item("U_enderDestxBairro").Value = nfe.NFe.infNFe.dest.enderDest.xBairro.ToString();
                            //oTable.UserFields.Fields.Item("U_enderDestcMun").Value = nfe.NFe.infNFe.dest.enderDest.cMun.ToString();
                            //oTable.UserFields.Fields.Item("U_enderDestxMun").Value = nfe.NFe.infNFe.dest.enderDest.xMun.ToString();
                            //oTable.UserFields.Fields.Item("U_enderDestCEP").Value = nfe.NFe.infNFe.dest.enderDest.CEP.ToString();
                            //oTable.UserFields.Fields.Item("U_enderDestcPais").Value = Convert.ToInt32(nfe.NFe.infNFe.dest.enderDest.cPais);
                            //oTable.UserFields.Fields.Item("U_enderDestxPais").Value = nfe.NFe.infNFe.dest.enderDest.xPais.ToString();
                            //oTable.UserFields.Fields.Item("U_enderDestfone").Value = nfe.NFe.infNFe.dest.enderDest.fone.ToString();
                            //oTable.UserFields.Fields.Item("U_indIEDest").Value = nfe.NFe.infNFe.dest.indIEDest.ToString();
                            //oTable.UserFields.Fields.Item("U_enderDestIE").Value = nfe.NFe.infNFe.dest.IE.ToString();
                            //oTable.UserFields.Fields.Item("U_enderDestEmail").Value = nfe.NFe.infNFe.dest.email.ToString();

                            int lRet = oTable.Add();

                            if (lRet != 0)
                            {
                                throw new Exception(Program.oCompany.GetLastErrorDescription());
                            }
                        }

                    }

                    LoadMatrix(chaveAcesso);
                    Application.SBO_Application.StatusBar.SetText("Processo finalizado com sucesso.", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
                }


            }
            catch (InvalidOperationException ex)
            {

                if (ex.InnerException != null)
                {
                    Application.SBO_Application.MessageBox($"Erro na leitura do XML. {Environment.NewLine} Detalhes do erro: {ex.InnerException.Message}");
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.Message);
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
                if (oRstCardCode != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRstCardCode);
                }
                this.UIAPIRawForm.Freeze(false);
            }
        }
        private void LoadMatrix(string chaveAcesso)
        {
            try
            {

                this.UIAPIRawForm.Freeze(true);

                string sqlQuery = string.Format(Resources.Resource.CarregarXmlImp, chaveAcesso);

                this.UIAPIRawForm.DataSources.DataTables.Item("dtAssisAp").ExecuteQuery(sqlQuery);

                mtxImpo.Columns.Item("colCheck").DataBind.Bind("dtAssisAp", "Check");
                mtxImpo.Columns.Item("colForne").DataBind.Bind("dtAssisAp", "CardCode");
                mtxImpo.Columns.Item("colFName").DataBind.Bind("dtAssisAp", "CardName");
                mtxImpo.Columns.Item("colCnpj").DataBind.Bind("dtAssisAp", "CNPJ");
                mtxImpo.Columns.Item("colInscri").DataBind.Bind("dtAssisAp", "IE");
                mtxImpo.Columns.Item("colItemC").DataBind.Bind("dtAssisAp", "ItemCode");
                mtxImpo.Columns.Item("colPrice").DataBind.Bind("dtAssisAp", "Preço");
                mtxImpo.Columns.Item("colQtd").DataBind.Bind("dtAssisAp", "Quantidade");
                mtxImpo.Columns.Item("colEan").DataBind.Bind("dtAssisAp", "EAN");

                mtxImpo.LoadFromDataSource();
                mtxImpo.AutoResizeColumns();

            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.Message, 1, "Ok", "Cancel");
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }
        private void Button4_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {

            string[] arqSelected = this.UIAPIRawForm.DataSources.UserDataSources.Item("udArquivo").Value.Split(';');

            ReadXML(arqSelected);

        }
        private void Button3_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                this.UIAPIRawForm.Freeze(true);


                try
                {
                    Thread t = new Thread(() =>
                    {
                        using (OpenFileDialog openFileDialog = new OpenFileDialog())
                        {
                            openFileDialog.Multiselect = false;
                            openFileDialog.Filter = "XML Files (*.XML)|*.XML";
                            openFileDialog.RestoreDirectory = true;

                            var processes = Process.GetProcessesByName("SAP Business One");
                            if (processes.Length == 1)
                            {
                                var windowHandle = processes[0].MainWindowHandle;
                                var windowWrapper = new WindowWrapper(windowHandle);
                                var result = openFileDialog.ShowDialog(windowWrapper);

                                if (result == DialogResult.OK)
                                {
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item("udArquivo").Value = string.Empty;
                                    this.UIAPIRawForm.DataSources.UserDataSources.Item("udArquivo").Value = string.Join(";", openFileDialog.FileNames);
                                }
                            }
                        }
                    });          
                    t.IsBackground = true;
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }




                //Thread t = new Thread(() =>
                //{

                //    OpenFileDialog openFileDialog = new OpenFileDialog();
                //    openFileDialog.Multiselect = true;
                //    openFileDialog.Filter = "Arquivos xml|*.xml";
                //    openFileDialog.Title = "Selecione os Arquivos";



                //    DialogResult dr = openFileDialog.ShowDialog(mainForm);


                //    mainForm.TopMost = true;
                //    mainForm.StartPosition = FormStartPosition.CenterScreen;
                //    mainForm.ShowInTaskbar = true;


                //    if (dr == DialogResult.OK)
                //    {

                //        this.UIAPIRawForm.DataSources.UserDataSources.Item("udArquivo").Value = string.Empty;
                //        this.UIAPIRawForm.DataSources.UserDataSources.Item("udArquivo").Value = string.Join(";", openFileDialog.FileNames);
                //    }
                //});

                //t.IsBackground = false;
                //t.SetApartmentState(ApartmentState.STA);
                //t.Start();

            }
            catch (Exception ex)
            {
                Program.Sbo_App.MessageBox(ex.Message, 1, "Ok", "Cancelar");
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }

        }

        private class WindowWrapper : IWin32Window
        {
            private IntPtr _handle;

            public WindowWrapper(IntPtr handle)
            {
                _handle = handle;
            }

            public IntPtr Handle => _handle;
        }
        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            bool pedCompra = ((SAPbouiCOM.OptionBtn)this.UIAPIRawForm.Items.Item("rdPedC").Specific).Selected;
            bool recMercad = ((SAPbouiCOM.OptionBtn)this.UIAPIRawForm.Items.Item("rdRecM").Specific).Selected;
            bool nfeEntrada = ((SAPbouiCOM.OptionBtn)this.UIAPIRawForm.Items.Item("radNfE").Specific).Selected;

            try
            {
                if (pedCompra == true)
                {
                    GerarPedCompra();
                }
                else if (recMercad == true)
                {
                    GerarRecMercadoria();
                }
                else if (nfeEntrada == true)
                {

                }
                else
                {
                    Application.SBO_Application.MessageBox("Selecione o tipo de documento a ser gerado.", 1, "Ok", "Cancelar");
                }

            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }

        }
        private void Utilizacao()
        {
            SAPbouiCOM.Column oColumn = null;
            SAPbouiCOM.Matrix oMatrix = null;
            SAPbobsCOM.Recordset oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            oMatrix = (SAPbouiCOM.Matrix)this.UIAPIRawForm.Items.Item("mtxImpo").Specific;
            oColumn = oMatrix.Columns.Item("colUsage");

            try
            {
                this.UIAPIRawForm.Freeze(true);

                oRst.DoQuery($@"SELECT T0.""ID"", T0.""Usage"" FROM OUSG T0 WHERE T0.""U_UtilImpXml"" = 'S'");
                if (oRst.RecordCount > 0)
                {
                    oRst.MoveFirst();
                    for (int i = 0; i < oRst.RecordCount; i++)
                    {
                        oColumn.ValidValues.Add(oRst.Fields.Item("Id").Value.ToString(), oRst.Fields.Item("Usage").Value.ToString());
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
                if (oColumn != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oColumn);
                }
                if (oMatrix != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oMatrix);
                }
                if (oRst != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRst);
                }

                this.UIAPIRawForm.Freeze(false);
            }
        }
        private string FormatarCnpj(string CNPJ)
        {
            try
            {
                return Convert.ToUInt64(CNPJ).ToString(@"00\.000\.000\/0000\-00");
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return string.Empty;
            }
        }
        private void GerarPedCompra()
        {
            SAPbobsCOM.Documents oPed = (SAPbobsCOM.Documents)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);
            SAPbouiCOM.DataTable oDT = this.UIAPIRawForm.DataSources.DataTables.Item("dtAssisAp");

            try
            {
                mtxImpo.FlushToDataSource();

                for (int i = 0; i < oDT.Rows.Count; i++)
                {
                    string selected = oDT.GetValue("Check", i).ToString();

                    if (selected == "N" || selected == "")
                        continue;


                    switch (selected)
                    {
                        case "Y":
                            oPed.CardCode = oDT.GetValue("CardCode", i).ToString();
                            oPed.CardName = oDT.GetValue("CardName", i).ToString();
                            oPed.BPL_IDAssignedToInvoice = Convert.ToInt32(this.UIAPIRawForm.DataSources.UserDataSources.Item("udFilial").ValueEx);
                            oPed.Comments = ((SAPbouiCOM.EditText)mtxImpo.Columns.Item("colObs").Cells.Item(i + 1).Specific).Value;

                            oPed.Lines.SetCurrentLine(i);
                            oPed.Lines.ItemCode = oDT.GetValue("ItemCode", i).ToString();
                            oPed.Lines.Quantity = Convert.ToDouble(oDT.GetValue("Quantidade", i).ToString());
                            oPed.Lines.Price = Convert.ToDouble(oDT.GetValue("Preço", i));
                            oPed.Lines.Usage = ((SAPbouiCOM.ComboBox)mtxImpo.Columns.Item("colUsage").Cells.Item(i + 1).Specific).Value;
                            oPed.Lines.Add();
                            break;
                    }
                }

                oPed.Lines.Delete();
                int lRet = oPed.Add();

                if (lRet != 0)
                {
                    throw new Exception(Program.oCompany.GetLastErrorDescription());
                }

                var docEntry = Program.oCompany.GetNewObjectKey();
                Application.SBO_Application.MessageBox($"Pedido de compra gerado com sucesso: Nº:{docEntry} ", 1, "Ok", "Cancelar");

            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.Message, 1, "Ok", "Cancelar");
            }
            finally
            {
                if (oPed != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oPed);
                }

                if (oDT != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oDT);
                }
            }
        }
        private void GerarRecMercadoria()
        {
            try
            {
                SAPbobsCOM.Documents oRec = (SAPbobsCOM.Documents)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseDeliveryNotes);
                SAPbouiCOM.DataTable oDT = this.UIAPIRawForm.DataSources.DataTables.Item("dtAssisAp");

                try
                {
                    mtxImpo.FlushToDataSource();

                    for (int i = 0; i < oDT.Rows.Count; i++)
                    {
                        string selected = oDT.GetValue("Check", i).ToString();

                        if (selected == "N" || selected == "")
                            continue;


                        switch (selected)
                        {
                            case "Y":
                                oRec.CardCode = oDT.GetValue("CardCode", i).ToString();
                                oRec.CardName = oDT.GetValue("CardName", i).ToString();
                                oRec.BPL_IDAssignedToInvoice = Convert.ToInt32(this.UIAPIRawForm.DataSources.UserDataSources.Item("udFilial").ValueEx);
                                oRec.Comments = ((SAPbouiCOM.EditText)mtxImpo.Columns.Item("colObs").Cells.Item(i + 1).Specific).Value;

                                oRec.Lines.SetCurrentLine(i);
                                oRec.Lines.ItemCode = oDT.GetValue("ItemCode", i).ToString();
                                oRec.Lines.Quantity = Convert.ToDouble(oDT.GetValue("Quantidade", i).ToString());
                                oRec.Lines.Price = Convert.ToDouble(oDT.GetValue("Preço", i));
                                oRec.Lines.Usage = ((SAPbouiCOM.ComboBox)mtxImpo.Columns.Item("colUsage").Cells.Item(i + 1).Specific).Value;
                                oRec.Lines.Add();
                                break;
                        }
                    }

                    oRec.Lines.Delete();
                    int lRet = oRec.Add();

                    if (lRet != 0)
                    {
                        throw new Exception(Program.oCompany.GetLastErrorDescription());
                    }

                    var docEntry = Program.oCompany.GetNewObjectKey();
                    Application.SBO_Application.MessageBox($"Pedido de compra gerado com sucesso: Nº:{docEntry} ", 1, "Ok", "Cancelar");

                }
                catch (Exception ex)
                {
                    Application.SBO_Application.MessageBox(ex.Message, 1, "Ok", "Cancelar");
                }
                finally
                {
                    if (oRec != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oRec);
                    }

                    if (oDT != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oDT);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private void Filial()
        {
            SAPbobsCOM.Recordset oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            try
            {
                oRst.DoQuery(@"SELECT T0.""BplId"", T0.""BPLName"" FROM OBPL T0 WHERE T0.""Disabled"" = 'N'");
                if (oRst.RecordCount > 0)
                {
                    oRst.MoveFirst();
                    for (int i = 0; i < oRst.RecordCount; i++)
                    {
                        cbxFilial.ValidValues.Add(oRst.Fields.Item("BplId").Value.ToString(), oRst.Fields.Item("BPLName").Value.ToString());
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
    }
}
