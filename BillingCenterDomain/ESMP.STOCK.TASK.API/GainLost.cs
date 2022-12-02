﻿using ESMP.STOCK.DB.TABLE;
using ESMP.STOCK.FORMAT;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace ESMP.STOCK.TASK.API
{
    //處理未實現損益查詢
    public class GainLost
    {
        int _type;                                          //查詢與回覆格式設定
        string _searchStr;                                  //查詢xml或json格式字串
        SqlSearch _sqlSearch = new SqlSearch();             //自訂SqlSearch類別 (ESMP.STOCK.TASK.API)
        static List<MCSRH> _MCSRHList = new List<MCSRH>();  //自訂MCUMS類別List (ESMP.STOCK.DB.TABLE.API)
        static Dictionary<string, List<MCSRH>> _MCSRH_Dic;

        //--------------------------------------------------------------------------------------------
        //function getGainLostSearch() - 未實現損益查詢的對外接口function
        //--------------------------------------------------------------------------------------------
        public (string,string) getGainLostSearch(string QTYPE, string BHNO, string CSEQ, string stockSymbol, int type)
        {
            //宣告物件 變數
            _type = type;
            List<TCNUD> TCNUDList = new List<TCNUD>();                                      //自訂TCNUD類別List (ESMP.STOCK.DB.TABLE.API)
            List<TMHIO> TMHIOList = new List<TMHIO>();                                      //自訂TMHIO類別List (ESMP.STOCK.DB.TABLE.API)
            List<TCSIO> TCSIOList = new List<TCSIO>();                                      //自訂TCSIO類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCNRH> HCNRHList = new List<HCNRH>();                                      //自訂HCNRH類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCNTD> HCNTDList = new List<HCNTD>();                                      //自訂HCNTD類別List (ESMP.STOCK.DB.TABLE.API)
            List<unoffset_qtype_detail> detailList = new List<unoffset_qtype_detail>();     //自訂unoffset_qtype_detail類別List (階層三:個股明細)
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();              //自訂unoffset_qtype_sum類別List    (階層二:個股未實現損益)
            List<unoffset_qtype_accsum> accsumList = new List<unoffset_qtype_accsum>();     //自訂unoffset_qtype_accsum類別List (階層一:帳戶未實現損益)
            string txtSearchContent = "";
            string txtSearchResultContent = "";

            //取得查詢xml或json格式字串
            _searchStr = searchSerilizer(QTYPE, BHNO, CSEQ, stockSymbol, _type);
            txtSearchContent = _searchStr;
            //取得查詢字串Element
            var obj = GetElement(_searchStr, _type);
            root SearchElement = obj as root;
            //查詢資料庫資料
            TCNUDList = _sqlSearch.selectTCNUD(SearchElement);
            TMHIOList = _sqlSearch.selectTMHIO(SearchElement);
            TCSIOList = _sqlSearch.selectTCSIO(SearchElement);
            _MCSRHList = _sqlSearch.selectMCSRH();

            //依據 BHNO CSEQ STOCK 建立 MCSRH Dictionary
            _MCSRH_Dic = _MCSRHList.GroupBy(d => d.BHNO + d.CSEQ + d.STOCK).ToDictionary(x => x.Key, x => x.ToList());

            //盤中現股沖銷 當沖 現股賣出處理
            (TCNUDList, HCNRHList, HCNTDList) = ESMPData.GetESMPData(TCNUDList, TMHIOList, TCSIOList, BHNO, CSEQ);
            
            if (TCNUDList.Count > 0)
            {
                //未實現損益
                sumList = searchSum(TCNUDList);
                accsumList = searchAccSum(sumList);
                //查詢結果
                txtSearchResultContent = resultListSerilizer(accsumList, _type);
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
        private string searchSerilizer(string QTYPE, string BHNO, string CSEQ, string stockSymbol, int type)
        {
            var root = new root()
            {
                qtype = QTYPE,
                bhno = BHNO,
                cseq = CSEQ,
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
        // function searchSum() - 計算取得 查詢回復階層二的個股未實現損益與個股明細
        //--------------------------------------------------------------------------------------------
        public List<unoffset_qtype_sum> searchSum(List<TCNUD> TCNUD)
        {
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();          //自訂unoffset_qtype_sum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用
            //依照股票代號產生新的清單群組grp_TCNUD
            var grp_TCNUD_Buy = TCNUD.Where(x => x.BQTY > 0).GroupBy(d => d.STOCK).Select(grp => grp.ToList()).ToList();
            var grp_TCNUD_Sell = TCNUD.Where(x => x.BQTY < 0).GroupBy(d => d.STOCK).Select(grp => grp.ToList()).ToList();

            //(現買)迴圈處理grp_TCNUD_Buy群組資料 存入個股未實現損益 List
            foreach (var grp_item in grp_TCNUD_Buy)
            {
                //取得個股明細資料 List (第三階層)
                List<unoffset_qtype_detail> detailList = new List<unoffset_qtype_detail>();
                foreach (var item in grp_item)
                {
                    var row = new unoffset_qtype_detail();
                    row.tdate = item.TDATE;
                    row.dseq = item.DSEQ;
                    row.dno = item.DNO;
                    row.bqty = item.BQTY;
                    row.mprice = item.PRICE;
                    row.lastprice = _sqlSearch.selectStockCprice(item.STOCK);
                    row.fee = item.FEE;
                    row.ttypename = "現買";
                    row.bstype = "B";
                    row.mamt = row.bqty * row.mprice;
                    row.tax = 0;
                    row.cost = item.COST;
                    row.estimateAmt = decimal.Truncate(row.lastprice * row.bqty);
                    row.estimateFee = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(row.estimateAmt) * 0.001425));
                    if (row.estimateFee < 20)
                        row.estimateFee = 20;
                    row.estimateTax = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(row.estimateAmt) * 0.003));

                    //匯撥來源說明
                    if (item.WTYPE == "A")
                    {
                        row.ioflag = item.IOFLAG;
                        if(row.ioflag != null)
                            row.ioname = BasicData.getIoflagName(item.IOFLAG);
                    }
                    detailList.Add(row);
                }
                detailList.ForEach(p => p.marketvalue = p.estimateAmt - p.estimateFee - p.estimateTax);
                detailList.ForEach(p => p.profit = p.marketvalue - p.cost);
                detailList.Where(x => x.cost != 0).ToList().ForEach(p => p.pl_ratio = decimal.Round(((p.profit / p.cost) * 100), 2).ToString() + "%");
                detailList.Where(x => x.cost == 0).ToList().ForEach(p => p.pl_ratio = "0%");

                //字典搜尋客戶此股票 昨日庫存股數
                decimal yesterdayBqty = 0;
                string searchKey = grp_item.First().BHNO + grp_item.First().CSEQ + grp_item.First().STOCK;
                if (_MCSRH_Dic.ContainsKey(searchKey))
                    yesterdayBqty = _MCSRH_Dic[searchKey][0].CNQBAL;
                else
                    yesterdayBqty = 0;             //如果查不到昨日庫存股數, 假設昨日庫存股數為0

                //取得個股未實現損益 List (第二階層)
                unoffset_qtype_sum row_Sum = new unoffset_qtype_sum();
                row_Sum.stock = grp_item.First().STOCK;
                row_Sum.stocknm = _sqlSearch.selectStockName(row_Sum.stock);
                row_Sum.ttypename = detailList.First().ttypename;
                row_Sum.bstype = detailList.First().bstype;
                row_Sum.bqty = yesterdayBqty;
                row_Sum.real_qty = detailList.Sum(x => x.bqty);
                row_Sum.cost = detailList.Sum(x => x.cost);
                row_Sum.lastprice = detailList.First().lastprice;
                row_Sum.marketvalue = detailList.Sum(x => x.marketvalue);
                row_Sum.estimateAmt = detailList.Sum(x => x.estimateAmt);
                row_Sum.estimateFee = detailList.Sum(x => x.estimateFee);
                row_Sum.estimateTax = detailList.Sum(x => x.estimateTax);
                row_Sum.profit = detailList.Sum(x => x.profit);
                row_Sum.fee = detailList.Sum(x => x.fee);
                row_Sum.tax = detailList.Sum(x => x.tax);
                row_Sum.amt = detailList.Sum(x => x.mamt);
                //第三階層資料存入第二階層List
                row_Sum.unoffset_qtype_detail = detailList;
                sumList.Add(row_Sum);
                sumList.ForEach(x => x.avgprice = decimal.Round((x.cost / x.bqty), 2));
                sumList.Where(n => n.cost != 0).ToList().ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");
                sumList.Where(n => n.cost == 0).ToList().ForEach(p => p.pl_ratio = "0%");
            }

            foreach (var grp_item in grp_TCNUD_Sell)
            {
                //取得個股明細資料 List (第三階層)
                List<unoffset_qtype_detail> detailList = new List<unoffset_qtype_detail>();
                foreach (var item in grp_item)
                {
                    var row = new unoffset_qtype_detail();
                    row.tdate = item.TDATE;
                    row.dseq = item.DSEQ;
                    row.dno = item.DNO;
                    row.bqty = item.BQTY;
                    row.mprice = item.PRICE;
                    row.lastprice = _sqlSearch.selectStockCprice(item.STOCK);
                    row.fee = item.FEE;
                    row.ttypename = "現賣";
                    row.bstype = "S";
                    row.mamt = row.bqty * row.mprice * -1;
                    row.tax = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(row.mamt) * 0.003));
                    row.cost = (row.mamt - row.fee - row.tax) * -1;
                    row.estimateAmt = decimal.Truncate(row.lastprice * row.bqty * -1);
                    row.estimateFee = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(row.estimateAmt) * 0.001425));
                    row.estimateTax = 0;

                    //匯撥來源說明
                    if (item.WTYPE == "A")
                    {
                        row.ioflag = item.IOFLAG;
                        if (row.ioflag != null)
                            row.ioname = BasicData.getIoflagName(item.IOFLAG);
                    }
                    detailList.Add(row);
                }
                detailList.ForEach(p => p.marketvalue = (p.estimateAmt + p.estimateFee) * -1);
                detailList.ToList().ForEach(p => p.profit = p.cost - p.marketvalue);
                detailList.Where(x => x.cost != 0).ToList().ForEach(p => p.pl_ratio = decimal.Round(((p.profit / p.cost) * 100), 2).ToString() + "%");
                detailList.Where(x => x.cost == 0).ToList().ForEach(p => p.pl_ratio = "0%");

                //取得個股未實現損益 List (第二階層)
                unoffset_qtype_sum row_Sum = new unoffset_qtype_sum();
                row_Sum.stock = grp_item.First().STOCK;
                row_Sum.stocknm = _sqlSearch.selectStockName(row_Sum.stock);
                row_Sum.ttypename = detailList.First().ttypename;
                row_Sum.bstype = detailList.First().bstype;
                row_Sum.bqty = 0;
                row_Sum.real_qty = detailList.Sum(x => x.bqty);
                row_Sum.cost = detailList.Sum(x => x.cost);
                row_Sum.lastprice = detailList.First().lastprice;
                row_Sum.marketvalue = detailList.Sum(x => x.marketvalue);
                row_Sum.estimateAmt = detailList.Sum(x => x.estimateAmt);
                row_Sum.estimateFee = detailList.Sum(x => x.estimateFee);
                row_Sum.estimateTax = detailList.Sum(x => x.estimateTax);
                row_Sum.profit = detailList.Sum(x => x.profit);
                row_Sum.fee = detailList.Sum(x => x.fee);
                row_Sum.tax = detailList.Sum(x => x.tax);
                row_Sum.amt = detailList.Sum(x => x.mamt);
                //第三階層資料存入第二階層List
                row_Sum.unoffset_qtype_detail = detailList;
                sumList.Add(row_Sum);
                sumList.ForEach(x => x.avgprice = decimal.Round((x.cost / x.real_qty), 2));
                sumList.Where(n => n.cost != 0).ToList().ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");
                sumList.Where(n => n.cost == 0).ToList().ForEach(p => p.pl_ratio = "0%");
            }
            return sumList;
        }

        //--------------------------------------------------------------------------------------------
        // function searchAccSum() - 計算取得 查詢回復階層一 帳戶未實現損益
        //--------------------------------------------------------------------------------------------
        public List<unoffset_qtype_accsum> searchAccSum(List<unoffset_qtype_sum> sumList)
        {
            List<unoffset_qtype_accsum> accsumList = new List<unoffset_qtype_accsum>(); //自訂unoffset_qtype_accsum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用
            #region 方法一: foreach彙總帳戶未實現損益
            //var row = new unoffset_qtype_accsum();
            //row.bqty = sumList.Sum(x => x.bqty);
            //foreach (var item in sumList)
            //{
            //    row.bqty += item.bqty;
            //    row.cost += item.cost;
            //    row.marketvalue += item.marketvalue;
            //    row.estimateAmt += item.estimateAmt;
            //    row.estimateFee += item.estimateFee;
            //    row.estimateTax += item.estimateTax;
            //    row.profit += item.profit;
            //    row.fee += item.fee;
            //    row.tax += item.tax;
            //}
            //accsumList.Add(row);
            //foreach (var item in accsumList)
            //{
            //    item.pl_ratio = decimal.Round(((item.profit / item.cost) * 100), 2).ToString() + "%";
            //    item.unoffset_qtype_sum = sumList;
            //}
            #endregion
            //方法二: 使用linq
            var row = new unoffset_qtype_accsum();
            row.bqty = sumList.Sum(x => x.bqty);
            row.cost = sumList.Sum(x => x.cost);
            row.marketvalue = sumList.Sum(x => x.marketvalue);
            row.estimateAmt = sumList.Sum(x => x.estimateAmt);
            row.estimateTax = sumList.Sum(x => x.estimateTax);
            row.profit = sumList.Sum(x => x.profit);
            row.fee = sumList.Sum(x => x.fee);
            row.tax = sumList.Sum(x => x.tax);
            row.unoffset_qtype_sum = sumList;
            accsumList.Add(row);

            accsumList.ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");
            accsumList.ForEach(x => x.unoffset_qtype_sum = sumList);
            return accsumList;
        }

        //--------------------------------------------------------------------------------------------
        //function resultListSerilizer() - 將QTYPE"0001"查詢結果 序列化為xml或json格式字串
        //--------------------------------------------------------------------------------------------
        private string resultListSerilizer(List<unoffset_qtype_accsum> accsumList, int type)
        {
            foreach (var item in accsumList)
            {
                #region 重構前 處理第三階層反序列內容 
                ////處理第三階層反序列內容 
                //int index = 0;
                //for (int i = 0; i < item.unoffset_qtype_sum.Count; i++)
                //{
                //    //初始化 unoffset_qtype_sum[].unoffset_qtype_detail 清單長度為detaillist長度
                //    List<unoffset_qtype_detail> initialization = new List<unoffset_qtype_detail>();
                //    int count = 0;
                //    while (count < detailList.Count)
                //    {
                //        initialization.Add(null);
                //        count++;
                //    }
                //    item.unoffset_qtype_sum[i].unoffset_qtype_detail = initialization;

                //    for (int j = index; j < detailList.Count; j++)
                //    {
                //        //合併第三階層相同股票代號到第二階層
                //        if (item.unoffset_qtype_sum[i].stock.Equals(detailList[j].stock))
                //        {
                //            item.unoffset_qtype_sum[i].unoffset_qtype_detail[j] = detailList[j];
                //        }
                //        else
                //        {
                //            index = j;
                //            break;
                //        }
                //    }
                //    item.unoffset_qtype_sum[i].unoffset_qtype_detail.RemoveAll(s => s == null);
                //}
                #endregion

                //透過unoffset_qtype_accsum自訂類別反序列 -> accsumSer
                var accsumSer = new unoffset_qtype_accsum()
                {
                    errcode = "0000",
                    errmsg = "成功",
                    bqty = item.bqty,
                    marketvalue = item.marketvalue,
                    fee = item.fee,
                    tax = item.tax,
                    cost = item.cost,
                    estimateAmt = item.estimateAmt,
                    estimateFee = item.estimateFee,
                    estimateTax = item.estimateTax,
                    profit = item.profit,
                    pl_ratio = item.pl_ratio,
                    unoffset_qtype_sum = item.unoffset_qtype_sum
                };
                //序列化為xml格式字串
                if (type == 0)
                {
                    using (var stringwriter = new System.IO.StringWriter())
                    {
                        var serializer = new XmlSerializer(typeof(unoffset_qtype_accsum));
                        serializer.Serialize(stringwriter, accsumSer);
                        return stringwriter.ToString();
                    }
                }
                //序列化為json格式字串
                else if (type == 1)
                {
                    var settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                    string jsonString = JsonConvert.SerializeObject(accsumSer, settings);
                    return jsonString.ToString();
                }
            }
            return string.Empty;
        }

        //--------------------------------------------------------------------------------------------
        //function resultErrListSerilizer() - 將查詢失敗結果 序列化為xml或json格式字串
        //--------------------------------------------------------------------------------------------
        private string resultErrListSerilizer(int type)
        {
            //透過unoffset_qtype_accsum自訂類別反序列 -> accsumSer
            var accsumSer = new unoffset_qtype_accsum()
            {
                errcode = "0001",
                errmsg = "查詢失敗",
            };
            //序列化為xml格式字串
            if (type == 0)
            {
                using (var stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(unoffset_qtype_accsum));
                    serializer.Serialize(stringwriter, accsumSer);
                    return stringwriter.ToString();
                }
            }
            //序列化為json格式字串
            else if (type == 1)
            {
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting = Newtonsoft.Json.Formatting.Indented;
                string jsonString = JsonConvert.SerializeObject(accsumSer, settings);
                return jsonString.ToString();
            }
            return string.Empty;
        }
    }
}
