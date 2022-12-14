using Newtonsoft.Json;
using System.Xml.Serialization;
using JsonIgnoreAttribute = Newtonsoft.Json.JsonIgnoreAttribute;

namespace ESMP.STOCK.FORMAT
{
    public class unoffset_qtype_detail
    {
        public string tdate { get; set; }
        public string ttype { get; set; } = "0";
        public string ttypename { get; set; }
        public string bstype { get; set; }
        public string dseq { get; set; }
        public string dno { get; set; }
        public decimal bqty { get; set; }
        public decimal mprice { get; set; }
        public decimal mamt { get; set; }
        public decimal lastprice { get; set; }
        public decimal marketvalue { get; set; }
        public decimal fee { get; set; }
        public decimal tax { get; set; } = 0;
        public decimal cramt { get; set; }
        public decimal bcramt { get; set; }
        public decimal gtamt { get; set; }
        public decimal bgtamt { get; set; }
        public decimal dnamt { get; set; }
        public decimal bdnamt { get; set; }
        public decimal interest { get; set; }
        public decimal dbfee { get; set; }
        public decimal dlfee { get; set; }
        public decimal dlint { get; set; }
        public decimal cost { get; set; }
        public decimal estimateAmt { get; set; }
        public decimal estimateFee { get; set; }
        public decimal estimateTax { get; set; }
        public decimal profit { get; set; }
        public string pl_ratio { get; set; }
        public string keeprate { get; set; }
        public string wtype { get; set; }
        public string ioflag { get; set; }
        public string ioname { get; set; }
    }
}
