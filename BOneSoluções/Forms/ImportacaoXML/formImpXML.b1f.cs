using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BOneSolucoes.Forms.ImportacaoXML
{
    [FormAttribute("formImpXML", "Forms/ImportacaoXML/formImpXML.b1f")]
    class formImpXML : UserFormBase
    {

        private SAPbouiCOM.Button bImport;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Button bArquivo;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.OptionBtn rdPedC;
        private SAPbouiCOM.OptionBtn radNfE;
        private SAPbouiCOM.OptionBtn rdRecM;
        private SAPbouiCOM.EditText EditText0;
        public formImpXML()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_0").Specific));
            this.bImport = ((SAPbouiCOM.Button)(this.GetItem("bImport").Specific));
            this.bImport.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.bImport_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.bArquivo = ((SAPbouiCOM.Button)(this.GetItem("bArquivo").Specific));
            this.bArquivo.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.bArquivo_PressedBefore);
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.rdPedC = ((SAPbouiCOM.OptionBtn)(this.GetItem("rdPedC").Specific));
            this.radNfE = ((SAPbouiCOM.OptionBtn)(this.GetItem("radNfE").Specific));
            this.rdRecM = ((SAPbouiCOM.OptionBtn)(this.GetItem("rdRecM").Specific));
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_9").Specific));
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
            radNfE.GroupWith("rdPedC");
            rdRecM.GroupWith("radNfE");
        }

        private void bArquivo_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {

                Thread t = new Thread(() =>
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();

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
                Program.Sbo_App.MessageBox(ex.Message,1,"Ok","Cancelar");                
            }

        }

        private SAPbouiCOM.StaticText StaticText1;

        private void bImport_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            formAssisImp formAssis = new formAssisImp();
            formAssis.Show();
        }
    }
}
