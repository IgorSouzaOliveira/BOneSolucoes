using SAPbouiCOM.Framework;
using System;
using System.Xml;

namespace BOneSoluções.Entities
{
    class EventClass
    {

        private static String fieldValid;

        public EventClass()
        {
           
        }
        public static void SBO_Application_ItemEvent(string FormUID, ref SAPbouiCOM.ItemEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            SAPbouiCOM.Form oForm = null;
            SAPbouiCOM.IForm IForm = null;
            SAPbouiCOM.Matrix oMatrix = null;


            GC.Collect();

            SAPbobsCOM.Recordset oRst = null;
            oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);



            if (pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_CLOSE)
            {
                return;
            }

            if (pVal.BeforeAction && pVal.EventType == SAPbouiCOM.BoEventTypes.et_ITEM_PRESSED && (pVal.FormMode == 3 || pVal.FormMode == 2))
            {
                IForm = Application.SBO_Application.Forms.ActiveForm;

                try
                {
                    oRst.DoQuery($@"SELECT T0.""Code"",T0.""U_IDForm"",T0.""U_TipoValidacao"",T0.""U_IDCampo"",T0.""U_Campo"",T0.""U_TipoCampo"",T0.""U_Msg"",T0.""U_Ativo"", T0.""U_Obs"" FROM [@SOCONFMAIN] T0 WHERE T0.""U_Ativo"" = 'Y' AND T0.""U_IDForm"" = {IForm.Type}");

                    if (oRst.RecordCount > 0)
                    {
                        oRst.MoveFirst();
                        for (int i = 0; i < oRst.RecordCount; i++)
                        {
                            var idForm = oRst.Fields.Item("U_IDForm").Value.ToString();
                            var tipoValid = oRst.Fields.Item("U_TipoValidacao").Value.ToString();
                            var idCampo = oRst.Fields.Item("U_IDCampo").Value.ToString();
                            var campo = oRst.Fields.Item("U_Campo").Value.ToString();
                            var tipoCampo = oRst.Fields.Item("U_TipoCampo").Value.ToString();
                            var msg = oRst.Fields.Item("U_Msg").Value.ToString();
                            var ativo = oRst.Fields.Item("U_Ativo").Value.ToString();
                            oForm = Application.SBO_Application.Forms.GetFormByTypeAndCount(Convert.ToInt32(idForm), pVal.FormTypeCount);

                            oRst.MoveNext();

                            fieldValid = campo;
                            switch (tipoValid)
                            {
                                case "1":
                                    switch (tipoCampo)
                                    {
                                        case "EditText":
                                            fieldValid = ((SAPbouiCOM.EditText)oForm.Items.Item(idCampo).Specific).String;
                                            break;

                                        case "ComboBox":
                                            fieldValid = ((SAPbouiCOM.ComboBox)oForm.Items.Item(idCampo).Specific).Value;
                                            break;

                                        case "CheckBox":
                                            fieldValid = ((SAPbouiCOM.CheckBox)oForm.Items.Item(idCampo).Specific).Caption;
                                            break;
                                    }
                                    break;

                                case "2":
                                    oMatrix = (SAPbouiCOM.Matrix)oForm.Items.Item(idCampo).Specific;
                                    fieldValid = ((SAPbouiCOM.EditText)oMatrix.Columns.Item("U_LoteFat").Cells.Item(1).Specific).Value;
                                    break;
                            }

                            if (string.IsNullOrWhiteSpace(fieldValid))
                            {
                                Application.SBO_Application.SetStatusBarMessage($"{msg}", SAPbouiCOM.BoMessageTime.bmt_Short, true);
                                BubbleEvent = false;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    Application.SBO_Application.SetStatusBarMessage(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, true);
                }
                finally
                {
                    if (oRst != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oRst);
                    }

                    if (IForm != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(IForm);
                    }
                    if (oForm != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oForm);
                    }

                    if (oMatrix != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(oMatrix);
                    }

                    GC.Collect();
                }
            }
        }

        public static void GetDocEntryPed(ref SAPbouiCOM.BusinessObjectInfo pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            GC.Collect();

            SAPbobsCOM.Documents oPed = null;
            oPed = (SAPbobsCOM.Documents)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseOrders);

            SAPbobsCOM.Recordset oRst = null;
            oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            try
            {
                if (pVal.ActionSuccess && pVal.EventType == SAPbouiCOM.BoEventTypes.et_FORM_DATA_ADD && pVal.Type == "22")
                {
                    String docEntry = null;
                    string xml = $@"{pVal.ObjectKey}";
                    XmlDocument document = new XmlDocument();
                    document.LoadXml(xml);

                    if (xml == null)
                    {
                        return;
                    }

                    XmlNodeList xnList = document.GetElementsByTagName("DocEntry");

                    if (xnList.Count > 0)
                    {
                        docEntry = xnList[0].InnerText;
                    }

                    if (oPed.GetByKey(Convert.ToInt32(docEntry)))
                    {
                        oRst.DoQuery($@"SELECT 'TRUE' FROM OPOR T0 WHERE T0.DocEntry = {docEntry} AND T0.CardCode = 'FS000919'");

                        if (oRst.RecordCount > 0)
                        {
                            oPed.Confirmed = SAPbobsCOM.BoYesNoEnum.tNO;

                            Int32 lRet = oPed.Update();
                            if (lRet != 0)
                            {
                                throw new Exception(Program.oCompany.GetLastErrorDescription());
                            }
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
                if (pVal != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(pVal);
                }

                if (oPed != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oPed);
                }

                if (oRst != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oRst);
                }

                GC.Collect();
            }
        }


    }
}
