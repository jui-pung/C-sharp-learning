﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace _1_UnrealizedGainsOrLosses
{
    public class unoffset_qtype_accsum
    {
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
        [XmlElement("unoffset_qtype_sum")]
        public List<unoffset_qtype_sum> unoffset_qtype_sum { get; set; }
    }
}
