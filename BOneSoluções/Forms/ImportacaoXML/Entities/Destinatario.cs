using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BOneSolucoes.Forms.ImportacaoXML.Entities
{
    public class Destinatario
    {
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string xNome { get; set; }
        [XmlElement("enderDest")]
        public Endereco Endereco { get; set; }
        public string email { get; set; }
    }
}
