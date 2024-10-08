﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BOneSolucoes.Forms.ImportacaoXML.Entities
{
    public class Emitente
    {
        public string CNPJ { get; set; }
        public string xNome { get; set; }
        public string xFant { get; set; }

        [XmlElement("enderEmit")]
        public Endereco Endereco { get; set; }
        public string IE { get; set; }
        public string IEST { get; set; }
        public int CRT { get; set; }
    }
}
