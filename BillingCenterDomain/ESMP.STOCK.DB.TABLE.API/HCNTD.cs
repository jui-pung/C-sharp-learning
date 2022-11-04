using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.DB.TABLE
{
    public class HCNTD
    {
        public string BHNO { get; set; }
        public string TDATE { get; set; }
        public string CSEQ { get; set; }
        public string BDSEQ { get; set; }
        public string BDNO { get; set; }
        public string SDSEQ { get; set; }
        public string SDNO { get; set; }
        public string STOCK { get; set; }
        public decimal CQTY { get; set; }
        public decimal BPRICE { get; set; }
        public decimal BFEE { get; set; }
        public decimal SPRICE { get; set; }
        public decimal SFEE { get; set; }
        public decimal TAX { get; set; }
        public decimal INCOME { get; set; }
        public decimal COST { get; set; }
        public decimal PROFIT { get; set; }
        public decimal BQTY { get; set; }
        public decimal SQTY { get; set; }
        public string TRDATE { get; set; }
        public string TRTIME { get; set; }
        public string MODDATE { get; set; }
        public string MODTIME { get; set; }
        public string MODUSER { get; set; }
    }
}
