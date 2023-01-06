using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.DB.TABLE
{
    /// <summary>
    /// 歷史信用當沖檔
    /// </summary>
    public class HCDTD
    {
        public string TDATE { get; set; }
        public string BHNO { get; set; }
        public string CRDSEQ { get; set; }
        public string CRDNO { get; set; }
        public string DBDSEQ { get; set; }
        public string DBDNO { get; set; }
        public string CSEQ { get; set; }
        public string STOCK { get; set; }
        public decimal QTY { get; set; }
        public decimal BPRICE { get; set; }
        public decimal BQTY { get; set; }
        public decimal BFEE { get; set; }
        public decimal SPRICE { get; set; }
        public decimal SQTY { get; set; }
        public decimal SFEE { get; set; }
        public decimal TAX { get; set; }
        public decimal DBFEE { get; set; }
        public decimal INCOME { get; set; }
        public decimal COST { get; set; }
        public decimal PROFIT { get; set; }
        public string SFCODE { get; set; }
        public decimal STINTAX { get; set; }
        public decimal HEALTHFEE { get; set; }
        public string TRDATE { get; set; }
        public string TRTIME { get; set; }
        public string MODDATE { get; set; }
        public string MODTIME { get; set; }
        public string MODUSER { get; set; }
    }
}
