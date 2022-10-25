using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ESMP.STOCK.FORMAT.API
{
    public class profit_sum
    {
        public string bhno { get; set; }
        public string cseq { get; set; }
        public string tdate { get; set; }
        public string dseq { get; set; }
        public string dno { get; set; }
        public string ttype { get; set; } = "0";
        public string ttypename { get; set; } = "現股";
        public string bstype { get; set; } = "S";
        public string stock { get; set; }
        public string stocknm { get; set; }
        public decimal cqty { get; set; }
        public string mprice { get; set; }
        public decimal fee { get; set; }
        public decimal tax { get; set; }
        public decimal cost { get; set; }
        public decimal income { get; set; }
        public decimal profit { get; set; }
        public string pl_ratio { get; set; }
        public string ctype { get; set; } = "0";
        public string ttypename2 { get; set; }
        [XmlElement("profit_detail")]
        public List<profit_detail> profit_detail { get; set; }
        [XmlElement("profit_detail_out")]
        public List<profit_detail_out> profit_detail_out { get; set; }
    }
}
