using BOneSolucoes.Comonn;
using BOneSolucoes.Models;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOneSolucoes.Entities
{
    class Invoice
    {
        public static void AddInvoice(int pedido)
        {
            SAPbobsCOM.Recordset oRst = (SAPbobsCOM.Recordset)Program.oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

            try
            {
                string queryPed = String.Format(Resources.Resource.PedidosFaturar,pedido);

                oRst.DoQuery(queryPed);
                if (oRst.RecordCount > 0)
                {
                    oRst.MoveFirst();

                    InvoiceModel invoice = new InvoiceModel();

                    invoice.CardCode = oRst.Fields.Item("CardCode").Value.ToString();
                    invoice.CardName = oRst.Fields.Item("CardName").Value.ToString();
                    invoice.BPL_IDAssignedToInvoice = oRst.Fields.Item("BPLId").Value.ToString();
                    invoice.Comments = oRst.Fields.Item("Comments").Value.ToString();
                    invoice.PaymentGroupCode = Convert.ToInt32(oRst.Fields.Item("GroupNum").Value);
                    invoice.PaymentMethod = oRst.Fields.Item("PeyMethod").Value.ToString();
                    invoice.SalesPersonCode = Convert.ToInt32(oRst.Fields.Item("SlpCode").Value);


                    invoice.DocumentLines = new List<ItemModel>();

                    for (int i = 0; i < oRst.RecordCount; i++)
                    {                        
                        ItemModel item = new ItemModel();

                        item.ItemCode = oRst.Fields.Item("ItemCode").Value.ToString();
                        item.Quantity = Convert.ToDouble(oRst.Fields.Item("Quantity").Value);
                        item.Price = Convert.ToDouble(oRst.Fields.Item("Price").Value);
                        item.Usage = Convert.ToInt32(oRst.Fields.Item("Usage").Value);
                        item.BaseType = Convert.ToInt32(oRst.Fields.Item("ObjType").Value);
                        item.BaseEntry = Convert.ToInt32(oRst.Fields.Item("DocEntry").Value);
                        item.BaseLine = Convert.ToInt32(oRst.Fields.Item("LineNum").Value);
                        invoice.DocumentLines.Add(item);
                        oRst.MoveNext();
                        
                        
                    }

                    SAPCommon.AddInvoice(invoice);

                }


            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message,SAPbouiCOM.BoMessageTime.bmt_Short,SAPbouiCOM.BoStatusBarMessageType.smt_Error);
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
}
