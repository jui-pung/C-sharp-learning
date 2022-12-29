using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ESMP.STOCK.FORMAT
{
    /// <summary>
    /// 查詢要求格式
    /// </summary>
    [DataContract]
    public class root
    {
        /// <summary>
        /// 查詢類別
        /// </summary>
        [XmlElement("qtype")]
        [DataMember(Name = "qtype")]
        public string qtype;
        /// <summary>
        /// 分公司
        /// </summary>
        [XmlElement("bhno")]
        [DataMember(Name = "bhno")]
        public string bhno;
        /// <summary>
        /// 客戶帳號
        /// </summary>
        [XmlElement("cseq")]
        [DataMember(Name = "cseq")]
        public string cseq;
        /// <summary>
        /// 查詢起日
        /// </summary>
        [XmlElement("sdate")]
        [DataMember(Name = "sdate")]
        public string sdate;
        /// <summary>
        /// 查詢迄日
        /// </summary>
        [XmlElement("edate")]
        [DataMember(Name = "edate")]
        public string edate;
        /// <summary>
        /// 股票代號
        /// </summary>
        [XmlElement("stockSymbol")]
        [DataMember(Name = "stockSymbol")]
        public string stockSymbol;
        /// <summary>
        /// 交易類別
        /// </summary>
        [XmlElement("ttype")]
        [DataMember(Name = "ttype")]
        public string ttype;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("QTYPE:");
            sb.Append(qtype);
            sb.Append(" ");
            sb.Append("BHNO:");
            sb.Append(bhno);
            sb.Append(" ");
            sb.Append("CSEQ:");
            sb.Append(cseq);
            sb.Append(" ");
            sb.Append("stockSymbol:");
            sb.Append(stockSymbol);
            sb.Append(" ");
            sb.Append("TTYPE:");
            sb.Append(ttype);
            sb.Append(" ");
            return sb.ToString();
        }
    }
}
