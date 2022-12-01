using ESMP.STOCK.DB.TABLE;
using ESMP.STOCK.FORMAT;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.TASK.API
{
    public class ESMPData
    {
        static SqlSearch _sqlSearch;
        static List<MSTMB> _MSTMBList = new List<MSTMB>();             //自訂MSTMB類別List (ESMP.STOCK.DB.TABLE.API)
        static List<MCUMS> _MCUMSList = new List<MCUMS>();             //自訂MCUMS類別List (ESMP.STOCK.DB.TABLE.API)
        static Dictionary<string, List<MSTMB>> _StockMSTMB_Dic;
        static Dictionary<string, List<MCUMS>> _CseqMCUMS_Dic;
        public static void CreateDic()
        {
            _sqlSearch = new SqlSearch();
            _MSTMBList = _sqlSearch.selectMSTMB();
            _MCUMSList = _sqlSearch.selectMCUMS();
            //依據 STOCK 建立 MSTMB Dictionary
            _StockMSTMB_Dic = _MSTMBList.GroupBy(x => x.STOCK).ToDictionary(x => x.Key, x => x.ToList());
            //依據 BHNO CSEQ 建立 MCUMS Dictionary
            _CseqMCUMS_Dic = _MCUMSList.GroupBy(d => d.BHNO + d.CSEQ).ToDictionary(x => x.Key, x => x.ToList());
        }
        
        public static (List<TCNUD>, List<HCNRH>, List<HCNTD>) GetESMPData(List<TCNUD> TCNUDList, List<TMHIO> TMHIOList, List<TCSIO> TCSIOList)
        {
            CreateDic();
            List<HCNRH> HCNRHList = new List<HCNRH>();          //自訂HCNRH類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCMIO> HCMIOList = new List<HCMIO>();          //自訂HCMIO類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCNTD> HCNTDList = new List<HCNTD>();          //自訂HCMIO類別List (ESMP.STOCK.DB.TABLE.API)
            //將TMHIO List與TCSIO List資料轉入(Ram)HCMIO中
            HCMIOList = GetHCMIO(TCSIOList, TMHIOList);
            //今日現股當沖處理
            (HCMIOList, HCNTDList) = DayTrade(HCMIOList);
            //重新計算現股當沖TAX、INCOME、PROFIT
            HCNTDList = CalculateHCNTD(HCNTDList);
            //今日匯入（TCSIO）加入現股餘額
            TCNUDList = AddTCSIO(TCNUDList, HCMIOList);
            //今日賣出 匯出現股扣除現股餘額資料
            (HCNRHList,TCNUDList,HCMIOList) = CurrentStockSell(TCNUDList, HCMIOList);
            //今日買進（TMHIO）加入現股餘額
            TCNUDList = AddTMHIOBuy(TCNUDList, HCMIOList);
            //剩餘賣出未沖銷，加入現股餘額
            TCNUDList = AddTMHIOSell(TCNUDList, HCMIOList);
            return (TCNUDList, HCNRHList, HCNTDList);
        }

        //--------------------------------------------------------------------------------------------
        //function GetHCMIO() - 將TCSIOList資料與TMHIOList資料加入新增至HCMIOList
        //--------------------------------------------------------------------------------------------
        protected static List<HCMIO> GetHCMIO(List<TCSIO> TCSIOList, List<TMHIO> TMHIOList)
        {
            List<HCMIO> HCMIOList = new List<HCMIO>();          //自訂HCMIO類別List (ESMP.STOCK.DB.TABLE.API)
            foreach (var TCSIO_item in TCSIOList)
            {
                var row = new HCMIO();
                row.TDATE = TCSIO_item.TDATE;
                row.BHNO = TCSIO_item.BHNO;
                row.CSEQ = TCSIO_item.CSEQ;
                row.DSEQ = TCSIO_item.DSEQ;
                row.DNO = TCSIO_item.DNO;
                row.WTYPE = "A";
                row.STOCK = TCSIO_item.STOCK;
                row.BSTYPE = TCSIO_item.BSTYPE;
                row.PRICE = 0;
                row.QTY = TCSIO_item.QTY;       //原始股數
                row.BQTY = TCSIO_item.QTY;      //未沖銷股數
                row.AMT = 0;
                row.FEE = 0;
                row.TAX = 0;
                row.IOFLAG = TCSIO_item.IOFLAG;
                row.TRDATE = TCSIO_item.TRDATE;
                row.TRTIME = TCSIO_item.TRTIME;
                row.MODDATE = TCSIO_item.MODDATE;
                row.MODTIME = TCSIO_item.MODTIME;
                row.MODUSER = TCSIO_item.MODUSER;
                HCMIOList.Add(row);
            }
            foreach (var TMHIO_item in TMHIOList)
            {
                var row = new HCMIO();
                row.TDATE = TMHIO_item.TDATE;
                row.BHNO = TMHIO_item.BHNO;
                row.CSEQ = TMHIO_item.CSEQ;
                row.DSEQ = TMHIO_item.DSEQ;
                row.DNO = TMHIO_item.JRNUM;
                row.WTYPE = "0";
                row.STOCK = TMHIO_item.STOCK;
                row.TTYPE = TMHIO_item.TTYPE;
                row.ETYPE = TMHIO_item.ETYPE;
                row.BSTYPE = TMHIO_item.BSTYPE;
                row.PRICE = TMHIO_item.PRICE;
                row.QTY = TMHIO_item.QTY;       //原始股數
                row.BQTY = TMHIO_item.QTY;      //未沖銷股數
                row.AMT = decimal.Truncate(TMHIO_item.PRICE * TMHIO_item.QTY);
                row.FEE = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(row.AMT) * 0.001425));
                if (TMHIO_item.QTY >= 1000 && row.FEE < 20)
                    row.FEE = 20;
                else if(TMHIO_item.QTY < 1000 && row.FEE < 1)
                    row.FEE = 1;
                if (TMHIO_item.BSTYPE == "S")
                {
                    row.TAX = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(row.AMT) * 0.003));
                    row.NETAMT = row.AMT - row.FEE - row.TAX;
                }
                else if(TMHIO_item.BSTYPE == "B")
                {
                    row.TAX = 0;
                    row.NETAMT = (row.AMT + row.FEE) * -1;
                }    
                row.ORIGN = TMHIO_item.ORGIN;
                row.SALES = TMHIO_item.SALES;
                row.TRDATE = TMHIO_item.TRDATE;
                row.TRTIME = TMHIO_item.TRTIME;
                row.MODDATE = TMHIO_item.MODDATE;
                row.MODTIME = TMHIO_item.MODTIME;
                row.MODUSER = TMHIO_item.MODUSER;
                HCMIOList.Add(row);
            }
            return HCMIOList;
        }
        //--------------------------------------------------------------------------------------------
        //function GetClientCNTDTYPE() - 取得客戶當沖資格
        //--------------------------------------------------------------------------------------------
        protected static string GetClientCNTDTYPE(List<HCMIO> HCMIOList)
        {
            string cseqCNTDTYPE = string.Empty;
            //字典搜尋客戶當沖資格
            string client = HCMIOList.First().BHNO + HCMIOList.First().CSEQ;
            if (_CseqMCUMS_Dic.ContainsKey(client))
                cseqCNTDTYPE = _CseqMCUMS_Dic[client][0].CNTDTYPE;
            else
                //cseqCNTDTYPE = "N";             //如果查不到客戶的沖銷資格, 假設此客戶不可現股當沖
                cseqCNTDTYPE = "X";           //(測試)如果查不到客戶的沖銷資格, 假設此客戶可先賣後買
            return cseqCNTDTYPE;
        }

        //--------------------------------------------------------------------------------------------
        //function GetStockCNTDTYPE() - 取得股票當沖資格
        //--------------------------------------------------------------------------------------------
        protected static string GetStockCNTDTYPE(HCMIO HCMIOSell_item)
        {
            string stockCNTDTYPE = string.Empty;
            //字典搜尋股票當沖資格
            if (_StockMSTMB_Dic.ContainsKey(HCMIOSell_item.STOCK))
                stockCNTDTYPE = _StockMSTMB_Dic[HCMIOSell_item.STOCK][0].CNTDTYPE;
            else
                stockCNTDTYPE = "N";            //如果查不到股票的沖銷資格, 假設此股票不可現股當沖
            return stockCNTDTYPE;
        }

        //--------------------------------------------------------------------------------------------
        //function DayTrade() - 今日現股當沖處理
        //--------------------------------------------------------------------------------------------
        protected static (List<HCMIO>, List<HCNTD>) DayTrade(List<HCMIO> HCMIOList)
        {
            List<HCNTD> HCNTDList = new List<HCNTD>();          //自訂HCNTD類別List (ESMP.STOCK.DB.TABLE.API)
            string cseqCNTDTYPE = GetClientCNTDTYPE(HCMIOList);
            if (cseqCNTDTYPE == "N" || string.IsNullOrWhiteSpace(cseqCNTDTYPE))
            {
                Console.WriteLine("此客戶不可現股當沖 ( N )");
            }
            else
            {
                List<HCMIO> HCMIOSellList = HCMIOList.Where(m => m.WTYPE == "0" && m.QTY >= 1000 && m.BSTYPE == "S").OrderBy(x => x.DSEQ).ThenBy(x => x.DNO).ToList();
                List<HCMIO> HCMIOBuyList = HCMIOList.Where(m => m.WTYPE == "0" && m.QTY >= 1000 && m.BSTYPE == "B").ToList();
                foreach (var HCMIOSell_item in HCMIOSellList)
                {
                    List<HCMIO> HCMIOCurrentList = new List<HCMIO>();
                    string stockCNTDTYPE = GetStockCNTDTYPE(HCMIOSell_item);
                    //判斷股票當沖資格
                    if (stockCNTDTYPE == "N" || string.IsNullOrWhiteSpace(stockCNTDTYPE))
                    {
                        Console.WriteLine("此股票不可現股當沖 ( N )");
                        continue;
                    }
                    else if (stockCNTDTYPE == "Y" || cseqCNTDTYPE == "Y")
                    {
                        Console.WriteLine("可先買後賣");
                        HCMIOCurrentList = HCMIOBuyList.Where(s => s.STOCK == HCMIOSell_item.STOCK && s.BQTY > 0 && String.Compare(HCMIOSell_item.DNO, s.DNO) > 0).OrderBy(x => x.DSEQ).ThenBy(x => x.DNO).ToList();
                    }
                    else if (stockCNTDTYPE == "X" && cseqCNTDTYPE == "B" || cseqCNTDTYPE == "X")
                    {
                        Console.WriteLine("可先賣後買");
                        HCMIOCurrentList = HCMIOBuyList.Where(s => s.STOCK == HCMIOSell_item.STOCK && s.BQTY > 0).OrderBy(x => x.DSEQ).ThenBy(x => x.DNO).ToList();
                    }
                    //如果沒有買單可以當沖,繼續下一筆賣單
                    if (HCMIOCurrentList.Count == 0)
                        continue;

                    decimal originalSFEE = HCMIOSell_item.FEE;          //原始剩餘賣出手續費
                    decimal originalTAX = HCMIOSell_item.TAX;           //原始剩餘賣出交易稅
                    decimal originalBQTY = HCMIOSell_item.BQTY;         //原始剩餘賣出股數
                    foreach (var HCMIOBuy_item in HCMIOCurrentList)
                    {
                        //計算當沖狀況 ----CQTY本次沖銷股數
                        decimal CQTY = Math.Min(HCMIOSell_item.BQTY, HCMIOBuy_item.BQTY);
                        decimal BFEE = decimal.Round(HCMIOBuy_item.FEE * (CQTY / HCMIOBuy_item.BQTY), 0, MidpointRounding.AwayFromZero);
                        decimal COST = decimal.Truncate(HCMIOBuy_item.PRICE * CQTY) + BFEE;
                        decimal SFEE = decimal.Round(originalSFEE * (CQTY / originalBQTY), 0, MidpointRounding.AwayFromZero);
                        decimal TAX = decimal.Round(originalTAX * (CQTY / originalBQTY), 0, MidpointRounding.AwayFromZero);
                        decimal INCOME = decimal.Truncate(HCMIOSell_item.PRICE * CQTY - SFEE - TAX);
                        decimal PROFIT = INCOME - COST;

                        HCMIOSell_item.BQTY -= CQTY;        //賣單剩餘未冲股數
                        HCMIOBuy_item.BQTY -= CQTY;         //買單剩餘未冲股數
                        //最後一筆當沖賣出，剩餘的SFEE、TAX與INCOME放入最後一筆資料
                        if (HCMIOSell_item.BQTY == 0)
                        {
                            SFEE = HCMIOSell_item.FEE;
                            TAX = HCMIOSell_item.TAX;
                            INCOME = HCMIOSell_item.NETAMT;
                        }
                        if (HCMIOBuy_item.BQTY == 0)
                        {
                            BFEE = HCMIOBuy_item.FEE;
                        }
                        //增加HCNTD資料 已實現損益
                        HCNTDList = AddHCNTD(HCNTDList, HCMIOBuy_item, HCMIOSell_item, CQTY, BFEE, SFEE, TAX, INCOME, COST, PROFIT);

                        //更新TCNUD HCMIO的剩餘FEE TAX NETAMT
                        HCMIOBuy_item.FEE -= BFEE;
                        HCMIOSell_item.FEE -= SFEE;
                        HCMIOSell_item.TAX -= TAX;
                        HCMIOSell_item.NETAMT -= INCOME;

                        //此筆賣單已沖銷完成
                        if (HCMIOSell_item.BQTY == 0)
                            break;
                    }
                }
            }
            return (HCMIOList, HCNTDList);
        }
        //--------------------------------------------------------------------------------------------
        //function AddHCNTD() - 增加HCNTD資料
        //--------------------------------------------------------------------------------------------
        private static List<HCNTD> AddHCNTD(List<HCNTD> HCNTDList, HCMIO HCMIOBuy_item, HCMIO HCMIOSell_item, decimal HCNRH_CQTY, decimal currBFEE, decimal currSFEE, decimal currTAX, decimal currINCOME, decimal currCOST, decimal currPROFIT)
        {
            var HCNTD_row = new HCNTD();
            HCNTD_row.BQTY = HCMIOBuy_item.QTY;
            HCNTD_row.SQTY = HCMIOSell_item.QTY;
            HCNTD_row.BHNO = HCMIOBuy_item.BHNO;
            HCNTD_row.TDATE = HCMIOSell_item.TDATE;
            HCNTD_row.CSEQ = HCMIOBuy_item.CSEQ;
            HCNTD_row.BDSEQ = HCMIOBuy_item.DSEQ;
            HCNTD_row.BDNO = HCMIOBuy_item.DNO;
            HCNTD_row.SDSEQ = HCMIOSell_item.DSEQ;
            HCNTD_row.SDNO = HCMIOSell_item.DNO;
            HCNTD_row.STOCK = HCMIOBuy_item.STOCK;
            HCNTD_row.CQTY = HCNRH_CQTY;
            HCNTD_row.BPRICE = HCMIOBuy_item.PRICE;
            HCNTD_row.BFEE = currBFEE;
            HCNTD_row.SPRICE = HCMIOSell_item.PRICE;
            HCNTD_row.SFEE = currSFEE;
            HCNTD_row.TAX = currTAX;
            HCNTD_row.INCOME = currINCOME;
            HCNTD_row.COST = currCOST;
            HCNTD_row.PROFIT = currPROFIT;
            HCNTDList.Add(HCNTD_row);
            return HCNTDList;
        }
        //--------------------------------------------------------------------------------------------
        //function CalculateHCNTD() - 重新計算現股當沖TAX、INCOME、PROFIT
        //--------------------------------------------------------------------------------------------
        protected static List<HCNTD> CalculateHCNTD(List<HCNTD> HCNTDList)
        {
            //依照賣出委託書號、賣出分單號產生新的清單群組grp_HCNTDList
            var grp_HCNTDList = HCNTDList.GroupBy(d => new { d.SDSEQ, d.SDNO }).Select(grp => grp.ToList()).ToList();
            
            foreach (var grp_HCNTD_item in grp_HCNTDList)
            {
                //計算當沖優惠手續費後，再分配至多筆HCNTD
                decimal totalTax = decimal.Truncate(grp_HCNTD_item.Sum(s => s.CQTY) * grp_HCNTD_item.First().SPRICE * (decimal)0.0015);
                decimal leftTAX = totalTax;
                HCNTD last = grp_HCNTD_item.Last();
                foreach (var item in grp_HCNTD_item)
                {
                    if (item.Equals(last))
                    {
                        item.TAX = leftTAX;
                    }
                    else
                    {
                        item.TAX = decimal.Round(totalTax * (item.CQTY / item.SQTY), 0, MidpointRounding.AwayFromZero);
                        leftTAX -= item.TAX;
                    }
                    item.INCOME = decimal.Truncate(item.SPRICE * item.CQTY) - item.SFEE - item.TAX;
                    item.PROFIT = item.INCOME - item.COST;
                }
            }
            return HCNTDList;
        }
        //--------------------------------------------------------------------------------------------
        //function AddTCSIO() - 今日匯入（TCSIO）加入現股餘額
        //--------------------------------------------------------------------------------------------
        protected static List<TCNUD> AddTCSIO(List<TCNUD> TCNUDList, List<HCMIO> HCMIOList)
        {
            List<HCMIO> HCMIOImportList = HCMIOList.Where(m => m.WTYPE == "A" && m.BSTYPE == "B").ToList();
            foreach (var item in HCMIOImportList)
            {
                var row = new TCNUD();
                row.TDATE = item.TDATE;
                row.BHNO = item.BHNO;
                row.CSEQ = item.CSEQ;
                row.STOCK = item.STOCK;
                row.PRICE = 0;
                row.QTY = item.QTY;
                row.BQTY = item.QTY;
                row.FEE = 0;
                row.COST = 0;
                row.DSEQ = item.DSEQ;
                row.DNO = item.DNO;
                row.WTYPE = "A";
                row.IOFLAG = item.IOFLAG;
                TCNUDList.Add(row);
            }
            return TCNUDList;
        }
        //--------------------------------------------------------------------------------------------
        //function CurrentStockSell() - 將賣出 匯出資料扣除現股餘額資料
        //--------------------------------------------------------------------------------------------
        protected static (List<HCNRH>, List<TCNUD>, List<HCMIO>) CurrentStockSell(List<TCNUD> TCNUDList, List<HCMIO> HCMIOList)
        {
            //依據STOCK建立TCNUD現股庫存Dictionary
            Dictionary<string, List<TCNUD>> grpStockTCNUD_Dic = TCNUDList.GroupBy(x => x.STOCK).ToDictionary(x => x.Key, x => x.ToList());

            List<HCNRH> HCNRHList = new List<HCNRH>();          //自訂HCNRH類別List (ESMP.STOCK.DB.TABLE.API)

            //挑選出當日賣出(WTYPE=0)賣單(BSTYPE=S)與當日匯出(WTYPE=A)賣單(BSTYPE=S) 並依照WTYPE 股票代號 賣單號 排序
            List<HCMIO> HCMIOSellList = HCMIOList.Where(m => m.BSTYPE == "S" && m.BQTY > 0).OrderBy(x => x.WTYPE).ThenBy(x => x.STOCK).ThenBy(x => x.DNO).ToList();

            //迴圈歷遍所有當日賣單
            foreach (var HCMIO_item in HCMIOSellList)
            {
                decimal originalSFEE = HCMIO_item.FEE;          //原始剩餘賣出手續費
                decimal originalTAX = HCMIO_item.TAX;           //原始剩餘賣出交易稅
                decimal originalBQTY = HCMIO_item.BQTY;         //原始剩餘賣出股數

                //挑選出相同股票代號與未沖銷完成的買單現股庫存(BQTY > 0)
                List<TCNUD> TCNUDCurrentList = new List<TCNUD>();
                if(grpStockTCNUD_Dic.TryGetValue(HCMIO_item.STOCK, out TCNUDCurrentList))
                {
                    TCNUDCurrentList = TCNUDCurrentList.Where(s => s.BQTY > 0).OrderBy(x => x.TDATE).ThenBy(x => x.WTYPE).ThenBy(x => x.DNO).ToList();
                }
                else
                    continue;

                //該筆賣單為整股且餘額總和不足1000股時，不沖銷零股餘額資料
                if (HCMIO_item.BQTY >= 1000 && TCNUDCurrentList.Sum((x) => x.BQTY) < 1000)
                    continue;

                //迴圈歷遍相同股票代號與未沖銷完成的買單現股庫存
                foreach (var TCNUD_item in TCNUDCurrentList)
                {
                    //計算沖銷狀況 ----CQTY本次沖銷股數
                    decimal CQTY = Math.Min(HCMIO_item.BQTY, TCNUD_item.BQTY);
                    decimal BFEE = decimal.Round(TCNUD_item.FEE * (CQTY / TCNUD_item.BQTY), 0, MidpointRounding.AwayFromZero);
                    decimal COST = decimal.Truncate(TCNUD_item.PRICE * CQTY) + BFEE;
                    decimal SFEE = decimal.Round(originalSFEE * (CQTY / originalBQTY), 0, MidpointRounding.AwayFromZero);
                    decimal TAX = decimal.Round(originalTAX * (CQTY / originalBQTY), 0, MidpointRounding.AwayFromZero);
                    decimal INCOME = decimal.Truncate(HCMIO_item.PRICE * CQTY - SFEE - TAX);
                    decimal PROFIT = INCOME - COST;
                    
                    HCMIO_item.BQTY -= CQTY;        //賣單剩餘未冲股數
                    TCNUD_item.BQTY -= CQTY;        //買單剩餘未冲股數
                    //最後一筆沖銷賣出，剩餘的SFEE、TAX與INCOME放入最後一筆資料
                    if (HCMIO_item.BQTY == 0)
                    {
                        SFEE = HCMIO_item.FEE;
                        TAX = HCMIO_item.TAX;
                        INCOME = HCMIO_item.NETAMT;
                    }
                    if (TCNUD_item.BQTY == 0)
                    {
                        BFEE = TCNUD_item.FEE;
                        COST = TCNUD_item.COST;
                    }
                    //增加HCNRH資料 已實現損益
                    HCNRHList = AddHCNRH(HCNRHList, TCNUD_item, HCMIO_item, CQTY, BFEE, SFEE, TAX, INCOME, COST, PROFIT);

                    //更新TCNUD HCMIO的剩餘FEE COST TAX NETAMT
                    TCNUD_item.FEE -= BFEE;
                    TCNUD_item.COST -= COST;
                    HCMIO_item.FEE -= SFEE;
                    HCMIO_item.TAX -= TAX;
                    HCMIO_item.NETAMT -= INCOME;
                    
                    //此筆賣單已沖銷完成
                    if (HCMIO_item.BQTY == 0)
                        break;
                }
            }
            TCNUDList.RemoveAll(x => x.BQTY == 0);
            return (HCNRHList, TCNUDList, HCMIOList);
        }

        //--------------------------------------------------------------------------------------------
        //function AddHCNRH() - 增加HCNRH資料
        //--------------------------------------------------------------------------------------------
        private static List<HCNRH> AddHCNRH(List<HCNRH> HCNRHList, TCNUD TCNUD_item, HCMIO HCMIO_item, decimal HCNRH_CQTY, decimal currBFEE, decimal currSFEE, decimal currTAX, decimal currINCOME, decimal currCOST, decimal currPROFIT)
        {
            var HCNRH_row = new HCNRH();
            HCNRH_row.BQTY = TCNUD_item.QTY;
            HCNRH_row.SQTY = HCMIO_item.QTY;
            HCNRH_row.BHNO = TCNUD_item.BHNO;
            HCNRH_row.TDATE = HCMIO_item.TDATE;
            HCNRH_row.RDATE = TCNUD_item.TDATE;
            HCNRH_row.CSEQ = TCNUD_item.CSEQ;
            HCNRH_row.BDSEQ = TCNUD_item.DSEQ;
            HCNRH_row.BDNO = TCNUD_item.DNO;
            HCNRH_row.SDSEQ = HCMIO_item.DSEQ;
            HCNRH_row.SDNO = HCMIO_item.DNO;
            HCNRH_row.STOCK = TCNUD_item.STOCK;
            HCNRH_row.CQTY = HCNRH_CQTY;
            HCNRH_row.BPRICE = TCNUD_item.PRICE;
            HCNRH_row.BFEE = currBFEE;
            HCNRH_row.SPRICE = HCMIO_item.PRICE;
            HCNRH_row.SFEE = currSFEE;
            HCNRH_row.TAX = currTAX;
            HCNRH_row.INCOME = currINCOME;
            HCNRH_row.COST = currCOST;
            HCNRH_row.PROFIT = currPROFIT;
            HCNRH_row.ADJDATE = "";
            HCNRH_row.WTYPE = TCNUD_item.WTYPE;
            HCNRHList.Add(HCNRH_row);
            return HCNRHList;
        }
        //--------------------------------------------------------------------------------------------
        //function AddTMHIOBuy() - 今日買進（TMHIO）加入現股餘額
        //--------------------------------------------------------------------------------------------
        protected static List<TCNUD> AddTMHIOBuy(List<TCNUD> TCNUDList, List<HCMIO> HCMIOList)
        {
            List<HCMIO> HCMIOBuyList = HCMIOList.Where(m => m.WTYPE == "0" && m.BSTYPE == "B").ToList();
            foreach (var item in HCMIOBuyList)
            {
                var row = new TCNUD();
                row.TDATE = item.TDATE;
                row.STOCK = item.STOCK;
                row.PRICE = item.PRICE;
                row.QTY = item.QTY;
                row.BQTY = item.BQTY;
                if (item.FEE != 0)
                {
                    row.FEE = item.FEE;
                }
                else
                {
                    row.FEE = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(item.PRICE) * decimal.ToDouble(item.QTY) * 0.001425));
                    if (item.ETYPE == "2" && row.FEE < 1)
                    {
                        row.FEE = 1;
                    }
                    else if (item.ETYPE == "0" && row.FEE < 20)
                    {
                        row.FEE = 20;
                    }
                }
                row.COST = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(item.PRICE) * decimal.ToDouble(item.BQTY))) + row.FEE;
                row.DSEQ = item.DSEQ;
                row.DNO = item.DNO;
                row.WTYPE = "0";
                TCNUDList.Add(row);
            }
            return TCNUDList;
        }

        //--------------------------------------------------------------------------------------------
        //function AddTMHIOSell() - 剩餘賣出未沖銷，加入現股餘額
        //--------------------------------------------------------------------------------------------
        protected static List<TCNUD> AddTMHIOSell(List<TCNUD> TCNUDList, List<HCMIO> HCMIOList)
        {
            string cseqCNTDTYPE = GetClientCNTDTYPE(HCMIOList);
            if (cseqCNTDTYPE == "X" || cseqCNTDTYPE == "B")
            {
                Console.WriteLine("此客戶可現股賣出 ( X )");
                List<HCMIO> HCMIOSellList = HCMIOList.Where(m => m.WTYPE == "0" && m.BSTYPE == "S").ToList();
                foreach (var item in HCMIOSellList)
                {
                    string stockCNTDTYPE = GetStockCNTDTYPE(item);
                    if (stockCNTDTYPE == "X")
                    {
                        Console.WriteLine("此股票可現股賣出 ( X )");
                        var row = new TCNUD();
                        row.TDATE = item.TDATE;
                        row.BHNO = item.BHNO;
                        row.CSEQ = item.CSEQ;
                        row.STOCK = item.STOCK;
                        row.PRICE = item.PRICE;
                        row.QTY = item.QTY;
                        row.BQTY = item.BQTY * -1;
                        row.FEE = item.FEE;
                        //row.COST ??
                        row.DSEQ = item.DSEQ;
                        row.DNO = item.DNO;
                        row.ADJDATE = item.ADJDATE;
                        row.WTYPE = item.WTYPE;
                        row.TRDATE = item.TRDATE;
                        row.TRTIME = item.TRTIME;
                        row.MODDATE = item.MODDATE;
                        row.MODTIME = item.MODTIME;
                        row.MODUSER = item.MODUSER;
                        row.IOFLAG = item.IOFLAG;
                        TCNUDList.Add(row);
                    }
                }
            }
            return TCNUDList;
        }
    }
}
