using ESMP.STOCK.FORMAT.API;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ESMP.STOCK.TASK.API
{
    public class GainPay
    {
        string QTYPE, BHNO, CSEQ, SDATE, EDATE;                                     //使用者於Form輸入之欄位值
        SqlSearch sqlSearch = new SqlSearch();                                      //自訂SqlSearch類別 (ESMP.STOCK.TASK.API)                                           
        List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();          //自訂unoffset_qtype_sum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用
        List<unoffset_qtype_accsum> accsumList = new List<unoffset_qtype_accsum>(); //自訂unoffset_qtype_accsum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用

        //--------------------------------------------------------------------------------------------
        //function getFormField() - 取得使用者於Form1輸入的欄位值 - comboBoxQTYPE, txtBHNO, txtCSEQ,
        //                                                          txtSDATE, txtEDATE
        //--------------------------------------------------------------------------------------------
        public void getFormField(string comboBoxQTYPE, string txtBHNO, string txtCSEQ, string txtSDATE, string txtEDATE)
        {
            QTYPE = comboBoxQTYPE;
            BHNO = txtBHNO;
            CSEQ = txtCSEQ;
            SDATE = txtSDATE;
            EDATE = txtEDATE;
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
                edate = EDATE
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

        //--------------------------------------------------------------------------------------------
        // function searchDetails() - 計算取得 查詢回復階層三的個股明細資料 (賣出)
        //--------------------------------------------------------------------------------------------
        public List<profit_detail_out> searchDetails(List<profit_detail_out> detailList)
        {
            //判斷TDATE、SDSEQ、SDNO欄位是否相同 相同->資料加總
            var lst_detail = detailList.GroupBy(d => new { d.tdate, d.dseq, d.dno }).Select(
            g => new profit_detail_out
            {
                stock = g.First().stock,
                tdate = g.First().tdate,
                dseq = g.First().dseq,
                dno = g.First().dno,
                mqty = g.First().mqty,
                cqty = g.Sum(s => s.cqty),
                mprice = g.First().mprice,
                cost = g.Sum(s => s.cost),
                income = g.Sum(s => s.income),
                netamt = g.Sum(s => s.netamt),
                fee = g.Sum(s => s.fee),
                tax = g.Sum(s => s.tax),
                wtype = g.First().wtype,
                profit = g.Sum(s => s.profit),
            }).ToList();

            //計算計算明細資料(賣出)的成交價金、報酬率
            lst_detail.ForEach(x => x.mamt = (x.cqty * Convert.ToDecimal(x.mprice)).ToString());
            lst_detail.ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");

            return lst_detail;
        }
    }
}
