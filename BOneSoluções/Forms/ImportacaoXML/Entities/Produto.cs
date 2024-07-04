using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BOneSolucoes.Forms.ImportacaoXML.Entities
{
    public class Produto
    {
        [XmlElement("cProd")]
        public string cProd { get; set; }
        public string cEAN { get; set; }

        [XmlElement("xProd")]
        public string xProd { get; set; }
        public string NCM { get; set; }
        public string CFOP { get; set; }
        public string uCom { get; set; }
        public double qCom { get; set; }
        public double vUnCom { get; set; }
    }
}
