using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOneSoluções.Entities
{
    class Invoice
    {
        public static string AddInvoice(int pedido)
        {
            SAPbobsCOM.Documents oOrder = (SAPbobsCOM.Documents)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders);
            SAPbobsCOM.Documents oInvoice = (SAPbobsCOM.Documents)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices);

            try
            {
                if (oOrder.GetByKey(pedido))
                {

                    /*Cabeçalho*/
                    oInvoice.CardCode = oOrder.CardCode;
                    oInvoice.CardName = oOrder.CardName;
                    oInvoice.PaymentGroupCode = oOrder.PaymentGroupCode;
                    oInvoice.PaymentMethod = oOrder.PaymentMethod;
                    oInvoice.BPL_IDAssignedToInvoice = oOrder.BPL_IDAssignedToInvoice;
                    oInvoice.Comments = $"{oOrder.Comments} - Nota Fiscal gerada através do pedido: {oOrder.DocEntry}.";

                    /*Linha*/
                    for (int i = 0; i < oOrder.Lines.Count; i++)
                    {
                        oOrder.Lines.SetCurrentLine(i);

                        oInvoice.Lines.ItemCode = oOrder.Lines.ItemCode;
                        oInvoice.Lines.BaseEntry = oOrder.DocEntry;
                        oInvoice.Lines.BaseLine = oOrder.Lines.LineNum;
                        oInvoice.Lines.BaseType = (int)SAPbobsCOM.BoObjectTypes.oOrders;

                        /*Itens administrado por lote */
                        oInvoice.Lines.BatchNumbers.ItemCode = oOrder.Lines.BatchNumbers.ItemCode;
                        oInvoice.Lines.BatchNumbers.BatchNumber = oOrder.Lines.BatchNumbers.BatchNumber;
                        oInvoice.Lines.BatchNumbers.Quantity = oOrder.Lines.BatchNumbers.Quantity;
                        oInvoice.Lines.BatchNumbers.BaseLineNumber = oOrder.Lines.BatchNumbers.BaseLineNumber;

                        oInvoice.Lines.Add();
                    }

                    oInvoice.Lines.Delete();
                    int lRet = oInvoice.Add();

                    if (lRet != 0)
                    {
                        throw new Exception($"{Program.oCompany.GetLastErrorDescription()} - Pedido Nº: {oOrder.DocEntry}");
                    }

                }

                var invoiceDoc = Program.oCompany.GetNewObjectKey();

                return $"Nota Fiscal: {invoiceDoc}, adicionada com sucesso.";
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
                return "";
            }
            finally
            {
                if (oOrder != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oOrder);
                }
                if (oInvoice != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oInvoice);
                }
            }
        }
    }
}
