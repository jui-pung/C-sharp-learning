using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.DB.TABLE
{
    public class TDBUD
    {
        public string TDATE { get; set; }
        public string BHNO { get; set; }
        public string DSEQ { get; set; }
        public string DNO { get; set; }
        public string CSEQ { get; set; }
        public string STOCK { get; set; }
        public decimal PRICE { get; set; }
        public decimal QTY { get; set; }
        public decimal DBAMT { get; set; }
        public decimal GTAMT { get; set; }
        public decimal DNAMT { get; set; }
        public decimal BQTY { get; set; }
        public decimal BDBAMT { get; set; }
        public decimal BGTAMT { get; set; }
        public decimal BDNAMT { get; set; }
        public decimal GTINT { get; set; }
        public decimal DNINT { get; set; }
        public decimal DLFEE { get; set; }
        public decimal DLINT { get; set; }
        public decimal DBRATIO { get; set; }
        public string SFCODE { get; set; }
        public decimal AGTAMT { get; set; }
        public decimal FEE { get; set; }
        public decimal TAX { get; set; }
        public decimal DBFEE { get; set; }
        public decimal COST { get; set; }
        public decimal STINTAX { get; set; }
        public decimal HEALTHFEE { get; set; }
        public string TRDATE { get; set; }
        public string TRTIME { get; set; }
        public string MODDATE { get; set; }
        public string MODTIME { get; set; }
        public string MODUSER { get; set; }
    }
}
