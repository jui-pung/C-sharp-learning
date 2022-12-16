using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ESMP.STOCK.FORMAT
{
    public class unoffset_qtype_accsum
    {
        public string errcode { get; set; }
        public string errmsg { get; set; }
        public decimal bqty { get; set; }
        public decimal real_qty { get; set; }
        public decimal cost { get; set; }
        public decimal marketvalue { get; set; }
        public decimal profit { get; set; }
        public string pl_ratio { get; set; }
        public decimal fee { get; set; }
        public decimal tax { get; set; }
        public decimal estimateAmt { get; set; }
        public decimal estimateFee { get; set; }
        public decimal estimateTax { get; set; }
        public decimal cramt { get; set; }
        public decimal bcramt { get; set; }
        public decimal gtamt { get; set; }
        public decimal bgtamt { get; set; }
        public decimal dnamt { get; set; }
        public decimal bdnamt { get; set; }
        public decimal interest { get; set; }
        public decimal dbfee { get; set; }
        [XmlElement("unoffset_qtype_sum")]
        [JsonProperty("unoffset_qtype_sum", NullValueHandling = NullValueHandling.Ignore)]
        public List<unoffset_qtype_sum> unoffset_qtype_sum { get; set; }
    }
}
