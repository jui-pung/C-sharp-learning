using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ESMP.STOCK.FORMAT
{
    public class unoffset_qtype_sum
    {
        public string stock { get; set; }
        public string stocknm { get; set; }
        public string ttype { get; set; } = "0";
        public string ttypename { get; set; }
        public string bstype { get; set; }
        public decimal bqty { get; set; }
        public decimal real_qty { get; set; }
        public decimal cost { get; set; }
        public decimal avgprice { get; set; }
        public decimal lastprice { get; set; }
        public decimal marketvalue { get; set; }
        public decimal profit { get; set; }
        public string pl_ratio { get; set; }
        public decimal fee { get; set; }
        public decimal tax { get; set; }
        public decimal estimateAmt { get; set; }
        public decimal estimateFee { get; set; }
        public decimal estimateTax { get; set; }
        public decimal amt { get; set; }
        [XmlElement("unoffset_qtype_detail")]
        [JsonProperty("unoffset_qtype_detail", NullValueHandling = NullValueHandling.Ignore)]

        public List<unoffset_qtype_detail> unoffset_qtype_detail { get; set; }
    }
}
