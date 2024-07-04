using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BOneSolucoes.Forms.ImportacaoXML.Entities
{
    public class ProtNFe
    {
        [XmlElement("infProt")]
        public InfProtNFe InfoProtocolo { get; set; }

        public class InfProtNFe
        {
            [XmlElement("chNFe")]
            public string chNFe { get; set; }
        }

    }
}
