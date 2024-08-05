using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOneSolucoes.Models
{
    class InvoiceModel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String DocEntry { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String SequenceSerial { get; set; }
        public String CardCode { get; set; }
        public String CardName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String BPL_IDAssignedToInvoice { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String Comments { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int PaymentGroupCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String PaymentMethod { get; set; }
        public int SalesPersonCode { get; set; }
        public List<ItemModel> DocumentLines { get; set; } = new List<ItemModel>();
    }
}
