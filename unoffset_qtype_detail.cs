using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _1_UnrealizedGainsOrLosses
{
    public class unoffset_qtype_detail
    {
        [XmlIgnore]
        public string stock { get; set; }
        public string tdate { get; set; }
        public string ttype { get; set; } = "0";
        public string ttypename { get; set; } = "現買";
        public string bstype { get; set; } = "B";
        public string dseq { get; set; }
        public string dno { get; set; }
        public decimal bqty { get; set; }
        public decimal mprice { get; set; }
        public decimal mamt { get; set; }
        public decimal lastprice { get; set; }
        public decimal marketvalue { get; set; }
        public decimal fee { get; set; }
        public decimal tax { get; set; } = 0;
        public decimal cost { get; set; }
        public decimal estimateAmt { get; set; }
        public decimal estimateFee { get; set; }
        public decimal estimateTax { get; set; }
        public decimal profit { get; set; }
        public string pl_ratio { get; set; }

    }
}
