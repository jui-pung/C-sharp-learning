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
        public static void getESMPData(List<TCNUD> TCNUDList, List<TMHIO> TMHIOList, List<TCSIO> TCSIOList)
        {
            List<HCNRH> HCNRHList = new List<HCNRH>();          //自訂HCNRH類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCMIO> HCMIOList = new List<HCMIO>();          //自訂HCMIO類別List (ESMP.STOCK.DB.TABLE.API)
            //現股匯入資料TCSIO與TMHIO當日明細 儲存為歷史交易明細格式List
            HCMIOList = getHCMIO(TCSIOList, TMHIOList);
            //新增當日現股匯入資料TCSIOList
            //TCNUDCurrentList = getTCSIO(TCNUDCurrentList, TCSIOList);
            //今日賣出現股扣除現股餘額資料
            HCNRHList = currentStockSell(TCNUDList, HCMIOList);
        }

        //--------------------------------------------------------------------------------------------
        //function getHCMIO() - 將TCSIOList資料與TMHIOList資料加入新增至HCMIOList
        //--------------------------------------------------------------------------------------------
        private static List<HCMIO> getHCMIO(List<TCSIO> TCSIOList, List<TMHIO> TMHIOList)
        {
            List<HCMIO> HCMIOList = new List<HCMIO>();          //自訂HCMIO類別List (ESMP.STOCK.DB.TABLE.API)
            foreach (var item in TCSIOList)
            {
                var row = new HCMIO();
                row.TDATE = item.TDATE;
                row.BHNO = item.BHNO;
                row.CSEQ = item.CSEQ;
                row.DSEQ = item.DSEQ;
                row.DNO = item.DNO;
                row.WTYPE = "A";
                row.STOCK = item.STOCK;
                row.BSTYPE = "B";
                row.PRICE = 0;
                row.QTY = item.QTY;
                row.AMT = 0;
                row.FEE = 0;
                row.TAX = 0;
                row.TRDATE = item.TRDATE;
                row.TRTIME = item.TRTIME;
                row.MODDATE = item.MODDATE;
                row.MODTIME = item.MODTIME;
                row.MODUSER = item.MODUSER;
                HCMIOList.Add(row);
            }
            foreach (var item in TMHIOList)
            {
                var row = new HCMIO();
                row.TDATE = item.TDATE;
                row.BHNO = item.BHNO;
                row.CSEQ = item.CSEQ;
                row.DSEQ = item.DSEQ;
                row.DNO = item.JRNUM;
                row.WTYPE = "0";
                row.STOCK = item.STOCK;
                row.TTYPE = item.TTYPE;
                row.ETYPE = item.ETYPE;
                row.BSTYPE = item.BSTYPE;
                row.PRICE = item.PRICE;
                row.QTY = item.QTY;
                row.BQTY = item.QTY;    //未沖銷股數
                row.AMT = decimal.Truncate(item.PRICE * item.QTY);
                row.FEE = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(row.AMT) * 0.001425));
                if(item.BSTYPE == "S")
                {
                    row.TAX = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(row.AMT) * 0.003));
                    row.NETAMT = row.AMT - row.FEE - row.TAX;
                }
                else if(item.BSTYPE == "B")
                    row.NETAMT = (row.AMT + row.FEE) * -1;
                row.ORIGN = item.ORGIN;
                row.SALES = item.SALES;
                row.TRDATE = item.TRDATE;
                row.TRTIME = item.TRTIME;
                row.MODDATE = item.MODDATE;
                row.MODTIME = item.MODTIME;
                row.MODUSER = item.MODUSER;
                HCMIOList.Add(row);
            }
            return HCMIOList;
        }

        //--------------------------------------------------------------------------------------------
        //function currentStockSell() - 將賣出資料扣除現股餘額資料
        //--------------------------------------------------------------------------------------------
        private static List<HCNRH> currentStockSell(List<TCNUD> TCNUDList, List<HCMIO> HCMIOList)
        {
            List<HCNRH> HCNRHList = new List<HCNRH>();          //自訂HCNRH類別List (ESMP.STOCK.DB.TABLE.API)
            List<HCMIO> HCMIOSellList = HCMIOList.Where(m => m.BSTYPE == "S").Where(u => u.WTYPE == "0").OrderBy(x => x.STOCK).ThenBy(x => x.DNO).ToList();
            decimal currQty = 0;            //記錄此筆賣單的賣出股數
            string currStockNo = "";        //紀錄此筆賣單的賣出股票代號
            //迴圈歷遍所有當日賣單
            foreach (var HCMIO in HCMIOSellList)
            {
                //目前迴圈處理賣單的賣出股數與股票代號
                currQty = HCMIO.QTY;
                currStockNo = HCMIO.STOCK;
                //挑選出相同股票代號與未沖銷完成的買單現股庫存(BQTY > 0)
                List<TCNUD> TCNUDCurrentList = TCNUDList.Where(s => s.STOCK == currStockNo).Where(b => b.BQTY > 0).ToList();
                //迴圈歷遍現股庫存
                foreach(var TCNUD in TCNUDCurrentList)
                {
                    decimal bqtyState = Math.Min(currQty, TCNUD.BQTY);
                    //代表此筆賣單未沖銷完成 現股沖銷股數不足接續下一筆庫存現股沖銷 ex.一筆賣單資料賣出三張台積電 買進現股庫存資料三張台積電分三天買進各一張 三筆現股庫存
                    if (bqtyState != currQty && bqtyState == TCNUD.BQTY)
                    {
                        //CQTY紀錄本次沖銷股數
                        TCNUD.CQTY = TCNUD.BQTY;
                        HCMIO.BQTY = currQty - bqtyState;
                        currQty = HCMIO.BQTY;

                        //計算此筆HCMIO部分沖銷手續費 交易稅 成本
                        decimal currTAX = decimal.Round(HCMIO.TAX * (TCNUD.CQTY / HCMIO.QTY));
                        decimal currSFEE = decimal.Round(HCMIO.FEE * (TCNUD.CQTY / HCMIO.QTY));
                        decimal currINCOME = decimal.Truncate(HCMIO.PRICE * TCNUD.CQTY) - currSFEE - currTAX;

                        #region 增加HCNRH資料 已實現損益
                        var row = getHCNRH_ROW(TCNUD.FEE, currSFEE, currTAX, currINCOME, TCNUD.COST);
                        HCNRHList.Add(row);
                        #endregion
                        TCNUD.BQTY = 0;
                        TCNUDList.Where(s => s.DSEQ == TCNUD.DSEQ && s.DNO == TCNUD.DNO && s.STOCK == TCNUD.STOCK).ToList().ForEach(x => { x.BQTY = TCNUD.BQTY; x.CQTY = TCNUD.CQTY; });
                        continue;
                    }
                    //代表此筆賣單已沖銷完成 現股沖銷股數剩下
                    else if (bqtyState == currQty && bqtyState != TCNUD.BQTY)
                    {
                        TCNUD.CQTY = bqtyState;
                        HCMIO.BQTY = 0;
                        
                        //計算此筆部分沖銷手續費 交易稅 成本
                        decimal currBQTY = TCNUD.BQTY;       //原始剩餘股數 
                        decimal currCQTY = TCNUD.CQTY;       //此筆沖銷股數
                        decimal currBFEE = decimal.Round(TCNUD.FEE * (currCQTY / currBQTY));
                        decimal currCOST = decimal.Truncate(TCNUD.PRICE * currCQTY) + currBFEE;

                        decimal currTAX = decimal.Round(HCMIO.TAX * (currCQTY / HCMIO.QTY));
                        decimal currSFEE = decimal.Round(HCMIO.FEE * (currCQTY / HCMIO.QTY));
                        decimal currINCOME = decimal.Truncate(HCMIO.PRICE * currCQTY) - currSFEE - currTAX;

                        #region 增加HCNRH資料 已實現損益
                        var row = getHCNRH_ROW(currBFEE, currSFEE, currTAX, currINCOME, currCOST);
                        HCNRHList.Add(row);
                        #endregion

                        //剩餘庫存股數
                        TCNUD.BQTY = currBQTY - currCQTY;
                        TCNUD.FEE = TCNUD.FEE - currBFEE;
                        TCNUD.COST = TCNUD.COST - currCOST;
                        TCNUDList.Where(s => s.DSEQ == TCNUD.DSEQ && s.DNO == TCNUD.DNO && s.STOCK == TCNUD.STOCK).ToList().ForEach(x => { x.BQTY = TCNUD.BQTY; x.CQTY = TCNUD.CQTY; });
                        break;
                    }
                    //代表此筆賣單已沖銷完成 現股沖銷股數沒有剩下(沒有部份沖銷)
                    else if (bqtyState == currQty && bqtyState == TCNUD.BQTY)
                    {
                        TCNUD.CQTY = bqtyState;
                        HCMIO.BQTY = 0;

                        #region 增加HCNRH資料 已實現損益
                        var row = getHCNRH_ROW(TCNUD.FEE, HCMIO.FEE, HCMIO.TAX, HCMIO.NETAMT, TCNUD.COST);
                        HCNRHList.Add(row);
                        #endregion
                        TCNUD.BQTY = 0;
                        TCNUDList.Where(s => s.DSEQ == TCNUD.DSEQ && s.DNO == TCNUD.DNO && s.STOCK == TCNUD.STOCK).ToList().ForEach(x => { x.BQTY = TCNUD.BQTY; x.CQTY = TCNUD.CQTY; });
                        break;
                    }
                    HCNRH getHCNRH_ROW(decimal currBFEE, decimal currSFEE, decimal currTAX, decimal currINCOME, decimal currCOST)
                    {
                        var row = new HCNRH();
                        row.BQTY = TCNUD.QTY;
                        row.SQTY = HCMIO.QTY;
                        row.BHNO = TCNUD.BHNO;
                        row.TDATE = HCMIO.TDATE;
                        row.RDATE = TCNUD.TDATE;
                        row.CSEQ = TCNUD.CSEQ;
                        row.BDSEQ = TCNUD.DSEQ;
                        row.BDNO = TCNUD.DNO;
                        row.SDSEQ = HCMIO.DSEQ;
                        row.SDNO = HCMIO.DNO;
                        row.STOCK = TCNUD.STOCK;
                        row.CQTY = TCNUD.CQTY;
                        row.BPRICE = TCNUD.PRICE;
                        row.BFEE = decimal.Truncate(currBFEE);
                        row.SPRICE = HCMIO.PRICE;
                        row.SFEE = currSFEE;
                        row.TAX = currTAX;
                        row.INCOME = currINCOME;
                        row.COST = decimal.Truncate(currCOST);
                        row.PROFIT = decimal.Truncate(row.INCOME - row.COST);
                        row.ADJDATE = "";
                        row.WTYPE = TCNUD.WTYPE;
                        return row;
                    }
                }
            }
            
            TCNUDList.RemoveAll(x => x.BQTY == 0);
            return HCNRHList;
        }
    }
}
