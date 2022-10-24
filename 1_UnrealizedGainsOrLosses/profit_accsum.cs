using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _1_UnrealizedGainsOrLosses
{
    public class profit_accsum
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public decimal cqty { get; set; }
        public decimal cost { get; set; }
        public decimal income { get; set; }
        public decimal profit { get; set; }
        public string pl_ratio { get; set; }
        public decimal fee { get; set; }
        public decimal tax { get; set; }
        [XmlElement("profit_sum")]
        public List<profit_sum> profit_sum { get; set; }
    }
}
