using ESMP.STOCK.DB.TABLE;
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
        public static (List<TCNUD>,List<HCNRH>) getESMPData(List<TCNUD> TCNUDList, List<TMHIO> TMHIOList, List<TCSIO> TCSIOList)
        {
            List<HCNRH> HCNRHList = new List<HCNRH>();          //自訂HCNRH類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCMIO> HCMIOList = new List<HCMIO>();          //自訂HCMIO類別List (ESMP.STOCK.DB.TABLE.API)
            //將TMHIO List與TCSIO List資料轉入(Ram)HCMIO中
            HCMIOList = getHCMIO(TCSIOList, TMHIOList);
            //今日現股當沖處理
            HCMIOList = dayTrade(HCMIOList);
            //今日匯入（TCSIO）加入現股餘額
            TCNUDList = addTCSIO(TCNUDList, HCMIOList);
            //今日賣出 匯出現股扣除現股餘額資料
            (HCNRHList,TCNUDList) = currentStockSell(TCNUDList, HCMIOList);
            //今日買進（TMHIO）加入現股餘額
            TCNUDList = addTMHIOBuy(TCNUDList, HCMIOList);
            return (TCNUDList, HCNRHList);
        }

        //--------------------------------------------------------------------------------------------
        //function getHCMIO() - 將TCSIOList資料與TMHIOList資料加入新增至HCMIOList
        //--------------------------------------------------------------------------------------------
        public static List<HCMIO> getHCMIO(List<TCSIO> TCSIOList, List<TMHIO> TMHIOList)
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
        //function dayTrade() - 今日現股當沖處理
        //--------------------------------------------------------------------------------------------
        private static List<HCMIO> dayTrade(List<HCMIO> HCMIOList)
        {
            List<HCMIO> HCMIODayTradeList = HCMIOList.Where(m => m.QTY > 1000).OrderBy(x => x.DSEQ).ThenBy(x => x.DNO).ToList();
            foreach (var item in HCMIODayTradeList)
            {
                //Form1._StockMSTMB_Dic
            }
            SqlSearch sqlSearch = new SqlSearch();
            //sqlSearch.selectStockCNTDTYPE(HCMIOList);
            return HCMIOList;
        }

        //--------------------------------------------------------------------------------------------
        //function addTCSIO() - 今日匯入（TCSIO）加入現股餘額
        //--------------------------------------------------------------------------------------------
        public static List<TCNUD> addTCSIO(List<TCNUD> TCNUDList, List<HCMIO> HCMIOList)
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
        //function currentStockSell() - 將賣出 匯出資料扣除現股餘額資料
        //--------------------------------------------------------------------------------------------
        public static (List<HCNRH>,List<TCNUD>) currentStockSell(List<TCNUD> TCNUDList, List<HCMIO> HCMIOList)
        {
            //依據STOCK建立TCNUD現股庫存Dictionary
            Dictionary<string, List<TCNUD>> grpStockTCNUD_Dic = TCNUDList.GroupBy(x => x.STOCK).ToDictionary(x => x.Key, x => x.ToList());

            List<HCNRH> HCNRHList = new List<HCNRH>();          //自訂HCNRH類別List (ESMP.STOCK.DB.TABLE.API)

            //挑選出當日賣出(WTYPE=0)賣單(BSTYPE=S)與當日匯出(WTYPE=A)賣單(BSTYPE=S) 並依照WTYPE 股票代號 賣單號 排序
            List<HCMIO> HCMIOSellList = HCMIOList.Where(m => m.BSTYPE == "S").OrderBy(x => x.WTYPE).ThenBy(x => x.STOCK).ThenBy(x => x.DNO).ToList();

            //迴圈歷遍所有當日賣單
            foreach (var HCMIO_item in HCMIOSellList)
            {
                decimal originalSFEE = HCMIO_item.FEE;          //原始剩餘賣出手續費
                decimal originalTAX = HCMIO_item.TAX;           //原始剩餘賣出交易稅

                //挑選出相同股票代號與未沖銷完成的買單現股庫存(BQTY > 0)
                List<TCNUD> TCNUDCurrentList = new List<TCNUD>();
                if(grpStockTCNUD_Dic.TryGetValue(HCMIO_item.STOCK, out TCNUDCurrentList))
                {
                    TCNUDCurrentList = TCNUDCurrentList.Where(s => s.BQTY > 0).OrderBy(x => x.TDATE).ThenBy(x => x.WTYPE).ThenBy(x => x.DNO).ToList();
                }
                else
                    continue;

                //迴圈歷遍相同股票代號與未沖銷完成的買單現股庫存
                foreach (var TCNUD_item in TCNUDCurrentList)
                {
                    //計算沖銷狀況 ----CQTY本次沖銷股數
                    decimal CQTY = Math.Min(HCMIO_item.BQTY, TCNUD_item.BQTY);
                    decimal BFEE = decimal.Round(TCNUD_item.FEE * (CQTY / TCNUD_item.BQTY), 0);
                    decimal COST = decimal.Truncate(TCNUD_item.PRICE * CQTY) + BFEE;
                    decimal SFEE = decimal.Round(originalSFEE * (CQTY / HCMIO_item.QTY), 0);
                    decimal TAX = decimal.Round(originalTAX * (CQTY / HCMIO_item.QTY), 0);
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
                    var row = getHCNRH_ROW(CQTY, BFEE, SFEE, TAX, INCOME, COST, PROFIT);
                    HCNRHList.Add(row);

                    //更新TCNUD HCMIO的剩餘FEE COST TAX NETAMT
                    TCNUD_item.FEE -= BFEE;
                    TCNUD_item.COST -= COST;
                    HCMIO_item.FEE -= SFEE;
                    HCMIO_item.TAX -= TAX;
                    HCMIO_item.NETAMT -= INCOME;
                    
                    //此筆賣單已沖銷完成
                    if (HCMIO_item.BQTY == 0)
                        break;
                    
                    //HCNRH資料欄位
                    HCNRH getHCNRH_ROW(decimal HCNRH_CQTY, decimal currBFEE, decimal currSFEE, decimal currTAX, decimal currINCOME, decimal currCOST, decimal currPROFIT)
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
                        return HCNRH_row;
                    }
                }
            }
            TCNUDList.RemoveAll(x => x.BQTY == 0);
            return (HCNRHList, TCNUDList);
        }

        //--------------------------------------------------------------------------------------------
        //function addTMHIOBuy() - 今日買進（TMHIO）加入現股餘額
        //--------------------------------------------------------------------------------------------
        public static List<TCNUD> addTMHIOBuy(List<TCNUD> TCNUDList, List<HCMIO> HCMIOList)
        {
            List<HCMIO> HCMIOBuyList = HCMIOList.Where(m => m.WTYPE == "0" && m.BSTYPE == "B").ToList();
            foreach (var item in HCMIOBuyList)
            {
                var row = new TCNUD();
                row.TDATE = item.TDATE;
                row.STOCK = item.STOCK;
                row.PRICE = item.PRICE;
                row.QTY = item.QTY;
                row.BQTY = item.QTY;
                row.FEE = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(item.PRICE) * decimal.ToDouble(item.QTY) * 0.001425));
                if (item.ETYPE == "2" && row.FEE < 1)
                {
                    row.FEE = 1;
                }
                else if (item.ETYPE == "0" && row.FEE < 20)
                {
                    row.FEE = 20;
                }
                row.COST = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(item.PRICE) * decimal.ToDouble(item.QTY))) + row.FEE;
                row.DSEQ = item.DSEQ;
                row.DNO = item.DNO;
                row.WTYPE = "0";
                TCNUDList.Add(row);
            }
            return TCNUDList;
        }
    }
}
