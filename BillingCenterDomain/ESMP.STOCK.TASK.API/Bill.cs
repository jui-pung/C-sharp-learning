using ESMP.STOCK.DB.TABLE;
using ESMP.STOCK.FORMAT;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ESMP.STOCK.TASK.API
{
    public class Bill
    {
        int _type;                                          //查詢與回覆格式設定
        string _searchStr;                                  //查詢xml或json格式字串
        SqlSearch _sqlSearch = new SqlSearch();             //自訂SqlSearch類別 (ESMP.STOCK.TASK.API)

        //--------------------------------------------------------------------------------------------
        //function SearchSerilizer() - 將輸入的查詢資訊序列化為xml格式字串
        //--------------------------------------------------------------------------------------------
        public (string, string) getBillSearch(string QTYPE, string BHNO, string CSEQ, string SDATE, string EDATE, string stockSymbol, int type)
        {
            _type = type;
            List<HCMIO> HCMIOList = new List<HCMIO>();                  //自訂HCMIO類別List (ESMP.STOCK.DB.TABLE.API)
            List<TMHIO> TMHIOList = new List<TMHIO>();                  //自訂TMHIO類別List (ESMP.STOCK.DB.TABLE.API)
            List<TCNUD> TCNUDList = new List<TCNUD>();                  //自訂TCNUD類別List (ESMP.STOCK.DB.TABLE.API)
            List<TCSIO> TCSIOList = new List<TCSIO>();                  //自訂TCSIO類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCNRH> HCNRHList = new List<HCNRH>();                  //自訂HCNRH類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCNTD> HCNTDList = new List<HCNTD>();                  //自訂HCNTD類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCMIO> HCMIOList_Today = new List<HCMIO>();            //自訂HCMIO類別List (ESMP.STOCK.DB.TABLE.API)

            List<profile> profileList = new List<profile>();            //自訂profile類別List           (階層二:對帳單明細資料)  
            List<profile> profileList_Today = new List<profile>();      //自訂profile類別List           (階層二:對帳單明細資料)  
            billSum billsum = new billSum();                            //自訂billSum類別class          (階層二:對帳單匯總資料)  
            profile_sum profileSum = new profile_sum();                 //自訂profile_sum類別Class      (階層一:對帳單彙總資料)  
            string txtSearchContent = "";
            string txtSearchResultContent = "";

            //取得查詢xml或json格式字串
            _searchStr = searchSerilizer(QTYPE, BHNO, CSEQ, SDATE, EDATE, stockSymbol, _type);
            txtSearchContent = _searchStr;
            //取得查詢字串Element
            var obj = GetElement(_searchStr, _type);
            root SearchElement = obj as root;

            //查詢資料庫資料
            TCNUDList = _sqlSearch.selectTCNUD(SearchElement);
            TMHIOList = _sqlSearch.selectTMHIO(SearchElement);
            HCMIOList = _sqlSearch.selectHCMIO(SearchElement);
            TCSIOList = _sqlSearch.selectTCSIO(SearchElement);

            //盤中現股沖銷 當沖 現股賣出處理
            (TCNUDList, HCNRHList, HCNTDList, HCMIOList_Today) = ESMPData.GetESMPData(TCNUDList, TMHIOList, TCSIOList, BHNO, CSEQ);

            if (HCMIOList.Count > 0 || TMHIOList.Count > 0)
            {
                profileList = searchDetails(HCMIOList, BHNO, CSEQ);
                profileList_Today = searchDetails_Today(HCMIOList_Today, HCNTDList, HCNRHList, BHNO, CSEQ);
                //合併歷史與當日資料
                profileList = profileList.Concat(profileList_Today).ToList();
                billsum = searchSum(profileList);
                profileSum = searchProfileSum(profileList, billsum);
                //呈現查詢結果
                txtSearchResultContent = resultListSerilizer(profileSum, _type);
            }
            else
            {
                txtSearchResultContent = resultErrListSerilizer(_type);
            }
            return (txtSearchContent, txtSearchResultContent);
        }
        //--------------------------------------------------------------------------------------------
        //function SearchSerilizer() - 將輸入的查詢資訊序列化為xml格式字串
        //--------------------------------------------------------------------------------------------
        private string searchSerilizer(string QTYPE, string BHNO, string CSEQ, string SDATE, string EDATE, string stockSymbol, int type)
        {
            var root = new root()
            {
                qtype = QTYPE,
                bhno = BHNO,
                cseq = CSEQ,
                sdate = SDATE,
                edate = EDATE,
                stockSymbol = stockSymbol
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
        private object GetElement(string Content, int type)
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
        public List<profile> searchDetails(List<HCMIO> HCMIOList, string BHNO, string CSEQ)
        {
            List<profile> profileList = new List<profile>();                //自訂profile類別List (ESMP.STOCK.FORMAT.API)                                                                    //

            //提供歷史對帳單資料
            foreach (var item in HCMIOList)
            {
                //字典搜尋此股票 中文名稱
                string cname = "";
                if (BasicData._MSTMB_Dic.ContainsKey(item.STOCK))
                    cname = BasicData._MSTMB_Dic[item.STOCK][0].CNAME;
                else
                    cname = "";             //如果查不到股票中文名稱, 假設中文名稱為" "

                profile row = new profile();
                row.bhno = BHNO;
                row.cseq = CSEQ;
                row.stock = item.STOCK;
                row.stocknm = cname;
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
                profileList.Add(row);
            }
            return profileList;
        }

        /// <summary>
        /// function searchDetails_Today() - 計算取得 查詢回復階層二的對帳明細資料 (當日交易明細)
        /// </summary>
        /// <param name="HCMIOList"></param>
        /// <param name="BHNO"></param>
        /// <param name="CSEQ"></param>
        /// <returns></returns>
        public List<profile> searchDetails_Today(List<HCMIO> HCMIOList, List<HCNTD> HCNTDList, List<HCNRH> HCNRHList, string BHNO, string CSEQ)
        {
            List<profile> profileList = new List<profile>();                //自訂profile類別List (ESMP.STOCK.FORMAT.API)
            HCMIOList = HCMIOList.Where(m => m.WTYPE == "0").ToList();
            //若盤中現股賣出為部分當沖、部分現沖，則拆成2筆對帳單資料提供
            List<HCMIO> HCMIOList_Part = HCMIOList.Where(m => m.BSTYPE == "S").ToList();
            foreach (var HCMIO_item in HCMIOList_Part)
            {
                List<HCNRH> HCNRHList_Part = HCNRHList.Where(x => x.SDSEQ == HCMIO_item.DSEQ && x.SDNO == HCMIO_item.DNO).ToList();
                List<HCNTD> HCNTDList_Part = HCNTDList.Where(x => x.SDSEQ == HCMIO_item.DSEQ && x.SDNO == HCMIO_item.DNO).ToList();

                if (HCNRHList_Part.Count > 0)
                {
                    foreach (var HCNRH_item in HCNRHList)
                    {
                        var row = new HCMIO();
                        row.TDATE = HCMIO_item.TDATE;
                        row.BHNO = HCMIO_item.BHNO;
                        row.CSEQ = HCMIO_item.CSEQ;
                        row.DSEQ = HCMIO_item.DSEQ;
                        row.DNO = HCMIO_item.DNO;
                        row.WTYPE = "0";
                        row.STOCK = HCMIO_item.STOCK;
                        row.TTYPE = HCMIO_item.TTYPE;
                        row.ETYPE = HCMIO_item.ETYPE;
                        row.BSTYPE = HCMIO_item.BSTYPE;
                        row.PRICE = HCMIO_item.PRICE;
                        row.QTY = HCNRH_item.CQTY;          //合計成交股數????
                        row.BQTY = 0;                       //未沖銷股數
                        row.AMT = decimal.Truncate(HCMIO_item.PRICE * row.QTY);
                        row.FEE = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(row.AMT) * 0.001425));
                        if (HCMIO_item.QTY >= 1000 && row.FEE < 20)
                            row.FEE = 20;
                        else if (HCMIO_item.QTY < 1000 && row.FEE < 1)
                            row.FEE = 1;
                        row.TAX = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(row.AMT) * 0.0015));
                        row.NETAMT = row.AMT - row.FEE - row.TAX;
                        row.ORIGN = HCMIO_item.ORIGN;
                        row.SALES = HCMIO_item.SALES;
                        row.TRDATE = HCMIO_item.TRDATE;
                        row.TRTIME = HCMIO_item.TRTIME;
                        row.MODDATE = HCMIO_item.MODDATE;
                        row.MODTIME = HCMIO_item.MODTIME;
                        row.MODUSER = HCMIO_item.MODUSER;
                        HCMIOList.Add(row);
                    }
                }
                foreach (var HCNTD_item in HCNTDList)
                {
                    HCMIO_item.QTY = HCNTD_item.CQTY;          //合計成交股數????
                    HCMIO_item.AMT = decimal.Truncate(HCMIO_item.PRICE * HCMIO_item.QTY);
                    HCMIO_item.FEE = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(HCMIO_item.AMT) * 0.001425));
                    if (HCMIO_item.QTY >= 1000 && HCMIO_item.FEE < 20)
                        HCMIO_item.FEE = 20;
                    else if (HCMIO_item.QTY < 1000 && HCMIO_item.FEE < 1)
                        HCMIO_item.FEE = 1;
                    HCMIO_item.TAX = HCNTD_item.TAX;
                    HCMIO_item.NETAMT = HCMIO_item.AMT - HCMIO_item.FEE - HCMIO_item.TAX;
                }
            }
            //提供當日對帳單資料
            foreach (var item in HCMIOList)
            {
                //字典搜尋此股票 中文名稱
                string cname = "";
                if (BasicData._MSTMB_Dic.ContainsKey(item.STOCK))
                    cname = BasicData._MSTMB_Dic[item.STOCK][0].CNAME;
                else
                    cname = "";             //如果查不到股票中文名稱, 假設中文名稱為" "

                profile row = new profile();
                row.bhno = BHNO;
                row.cseq = CSEQ;
                row.stock = item.STOCK;
                row.stocknm = cname;
                row.mdate = item.TDATE;
                row.dseq = item.DSEQ;
                row.dno = item.DNO;
                row.ttype = item.TTYPE;
                row.bstype = item.BSTYPE;
                if (item.BSTYPE == "B")
                    row.bstypename = "買";
                else if (item.BSTYPE == "S")
                    row.bstypename = "賣";
                row.etype = item.ETYPE;
                row.mprice = item.PRICE;
                row.mqty = item.QTY;
                if (item.BQTY > 0)
                {
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
                    row.mamt = item.AMT;
                    row.fee = item.FEE;
                    //零股最小手續費
                    if (row.etype == "1" && row.fee < 1)
                        row.fee = 1;
                    //整股最小手續費
                    else if (row.etype == "0" && row.fee < 20)
                        row.fee = 20;
                    row.tax = item.TAX;
                    row.netamt = item.NETAMT;
                }
                else
                {
                    if (item.BSTYPE == "B")
                    {
                        row.mamt = item.PRICE * item.QTY;
                        row.fee = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(row.mprice) * decimal.ToDouble(row.mqty) * 0.001425));
                        //零股最小手續費
                        if (row.etype == "1" && row.fee < 1)
                            row.fee = 1;
                        //整股最小手續費
                        else if (row.etype == "0" && row.fee < 20)
                            row.fee = 20;
                        row.ttypename = "買沖";
                        row.tax = 0;
                        //買入資料計算淨收付
                        row.netamt = ((row.mamt + row.fee) * -1);
                    }   
                    else if (item.BSTYPE == "S")
                    {
                        row.mamt = item.AMT;
                        row.fee = item.FEE;
                        row.ttypename = "賣沖";       //若盤中現股賣出為部分當沖、部分現沖 (都給賣沖??)
                        row.tax = item.TAX;
                        row.netamt = item.NETAMT;
                    }
                }
                profileList.Add(row);
            }
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
        private profile_sum searchProfileSum(List<profile> profileList, billSum billsum)
        {
            profile_sum profileSum = new profile_sum();         //自訂billSum類別Class (ESMP.STOCK.FORMAT.API) -函式回傳使用

            profileSum.errcode = "0000";
            profileSum.errmsg = "查詢成功";
            profileSum.netamt = profileList.Sum(x => x.netamt);
            profileSum.fee = profileList.Sum(x => x.fee);
            profileSum.tax = profileList.Sum(x => x.tax);
            profileSum.mqty = profileList.Sum(x => x.mqty);
            profileSum.mamt = profileList.Sum(x => x.mamt);
            //存入第二階層資料
            profileSum.billSum = billsum;
            profileSum.profile = profileList;
            return profileSum;
        }

        //--------------------------------------------------------------------------------------------
        //function resultListSerilizer() - 將QTYPE"0003"查詢結果 序列化為xml或json格式字串
        //--------------------------------------------------------------------------------------------
        private string resultListSerilizer(profile_sum profileSum, int type)
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
        private string resultErrListSerilizer(int type)
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
