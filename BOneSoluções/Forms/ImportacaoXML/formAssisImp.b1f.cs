using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace BOneSolucoes.Forms.ImportacaoXML
{
    [FormAttribute("formAssisImp", "Forms/ImportacaoXML/formAssisImp.b1f")]
    class formAssisImp : UserFormBase
    {

        private SAPbouiCOM.Matrix mtxImpo;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Button Button2;
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
        }

        private void ReadXML()
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                string path = @"C:\Users\igor.oliveira\Documents\32240323400263000193570010000146251000756532.xml";

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
                Application.SBO_Application.MessageBox(ex.Message,1,"Ok","Cancelar");
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
    }
}
