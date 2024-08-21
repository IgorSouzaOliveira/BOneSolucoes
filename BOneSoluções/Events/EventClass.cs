using SAPbouiCOM.Framework;
using System;
using System.Xml;

namespace BOneSolucoes.Entities
{
    class EventClass
    {
        public EventClass()
        {

        }
        public static void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            SAPbouiCOM.Form oForm = null;
            SAPbouiCOM.Button oButton;

            if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_CLOSE)
                return;

            try
            {

                if (pVal.BeforeAction && pVal.FormType == 139 && pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_LOAD)
                {

                    oForm = Application.SBO_Application.Forms.GetForm(pVal.FormType.ToString(), pVal.FormTypeCount);

                    SAPbouiCOM.Item oItem = oForm.Items.Item("2");  /// Existing Item on the form

                    SAPbouiCOM.Item oItem1 = oForm.Items.Add("btnExec", SAPbouiCOM.BoFormItemTypes.it_BUTTON);

                    oItem1.Top = oItem.Top;

                    oItem1.Left = oItem.Left + 70;

                    oItem1.Width = oItem.Width + 30;

                    oItem1.Height = oItem.Height;

                    oItem1.Enabled = true;

                    oButton = (SAPbouiCOM.Button)oItem1.Specific;

                    oButton.Caption = "Custo do momento.";

                    Application.SBO_Application.ItemEvent += EventoClick;


                }


            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.Message, 1, "OK", "Cancelar");
            }
        }
        private static void EventoClick(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                SAPbobsCOM.Recordset oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED && pVal.ItemUID == "btnExec" && pVal.FormType == 139 && pVal.BeforeAction == true)
                {

                    SAPbouiCOM.Form oForm = Application.SBO_Application.Forms.GetForm("139", pVal.FormTypeCount);
                    SAPbouiCOM.Item oStatus = oForm.Items.Item("81");
                    SAPbouiCOM.Item oCondPagamento = oForm.Items.Item("47");

                    if (((SAPbouiCOM.ComboBox)oStatus.Specific).Value == "3")
                        throw new Exception("Erro: Documento com status Fechado. Atualização não disponivel.");

                    String cardCode = ((SAPbouiCOM.EditText)oForm.Items.Item("4").Specific).String;
                    SAPbouiCOM.Matrix oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item("38").Specific;


                    String usage = string.Empty;

                    for (int i = 1; i <= oMatrix.RowCount; i++)
                    {
                        usage = ((SAPbouiCOM.ComboBox)oMatrix.Columns.Item("2011").Cells.Item(i).Specific).Value;
                        oRst.DoQuery($"SELECT T0.Price FROM ITM1 T0 WHERE T0.PriceList = 2 AND T0.ItemCode = '{((SAPbouiCOM.EditText)oMatrix.Columns.Item("1").Cells.Item(i).Specific).Value}'");

                        if (usage == "10")
                        {
                            ((SAPbouiCOM.EditText)oMatrix.Columns.Item("14").Cells.Item(i).Specific).Value = oRst.Fields.Item("Price").Value.ToString();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message,SAPbouiCOM.BoMessageTime.bmt_Short,SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                BubbleEvent = false;
            }
        }
        public static void GetDocEntryPed(ref SAPbouiCOM.BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            GC.Collect();



            if (pVal.ActionSuccess && pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD)
            {
                SAPbobsCOM.Recordset oRst = null;

                try
                {

                    oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    oRst.DoQuery("SELECT 'TRUE' FROM [@BONECONFMAIN] T0 WHERE T0.U_BOne_AtivoAprov = 'Y'");

                    if (oRst.RecordCount > 0)
                    {

                        oRst.DoQuery($@"SELECT T0.""U_BONE_Query"",""U_BOne_CodeEtapa"",T0.""U_BOne_EtapaAut"", T0.""U_BOne_NomeConsulta"" FROM [@BONMODAPROV] T0 WHERE T0.""U_BOne_Ativo"" = 'Y' AND T0.""U_BONE_ObjectType"" = {pVal.Type}");
                        if (oRst.RecordCount > 0)
                        {
                            oRst.MoveFirst();
                            for (int i = 0; i < oRst.RecordCount; i++)
                            {
                                var query = oRst.Fields.Item("U_BONE_Query").Value.ToString();
                                int codeEtapa = (int)oRst.Fields.Item("U_BOne_CodeEtapa").Value;
                                var nameEtapa = oRst.Fields.Item("U_BOne_EtapaAut").Value.ToString();
                                var modeloAut = oRst.Fields.Item("U_BOne_NomeConsulta").Value.ToString();

                                ModelAprov(pVal.Type, pVal.ObjectKey, query, codeEtapa, nameEtapa, modeloAut);

                                oRst.MoveNext();
                            }

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

        /* Metodo para modificar o status do Documento*/
        public static void ModelAprov(string pvalType, string objectKey, string query, int codeEtapa, string nameEtapa, string modeloAut)
        {
            SAPbobsCOM.Documents oDoc = null;
            SAPbobsCOM.Recordset oRst = null;

            try
            {
                String docEntry = null;
                string xml = $@"{objectKey}";
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);

                if (xml == null)
                    return;

                XmlNodeList xnList = document.GetElementsByTagName("DocEntry");

                if (xnList.Count > 0)
                {
                    docEntry = xnList[0].InnerText;
                }

                oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRst.DoQuery(query.ToString().Replace("@DocEntry", docEntry));

                if (oRst.RecordCount > 0)
                {
                    oRst.MoveFirst();
                    for (int i = 0; i < oRst.RecordCount; i++)
                    {
                        switch (pvalType)
                        {
                            case "17":
                                oDoc = (SAPbobsCOM.Documents)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
                                break;

                            case "22":
                                oDoc = (SAPbobsCOM.Documents)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);
                                break;

                            case "540000006":
                                oDoc = (SAPbobsCOM.Documents)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseQuotations);
                                break;
                        }


                        if (oDoc.GetByKey(Convert.ToInt32(docEntry)))
                        {
                            oDoc.Confirmed = SAPbobsCOM.BoYesNoEnum.tNO;
                            int lRet = oDoc.Update();

                            if (lRet != 0)
                            {
                                throw new Exception(Program.oCompany.GetLastErrorDescription());
                            }

                            InsertTableAprov(oDoc.DocDate, pvalType, docEntry, oDoc.CardCode, oDoc.CardName, oDoc.BPL_IDAssignedToInvoice, oDoc.BPLName, oDoc.SalesPersonCode, oDoc.UserSign,
                                oDoc.PaymentGroupCode, oDoc.PaymentMethod, oDoc.DocTotal, codeEtapa, nameEtapa, modeloAut, query, "FALSE", 0); ;
                        }

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
                if (oDoc != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oDoc);
                }
                if (oRst != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRst);
                }

                GC.Collect();
            }
        }

        /* Metodo para adicionar na tabela de aprovação*/
        public static void InsertTableAprov(DateTime docDate, string tipoDoc, string numDoc, string cardCode, string cardName, int bplID, string bplName, int salesPersonCode, int userSign, int paymentCode, string paymentMethod, double docTotal, int codigoEtapa, string nomeEtapa, string modeloAut, string queryAut, string autorizado, int processado)
        {
            SAPbobsCOM.UserTable oTable = Program.oCompany.UserTables.Item("BONEAPROV");

            try
            {
                oTable.UserFields.Fields.Item("U_BOneDocDate").Value = docDate;
                oTable.UserFields.Fields.Item("U_BOneTipoDoc").Value = tipoDoc;
                oTable.UserFields.Fields.Item("U_BOneNumDoc").Value = numDoc;
                oTable.UserFields.Fields.Item("U_BOneCardCode").Value = cardCode;
                oTable.UserFields.Fields.Item("U_BOneCardName").Value = cardName;
                oTable.UserFields.Fields.Item("U_BOneBplID").Value = bplID;
                oTable.UserFields.Fields.Item("U_BOneBplName").Value = bplName;
                oTable.UserFields.Fields.Item("U_BOneSlpCode").Value = salesPersonCode;
                oTable.UserFields.Fields.Item("U_BOneUserSign").Value = userSign;
                oTable.UserFields.Fields.Item("U_BOnePaymentCode").Value = paymentCode;
                oTable.UserFields.Fields.Item("U_BOnePaymentMethod").Value = paymentMethod;
                oTable.UserFields.Fields.Item("U_BOneDocTotal").Value = docTotal;
                oTable.UserFields.Fields.Item("U_BOneCodEtapa").Value = codigoEtapa;
                oTable.UserFields.Fields.Item("U_BOneNameEtapa").Value = nomeEtapa;
                oTable.UserFields.Fields.Item("U_BOneModeloAut").Value = modeloAut;
                oTable.UserFields.Fields.Item("U_BOneQueryAut").Value = queryAut;
                oTable.UserFields.Fields.Item("U_BOneAutorizado").Value = autorizado;
                oTable.UserFields.Fields.Item("U_BOneProcessado").Value = processado;

                int lRet = oTable.Add();

                if (lRet != 0)
                {
                    throw new Exception(Program.oCompany.GetLastErrorDescription());
                }

            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
            finally
            {
                if (oTable != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oTable);
                }

                GC.Collect();
            }
        }

        private void MetodoAntigo()
        {
            //SAPbouiCOM.Form oForm = null;
            //SAPbouiCOM.IForm IForm = null;
            //SAPbouiCOM.Matrix oMatrix = null;


            //GC.Collect();

            //SAPbobsCOM.Recordset oRst = null;
            //oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);




            //if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_CLOSE)
            //{
            //    return;
            //}

            //if (pVal.BeforeAction && pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED && (pVal.FormMode == 3 || pVal.FormMode == 2))
            //{
            //    IForm = Application.SBO_Application.Forms.ActiveForm;

            //    try
            //    {
            //        oRst.DoQuery($@"SELECT T0.""Code"",T0.""U_IDForm"",T0.""U_TipoValidacao"",T0.""U_IDCampo"",T0.""U_Campo"",T0.""U_TipoCampo"",T0.""U_Msg"",T0.""U_Ativo"", T0.""U_Obs"" FROM [@SOCONFMAIN] T0 WHERE T0.""U_Ativo"" = 'Y' AND T0.""U_IDForm"" = {IForm.Type}");

            //        if (oRst.RecordCount > 0)
            //        {
            //            oRst.MoveFirst();
            //            for (int i = 0; i < oRst.RecordCount; i++)
            //            {
            //                var idForm = oRst.Fields.Item("U_IDForm").Value.ToString();
            //                var tipoValid = oRst.Fields.Item("U_TipoValidacao").Value.ToString();
            //                var idCampo = oRst.Fields.Item("U_IDCampo").Value.ToString();
            //                var campo = oRst.Fields.Item("U_Campo").Value.ToString();
            //                var tipoCampo = oRst.Fields.Item("U_TipoCampo").Value.ToString();
            //                var msg = oRst.Fields.Item("U_Msg").Value.ToString();
            //                var ativo = oRst.Fields.Item("U_Ativo").Value.ToString();
            //                oForm = Application.SBO_Application.Forms.GetFormByTypeAndCount(Convert.ToInt32(idForm), pVal.FormTypeCount);

            //                oRst.MoveNext();

            //                fieldValid = campo;
            //                switch (tipoValid)
            //                {
            //                    case "1":
            //                        switch (tipoCampo)
            //                        {
            //                            case "EditText":
            //                                fieldValid = ((SAPbouiCOM.EditText)oForm.Items.Item(idCampo).Specific).String;
            //                                break;

            //                            case "ComboBox":
            //                                fieldValid = ((SAPbouiCOM.ComboBox)oForm.Items.Item(idCampo).Specific).Value;
            //                                break;

            //                            case "CheckBox":
            //                                fieldValid = ((SAPbouiCOM.CheckBox)oForm.Items.Item(idCampo).Specific).Caption;
            //                                break;
            //                        }
            //                        break;

            //                    case "2":
            //                        oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(idCampo).Specific;
            //                        fieldValid = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("U_LoteFat").Cells.Item(1).Specific).Value;
            //                        break;
            //                }

            //                if (string.IsNullOrWhiteSpace(fieldValid))
            //                {
            //                    Application.SBO_Application.SetStatusBarMessage($"{msg}", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            //                    BubbleEvent = false;
            //                }
            //            }

            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
            //    }
            //    finally
            //    {
            //        if (oRst != null)
            //        {
            //            System.Runtime.InteropServices.Marshal.ReleaseComObject(oRst);
            //        }

            //        if (IForm != null)
            //        {
            //            System.Runtime.InteropServices.Marshal.ReleaseComObject(IForm);
            //        }
            //        if (oForm != null)
            //        {
            //            System.Runtime.InteropServices.Marshal.ReleaseComObject(oForm);
            //        }

            //        if (oMatrix != null)
            //        {
            //            System.Runtime.InteropServices.Marshal.ReleaseComObject(oMatrix);
            //        }

            //        GC.Collect();
            //    }
            //}
        }
    }
}
