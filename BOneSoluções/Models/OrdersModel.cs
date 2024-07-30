using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOneSolucoes.Models
{
    class OrdersModel

    {
        public String DocEntry { get; set; }
        public String CardCode { get; set; }
        public String CardName { get; set; }
        public String Comments { get; set; }        
        public int PaymentGroupCode { get; set; }
        public int SalesPersonCode { get; set; }
        public String PaymentMethod { get; set; }
        public String BPL_IDAssignedToInvoice { get; set; }
        public String SequenceSerial { get; set; }        
        public List<ItemModel> DocumentLines { get; set; }       

    }
}
