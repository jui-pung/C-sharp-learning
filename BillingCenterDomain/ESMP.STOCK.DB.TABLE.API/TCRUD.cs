using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.DB.TABLE
{
    public class TCRUD
    {
        public string TDATE { get; set; }
        public string BHNO { get; set; }
        public string DSEQ { get; set; }
        public decimal PRICE { get; set; }
        public string DNO { get; set; }
        public string CSEQ { get; set; }
        public string STOCK { get; set; }
        public decimal QTY { get; set; }
        public decimal CRAMT { get; set; }
        public decimal PAMT { get; set; }
        public decimal BQTY { get; set; }
        public decimal BCRAMT { get; set; }
        public decimal CRINT { get; set; }
        public string SFCODE { get; set; }
        public decimal CRRATIO { get; set; }
        public decimal ASFAMT { get; set; }
        public decimal FEE { get; set; }
        public decimal COST { get; set; }
        public string TRDATE { get; set; }
        public string TRTIME { get; set; }
        public string MODDATE { get; set; }
        public string MODTIME { get; set; }
        public string MODUSER { get; set; }
    }
}
