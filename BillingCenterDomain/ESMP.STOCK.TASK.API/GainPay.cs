using ESMP.STOCK.DB.TABLE;
using ESMP.STOCK.FORMAT;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ESMP.STOCK.TASK.API
{
    public class GainPay
    {
        int _type;                                          //查詢與回覆格式設定
        string _searchStr;                                  //查詢xml或json格式字串
        SqlSearch _sqlSearch = new SqlSearch();             //自訂SqlSearch類別 (ESMP.STOCK.TASK.API)                                           

        //--------------------------------------------------------------------------------------------
        //function getGainPaySearch() - 已實現損益查詢的對外接口function
        //--------------------------------------------------------------------------------------------
        public (string,string) getGainPaySearch(string QTYPE, string BHNO, string CSEQ, string SDATE, string EDATE, string stockSymbol, int type)
        {
            _type = type;
            List<HCNRH> HCNRHList = new List<HCNRH>();                                      //自訂HCNRH類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCNTD> HCNTDList = new List<HCNTD>();                                      //自訂HCNTD類別List (ESMP.STOCK.DB.TABLE.API)
            List<profit_sum> sumProfitList_HCNRH = new List<profit_sum>();                  //自訂profit_sum類別List            (階層二:個股已實現損益)  
            List<profit_sum> sumProfitList_HCNTD = new List<profit_sum>();                  //自訂profit_sum類別List            (階層二:個股已實現損益)  
            List<profit_sum> sumProfitList = new List<profit_sum>();                        //自訂profit_sum類別List            (階層二:個股已實現損益)  
            List<profit_accsum> accsumProfitList = new List<profit_accsum>();               //自訂profit_accsum類別List         (階層一:帳戶已實現損益)  
            string txtSearchContent = "";
            string txtSearchResultContent = "";

            //取得查詢xml或json格式字串
            _searchStr = searchSerilizer(QTYPE, BHNO, CSEQ, SDATE, EDATE, stockSymbol, _type);
            txtSearchContent = _searchStr;
            //取得查詢字串Element
            var obj = GetElement(_searchStr, _type);
            root SearchElement = obj as root;
            //查詢開始...
            HCNRHList = _sqlSearch.selectHCNRH(SearchElement);
            HCNTDList = _sqlSearch.selectHCNTD(SearchElement);
            if (HCNRHList.Count > 0 || HCNTDList.Count > 0)
            {
                sumProfitList_HCNRH = searchSum_HCNRH(HCNRHList, BHNO, CSEQ);
                sumProfitList_HCNTD = searchSum_HCNTD(HCNTDList, BHNO, CSEQ);
                //合併沖銷與當沖資料
                sumProfitList = sumProfitList_HCNRH.Concat(sumProfitList_HCNTD).ToList();
                accsumProfitList = searchAccSum(sumProfitList);
                //呈現查詢結果
                txtSearchResultContent = resultListSerilizer(accsumProfitList, _type);
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
        // function searchSum_HCNRH() - 計算取得 查詢回復階層二 個股已實現損益(沖銷)
        //--------------------------------------------------------------------------------------------
        private List<profit_sum> searchSum_HCNRH(List<HCNRH> dbHCNRH, string BHNO, string CSEQ)
        {
            List<profit_sum> sumList = new List<profit_sum>();              //自訂profit_sum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用
            
            //依照交易日、賣出委託書號、賣出分單號產生新的清單群組grp_HCNRH
            var grp_HCNRH = dbHCNRH.GroupBy(d => new { d.TDATE, d.SDSEQ, d.SDNO }).Select(grp => grp.ToList()).ToList();

            //迴圈處理grp_HCNRH群組資料 存入個股彙總資料 List
            for (int i = 0; i < grp_HCNRH.Count; i++)
            {
                //取得個股明細資料 (買入) List (第三階層)
                List<profit_detail> lst_detail = new List<profit_detail>();
                foreach (var item in grp_HCNRH[i])
                {
                    var row = new profit_detail();
                    row.tdate = item.RDATE;
                    row.dseq = item.BDSEQ;
                    row.dno = item.BDNO;
                    row.mqty = item.BQTY;
                    row.cqty = item.CQTY;
                    row.mprice = item.BPRICE.ToString();
                    row.cost = item.COST;
                    row.income = item.INCOME;
                    row.netamt = item.COST * -1;
                    row.fee = item.BFEE;
                    row.adjdate = item.ADJDATE;
                    row.wtype = item.WTYPE;
                    row.profit = item.PROFIT;
                    row.ioflag = item.IOFLAG;
                    row.ioname = " ";
                    lst_detail.Add(row);
                }
                //計算明細資料(買入)的成交價金、報酬率
                lst_detail.ForEach(x => x.mamt = (x.mqty * Convert.ToDecimal(x.mprice)).ToString());
                lst_detail.ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");

                //取得個股明細資料 (賣出) Class (第三階層)
                profit_detail_out detail_out = new profit_detail_out();
                detail_out.tdate = grp_HCNRH[i].First().TDATE;
                detail_out.dseq = grp_HCNRH[i].First().SDSEQ;
                detail_out.dno = grp_HCNRH[i].First().SDNO;
                detail_out.mqty = grp_HCNRH[i].First().SQTY;
                detail_out.cqty = grp_HCNRH[i].Sum(s => s.CQTY);
                detail_out.mprice = grp_HCNRH[i].First().SPRICE.ToString();
                detail_out.cost = grp_HCNRH[i].Sum(s => s.COST);
                detail_out.income = grp_HCNRH[i].Sum(s => s.INCOME);
                detail_out.netamt = grp_HCNRH[i].Sum(s => s.INCOME);
                detail_out.fee = grp_HCNRH[i].Sum(s => s.SFEE);
                detail_out.tax = grp_HCNRH[i].Sum(s => s.TAX);
                detail_out.wtype = grp_HCNRH[i].First().WTYPE;
                detail_out.profit = grp_HCNRH[i].Sum(s => s.PROFIT);
                detail_out.ttypename2 = "現賣";

                //取得個股彙總資料 List (第二階層)
                profit_sum profitSum = new profit_sum();
                profitSum.bhno = BHNO;
                profitSum.cseq = CSEQ;
                profitSum.tdate = detail_out.tdate;
                profitSum.dseq = detail_out.dseq;
                profitSum.dno = detail_out.dno;
                profitSum.stock = grp_HCNRH[i].First().STOCK;
                profitSum.stocknm = _sqlSearch.selectStockName(grp_HCNRH[i].First().STOCK);
                profitSum.cqty = detail_out.cqty;
                profitSum.mprice = detail_out.mprice;
                profitSum.fee = detail_out.fee;
                profitSum.tax = detail_out.tax;
                profitSum.cost = detail_out.cost;
                profitSum.income = detail_out.income;
                profitSum.profit = detail_out.profit;
                profitSum.pl_ratio = decimal.Round(((profitSum.profit / profitSum.cost) * 100), 2).ToString() + "%";
                profitSum.ttypename2 = "現賣";
                profitSum.profit_detail = lst_detail;
                profitSum.profit_detail_out = detail_out;
                sumList.Add(profitSum);

                //第三階層資料存入第二階層List
            }
            return sumList;
        }

        //--------------------------------------------------------------------------------------------
        // function searchSum_HCNTD() - 計算取得 查詢回復階層二 個股已實現損益(當沖)
        //--------------------------------------------------------------------------------------------
        private List<profit_sum> searchSum_HCNTD(List<HCNTD> dbHCNTD, string BHNO, string CSEQ)
        {
            List<profit_sum> sumList = new List<profit_sum>();              //自訂profit_sum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用

            //依照交易日、賣出委託書號、賣出分單號產生新的清單群組grp_HCNRH
            var grp_HCNTD = dbHCNTD.GroupBy(d => new { d.TDATE, d.SDSEQ, d.SDNO }).Select(grp => grp.ToList()).ToList();

            //迴圈處理grp_HCNRH群組資料 存入個股彙總資料 List
            for (int i = 0; i < grp_HCNTD.Count; i++)
            {
                //取得個股明細資料 (買入) List (第三階層)
                List<profit_detail> lst_detail = new List<profit_detail>();
                foreach (var item in grp_HCNTD[i])
                {
                    var row = new profit_detail();
                    row.tdate = item.TDATE;
                    row.dseq = item.BDSEQ;
                    row.dno = item.BDNO;
                    row.mqty = item.BQTY;
                    row.cqty = item.CQTY;
                    row.mprice = item.BPRICE.ToString();
                    row.cost = item.COST;
                    row.income = item.INCOME;
                    row.netamt = item.COST * -1;
                    row.fee = item.BFEE;
                    row.wtype = "0"; //??
                    row.profit = item.PROFIT;
                    row.ioname = " ";
                    lst_detail.Add(row);
                }
                //計算明細資料(買入)的成交價金、報酬率
                lst_detail.ForEach(x => x.mamt = (x.mqty * Convert.ToDecimal(x.mprice)).ToString());
                lst_detail.ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");

                //取得個股明細資料 (賣出) Class (第三階層)
                profit_detail_out detail_out = new profit_detail_out();
                detail_out.tdate = grp_HCNTD[i].First().TDATE;
                detail_out.dseq = grp_HCNTD[i].First().SDSEQ;
                detail_out.dno = grp_HCNTD[i].First().SDNO;
                detail_out.mqty = grp_HCNTD[i].First().SQTY;
                detail_out.cqty = grp_HCNTD[i].Sum(s => s.CQTY);
                detail_out.mprice = grp_HCNTD[i].First().SPRICE.ToString();
                detail_out.cost = grp_HCNTD[i].Sum(s => s.COST);
                detail_out.income = grp_HCNTD[i].Sum(s => s.INCOME);
                detail_out.netamt = grp_HCNTD[i].Sum(s => s.INCOME);
                detail_out.fee = grp_HCNTD[i].Sum(s => s.SFEE);
                detail_out.tax = grp_HCNTD[i].Sum(s => s.TAX);
                detail_out.wtype = "0";
                detail_out.profit = grp_HCNTD[i].Sum(s => s.PROFIT);
                detail_out.ttypename2 = "賣沖";

                //取得個股彙總資料 List (第二階層)
                profit_sum profitSum = new profit_sum();
                profitSum.bhno = BHNO;
                profitSum.cseq = CSEQ;
                profitSum.tdate = detail_out.tdate;
                profitSum.dseq = detail_out.dseq;
                profitSum.dno = detail_out.dno;
                profitSum.stock = grp_HCNTD[i].First().STOCK;
                profitSum.stocknm = _sqlSearch.selectStockName(grp_HCNTD[i].First().STOCK);
                profitSum.cqty = detail_out.cqty;
                profitSum.mprice = detail_out.mprice;
                profitSum.fee = detail_out.fee;
                profitSum.tax = detail_out.tax;
                profitSum.cost = detail_out.cost;
                profitSum.income = detail_out.income;
                profitSum.profit = detail_out.profit;
                profitSum.pl_ratio = decimal.Round(((profitSum.profit / profitSum.cost) * 100), 2).ToString() + "%";
                profitSum.ttypename2 = "賣沖";
                sumList.Add(profitSum);

                //第三階層資料存入第二階層List
                sumList[i].profit_detail = lst_detail;
                sumList[i].profit_detail_out = detail_out;
            }
            return sumList;
        }

        //--------------------------------------------------------------------------------------------
        // function searchAccSum() - 計算取得 查詢回復階層一 帳戶已實現損益
        //--------------------------------------------------------------------------------------------
        private List<profit_accsum> searchAccSum(List<profit_sum> sumList)
        {
            List<profit_accsum> accsumList = new List<profit_accsum>();         //自訂profit_accsum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用

            var row = new profit_accsum();
            row.cqty = sumList.Sum(x => x.cqty);
            row.cost = sumList.Sum(x => x.cost);
            row.income = sumList.Sum(x => x.income);
            row.profit = sumList.Sum(x => x.profit);
            row.fee = sumList.Sum(x => x.fee);
            row.tax = sumList.Sum(x => x.tax);
            accsumList.Add(row);

            accsumList.ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");

            //依照股票代號、賣出委託書號、分單號 排序sumList 並將List內容存放到accsumList的List<profit_sum> profit_sum
            List<profit_sum> sortedList = sumList.OrderBy(x => x.stock).ThenBy(n => n.dseq).ThenBy(n => n.dno).ToList();
            accsumList.ForEach(x => x.profit_sum = sortedList);

            return accsumList;
        }

        //--------------------------------------------------------------------------------------------
        //function resultListSerilizer() - 將QTYPE"0002"查詢結果 序列化為xml或json格式字串
        //--------------------------------------------------------------------------------------------
        private string resultListSerilizer(List<profit_accsum> accsumList, int type)
        {
            foreach (var item in accsumList)
            {
                //透過profit_accsum自訂類別反序列 -> accsumSer
                var accsumSer = new profit_accsum()
                {
                    errcode = "0000",
                    errmsg = "成功",
                    cqty = item.cqty,
                    cost = item.cost,
                    income = item.income,
                    profit = item.profit,
                    pl_ratio = item.pl_ratio,
                    fee = item.fee,
                    tax = item.tax,
                    profit_sum = item.profit_sum
                };
                //序列化為xml格式字串
                if (type == 0)
                {
                    using (var stringwriter = new System.IO.StringWriter())
                    {
                        var serializer = new XmlSerializer(typeof(profit_accsum));
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
            //透過profit_accsum自訂類別反序列 -> accsumSer
            var accsumSer = new profit_accsum()
            {
                errcode = "0001",
                errmsg = "查詢失敗",
            };
            //序列化為xml格式字串
            if (type == 0)
            {
                using (var stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(profit_accsum));
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
