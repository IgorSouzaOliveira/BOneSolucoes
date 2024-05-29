using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOneSoluções.Forms.Configuração
{
    [FormAttribute("BOneSoluções.Forms.Configuração.formConfigAprov", "Forms/Configuração/formConfigAprov.b1f")]
    class formConfigAprov : UserFormBase
    {

        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.ComboBox ComboBox0;
        private SAPbouiCOM.Button Button2;
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
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.ComboBox0 = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_4").Specific));
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("Item_5").Specific));
            this.Button2.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button2_PressedAfter);
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
                mtxConf.Columns.Item("colObj").DataBind.Bind("mtxConf", "U_ObjectType");
                mtxConf.Columns.Item("colQuery").DataBind.Bind("mtxConf", "U_Query");
                mtxConf.Columns.Item("colAtivo").DataBind.Bind("mtxConf", "U_Ativo");

                mtxConf.LoadFromDataSource();
                mtxConf.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message,SAPbouiCOM.BoMessageTime.bmt_Short,SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                this.UIAPIRawForm.Freeze(false);
            }
        }
        private void Button2_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            mtxConf.AddRow();            
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
    }
}
