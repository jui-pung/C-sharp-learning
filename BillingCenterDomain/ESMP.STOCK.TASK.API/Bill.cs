using ESMP.STOCK.FORMAT.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ESMP.STOCK.TASK.API
{
    public class Bill
    {
        string QTYPE, BHNO, CSEQ, SDATE, EDATE, stockSymbol;                //使用者於Form輸入之欄位值
        SqlSearch sqlSearch = new SqlSearch();                              //自訂SqlSearch類別 (ESMP.STOCK.TASK.API)                                           
        List<profile_sum> sumList = new List<profile_sum>();                //自訂profile_Sum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用
        List<billSum> billSumList = new List<billSum>();                    //自訂billSum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用

        //--------------------------------------------------------------------------------------------
        //function getFormField() - 取得使用者於Form1輸入的欄位值 - comboBoxQTYPE, txtBHNO, txtCSEQ,
        //                                                          txtSDATE, txtEDATE, txtstockSymbol
        //--------------------------------------------------------------------------------------------
        public void getFormField(string comboBoxQTYPE, string txtBHNO, string txtCSEQ, string txtSDATE, string txtEDATE, string txtstockSymbol)
        {
            QTYPE = comboBoxQTYPE;
            BHNO = txtBHNO;
            CSEQ = txtCSEQ;
            SDATE = txtSDATE;
            EDATE = txtEDATE;
            stockSymbol = txtstockSymbol;
        }

        //--------------------------------------------------------------------------------------------
        //function SearchSerilizer() - 將輸入的查詢資訊序列化為xml格式字串
        //--------------------------------------------------------------------------------------------
        public string searchSerilizer(int type)
        {
            var root = new root()
            {
                qtype = QTYPE,
                bhno = BHNO,
                cseq = CSEQ,
                sdate = SDATE,
                edate = EDATE,
                stockSymbol = stockSymbol,
            };
            if (type == 0)
            {
                using (var stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(root));
                    serializer.Serialize(stringwriter, root);
                    return stringwriter.ToString();
                }
            }
            else if (type == 1)
            {
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                string jsonString = JsonConvert.SerializeObject(root, settings);
                return jsonString;
            }
            return "";
        }

        //--------------------------------------------------------------------------------------------
        // function GetElement() - 取得xml格式字串 Element
        //--------------------------------------------------------------------------------------------
        public object GetElement(string Content, int type)
        {
            root root = new root();
            if (type == 0)
            {
                //建立serializer物件,並指定反序列化物件的型別(root)
                XmlSerializer ser = new XmlSerializer(typeof(root));
                //反序列化XML(obj為反序列化的型別的物件變數)
                root obj = (root)ser.Deserialize(new StringReader(Content));
                return obj;
            }
            else if (type == 1)
            {
                root obj = JsonConvert.DeserializeObject<root>(Content);
                return obj;
            }
            return root;
        }
    }
}
