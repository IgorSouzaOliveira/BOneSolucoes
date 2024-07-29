using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOneSolucoes.Models
{
    class BusinessPartnerModel
    {
        public String CardCode { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String Valid { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public String Frozen { get; set; }
    }
}
