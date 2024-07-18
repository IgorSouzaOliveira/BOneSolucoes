using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOneSolucoes.Models
{
    class ItemModelOrders
    {
        public String LineNum { get; set; }
        public String ItemCode { get; set; }
        public double Quantity { get; set; }
        public double Price { get; set; }
        public int Usage { get; set; }
        public List<BatchNumbersOrdersModel> BatchNumbers { get; set; }
        
    }
}
