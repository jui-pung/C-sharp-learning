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
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ESMP.STOCK.TASK.API
{
    public class GainPay
    {
        int _type;                                          //查詢與回覆格式設定
        string _searchStr;                                  //查詢xml或json格式字串
        SqlSearch _sqlSearch = new SqlSearch();             //自訂SqlSearch類別 (ESMP.STOCK.TASK.API)                                           
        /// <summary>
        /// 已實現損益查詢的對外接口function
        /// </summary>
        /// <param name="QTYPE">查詢類別</param>
        /// <param name="BHNO">分公司</param>
        /// <param name="CSEQ">帳號</param>
        /// <param name="SDATE">查詢起日</param>
        /// <param name="EDATE">查詢迄日</param>
        /// <param name="stockSymbol">股票代號</param>
        /// <param name="TTYPE">交易類別</param>
        /// <param name="type">回覆格式</param>
        /// <returns></returns>
        public (string,string) getGainPaySearch(string QTYPE, string BHNO, string CSEQ, string SDATE, string EDATE, string stockSymbol, string TTYPE, int type)
        {
            _type = type;
            List<TCNUD> TCNUDList = new List<TCNUD>();                                      //自訂TCNUD類別List (ESMP.STOCK.DB.TABLE.API)
            List<TMHIO> TMHIOList = new List<TMHIO>();                                      //自訂TMHIO類別List (ESMP.STOCK.DB.TABLE.API)
            List<TCSIO> TCSIOList = new List<TCSIO>();                                      //自訂TCSIO類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCNRH> HCNRHList = new List<HCNRH>();                                      //自訂HCNRH類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCMIO> HCMIOList = new List<HCMIO>();                                      //自訂HCMIO類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCNRH> addHCNRHList = new List<HCNRH>();                                   //自訂HCNRH類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCNTD> HCNTDList = new List<HCNTD>();                                      //自訂HCNTD類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCNTD> addHCNTDList = new List<HCNTD>();                                   //自訂HCNTD類別List (ESMP.STOCK.DB.TABLE.API)
            List<TCNTD> TCNTDList = new List<TCNTD>();                                      //自訂TCNTD類別List (ESMP.STOCK.DB.TABLE.API)
            List<T210> T210List = new List<T210>();                                         //自訂T210類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCRRH> HCRRHList = new List<HCRRH>();                                      //自訂HCRRH類別List (ESMP.STOCK.DB.TABLE.API)
            List<HDBRH> HDBRHList = new List<HDBRH>();                                      //自訂HDBRH類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCDTD> HCDTDList = new List<HCDTD>();                                      //自訂HCDTD類別List (ESMP.STOCK.DB.TABLE.API)
            List<profit_sum> sumProfitList = new List<profit_sum>();                        //自訂profit_sum類別List            (階層二:個股已實現損益)  
            List<profit_accsum> accsumProfitList = new List<profit_accsum>();               //自訂profit_accsum類別List         (階層一:帳戶已實現損益)  
            string txtSearchContent = "";
            string txtSearchResultContent = "";

            //取得查詢xml或json格式字串
            _searchStr = searchSerilizer(QTYPE, BHNO, CSEQ, SDATE, EDATE, stockSymbol, TTYPE, _type);
            txtSearchContent = _searchStr;
            //取得查詢字串Element
            var obj = GetElement(_searchStr, _type);
            root SearchElement = obj as root;
            //查詢資料庫資料
            TCNUDList = _sqlSearch.selectTCNUD(SearchElement);
            TMHIOList = _sqlSearch.selectTMHIO(SearchElement);
            TCSIOList = _sqlSearch.selectTCSIO(SearchElement);
            HCNRHList = _sqlSearch.selectHCNRH(SearchElement);
            HCNTDList = _sqlSearch.selectHCNTD(SearchElement);
            TCNTDList = _sqlSearch.selectTCNTD(SearchElement);
            T210List = _sqlSearch.selectT210(SearchElement);
            //查詢歷史融資沖銷檔 ---HCRRH
            if (SearchElement.ttype == "1")
                HCRRHList = _sqlSearch.selectHCRRH(SearchElement);
            //查詢歷史融券沖銷檔 ---HDBRH
            else if (SearchElement.ttype == "2")
                HDBRHList = _sqlSearch.selectHDBRH(SearchElement);
            //查詢歷史信用當沖檔 ---HCDTD
            else if (SearchElement.ttype == "3")
                HCDTDList = _sqlSearch.selectHCDTD(SearchElement);
            else if (SearchElement.ttype == "A")
            {
                HCRRHList = _sqlSearch.selectHCRRH(SearchElement);
                HDBRHList = _sqlSearch.selectHDBRH(SearchElement);
                HCDTDList = _sqlSearch.selectHCDTD(SearchElement);
            }

            //盤中現股沖銷 當沖 處理
            (TCNUDList, addHCNRHList, addHCNTDList, HCMIOList) = ESMPData.GetESMPData(TCNUDList, TMHIOList, TCSIOList, TCNTDList, T210List, BHNO, CSEQ);
            HCNRHList = HCNRHList.Concat(addHCNRHList).ToList();
            HCNTDList = HCNTDList.Concat(addHCNTDList).ToList();
            //全部
            if (TTYPE == "A" && HCNRHList.Count > 0 || HCRRHList.Count > 0 || HDBRHList.Count > 0 || HCDTDList.Count > 0 || HCNTDList.Count > 0)
                sumProfitList = SearchSum(HCNRHList).Concat(SearchSum(HCRRHList)).Concat(SearchSum(HDBRHList)).Concat(SearchSum(HCDTDList)).Concat(SearchSum(HCNTDList)).ToList();
            //現股
            else if (TTYPE == "0" && HCNRHList.Count > 0)
                sumProfitList = SearchSum(HCNRHList);
            //融資
            else if (TTYPE == "1" && HCRRHList.Count > 0)
                sumProfitList = SearchSum(HCRRHList);
            //融券
            else if (TTYPE == "2" && HDBRHList.Count > 0)
                sumProfitList = SearchSum(HDBRHList);
            //信用當沖
            else if (TTYPE == "3" && HCDTDList.Count > 0)
                sumProfitList = SearchSum(HCDTDList);
            //現股當沖
            else if (TTYPE == "4" && HCNTDList.Count > 0)
                sumProfitList = SearchSum(HCNTDList);
            accsumProfitList = SearchAccSum(sumProfitList);
            txtSearchResultContent = ResultListSerilizer(accsumProfitList, _type);
            return (txtSearchContent, txtSearchResultContent);
        }
        //--------------------------------------------------------------------------------------------
        //function SearchSerilizer() - 將輸入的查詢資訊序列化為xml格式字串
        //--------------------------------------------------------------------------------------------
        private string searchSerilizer(string QTYPE, string BHNO, string CSEQ, string SDATE, string EDATE, string stockSymbol, string TTYPE, int type)
        {
            var root = new root()
            {
                qtype = QTYPE,
                bhno = BHNO,
                cseq = CSEQ,
                sdate = SDATE,
                edate = EDATE,
                stockSymbol = stockSymbol,
                ttype = TTYPE,
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

        /// <summary>
        /// 計算取得 查詢回復階層二 個股已實現損益(現股沖銷)
        /// </summary>
        /// <param name="dbHCNRH"></param>
        /// <returns></returns>
        protected static List<profit_sum> SearchSum(List<HCNRH> dbHCNRH)
        {
            string BHNO = dbHCNRH.First().BHNO;
            string CSEQ = dbHCNRH.First().CSEQ;
            List<profit_sum> sumList = new List<profit_sum>();              //自訂profit_sum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用
            
            //依照交易日、賣出委託書號、賣出分單號產生新的清單群組grp_HCNRH
            var grp_HCNRH = dbHCNRH.GroupBy(d => new { d.TDATE, d.SDSEQ, d.SDNO }).Select(grp => grp.ToList()).ToList();

            //迴圈處理grp_HCNRH群組資料 存入個股彙總資料 List
            foreach (var grp_HCNRH_item in grp_HCNRH)
            {
                //取得個股明細資料 (買入) List (第三階層)
                List<profit_detail> lst_detail = new List<profit_detail>();
                foreach (var item in grp_HCNRH_item)
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
                lst_detail.Where(x => x.cost > 0).ToList().ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");
                lst_detail.Where(x => x.cost == 0).ToList().ForEach(x => x.pl_ratio = "0%");


                //取得個股明細資料 (賣出) Class (第三階層)
                profit_detail_out detail_out = new profit_detail_out();
                detail_out.tdate = grp_HCNRH_item.First().TDATE;
                detail_out.dseq = grp_HCNRH_item.First().SDSEQ;
                detail_out.dno = grp_HCNRH_item.First().SDNO;
                detail_out.mqty = grp_HCNRH_item.First().SQTY;
                detail_out.cqty = grp_HCNRH_item.Sum(s => s.CQTY);
                detail_out.mprice = grp_HCNRH_item.First().SPRICE.ToString();
                detail_out.mamt = (detail_out.mqty * Convert.ToDecimal(detail_out.mprice)).ToString();
                detail_out.cost = grp_HCNRH_item.Sum(s => s.COST);
                detail_out.income = grp_HCNRH_item.Sum(s => s.INCOME);
                detail_out.netamt = grp_HCNRH_item.Sum(s => s.INCOME);
                detail_out.fee = grp_HCNRH_item.Sum(s => s.SFEE);
                detail_out.tax = grp_HCNRH_item.Sum(s => s.TAX);
                detail_out.wtype = grp_HCNRH_item.First().WTYPE;
                detail_out.profit = grp_HCNRH_item.Sum(s => s.PROFIT);
                detail_out.ttypename2 = "現賣";

                //字典搜尋此股票 中文名稱
                string cname = "";
                if (BasicData._MSTMB_Dic.ContainsKey(grp_HCNRH_item.First().STOCK))
                    cname = BasicData._MSTMB_Dic[grp_HCNRH_item.First().STOCK][0].CNAME;
                else
                    cname = "";             //如果查不到股票中文名稱, 假設中文名稱為" "

                //取得個股彙總資料 List (第二階層)
                profit_sum profitSum = new profit_sum();
                profitSum.bhno = BHNO;
                profitSum.cseq = CSEQ;
                profitSum.tdate = detail_out.tdate;
                profitSum.dseq = detail_out.dseq;
                profitSum.dno = detail_out.dno;
                profitSum.stock = grp_HCNRH_item.First().STOCK;
                profitSum.stocknm = cname;
                profitSum.cqty = detail_out.cqty;
                profitSum.mprice = detail_out.mprice;
                profitSum.fee = detail_out.fee;
                profitSum.tax = detail_out.tax;
                profitSum.cost = detail_out.cost;
                profitSum.income = detail_out.income;
                profitSum.profit = detail_out.profit;
                if (profitSum.cost != 0)
                    profitSum.pl_ratio = decimal.Round(((profitSum.profit / profitSum.cost) * 100), 2).ToString() + "%";
                else
                    profitSum.pl_ratio = "0";
                profitSum.ttypename2 = "現賣";
                //第三階層資料存入第二階層List
                profitSum.profit_detail = lst_detail;
                profitSum.profit_detail_out = detail_out;
                sumList.Add(profitSum);
            }
            return sumList;
        }

        /// <summary>
        /// 計算取得 查詢回復階層二 個股已實現損益(現股當沖)
        /// </summary>
        /// <param name="dbHCNTD"></param>
        /// <param name="BHNO"></param>
        /// <param name="CSEQ"></param>
        /// <returns></returns>
        protected static List<profit_sum> SearchSum(List<HCNTD> dbHCNTD)
        {
            string BHNO = dbHCNTD.First().BHNO;
            string CSEQ = dbHCNTD.First().CSEQ;

            List<profit_sum> sumList = new List<profit_sum>();              //自訂profit_sum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用

            //依照交易日、賣出委託書號、賣出分單號產生新的清單群組grp_HCNRH
            var grp_HCNTD = dbHCNTD.GroupBy(d => new { d.TDATE, d.SDSEQ, d.SDNO }).Select(grp => grp.ToList()).ToList();

            //迴圈處理grp_HCNRH群組資料 存入個股彙總資料 List
            foreach (var grp_HCNTD_item in grp_HCNTD)
            {
                //取得個股明細資料 (買入) List (第三階層)
                List<profit_detail> lst_detail = new List<profit_detail>();
                foreach (var item in grp_HCNTD_item)
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
                lst_detail.Where(x => x.cost > 0).ToList().ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");
                lst_detail.Where(x => x.cost == 0).ToList().ForEach(x => x.pl_ratio = "0%");

                //取得個股明細資料 (賣出) Class (第三階層)
                profit_detail_out detail_out = new profit_detail_out();
                detail_out.tdate = grp_HCNTD_item.First().TDATE;
                detail_out.dseq = grp_HCNTD_item.First().SDSEQ;
                detail_out.dno = grp_HCNTD_item.First().SDNO;
                detail_out.mqty = grp_HCNTD_item.First().SQTY;
                detail_out.cqty = grp_HCNTD_item.Sum(s => s.CQTY);
                detail_out.mprice = grp_HCNTD_item.First().SPRICE.ToString();
                detail_out.mamt = (detail_out.mqty * Convert.ToDecimal(detail_out.mprice)).ToString();
                detail_out.cost = grp_HCNTD_item.Sum(s => s.COST);
                detail_out.income = grp_HCNTD_item.Sum(s => s.INCOME);
                detail_out.netamt = grp_HCNTD_item.Sum(s => s.INCOME);
                detail_out.fee = grp_HCNTD_item.Sum(s => s.SFEE);
                detail_out.tax = grp_HCNTD_item.Sum(s => s.TAX);
                detail_out.wtype = "0";
                detail_out.profit = grp_HCNTD_item.Sum(s => s.PROFIT);
                detail_out.ttypename2 = "賣沖";

                //字典搜尋此股票 中文名稱
                string cname = "";
                if (BasicData._MSTMB_Dic.ContainsKey(grp_HCNTD_item.First().STOCK))
                    cname = BasicData._MSTMB_Dic[grp_HCNTD_item.First().STOCK][0].CNAME;
                else
                    cname = "";             //如果查不到股票中文名稱, 假設中文名稱為" "

                //取得個股彙總資料 List (第二階層)
                profit_sum profitSum = new profit_sum();
                profitSum.bhno = BHNO;
                profitSum.cseq = CSEQ;
                profitSum.tdate = detail_out.tdate;
                profitSum.dseq = detail_out.dseq;
                profitSum.dno = detail_out.dno;
                profitSum.stock = grp_HCNTD_item.First().STOCK;
                profitSum.stocknm = cname;
                profitSum.cqty = detail_out.cqty;
                profitSum.mprice = detail_out.mprice;
                profitSum.fee = detail_out.fee;
                profitSum.tax = detail_out.tax;
                profitSum.cost = detail_out.cost;
                profitSum.income = detail_out.income;
                profitSum.profit = detail_out.profit;
                if (profitSum.cost > 0)
                    profitSum.pl_ratio = decimal.Round(((profitSum.profit / profitSum.cost) * 100), 2).ToString() + "%";
                else
                    profitSum.pl_ratio = "0 %";
                profitSum.ttypename2 = "現沖";
                //第三階層資料存入第二階層List
                profitSum.profit_detail = lst_detail;
                profitSum.profit_detail_out = detail_out;
                sumList.Add(profitSum);
            }
            return sumList;
        }

        /// <summary>
        /// 計算取得 查詢回復階層二 個股已實現損益(融資)
        /// </summary>
        /// <param name="HCRRH"></param>
        /// <returns></returns>
        protected static List<profit_sum> SearchSum(List<HCRRH> HCRRH)
        {
            string BHNO = HCRRH.First().BHNO;
            string CSEQ = HCRRH.First().CSEQ;

            List<profit_sum> sumList = new List<profit_sum>();              //自訂profit_sum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用

            //依照交易日、委託書號、分單號產生新的清單群組grp_HCRRH
            var grp_HCRRH = HCRRH.GroupBy(d => new { d.TDATE, d.DSEQ, d.DNO }).Select(grp => grp.ToList()).ToList();

            //迴圈處理grp_HCRRH群組資料 存入個股彙總資料 List
            foreach (var grp_HCRRH_item in grp_HCRRH)
            {
                //取得個股明細資料 (買入) List (第三階層)
                List<profit_detail> lst_detail = new List<profit_detail>();
                foreach (var item in grp_HCRRH_item)
                {
                    var row = new profit_detail();
                    row.tdate = item.RDATE;
                    row.dseq = item.RDSEQ;
                    row.dno = item.RDNO;
                    row.mqty = item.BQTY;
                    row.cqty = item.CQTY;
                    row.mprice = item.BPRICE.ToString();
                    row.cost = item.COST;
                    row.income = item.INCOME;
                    row.netamt = item.COST * -1;
                    row.ccramt = item.CCRAMT;
                    row.cdnamt = 0;
                    row.cgtamt = 0;
                    row.fee = item.BFEE;
                    row.interest = 0;
                    row.tax = 0;
                    row.dbfee = 0;
                    row.dlfee = 0;
                    row.adjdate = string.Empty;
                    row.ttype = "1";
                    if (row.ttype == "1")
                        row.ttypename = "融資";
                    row.bstype = "B";
                    row.wtype = "0";
                    row.profit = item.PROFIT;
                    row.ctype = "1";
                    row.ioflag = "0";
                    row.ioname = " ";
                    row.ttypename2 = "資買";
                    lst_detail.Add(row);
                }
                //計算明細資料(買入)的成交價金、報酬率
                lst_detail.ForEach(x => x.mamt = (x.mqty * Convert.ToDecimal(x.mprice)).ToString());
                lst_detail.Where(x => x.cost > 0).ToList().ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");
                lst_detail.Where(x => x.cost == 0).ToList().ForEach(x => x.pl_ratio = "0%");

                //取得個股明細資料 (賣出) Class (第三階層)
                profit_detail_out detail_out = new profit_detail_out();
                detail_out.tdate = grp_HCRRH_item.First().TDATE;
                detail_out.dseq = grp_HCRRH_item.First().DSEQ;
                detail_out.dno = grp_HCRRH_item.First().DNO;
                detail_out.mqty = grp_HCRRH_item.First().SQTY;
                detail_out.cqty = grp_HCRRH_item.Sum(s => s.CQTY);
                detail_out.mprice = grp_HCRRH_item.First().SPRICE.ToString();
                detail_out.mamt = (detail_out.mqty * Convert.ToDecimal(detail_out.mprice)).ToString();
                detail_out.cost = grp_HCRRH_item.Sum(s => s.COST);
                detail_out.income = grp_HCRRH_item.Sum(s => s.INCOME);
                detail_out.netamt = detail_out.income;
                detail_out.ccramt = grp_HCRRH_item.Sum(s => s.CCRAMT);
                detail_out.cdnamt = 0;
                detail_out.cgtamt = 0;
                detail_out.fee = grp_HCRRH_item.Sum(s => s.SFEE);
                detail_out.interest = grp_HCRRH_item.Sum(s => s.CCRINT);
                detail_out.tax = grp_HCRRH_item.Sum(s => s.TAX);
                detail_out.dbfee = 0;
                detail_out.dlfee = 0;
                detail_out.adjdate = string.Empty;
                detail_out.ttype = "1";
                if (detail_out.ttype == "1")
                    detail_out.ttypename = "融資";
                detail_out.bstype = "S";
                detail_out.wtype = "0";
                detail_out.profit = grp_HCRRH_item.Sum(s => s.PROFIT);
                detail_out.ctype = "1";
                detail_out.ttypename2 = "資賣";

                //字典搜尋此股票 中文名稱
                string cname = "";
                if (BasicData._MSTMB_Dic.ContainsKey(grp_HCRRH_item.First().STOCK))
                    cname = BasicData._MSTMB_Dic[grp_HCRRH_item.First().STOCK][0].CNAME;
                else
                    cname = "";             //如果查不到股票中文名稱, 假設中文名稱為" "

                //取得個股彙總資料 List (第二階層)
                profit_sum profitSum = new profit_sum();
                profitSum.bhno = BHNO;
                profitSum.cseq = CSEQ;
                profitSum.tdate = detail_out.tdate;
                profitSum.dseq = detail_out.dseq;
                profitSum.dno = detail_out.dno;
                profitSum.ttype = detail_out.ttype;
                profitSum.ttypename = detail_out.ttypename;
                profitSum.bstype = "S";
                profitSum.stock = grp_HCRRH_item.First().STOCK;
                profitSum.stocknm = cname;
                profitSum.cqty = detail_out.cqty;
                profitSum.mprice = detail_out.mprice;
                profitSum.ccramt = lst_detail.Sum(x => x.ccramt);
                profitSum.cdnamt = lst_detail.Sum(x => x.cdnamt);
                profitSum.cgtamt = lst_detail.Sum(x => x.cgtamt);
                profitSum.fee = detail_out.fee;
                profitSum.interest = lst_detail.Sum(x => x.interest);
                profitSum.tax = detail_out.tax;
                profitSum.dbfee = lst_detail.Sum(x => x.dbfee);
                profitSum.cost = detail_out.cost;
                profitSum.income = detail_out.income;
                profitSum.profit = detail_out.profit;
                if (profitSum.cost > 0)
                    profitSum.pl_ratio = decimal.Round(((profitSum.profit / profitSum.cost) * 100), 2).ToString() + "%";
                else
                    profitSum.pl_ratio = "0 %";
                profitSum.ctype = "1";
                profitSum.ttypename2 = "資賣";
                //第三階層資料存入第二階層List
                profitSum.profit_detail = lst_detail;
                profitSum.profit_detail_out = detail_out;
                sumList.Add(profitSum);
            }
            return sumList;
        }

        /// <summary>
        /// 計算取得 查詢回復階層二 個股已實現損益(融券)
        /// </summary>
        /// <param name="HDBRH"></param>
        /// <returns></returns>
        protected static List<profit_sum> SearchSum(List<HDBRH> HDBRH)
        {
            string BHNO = HDBRH.First().BHNO;
            string CSEQ = HDBRH.First().CSEQ;
            List<profit_sum> sumList = new List<profit_sum>();              //自訂profit_sum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用

            //依照交易日、委託書號、分單號產生新的清單群組grp_HDBRH
            var grp_HDBRH = HDBRH.GroupBy(d => new { d.TDATE, d.DSEQ, d.DNO }).Select(grp => grp.ToList()).ToList();

            //迴圈處理grp_HCRRH群組資料 存入個股彙總資料 List
            foreach (var grp_HDBRH_item in grp_HDBRH)
            {
                //取得個股明細資料 (券賣) List (第三階層)
                List<profit_detail> lst_detail = new List<profit_detail>();
                foreach (var item in grp_HDBRH_item)
                {
                    var row = new profit_detail();
                    row.tdate = item.RDATE;
                    row.dseq = item.RDSEQ;
                    row.dno = item.RDNO;
                    row.mqty = item.SQTY;
                    row.cqty = item.CQTY;
                    row.mprice = item.SPRICE.ToString();
                    row.cost = item.COST;
                    row.income = item.INCOME;
                    row.netamt = item.COST * -1;
                    row.ccramt = 0;
                    row.cdnamt = item.CDBAMT;
                    row.cgtamt = item.CGTAMT;
                    row.fee = item.SFEE;
                    row.interest = 0;
                    row.tax = item.TAX;
                    row.dbfee = item.DBFEE;
                    row.dlfee = item.CDLFEE;
                    row.adjdate = string.Empty;
                    row.ttype = "2";
                    if (row.ttype == "2")
                        row.ttypename = "融券";
                    row.bstype = "S";
                    row.wtype = "0";
                    row.profit = item.PROFIT;
                    row.ctype = "2";
                    row.ioflag = "0";
                    row.ioname = " ";
                    row.ttypename2 = "券賣";
                    lst_detail.Add(row);
                }
                //計算明細資料(券賣)的成交價金、報酬率
                lst_detail.ForEach(x => x.mamt = (x.mqty * Convert.ToDecimal(x.mprice)).ToString());
                lst_detail.Where(x => x.cost > 0).ToList().ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");
                lst_detail.Where(x => x.cost == 0).ToList().ForEach(x => x.pl_ratio = "0%");

                //取得個股明細資料 (券買) Class (第三階層)
                profit_detail_out detail_out = new profit_detail_out();
                detail_out.tdate = grp_HDBRH_item.First().TDATE;
                detail_out.dseq = grp_HDBRH_item.First().DSEQ;
                detail_out.dno = grp_HDBRH_item.First().DNO;
                detail_out.mqty = grp_HDBRH_item.First().BQTY;
                detail_out.cqty = grp_HDBRH_item.Sum(s => s.CQTY);
                detail_out.mprice = grp_HDBRH_item.First().BPRICE.ToString();
                detail_out.mamt = (detail_out.mqty * Convert.ToDecimal(detail_out.mprice)).ToString();
                detail_out.cost = grp_HDBRH_item.Sum(s => s.COST);
                detail_out.income = grp_HDBRH_item.Sum(s => s.INCOME);
                detail_out.netamt = detail_out.income;
                detail_out.ccramt = 0;
                detail_out.cdnamt = 0;
                detail_out.cgtamt = 0;
                detail_out.fee = grp_HDBRH_item.Sum(s => s.BFEE);
                detail_out.interest = grp_HDBRH_item.Sum(s => s.CGTINT) + grp_HDBRH_item.Sum(s => s.CDNINT);
                detail_out.tax = grp_HDBRH_item.Sum(s => s.TAX);
                detail_out.dbfee = 0;
                detail_out.dlfee = 0;
                detail_out.adjdate = string.Empty;
                detail_out.ttype = "2";
                if (detail_out.ttype == "2")
                    detail_out.ttypename = "融券";
                detail_out.bstype = "B";
                detail_out.wtype = "0";
                detail_out.profit = grp_HDBRH_item.Sum(s => s.PROFIT);
                detail_out.ctype = "2";
                detail_out.ttypename2 = "券買";

                //字典搜尋此股票 中文名稱
                string cname = "";
                if (BasicData._MSTMB_Dic.ContainsKey(grp_HDBRH_item.First().STOCK))
                    cname = BasicData._MSTMB_Dic[grp_HDBRH_item.First().STOCK][0].CNAME;
                else
                    cname = "";             //如果查不到股票中文名稱, 假設中文名稱為" "

                //取得個股彙總資料 List (第二階層)
                profit_sum profitSum = new profit_sum();
                profitSum.bhno = BHNO;
                profitSum.cseq = CSEQ;
                profitSum.tdate = detail_out.tdate;
                profitSum.dseq = detail_out.dseq;
                profitSum.dno = detail_out.dno;
                profitSum.ttype = detail_out.ttype;
                profitSum.ttypename = detail_out.ttypename;
                profitSum.bstype = "B";
                profitSum.stock = grp_HDBRH_item.First().STOCK;
                profitSum.stocknm = cname;
                profitSum.cqty = detail_out.cqty;
                profitSum.mprice = detail_out.mprice;
                profitSum.ccramt = lst_detail.Sum(x => x.ccramt);
                profitSum.cdnamt = lst_detail.Sum(x => x.cdnamt);
                profitSum.cgtamt = lst_detail.Sum(x => x.cgtamt);
                profitSum.fee = detail_out.fee;
                profitSum.interest = lst_detail.Sum(x => x.interest);
                profitSum.tax = detail_out.tax;
                profitSum.dbfee = lst_detail.Sum(x => x.dbfee);
                profitSum.cost = detail_out.cost;
                profitSum.income = detail_out.income;
                profitSum.profit = detail_out.profit;
                if (profitSum.cost > 0)
                    profitSum.pl_ratio = decimal.Round(((profitSum.profit / profitSum.cost) * 100), 2).ToString() + "%";
                else
                    profitSum.pl_ratio = "0 %";
                profitSum.ctype = "2";
                profitSum.ttypename2 = "券買";
                //第三階層資料存入第二階層List
                profitSum.profit_detail = lst_detail;
                profitSum.profit_detail_out = detail_out;
                sumList.Add(profitSum);
            }
            return sumList;
        }

        /// <summary>
        /// 計算取得 查詢回復階層二 個股已實現損益(信用當沖)
        /// </summary>
        /// <param name="HCDTD"></param>
        /// <returns></returns>
        protected static List<profit_sum> SearchSum(List<HCDTD> HCDTD)
        {
            string BHNO = HCDTD.First().BHNO;
            string CSEQ = HCDTD.First().CSEQ;

            List<profit_sum> sumList = new List<profit_sum>();              //自訂profit_sum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用

            //依照交易日、券委託書號、券分單號產生新的清單群組grp_HCDTD
            var grp_HCDTD = HCDTD.GroupBy(d => new { d.TDATE, d.DBDSEQ, d.DBDNO }).Select(grp => grp.ToList()).ToList();

            //迴圈處理grp_HCDTD群組資料 存入個股彙總資料 List
            foreach (var grp_HCDTD_item in grp_HCDTD)
            {
                //取得個股明細資料 (買入) List (第三階層)
                List<profit_detail> lst_detail = new List<profit_detail>();
                foreach (var item in grp_HCDTD_item)
                {
                    var row = new profit_detail();
                    row.tdate = item.TDATE;
                    row.dseq = item.CRDSEQ; //融資委託書號
                    row.dno = item.CRDNO;   //融資分單號
                    row.mqty = item.BQTY;
                    row.cqty = item.QTY;    //沖抵股數
                    row.mprice = item.BPRICE.ToString();
                    row.cost = item.COST;
                    row.income = item.INCOME;
                    row.netamt = item.COST * -1;
                    row.ccramt = 0;
                    row.cdnamt = 0;
                    row.cgtamt = 0;
                    row.fee = item.BFEE;
                    row.interest = 0;
                    row.tax = item.TAX;
                    row.dbfee = 0; //??
                    row.dlfee = 0;
                    row.adjdate = string.Empty;
                    row.ttype = "2";
                    if (row.ttype == "2")
                        row.ttypename = "融券";
                    row.bstype = "B";
                    row.wtype = "0";
                    row.profit = item.PROFIT;
                    row.ctype = "3";
                    row.ioflag = "0";
                    row.ioname = " ";
                    row.ttypename2 = "資買";
                    lst_detail.Add(row);
                }
                //計算明細資料(券賣)的成交價金、報酬率
                lst_detail.ForEach(x => x.mamt = (x.mqty * Convert.ToDecimal(x.mprice)).ToString());
                lst_detail.Where(x => x.cost > 0).ToList().ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");
                lst_detail.Where(x => x.cost == 0).ToList().ForEach(x => x.pl_ratio = "0%");

                //取得個股明細資料 (券買) Class (第三階層)
                profit_detail_out detail_out = new profit_detail_out();
                detail_out.tdate = grp_HCDTD_item.First().TDATE;
                detail_out.dseq = grp_HCDTD_item.First().DBDSEQ;
                detail_out.dno = grp_HCDTD_item.First().DBDNO;
                detail_out.mqty = grp_HCDTD_item.First().SQTY;
                detail_out.cqty = grp_HCDTD_item.Sum(s => s.QTY);
                detail_out.mprice = grp_HCDTD_item.First().SPRICE.ToString();
                detail_out.mamt = (detail_out.mqty * Convert.ToDecimal(detail_out.mprice)).ToString();
                detail_out.cost = grp_HCDTD_item.Sum(s => s.COST);
                detail_out.income = grp_HCDTD_item.Sum(s => s.INCOME);
                detail_out.netamt = detail_out.income;
                detail_out.ccramt = 0;
                detail_out.cdnamt = 0;
                detail_out.cgtamt = 0;
                detail_out.fee = grp_HCDTD_item.Sum(s => s.SFEE);
                detail_out.interest = 0;
                detail_out.tax = grp_HCDTD_item.Sum(s => s.TAX);
                detail_out.dbfee = 0;
                detail_out.dlfee = 0;
                detail_out.adjdate = string.Empty;
                detail_out.ttype = "2";
                if (detail_out.ttype == "2")
                    detail_out.ttypename = "融券";
                detail_out.bstype = "S";
                detail_out.wtype = "0";
                detail_out.profit = grp_HCDTD_item.Sum(s => s.PROFIT);
                detail_out.ctype = "3";
                detail_out.ttypename2 = "券賣";
                

                //字典搜尋此股票 中文名稱
                string cname = "";
                if (BasicData._MSTMB_Dic.ContainsKey(grp_HCDTD_item.First().STOCK))
                    cname = BasicData._MSTMB_Dic[grp_HCDTD_item.First().STOCK][0].CNAME;
                else
                    cname = "";             //如果查不到股票中文名稱, 假設中文名稱為" "

                //取得個股彙總資料 List (第二階層)
                profit_sum profitSum = new profit_sum();
                profitSum.bhno = BHNO;
                profitSum.cseq = CSEQ;
                profitSum.tdate = detail_out.tdate;
                profitSum.dseq = detail_out.dseq;
                profitSum.dno = detail_out.dno;
                profitSum.ttype = detail_out.ttype;
                profitSum.ttypename = detail_out.ttypename;
                profitSum.bstype = "S";
                profitSum.stock = grp_HCDTD_item.First().STOCK;
                profitSum.stocknm = cname;
                profitSum.cqty = detail_out.cqty;
                profitSum.mprice = detail_out.mprice;
                profitSum.ccramt = lst_detail.Sum(x => x.ccramt);
                profitSum.cdnamt = lst_detail.Sum(x => x.cdnamt);
                profitSum.cgtamt = lst_detail.Sum(x => x.cgtamt);
                profitSum.fee = detail_out.fee;
                profitSum.interest = lst_detail.Sum(x => x.interest);
                profitSum.tax = detail_out.tax;
                profitSum.dbfee = lst_detail.Sum(x => x.dbfee);
                profitSum.cost = detail_out.cost;
                profitSum.income = detail_out.income;
                profitSum.profit = detail_out.profit;
                if (profitSum.cost > 0)
                    profitSum.pl_ratio = decimal.Round(((profitSum.profit / profitSum.cost) * 100), 2).ToString() + "%";
                else
                    profitSum.pl_ratio = "0 %";
                profitSum.ctype = "3";
                profitSum.ttypename2 = "信沖";
                //第三階層資料存入第二階層List
                profitSum.profit_detail = lst_detail;
                profitSum.profit_detail_out = detail_out;
                sumList.Add(profitSum);
            }
            return sumList;
        }

        //--------------------------------------------------------------------------------------------
        // function SearchAccSum() - 計算取得 查詢回復階層一 帳戶已實現損益
        //--------------------------------------------------------------------------------------------
        protected static List<profit_accsum> SearchAccSum(List<profit_sum> sumList)
        {
            List<profit_accsum> accsumList = new List<profit_accsum>();         //自訂profit_accsum類別List (ESMP.STOCK.FORMAT.API) -函式回傳使用

            var row = new profit_accsum();
            row.cqty = sumList.Sum(x => x.cqty);
            row.cost = sumList.Sum(x => x.cost);
            row.income = sumList.Sum(x => x.income);
            row.profit = sumList.Sum(x => x.profit);
            row.fee = sumList.Sum(x => x.fee);
            row.tax = sumList.Sum(x => x.tax);
            row.dbfee = sumList.Sum(x => x.dbfee);
            row.interest = sumList.Sum(x => x.interest);
            row.ccramt = sumList.Sum(x => x.ccramt);
            accsumList.Add(row);

            accsumList.Where(x => x.cost > 0).ToList().ForEach(x => x.pl_ratio = decimal.Round(((x.profit / x.cost) * 100), 2).ToString() + "%");
            accsumList.Where(x => x.cost == 0).ToList().ForEach(x => x.pl_ratio = "0%");

            //依照股票代號、賣出委託書號、分單號 排序sumList 並將List內容存放到accsumList的List<profit_sum> profit_sum
            List<profit_sum> sortedList = sumList.OrderBy(x => x.stock).ThenBy(n => n.dseq).ThenBy(n => n.dno).ToList();
            accsumList.ForEach(x => x.profit_sum = sortedList);

            return accsumList;
        }

        //--------------------------------------------------------------------------------------------
        //function ResultListSerilizer() - 將QTYPE"0002"查詢結果 序列化為xml或json格式字串
        //--------------------------------------------------------------------------------------------
        private string ResultListSerilizer(List<profit_accsum> accsumList, int type)
        {
            if (accsumList.Count == 0)
                return resultErrListSerilizer(type);
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
