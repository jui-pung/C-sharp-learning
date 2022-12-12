using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ESMP.STOCK.FORMAT
{
    [XmlRoot("Symbols")]
    public class Symbols
    {
        [XmlElement("Symbol")]
        public List<Symbol> Symbol { get; set; }
    }
    public class Symbol
    {
        [XmlText]
        public string Value { get; set; }
        [XmlAttribute]
        public string id { get; set; }
        [XmlAttribute]
        public decimal dealprice { get; set; }
        [XmlAttribute]
        public string shortname { get; set; }
        [XmlAttribute]
        public decimal refprice { get; set; }
        [XmlAttribute]
        public string moddate { get; set; }
        [XmlAttribute]
        public string modtime { get; set; }
    }
}
