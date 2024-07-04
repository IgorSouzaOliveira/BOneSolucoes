using SAPbouiCOM.Framework;
using System;
using System.Threading;
using System.Windows.Forms;
using Application = SAPbouiCOM.Framework.Application;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using BOneSolucoes.Forms.ImportacaoXML.Entities;
using System.Collections.Generic;

namespace BOneSolucoes.Forms.ImportacaoXML
{
    [FormAttribute("formAssisImp", "Forms/ImportacaoXML/formAssisImp.b1f")]
    class formAssisImp : UserFormBase
    {

        private SAPbouiCOM.Matrix mtxImpo;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Button Button2;
        private SAPbouiCOM.Button Button3;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.Button Button4;
        private SAPbouiCOM.OptionBtn rdPedC, rdRecM, radNfE;


        public formAssisImp()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtxImpo = ((SAPbouiCOM.Matrix)(this.GetItem("mtxImpo").Specific));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("bProcessar").Specific));
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.Button3 = ((SAPbouiCOM.Button)(this.GetItem("bArquivo").Specific));
            this.Button3.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.Button3_PressedBefore);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.rdPedC = ((SAPbouiCOM.OptionBtn)(this.GetItem("rdPedC").Specific));
            this.rdRecM = ((SAPbouiCOM.OptionBtn)(this.GetItem("rdRecM").Specific));
            this.radNfE = ((SAPbouiCOM.OptionBtn)(this.GetItem("radNfE").Specific));
            this.Button4 = ((SAPbouiCOM.Button)(this.GetItem("Item_12").Specific));
            this.Button4.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button4_PressedAfter);
            this.EditText4 = ((SAPbouiCOM.EditText)(this.GetItem("Item_3").Specific));
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
            mtxImpo.AutoResizeColumns();
            radNfE.GroupWith("rdPedC");
            rdRecM.GroupWith("radNfE");
        }

        private void ReadXML()
        {
            SAPbobsCOM.UserTable oTable = Program.oCompany.UserTables.Item("BONEXMLDATA");
            SAPbobsCOM.Recordset oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            try
            {
                this.UIAPIRawForm.Freeze(true);
                string path = this.UIAPIRawForm.DataSources.UserDataSources.Item("udArquivo").ValueEx;


                if (!File.Exists(path))
                {
                    Application.SBO_Application.MessageBox("Arquivo não encontrado.", 1, "Ok", "Cancelar");
                }

                XmlSerializer ser = new XmlSerializer(typeof(NFeProc));

                using (TextReader textReader = new StreamReader(path))
                using (XmlTextReader reader = new XmlTextReader(textReader))
                {
                    NFeProc nfe = (NFeProc)ser.Deserialize(reader);

                    var chaveAcesso = nfe.ProtNFe.InfoProtocolo.chNFe;

                    oRst.DoQuery($"SELECT 'TRUE' FROM [@BONEXMLDATA] WHERE U_ChaveAcesso = '{chaveAcesso}'");
                    if (oRst.RecordCount > 0)
                        throw new Exception($"XML: {chaveAcesso}, já foi importado!");


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


                        ///* TAG <emit>*/
                        oTable.UserFields.Fields.Item("U_emitCNPJ").Value = nfe.NotaFiscalEletronica.InformacoesNFe.Emitente.CNPJ;
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

                        /* TAG <det>*/
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


                    LoadMatrix();
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
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void LoadMatrix()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                this.UIAPIRawForm.DataSources.DataTables.Item("dtAssisAp").ExecuteQuery(Resources.Resource.CarregarXmlImp);

                mtxImpo.Columns.Item("colCnpj").DataBind.Bind("dtAssisAp", "U_emitCNPJ");
                mtxImpo.Columns.Item("colInscri").DataBind.Bind("dtAssisAp", "U_enderEmitIE");
                mtxImpo.Columns.Item("colNome").DataBind.Bind("dtAssisAp", "U_emitxNome");
                mtxImpo.Columns.Item("colItemC").DataBind.Bind("dtAssisAp", "U_prodcProd");

                mtxImpo.LoadFromDataSource();
                mtxImpo.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.Message,1,"Ok","Cancel");
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }

        private void Button4_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {

            ReadXML();
        }

        private void Button3_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {

                Thread t = new Thread(() =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Multiselect = true;
                    openFileDialog.Filter = "Arquivos xml|*.xml";
                    openFileDialog.Title = "Selecione os Arquivos";

                    DialogResult dr = openFileDialog.ShowDialog(new Form());


                    if (dr == DialogResult.OK)
                    {
                        this.UIAPIRawForm.DataSources.UserDataSources.Item("udArquivo").Value = openFileDialog.FileName;
                    }
                });
                t.IsBackground = true;
                t.SetApartmentState(ApartmentState.STA);
                t.Start();
            }
            catch (Exception ex)
            {
                Program.Sbo_App.MessageBox(ex.Message, 1, "Ok", "Cancelar");
            }

        }
        private SAPbouiCOM.EditText EditText4;
    }
}
