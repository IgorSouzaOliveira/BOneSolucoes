using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOneSolucoes.Forms.ParceiroDeNegocios
{
    [FormAttribute("ParceiroDeNegocios.formPDN", "Forms/ParceiroDeNegocios/formPDN.b1f")]
    class formPDN : UserFormBase
    {
        private SAPbouiCOM.Button Button1, btnIna, oFilter;
        private SAPbouiCOM.Matrix mtxData;
        private SAPbouiCOM.EditText oTextFilter;
        private SAPbouiCOM.StaticText oLblClient, lblGroup;
        private SAPbouiCOM.LinkedButton oLinkFilter;
        private SAPbouiCOM.ComboBox ComboBox0, ComboBox1;
        private SAPbobsCOM.Recordset Rst;
        private SAPbobsCOM.BusinessPartners oBP;
        private SAPbouiCOM.OptionBtn radCliente, radForn;    
       
        public formPDN()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.mtxData = ((SAPbouiCOM.Matrix)(this.GetItem("mtxData").Specific));
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("btnAt").Specific));
            this.Button1.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button1_PressedAfter);
            this.oFilter = ((SAPbouiCOM.Button)(this.GetItem("colF").Specific));
            this.oFilter.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.oFilter_PressedAfter);
            this.oTextFilter = ((SAPbouiCOM.EditText)(this.GetItem("colText").Specific));
            this.oTextFilter.ChooseFromListBefore += new SAPbouiCOM._IEditTextEvents_ChooseFromListBeforeEventHandler(this.oTextFilter_ChooseFromListBefore);
            this.oTextFilter.ChooseFromListAfter += new SAPbouiCOM._IEditTextEvents_ChooseFromListAfterEventHandler(this.oTextFilter_ChooseFromListAfter);
            this.oLinkFilter = ((SAPbouiCOM.LinkedButton)(this.GetItem("Item_10").Specific));
            this.oLblClient = ((SAPbouiCOM.StaticText)(this.GetItem("lblClient").Specific));
            this.btnIna = ((SAPbouiCOM.Button)(this.GetItem("btnIna").Specific));
            this.btnIna.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button4_PressedAfter);
            this.lblGroup = ((SAPbouiCOM.StaticText)(this.GetItem("lblGroup").Specific));
            this.ComboBox0 = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_0").Specific));
            this.ComboBox1 = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_3").Specific));
            this.radCliente = ((SAPbouiCOM.OptionBtn)(this.GetItem("radCliente").Specific));
            this.radForn = ((SAPbouiCOM.OptionBtn)(this.GetItem("radForn").Specific));
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
            try
            {
                LoadMatrix();
                LoadGrupo();

                /* Agrupar RadionButton */
                radForn.GroupWith("radCliente");

            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
            finally
            {
                mtxData.AutoResizeColumns();
            }

        }
        private void LoadMatrix()
        {
            try
            {
                UIAPIRawForm.Freeze(true);
                UIAPIRawForm.DataSources.DataTables.Item("dtOCRD").ExecuteQuery(Resources.Resource.LoadBP);
                mtxData.Columns.Item("colChecked").DataBind.Bind("dtOCRD", "Checked");
                mtxData.Columns.Item("Col_0").DataBind.Bind("dtOCRD", "CardCode");
                mtxData.Columns.Item("Col_1").DataBind.Bind("dtOCRD", "CardName");
                mtxData.Columns.Item("Col_2").DataBind.Bind("dtOCRD", "Phone1");
                mtxData.Columns.Item("Col_3").DataBind.Bind("dtOCRD", "E_Mail");
                mtxData.Columns.Item("Col_4").DataBind.Bind("dtOCRD", "Endereço");
                mtxData.Columns.Item("Col_6").DataBind.Bind("dtOCRD", "GroupName");
                mtxData.Columns.Item("Col_7").DataBind.Bind("dtOCRD", "Situacao");

                mtxData.LoadFromDataSource();
                mtxData.AutoResizeColumns();

            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
            finally
            {
                UIAPIRawForm.Freeze(false);
            }
        }
        private void LoadGrupo()
        {
            Rst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            Rst.DoQuery($"SELECT T0.GroupName, T0.GroupCode FROM OCRG T0");

            if (Rst.RecordCount > 0)
            {
                Rst.MoveFirst();
                for (int row = 0; row < Rst.RecordCount; row++)
                {
                    ComboBox0.ValidValues.Add(Rst.Fields.Item("GroupName").Value.ToString(), Rst.Fields.Item("GroupCode").Value.ToString());
                    Rst.MoveNext();

                }
            }
        }
        private void oTextFilter_ChooseFromListBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            SAPbouiCOM.ChooseFromList oCFL = this.UIAPIRawForm.ChooseFromLists.Item("OCRD");

            if (radCliente.Selected)
            {
                SAPbouiCOM.Conditions oCons = new SAPbouiCOM.Conditions();
                SAPbouiCOM.Condition oCon = oCons.Add();
                oCon.Alias = "CardType";
                oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                oCon.CondVal = "C";
                oCFL.SetConditions(oCons);
            }
            else if (radForn.Selected)
            {
                SAPbouiCOM.Conditions oCons = new SAPbouiCOM.Conditions();
                SAPbouiCOM.Condition oCon = oCons.Add();
                oCon.Alias = "CardType";
                oCon.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                oCon.CondVal = "S";
                oCFL.SetConditions(oCons);
            }

        }
        private void oTextFilter_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            SAPbouiCOM.ISBOChooseFromListEventArg oCFLEvent = null;
            oCFLEvent = (SAPbouiCOM.ISBOChooseFromListEventArg)pVal;
            SAPbouiCOM.DataTable oDataTable = oCFLEvent.SelectedObjects;

            if (oDataTable == null)
                return;

            var cardCode = oDataTable.GetValue("CardCode", 0).ToString();
            this.UIAPIRawForm.DataSources.UserDataSources.Item("udCardCode").Value = cardCode;

        }
        private void oFilter_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                this.UIAPIRawForm.Freeze(true);

                var cardCode = this.UIAPIRawForm.DataSources.UserDataSources.Item("udCardCode").Value;
                var grupoCad = this.UIAPIRawForm.DataSources.UserDataSources.Item("udGrupo").ValueEx == "Todos" ? "" : this.UIAPIRawForm.DataSources.UserDataSources.Item("udGrupo").ValueEx;
                var situacao = this.UIAPIRawForm.DataSources.UserDataSources.Item("udStatus").ValueEx == "Todas" ? "" : this.UIAPIRawForm.DataSources.UserDataSources.Item("udStatus").ValueEx;

                String query = string.Format(Resources.Resource.LoadBPFilter, cardCode, grupoCad, situacao);

                this.UIAPIRawForm.DataSources.DataTables.Item("dtOCRD").ExecuteQuery(query);
                mtxData.LoadFromDataSource();
                mtxData.AutoResizeColumns();


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
        private void Button1_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            SAPbouiCOM.DataTable oDT = this.UIAPIRawForm.DataSources.DataTables.Item("dtOCRD");
            oBP = (SAPbobsCOM.BusinessPartners)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
            SAPbouiCOM.ProgressBar oProgressBar = null;

            try
            {

                if (Application.SBO_Application.MessageBox("Os cliente(s) selecionado(s) será ativado. Deseja prosseguir ?", 1, "Sim", "Não") != 1)
                    return;

                mtxData.FlushToDataSource();

                List<String> selectedBP = new List<string>();

                for (int i = 0; i < oDT.Rows.Count; i++)
                {
                    var selected = oDT.GetValue("Checked", i).ToString();

                    if (selected == "Y")
                    {
                        selectedBP.Add(oDT.GetValue("CardCode", i).ToString());
                    }
                }

                oProgressBar = Application.SBO_Application.StatusBar.CreateProgressBar("", selectedBP.Count, false);
                foreach (var list in selectedBP)
                {
                    if (oBP.GetByKey(list))
                    {
                        oBP.Valid = SAPbobsCOM.BoYesNoEnum.tYES;
                        oBP.Frozen = SAPbobsCOM.BoYesNoEnum.tNO;
                        int lRet = oBP.Update();

                        if (lRet != 0)
                        {
                            throw new Exception($"{Program.oCompany.GetLastErrorDescription()} - Código: {oBP.CardCode}");
                        }

                        oProgressBar.Text = $"Cadastro: {oBP.CardCode}, ativado com sucesso.";
                        oProgressBar.Value++;
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, true);
            }
            finally
            {
                if (oBP != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oBP);
                }

                if (oDT != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oDT);
                }

                if (oProgressBar != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oProgressBar);
                }


            }
        }
        private void Button4_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            SAPbouiCOM.DataTable oDT = this.UIAPIRawForm.DataSources.DataTables.Item("dtOCRD");
            oBP = (SAPbobsCOM.BusinessPartners)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners);
            SAPbouiCOM.ProgressBar oProgressBar = null;

            try
            {

                if (Application.SBO_Application.MessageBox("Cadastro selecionado será desativado." + Environment.NewLine + "Deseja prosseguir ?", 1, "Sim", "Não") != 1)
                    return;

                mtxData.FlushToDataSource();

                List<String> selectedBP = new List<string>();

                for (int i = 0; i < oDT.Rows.Count; i++)
                {
                    var selected = oDT.GetValue("Checked", i).ToString();

                    if (selected == "Y")
                    {
                        selectedBP.Add(oDT.GetValue("CardCode", i).ToString());
                    }
                }

                oProgressBar = Application.SBO_Application.StatusBar.CreateProgressBar("", selectedBP.Count, false);
                foreach (var list in selectedBP)
                {
                    if (oBP.GetByKey(list))
                    {
                        oBP.Valid = SAPbobsCOM.BoYesNoEnum.tNO;
                        oBP.Frozen = SAPbobsCOM.BoYesNoEnum.tYES;
                        int lRet = oBP.Update();

                        if (lRet != 0)
                        {
                            throw new Exception($"{Program.oCompany.GetLastErrorDescription()} - Código: {oBP.CardCode}");
                        }

                        oProgressBar.Text = $"Cadastro: {oBP.CardCode}, desativado com sucesso.";
                        oProgressBar.Value++;
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Medium, true);
            }
            finally
            {
                if (oBP != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oBP);
                }

                if (oDT != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oDT);
                }

                if (oProgressBar != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oProgressBar);
                }


            }
        }
    }
}
