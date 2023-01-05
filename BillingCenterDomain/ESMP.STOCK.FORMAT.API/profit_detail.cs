﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ESMP.STOCK.FORMAT
{
    public class profit_detail
    {
        //[XmlIgnore]
        //[JsonIgnore]
        //public string stock { get; set; }
        //[XmlIgnore]
        //[JsonIgnore]
        //public string sdseq { get; set; }
        //[XmlIgnore]
        //[JsonIgnore]
        //public string sdno { get; set; }
        //[XmlIgnore]
        //[JsonIgnore]
        //public string sdate { get; set; }
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
        public decimal ccramt { get; set; }
        public decimal cdnamt { get; set; }
        public decimal cgtamt { get; set; }
        public decimal fee { get; set; }
        public decimal interest { get; set; }
        public decimal tax { get; set; } = 0;
        public decimal dbfee { get; set; }
        public decimal dlfee { get; set; }
        public string adjdate { get; set; }
        public string ttype { get; set; } = "0";
        public string ttypename { get; set; } = "現買";
        public string bstype { get; set; } = "B";
        public string wtype { get; set; }
        public decimal profit { get; set; }
        public string pl_ratio { get; set; }
        public string ctype { get; set; } = "0";
        public string ioflag { get; set; }
        public string ioname { get; set; } = "";
        public string ttypename2 { get; set; } = "現買";
    }
}
