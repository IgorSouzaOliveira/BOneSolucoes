using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOneSolucoes.Models
{
    class BatchNumbersModel
    {
        public String BatchNumber { get; set; }
        public DateTime AddmisionDate { get; set; }
        public double Quantity { get; set; }
        public String BaseLineNumber { get; set; }
        public String ItemCode { get; set; }
        public int SystemSerialNumber { get; set; }
    }
}
