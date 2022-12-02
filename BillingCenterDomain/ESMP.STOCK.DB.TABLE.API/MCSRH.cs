using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.DB.TABLE
{
    public class MCSRH
    {
        public string BHNO { get; set; }
        public string CSEQ { get; set; }
        public string STOCK { get; set; }
        public decimal CNQBAL { get; set; }
        public decimal CRAQTY { get; set; }
        public decimal CROQTY { get; set; }
        public decimal DBAQTY { get; set; }
        public decimal DBOQTY { get; set; }
        public string CLDATE { get; set; }
        public string TRDATE { get; set; }
        public string TRTIME { get; set; }
        public string MODDATE { get; set; }
        public string MODTIME { get; set; }
        public string MODUSER { get; set; }
        public decimal CNQBAL_DE { get; set; }
    }
}
