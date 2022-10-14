using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_UnrealizedGainsOrLosses
{
    public class unoffset_qtype_sum
    {
        public string stock { get; set; }
        public string stocknm { get; set; }
        public string ttype { get; set; }
        public string ttypename { get; set; }
        public string bstype { get; set; }
        public float bqty { get; set; }
        public float cost { get; set; }
        public float avgprice { get; set; }
        public float lastprice { get; set; }
        public float marketvalue { get; set; }
        public float profit { get; set; }
        public string pl_ratio { get; set; }
        public float fee { get; set; }
        public float tax { get; set; }
        public float estimateAmt { get; set; }
        public float estimateFee { get; set; }
        public float estimateTax { get; set; }
        public float amt { get; set; }
        public List<unoffset_qtype_detail> unoffset_qtype_detail { get; set; }
    }
}
