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
            SAPbobsCOM.UserTable oTable = Program.oCompany.UserTables.Item("");

            try
            {
                this.UIAPIRawForm.Freeze(true);
                string path = this.UIAPIRawForm.DataSources.UserDataSources.Item("udArquivo").ValueEx;


                if (!File.Exists(path))
                {
                    Application.SBO_Application.MessageBox("Arquivo não encontrado.", 1, "Ok", "Cancelar");
                }

                XmlSerializer ser = new XmlSerializer(typeof(nfeProc));

                using (TextReader textReader = new StreamReader(path))
                using (XmlTextReader reader = new XmlTextReader(textReader))
                {
                    nfeProc nfe = (nfeProc)ser.Deserialize(reader);



                    for (int i = 1; i <= mtxImpo.RowCount; i++)
                    {
                        ((SAPbouiCOM.EditText)mtxImpo.Columns.Item("colCnpj").Cells.Item(i).Specific).Value = nfe.NFe.infNFe.emit.CNPJ.ToString();
                        ((SAPbouiCOM.EditText)mtxImpo.Columns.Item("colInscri").Cells.Item(i).Specific).Value = nfe.NFe.infNFe.emit.IE.ToString();
                        ((SAPbouiCOM.EditText)mtxImpo.Columns.Item("colNome").Cells.Item(i).Specific).Value = nfe.NFe.infNFe.emit.xNome.ToString();

                        var a = nfe.NFe.infNFe.det.GetValue(1).ToString();





                    }

                }

            }
            catch (InvalidOperationException ex)
            {

                if (ex.InnerException != null)
                {
                    Application.SBO_Application.MessageBox($"Erro na leitura do XML. {Environment.NewLine} Detalhes do erro: {ex.InnerException.Message}");
                }
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
