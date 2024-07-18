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
            try
            {

                var dataOrder = SAPCommon.GetOrders(pedido.ToString());

                InvoiceModel invoice = new InvoiceModel();

                invoice.CardCode = dataOrder.CardCode;
                invoice.CardName = dataOrder.CardName;
                invoice.BPL_IDAssignedToInvoice = dataOrder.BPL_IDAssignedToInvoice;
                invoice.Comments = dataOrder.Comments;
                invoice.PaymentGroupCode = dataOrder.PaymentGroupCode;
                invoice.PaymentMethod = dataOrder.PaymentMethod;
                invoice.SalesPersonCode = dataOrder.SalesPersonCode;
                
                
                invoice.DocumentLines = new List<ItemModelInvoice>();
                


                foreach (var docLine in dataOrder.DocumentLines)
                {
                    ItemModelInvoice item = new ItemModelInvoice();
                    item.BatchNumbers = new List<BatchNumbersInvoiceModel>();
                    

                    item.ItemCode = docLine.ItemCode;
                    item.Quantity = docLine.Quantity;
                    item.Price = docLine.Price;
                    item.Usage = docLine.Usage;
                    item.BaseType = "17";
                    item.BaseEntry = dataOrder.DocEntry;
                    item.BaseLine = docLine.LineNum;

                    foreach (var lotePed in docLine.BatchNumbers)
                    {
                        BatchNumbersInvoiceModel batchNumbers = new BatchNumbersInvoiceModel();

                        batchNumbers.BatchNumber = lotePed.BatchNumber;
                        batchNumbers.AddmisionDate = lotePed.AddmisionDate;
                        batchNumbers.Quantity = lotePed.Quantity;
                        batchNumbers.ItemCode = lotePed.ItemCode;
                        item.BatchNumbers.Add(batchNumbers);
                    }               

                    invoice.DocumentLines.Add(item);

                }

                SAPCommon.AddInvoice(invoice);
            }
            catch (Exception ex)
            {
                Application.SBO_Application.StatusBar.SetText(ex.Message, SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Error);
            }
        }
    }
}
