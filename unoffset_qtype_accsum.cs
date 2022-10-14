using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_UnrealizedGainsOrLosses
{
    public class unoffset_qtype_accsum
    {
        public float bqty { get; set; }
        public float real_qty { get; set; }
        public float cost { get; set; }
        public float marketvalue { get; set; }
        public float profit { get; set; }
        public string pl_ratio { get; set; }
        public float fee { get; set; }
        public float tax { get; set; }
        public float estimateAmt { get; set; }
        public float estimateFee { get; set; }
        public float estimateTax { get; set; }
        public List<unoffset_qtype_sum> unoffset_qtype_sum { get; set; }
    }
}
