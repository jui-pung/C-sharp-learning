using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.FORMAT.API
{
    public class profile
    {
        public string bhno { get; set; }
        public string cseq { get; set; }
        public string name { get; set; } = " ";
        public string stock { get; set; }
        public string stocknm { get; set; }
        public string mdate { get; set; }
        public string dseq { get; set; }
        public string dno { get; set; }
        public string ttype { get; set; }
        public string ttypename { get; set; }
        public string bstype { get; set; }
        public string bstypename { get; set; }
        public string etype { get; set; }
        public decimal mprice { get; set; }
        public decimal mqty { get; set; }
        public decimal mamt { get; set; }
        public decimal fee { get; set; }
        public decimal tax { get; set; }
        public decimal netamt { get; set; }
    }
}
