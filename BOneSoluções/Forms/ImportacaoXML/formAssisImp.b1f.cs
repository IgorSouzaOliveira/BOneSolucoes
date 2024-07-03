using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Application = SAPbouiCOM.Framework.Application;

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
            this.Button1.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button1_PressedAfter);
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.Button3 = ((SAPbouiCOM.Button)(this.GetItem("bArquivo").Specific));
            this.Button3.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.Button3_PressedBefore);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.rdPedC = ((SAPbouiCOM.OptionBtn)(this.GetItem("rdPedC").Specific));
            this.rdRecM = ((SAPbouiCOM.OptionBtn)(this.GetItem("rdRecM").Specific));
            this.radNfE = ((SAPbouiCOM.OptionBtn)(this.GetItem("radNfE").Specific));
            this.Button4 = ((SAPbouiCOM.Button)(this.GetItem("Item_12").Specific));
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
            mtxImpo.AddRow();
            mtxImpo.AutoResizeColumns();
            radNfE.GroupWith("rdPedC");
            rdRecM.GroupWith("radNfE");
        }

        private void ReadXML()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                string path = this.UIAPIRawForm.DataSources.UserDataSources.Item("udArquivo").ValueEx;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(path);

                XmlNodeList xnList = xmlDocument.GetElementsByTagName("emit");

                foreach (XmlNode xn in xnList)
                {
                    ((SAPbouiCOM.EditText)mtxImpo.Columns.Item("colCnpj").Cells.Item(1).Specific).Value = xn["CNPJ"].InnerText;
                    ((SAPbouiCOM.EditText)mtxImpo.Columns.Item("colInscri").Cells.Item(1).Specific).Value = xn["IE"].InnerText;
                    ((SAPbouiCOM.EditText)mtxImpo.Columns.Item("colNome").Cells.Item(1).Specific).Value = xn["xNome"].InnerText;
                    //((SAPbouiCOM.EditText)mtxImpo.Columns.Item("colDataE").Cells.Item(1).Specific).Value = DateTime.Parse(xn["dhEmi"].InnerText).ToString("dd/MM/yyyy");
                   

                    
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

        private void Button1_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
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
