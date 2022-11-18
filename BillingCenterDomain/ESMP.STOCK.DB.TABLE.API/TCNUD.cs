using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.DB.TABLE
{
    public class TCNUD
    {
        public string TDATE { get; set; }
        public string BHNO { get; set; }
        public string CSEQ { get; set; }
        public string STOCK { get; set; }
        public decimal PRICE { get; set; } = 0;
        public decimal QTY { get; set; }
        public decimal BQTY { get; set; }
        public decimal FEE { get; set; } = 0;
        public decimal COST { get; set; } = 0;
        public string DSEQ { get; set; }
        public string DNO { get; set; }
        public string ADJDATE { get; set; }
        public string WTYPE { get; set; }
        public string TRDATE { get; set; }
        public string TRTIME { get; set; }
        public string MODDATE { get; set; }
        public string MODTIME { get; set; }
        public string MODUSER { get; set; }
        public string IOFLAG { get; set; }
    }
}
