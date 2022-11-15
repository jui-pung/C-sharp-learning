using ESMP.STOCK.DB.TABLE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ESMP.STOCK.TASK.API
{
    public class ESMPData
    {
        public static List<TCNUD> getESMPData(List<TCNUD> TCNUDList, List<TMHIO> TMHIOList, List<TCSIO> TCSIOList)
        {
            List<HCNRH> HCNRHList = new List<HCNRH>();          //自訂HCNRH類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCMIO> HCMIOList = new List<HCMIO>();          //自訂HCMIO類別List (ESMP.STOCK.DB.TABLE.API)
            //現股匯入資料TCSIO與TMHIO當日明細 儲存為歷史交易明細格式List
            HCMIOList = getHCMIO(TCSIOList, TMHIOList);
            //今日匯入（TCSIO）加入現股餘額
            TCNUDList = addTCSIO(TCNUDList, HCMIOList);
            //今日賣出現股扣除現股餘額資料
            (HCNRHList,TCNUDList) = currentStockSell(TCNUDList, HCMIOList);
            //今日買進（TMHIO）加入現股餘額
            TCNUDList = addTMHIOBuy(TCNUDList, HCMIOList);
            return TCNUDList;
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
                row.QTY = TCSIO_item.QTY;
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
                row.QTY = TMHIO_item.QTY;
                row.BQTY = TMHIO_item.QTY;    //未沖銷股數
                row.AMT = decimal.Truncate(TMHIO_item.PRICE * TMHIO_item.QTY);
                row.FEE = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(row.AMT) * 0.001425));
                if(TMHIO_item.BSTYPE == "S")
                {
                    row.TAX = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(row.AMT) * 0.003));
                    row.NETAMT = row.AMT - row.FEE - row.TAX;
                }
                else if(TMHIO_item.BSTYPE == "B")
                    row.NETAMT = (row.AMT + row.FEE) * -1;
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
        //function currentStockSell() - 將賣出資料扣除現股餘額資料
        //--------------------------------------------------------------------------------------------
        public static (List<HCNRH>,List<TCNUD>) currentStockSell(List<TCNUD> TCNUDList, List<HCMIO> HCMIOList)
        {
            List<HCNRH> HCNRHList = new List<HCNRH>();          //自訂HCNRH類別List (ESMP.STOCK.DB.TABLE.API)
            //挑選出當日賣出(WTYPE=0)賣單(BSTYPE=S)與當日匯出(WTYPE=A)賣單(BSTYPE=S) 並依照股票代號與賣單號WTYPE排序
            List<HCMIO> HCMIOSellList = HCMIOList.Where(m => m.BSTYPE == "S").OrderBy(x => x.WTYPE).ThenBy(x => x.STOCK).ThenBy(x => x.DNO).ToList();

            //迴圈歷遍所有當日賣單
            foreach (var HCMIO_item in HCMIOSellList)
            {
                //目前迴圈處理賣單的賣出股數與股票代號
                decimal currSellQty = HCMIO_item.QTY;
                string currStockNo = HCMIO_item.STOCK;

                //挑選出相同股票代號與未沖銷完成的買單現股庫存(BQTY > 0)
                List<TCNUD> TCNUDCurrentList = TCNUDList.Where(s => s.STOCK == currStockNo && s.BQTY > 0).OrderBy(x => x.TDATE).ThenBy(x => x.WTYPE).ThenBy(x => x.DNO).ToList();

                //迴圈歷遍相同股票代號與未沖銷完成的買單現股庫存
                foreach (var TCNUD_item in TCNUDCurrentList)
                {
                    //計算沖銷狀況
                    decimal bqtyState = Math.Min(currSellQty, TCNUD_item.BQTY);

                    //代表此筆賣單未沖銷完成 現股沖銷股數不足接續下一筆庫存現股沖銷
                    if (bqtyState != currSellQty && bqtyState == TCNUD_item.BQTY)
                    {
                        TCNUD_item.CQTY = TCNUD_item.BQTY;                  //TCNUD_item.CQTY紀錄本次沖銷股數
                        HCMIO_item.BQTY = currSellQty - bqtyState;          //HCMIO_item.BQTY紀錄賣單未沖銷股數
                        currSellQty = HCMIO_item.BQTY;                      //更新目前賣單剩餘未沖銷股數

                        //計算此筆HCMIO部分沖銷手續費 交易稅 成本
                        decimal currTAX = decimal.Round(HCMIO_item.TAX * (TCNUD_item.CQTY / HCMIO_item.QTY));
                        decimal currSFEE = decimal.Round(HCMIO_item.FEE * (TCNUD_item.CQTY / HCMIO_item.QTY));
                        decimal currINCOME = decimal.Truncate(HCMIO_item.PRICE * TCNUD_item.CQTY) - currSFEE - currTAX;

                        //增加HCNRH資料 已實現損益
                        var row = getHCNRH_ROW(TCNUD_item.FEE, currSFEE, currTAX, currINCOME, TCNUD_item.COST);
                        HCNRHList.Add(row);

                        //TCNUD_item.BQTY更新買單未沖銷股數
                        TCNUD_item.BQTY = 0;
                        //更新原始TCNUDList BQTY CQTY欄位
                        TCNUDList.Where(s => s.DSEQ == TCNUD_item.DSEQ && s.DNO == TCNUD_item.DNO && s.STOCK == TCNUD_item.STOCK).ToList().ForEach(x => { x.BQTY = TCNUD_item.BQTY; x.CQTY = TCNUD_item.CQTY; });
                        continue;
                    }
                    //代表此筆賣單已沖銷完成 現股沖銷股數剩下
                    else if (bqtyState == currSellQty && bqtyState != TCNUD_item.BQTY)
                    {
                        TCNUD_item.CQTY = bqtyState;
                        HCMIO_item.BQTY = 0;
                        
                        //計算此筆部分沖銷手續費 交易稅 成本
                        decimal currBQTY = TCNUD_item.BQTY;       //原始剩餘股數 
                        decimal currCQTY = TCNUD_item.CQTY;       //此筆沖銷股數
                        decimal currBFEE = decimal.Round(TCNUD_item.FEE * (currCQTY / currBQTY));
                        decimal currCOST = decimal.Truncate(TCNUD_item.PRICE * currCQTY) + currBFEE;

                        decimal currTAX = decimal.Round(HCMIO_item.TAX * (currCQTY / HCMIO_item.QTY));
                        decimal currSFEE = decimal.Round(HCMIO_item.FEE * (currCQTY / HCMIO_item.QTY));
                        decimal currINCOME = decimal.Truncate(HCMIO_item.PRICE * currCQTY) - currSFEE - currTAX;

                        //增加HCNRH資料 已實現損益
                        var row = getHCNRH_ROW(currBFEE, currSFEE, currTAX, currINCOME, currCOST);
                        HCNRHList.Add(row);

                        //剩餘庫存股數
                        TCNUD_item.BQTY = currBQTY - currCQTY;
                        TCNUD_item.FEE = TCNUD_item.FEE - currBFEE;
                        TCNUD_item.COST = TCNUD_item.COST - currCOST;

                        //更新原始TCNUDList BQTY CQTY FEE COST欄位
                        TCNUDList.Where(s => s.DSEQ == TCNUD_item.DSEQ && s.DNO == TCNUD_item.DNO && s.STOCK == TCNUD_item.STOCK).ToList().ForEach(x => { x.BQTY = TCNUD_item.BQTY; x.CQTY = TCNUD_item.CQTY; x.FEE = TCNUD_item.FEE; x.COST = TCNUD_item.COST; });
                        break;
                    }
                    //代表此筆賣單已沖銷完成 現股沖銷股數沒有剩下(沒有部份沖銷)
                    else if (bqtyState == currSellQty && bqtyState == TCNUD_item.BQTY)
                    {
                        TCNUD_item.CQTY = bqtyState;
                        HCMIO_item.BQTY = 0;

                        //增加HCNRH資料 已實現損益
                        var row = getHCNRH_ROW(TCNUD_item.FEE, HCMIO_item.FEE, HCMIO_item.TAX, HCMIO_item.NETAMT, TCNUD_item.COST);
                        HCNRHList.Add(row);

                        TCNUD_item.BQTY = 0;
                        //更新原始TCNUDList BQTY CQTY欄位
                        TCNUDList.Where(s => s.DSEQ == TCNUD_item.DSEQ && s.DNO == TCNUD_item.DNO && s.STOCK == TCNUD_item.STOCK).ToList().ForEach(x => { x.BQTY = TCNUD_item.BQTY; x.CQTY = TCNUD_item.CQTY; });
                        break;
                    }

                    //HCNRH資料欄位
                    HCNRH getHCNRH_ROW(decimal currBFEE, decimal currSFEE, decimal currTAX, decimal currINCOME, decimal currCOST)
                    {
                        var row = new HCNRH();
                        row.BQTY = TCNUD_item.QTY;
                        row.SQTY = HCMIO_item.QTY;
                        row.BHNO = TCNUD_item.BHNO;
                        row.TDATE = HCMIO_item.TDATE;
                        row.RDATE = TCNUD_item.TDATE;
                        row.CSEQ = TCNUD_item.CSEQ;
                        row.BDSEQ = TCNUD_item.DSEQ;
                        row.BDNO = TCNUD_item.DNO;
                        row.SDSEQ = HCMIO_item.DSEQ;
                        row.SDNO = HCMIO_item.DNO;
                        row.STOCK = TCNUD_item.STOCK;
                        row.CQTY = TCNUD_item.CQTY;
                        row.BPRICE = TCNUD_item.PRICE;
                        row.BFEE = decimal.Truncate(currBFEE);
                        row.SPRICE = HCMIO_item.PRICE;
                        row.SFEE = currSFEE;
                        row.TAX = currTAX;
                        row.INCOME = currINCOME;
                        row.COST = decimal.Truncate(currCOST);
                        row.PROFIT = decimal.Truncate(row.INCOME - row.COST);
                        row.ADJDATE = "";
                        row.WTYPE = TCNUD_item.WTYPE;
                        return row;
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
