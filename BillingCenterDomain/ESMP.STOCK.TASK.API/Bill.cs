﻿using ESMP.STOCK.DB.TABLE.API;
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
        SqlSearch _sqlSearch = new SqlSearch();                              //自訂SqlSearch類別 (ESMP.STOCK.TASK.API)
        
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
        public List<profile> searchDetails(List<HCMIO> dbHCMIO, List<TMHIO> dbTMHIO, string BHNO, string CSEQ)
        {
            List<profile> profileList = new List<profile>();                //自訂profile類別List (ESMP.STOCK.FORMAT.API)                                                                    //
            List<profile> profileHCMIOList = new List<profile>();           //自訂profile類別List (ESMP.STOCK.FORMAT.API)                                                                    //
            List<profile> profileTMHIOList = new List<profile>();           //自訂profile類別List (ESMP.STOCK.FORMAT.API)                                                                    //
            //歷史資料
            foreach (var item in dbHCMIO)
            {
                profile row = new profile();
                row.bhno = BHNO;
                row.cseq = CSEQ;
                row.stock = item.STOCK;
                row.stocknm = _sqlSearch.selectStockName(item.STOCK);
                row.mdate = item.TDATE;
                row.dseq = item.DSEQ;
                row.dno = item.DNO;
                row.ttype = item.TTYPE;
                if (item.TTYPE == "0" && item.BSTYPE == "B")
                    row.ttypename = "現買";
                else if(item.TTYPE == "0" && item.BSTYPE == "S")
                    row.ttypename = "現賣";
                row.bstype = item.BSTYPE;
                if (item.BSTYPE == "B")
                    row.bstypename = "買";
                else if (item.BSTYPE == "S")
                    row.bstypename = "賣";
                row.etype = item.ETYPE;
                row.mprice = item.PRICE;
                row.mqty = item.QTY;
                row.mamt = item.AMT;
                row.fee = item.FEE;
                row.tax = item.TAX;
                row.netamt = item.NETAMT;
                profileHCMIOList.Add(row);
            }
            //當日資料
            foreach (var item in dbTMHIO)
            {
                profile row = new profile();
                row.bhno = BHNO;
                row.cseq = CSEQ;
                row.stock = item.STOCK;
                row.stocknm = _sqlSearch.selectStockName(item.STOCK);
                row.mdate = item.TDATE;
                row.dseq = item.DSEQ;
                row.dno = item.JRNUM;
                row.ttype = item.TTYPE;
                if (item.TTYPE == "0" && item.BSTYPE == "B" && item.ETYPE == "0")
                    row.ttypename = "現買";
                else if (item.TTYPE == "0" && item.BSTYPE == "B" && item.ETYPE == "2")
                    row.ttypename = "盤後零買";
                else if (item.TTYPE == "0" && item.BSTYPE == "B" && item.ETYPE == "5")
                    row.ttypename = "盤中零買";
                else if (item.TTYPE == "0" && item.BSTYPE == "S" && item.ETYPE == "0")
                    row.ttypename = "現賣";
                else if (item.TTYPE == "0" && item.BSTYPE == "S" && item.ETYPE == "2")
                    row.ttypename = "盤後零賣";
                else if (item.TTYPE == "0" && item.BSTYPE == "S" && item.ETYPE == "5")
                    row.ttypename = "盤中零賣";
                row.bstype = item.BSTYPE;
                if (item.BSTYPE == "B")
                    row.bstypename = "買";
                else if (item.BSTYPE == "S")
                    row.bstypename = "賣";
                if (item.ETYPE == "2")
                    row.etype = "1";
                else
                    row.etype = "0";
                row.mprice = item.PRICE;
                row.mqty = item.QTY;
                row.mamt = item.PRICE * item.QTY;
                row.fee = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(row.mprice) * decimal.ToDouble(row.mqty) * 0.001425));
                if (item.BSTYPE == "S")
                {
                    row.tax = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(row.mprice) * decimal.ToDouble(row.mqty) * 0.003));
                    row.netamt = row.mamt - row.fee - row.tax;
                }
                else if (item.BSTYPE == "B")
                    row.netamt = ((row.mamt + row.fee) * -1);
                profileTMHIOList.Add(row);
            }
            profileList = profileHCMIOList.Concat(profileTMHIOList).ToList();
            return profileList;
        }

        //--------------------------------------------------------------------------------------------
        // function searchSum() - 計算取得 查詢回復階層二 對帳單匯總
        //--------------------------------------------------------------------------------------------
        public billSum searchSum(List<profile> detailList)
        {
            billSum billsum = new billSum();            //自訂profile_Sum類別Class (ESMP.STOCK.FORMAT.API) -函式回傳使用

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
        public profile_sum searchProfileSum(List<profile> profileList, billSum billsum)
        {
            profile_sum profileSum = new profile_sum();         //自訂billSum類別Class (ESMP.STOCK.FORMAT.API) -函式回傳使用

            profileSum.errcode = "0000";
            profileSum.errmsg = "查詢成功";
            profileSum.netamt = profileList.Sum(x => x.netamt);
            profileSum.fee = profileList.Sum(x => x.fee);
            profileSum.tax = profileList.Sum(x => x.tax);
            profileSum.mqty = profileList.Sum(x => x.mqty);
            profileSum.mamt = profileList.Sum(x => x.mamt);
            profileSum.billSum = billsum;
            profileSum.profile = profileList;
            return profileSum;
        }

        //--------------------------------------------------------------------------------------------
        //function resultListSerilizer() - 將QTYPE"0003"查詢結果 序列化為xml或json格式字串
        //--------------------------------------------------------------------------------------------
        public string resultListSerilizer(profile_sum profileSum, int type)
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
