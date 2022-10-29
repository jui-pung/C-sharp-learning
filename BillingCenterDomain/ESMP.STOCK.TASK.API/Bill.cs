using ESMP.STOCK.FORMAT.API;
using Newtonsoft.Json;
using System;
using System.Collections;
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
        SqlSearch sqlSearch = new SqlSearch();                              //自訂SqlSearch類別 (ESMP.STOCK.TASK.API)
        List<profile> profileList;                                          //自訂profile類別List (ESMP.STOCK.FORMAT.API)                                                                    //
        billSum billsum = new billSum();                                    //自訂profile_Sum類別Class (ESMP.STOCK.FORMAT.API) -函式回傳使用
        profile_sum profileSum = new profile_sum();                         //自訂billSum類別Class (ESMP.STOCK.FORMAT.API) -函式回傳使用

        //--------------------------------------------------------------------------------------------
        //function SearchSerilizer() - 將輸入的查詢資訊序列化為xml格式字串
        //--------------------------------------------------------------------------------------------
        public string searchSerilizer(string QTYPE, string BHNO, string CSEQ, string SDATE, string EDATE, string stockSymbol, int type)
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

        //--------------------------------------------------------------------------------------------
        // function searchDetails() - 計算取得 查詢回復階層二的對帳明細資料
        //--------------------------------------------------------------------------------------------
        public List<profile> searchDetails(List<profile> detailList, string BHNO, string CSEQ)
        {
            //填入股票中文名稱、分公司、帳號
            detailList.ForEach(x => x.stocknm = sqlSearch.selectStockName(x.stock));
            detailList.ForEach(x => x.bhno = BHNO);
            detailList.ForEach(x => x.cseq = CSEQ);

            //計算當日淨收付
            foreach (var item in detailList)
            {
                if (item.netamt == 0 && item.bstype == "B")
                    item.netamt = (item.mamt + item.fee) * -1;
                else if(item.netamt == 0 && item.bstype == "S")
                    item.netamt = item.mamt - item.fee - item.tax;
            }
            profileList = detailList;
            return detailList;
        }

        //--------------------------------------------------------------------------------------------
        // function searchSum() - 計算取得 查詢回復階層二 對帳單匯總
        //--------------------------------------------------------------------------------------------
        public billSum searchSum(List<profile> detailList)
        {
            billsum.cnbamt = detailList.Where(x => x.ttypename == "現買").Sum(x => x.mamt);
            billsum.cnsamt = detailList.Where(x => x.ttypename == "現賣").Sum(x => x.mamt);
            billsum.cnfee = detailList.Where(x => x.ttype == "0").Sum(x => x.fee);
            billsum.cntax = detailList.Where(x => x.ttype == "0").Sum(x => x.tax);
            billsum.cnnetamt = detailList.Where(x => x.ttype == "0").Sum(x => x.netamt);
            billsum.bqty = detailList.Where(x => x.bstype == "B").Sum(x => x.mqty);
            billsum.sqty = detailList.Where(x => x.bstype == "S").Sum(x => x.mqty);

            return billsum;
        }

        //--------------------------------------------------------------------------------------------
        // function searchProfileSum() - 計算取得 查詢回復階層一 對帳單彙總
        //--------------------------------------------------------------------------------------------
        public profile_sum searchProfileSum(List<profile> detailList)
        {
            profileSum.errcode = "0000";
            profileSum.errmsg = "查詢成功";
            profileSum.netamt = detailList.Sum(x => x.netamt);
            profileSum.fee = detailList.Sum(x => x.fee);
            profileSum.tax = detailList.Sum(x => x.tax);
            profileSum.mqty = detailList.Sum(x => x.mqty);
            profileSum.mamt = detailList.Sum(x => x.mamt);
            profileSum.billSum = billsum;
            profileSum.profile = profileList;
            
            return profileSum;
        }

        //--------------------------------------------------------------------------------------------
        //function resultListSerilizer() - 將QTYPE"0003"查詢結果 序列化為xml或json格式字串
        //--------------------------------------------------------------------------------------------
        public string resultListSerilizer(int type)
        {
            //序列化為xml格式字串
            if (type == 0)
            {
                using (var stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(profile_sum));
                    serializer.Serialize(stringwriter, profileSum);
                    return stringwriter.ToString();
                }
            }
            //序列化為json格式字串
            else if (type == 1)
            {
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                string jsonString = JsonConvert.SerializeObject(profileSum, settings);
                return jsonString.ToString();
            }
            return string.Empty;
        }

        //--------------------------------------------------------------------------------------------
        //function resultErrListSerilizer() - 將查詢失敗結果 序列化為xml或json格式字串
        //--------------------------------------------------------------------------------------------
        public string resultErrListSerilizer(int type)
        {
            //透過unoffset_qtype_accsum自訂類別反序列 -> accsumSer
            var sumSer = new profile_sum()
            {
                errcode = "0001",
                errmsg = "查詢失敗",
            };
            //序列化為xml格式字串
            if (type == 0)
            {
                using (var stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(profile_sum));
                    serializer.Serialize(stringwriter, sumSer);
                    return stringwriter.ToString();
                }
            }
            //序列化為json格式字串
            else if (type == 1)
            {
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                string jsonString = JsonConvert.SerializeObject(sumSer, settings);
                return jsonString.ToString();
            }
            return string.Empty;
        }
    }
}
