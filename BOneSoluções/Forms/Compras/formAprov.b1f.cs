using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOneSolucoes.Forms.Compras
{
    [FormAttribute("formAprov", "Forms/Compras/formAprov.b1f")]
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
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
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
        public void LoadMatrix()
        {
            this.UIAPIRawForm.Freeze(true);
            try
            {
                string query = string.Format(Resources.Resource.BONE_ExecAprov, Program.oCompany.UserSignature);

                this.UIAPIRawForm.DataSources.DataTables.Item("mtxAprov").ExecuteQuery(query);
                mtxAprov.Columns.Item("colSel").DataBind.Bind("mtxAprov", "Sel");
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
    }
}