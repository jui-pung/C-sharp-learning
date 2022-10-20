using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _1_UnrealizedGainsOrLosses
{
    public class profit_detail_out
    {
        public string tdate { get; set; }
        public string dseq { get; set; } 
        public string dno { get; set; } 
        public decimal mqty { get; set; }
        public decimal cqty { get; set; }
        public string mprice { get; set; }
        public string mamt { get; set; }
        public decimal cost { get; set; }
        public decimal income { get; set; }
        public decimal netamt { get; set; }
        public decimal fee { get; set; }
        public decimal tax { get; set; }
        public string ttype { get; set; } = "0";
        public string ttypename { get; set; } = "現股";
        public string bstype { get; set; } = "S";
        public string wtype { get; set; }
        public decimal profit { get; set; }
        public string pl_ratio { get; set; }
        public string ctype { get; set; } = "0";
        public string ttypename2 { get; set; }
    }
}
