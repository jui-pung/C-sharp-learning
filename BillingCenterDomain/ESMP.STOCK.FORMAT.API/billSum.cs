using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.FORMAT.API
{
    public class billSum
    {
        public decimal cnbamt { get; set; }
        public decimal cnsamt { get; set; }
        public decimal cnfee { get; set; }
        public decimal cntax { get; set; }
        public decimal cnnetamt { get; set; }
        public decimal bqty { get; set; }
        public decimal sqty { get; set; }
    }
}
