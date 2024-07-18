using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOneSolucoes.Models
{
    class ItemModelInvoice
    {
        public String ItemCode { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public int Usage { get; set; }
        public String BaseType { get; set; }
        public String BaseEntry { get; set; }
        public String BaseLine { get; set; }
        public List<BatchNumbersInvoiceModel> BatchNumbers { get; set; }
       
    }
}
