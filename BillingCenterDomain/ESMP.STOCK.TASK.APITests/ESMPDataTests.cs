using Microsoft.VisualStudio.TestTools.UnitTesting;
using ESMP.STOCK.TASK.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESMP.STOCK.DB.TABLE;
using ESMP.STOCK.FORMAT;

namespace ESMP.STOCK.TASK.API.Tests
{
    [TestClass()]
    public class ESMPDataTests
    {
        //有昨日現股餘額、今日全部賣出
        [TestMethod()]
        public void currentStockSellTest_1()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "i0268",
                JRNUM = "01337974",
                MTYPE = "T",
                CSEQ = "0131263",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "S",
                STOCK = "2609",
                QTY = Convert.ToDecimal(1000),
                PRICE = Convert.ToDecimal(58.7000),
                SALES = "0056",
                ORGIN = "1",
                MTIME = "112803904",
                TRDATE = "20221017",
                TRTIME = "112804",
                MODDATE = "20221017",
                MODTIME = "112804",
                MODUSER = "REPLY"
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "i0295",
                JRNUM = "01443453",
                MTYPE = "T",
                CSEQ = "0131263",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "S",
                STOCK = "2609",
                QTY = Convert.ToDecimal(1000),
                PRICE = Convert.ToDecimal(59.1000),
                SALES = "0056",
                ORGIN = "1",
                MTIME = "115328092",
                TRDATE = "20221017",
                TRTIME = "115328",
                MODDATE = "20221017",
                MODTIME = "115328",
                MODUSER = "REPLY"
            });
            TCNUDList.Add(new TCNUD()
            {
                TDATE = "20220815",
                BHNO = "592S",
                CSEQ = "0131263",
                STOCK = "2609",
                PRICE = Convert.ToDecimal(87.4),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(124.00),
                COST = Convert.ToDecimal(87524.00),
                DSEQ = "t0350",
                DNO = "0000007",
                WTYPE = "0",
                TRDATE = "20220815",
                TRTIME = "190320",
                MODDATE = "20220815",
                MODTIME = "190320",
                MODUSER = "DailyJob"
            });
            TCNUDList.Add(new TCNUD()
            {
                TDATE = "20220815",
                BHNO = "592S",
                CSEQ = "0131263",
                STOCK = "2609",
                PRICE = Convert.ToDecimal(87.4),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(124.00),
                COST = Convert.ToDecimal(87524.00),
                DSEQ = "j0343",
                DNO = "0000008",
                WTYPE = "0",
                TRDATE = "20220815",
                TRTIME = "190320",
                MODDATE = "20220815",
                MODTIME = "190320",
                MODUSER = "DailyJob"
            });
            TCNUDList.Add(new TCNUD()
            {
                TDATE = "20220815",
                BHNO = "592S",
                CSEQ = "0131263",
                STOCK = "2609",
                PRICE = Convert.ToDecimal(87.3000),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(124.00),
                COST = Convert.ToDecimal(87424.00),
                DSEQ = "t0357",
                DNO = "0000009",
                WTYPE = "0",
                TRDATE = "20220815",
                TRTIME = "190320",
                MODDATE = "20220815",
                MODTIME = "190320",
                MODUSER = "DailyJob"
            });
            HCMIOList = ESMPData.getHCMIO(TCSIOList, TMHIOList);
            (HCNRHList,TCNUDList) = ESMPData.currentStockSell(TCNUDList, HCMIOList);
            Assert.AreEqual(HCNRHList.Count, 2);
            Assert.AreEqual(HCNRHList[0].SDNO, "01337974");
            Assert.AreEqual(HCNRHList[0].CQTY, Convert.ToDecimal(1000));
            Assert.AreEqual(HCNRHList[0].SFEE, Convert.ToDecimal(83));
            Assert.AreEqual(HCNRHList[0].TAX, Convert.ToDecimal(176));
            Assert.AreEqual(HCNRHList[0].INCOME, Convert.ToDecimal(58441));
            Assert.AreEqual(HCNRHList[0].PROFIT, Convert.ToDecimal(-29083));

            Assert.AreEqual(TCNUDList.Count, 1);
            Assert.AreEqual(TCNUDList[0].DSEQ, "t0357");
        }

        //有昨日現股餘額、今日部分賣出（需部分沖銷）
        [TestMethod()]
        public void currentStockSellTest_2()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "i0268",
                JRNUM = "01337974",
                MTYPE = "T",
                CSEQ = "0131263",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "S",
                STOCK = "2609",
                QTY = Convert.ToDecimal(1000),
                PRICE = Convert.ToDecimal(58.7000),
                SALES = "0056",
                ORGIN = "1",
                MTIME = "112803904",
                TRDATE = "20221017",
                TRTIME = "112804",
                MODDATE = "20221017",
                MODTIME = "112804",
                MODUSER = "REPLY"
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "i0295",
                JRNUM = "01443453",
                MTYPE = "T",
                CSEQ = "0131263",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "S",
                STOCK = "2609",
                QTY = Convert.ToDecimal(500),
                PRICE = Convert.ToDecimal(59.1000),
                SALES = "0056",
                ORGIN = "1",
                MTIME = "115328092",
                TRDATE = "20221017",
                TRTIME = "115328",
                MODDATE = "20221017",
                MODTIME = "115328",
                MODUSER = "REPLY"
            });
            TCNUDList.Add(new TCNUD()
            {
                TDATE = "20220815",
                BHNO = "592S",
                CSEQ = "0131263",
                STOCK = "2609",
                PRICE = Convert.ToDecimal(87.4),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(124.00),
                COST = Convert.ToDecimal(87524.00),
                DSEQ = "t0350",
                DNO = "0000007",
                WTYPE = "0",
                TRDATE = "20220815",
                TRTIME = "190320",
                MODDATE = "20220815",
                MODTIME = "190320",
                MODUSER = "DailyJob"
            });
            TCNUDList.Add(new TCNUD()
            {
                TDATE = "20220815",
                BHNO = "592S",
                CSEQ = "0131263",
                STOCK = "2609",
                PRICE = Convert.ToDecimal(87.4),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(124.00),
                COST = Convert.ToDecimal(87524.00),
                DSEQ = "j0343",
                DNO = "0000008",
                WTYPE = "0",
                TRDATE = "20220815",
                TRTIME = "190320",
                MODDATE = "20220815",
                MODTIME = "190320",
                MODUSER = "DailyJob"
            });
            TCNUDList.Add(new TCNUD()
            {
                TDATE = "20220815",
                BHNO = "592S",
                CSEQ = "0131263",
                STOCK = "2609",
                PRICE = Convert.ToDecimal(87.3000),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(124.00),
                COST = Convert.ToDecimal(87424.00),
                DSEQ = "t0357",
                DNO = "0000009",
                WTYPE = "0",
                TRDATE = "20220815",
                TRTIME = "190320",
                MODDATE = "20220815",
                MODTIME = "190320",
                MODUSER = "DailyJob"
            });
            HCMIOList = ESMPData.getHCMIO(TCSIOList, TMHIOList);
            (HCNRHList, TCNUDList) = ESMPData.currentStockSell(TCNUDList, HCMIOList);
            Assert.AreEqual(HCNRHList.Count, 2);
            Assert.AreEqual(HCNRHList[1].SDNO, "01443453");
            Assert.AreEqual(HCNRHList[1].CQTY, Convert.ToDecimal(500));
            Assert.AreEqual(HCNRHList[1].SFEE, Convert.ToDecimal(42));
            Assert.AreEqual(HCNRHList[1].TAX, Convert.ToDecimal(88));
            Assert.AreEqual(HCNRHList[1].INCOME, Convert.ToDecimal(29420));
            Assert.AreEqual(HCNRHList[1].COST, Convert.ToDecimal(43762));
            Assert.AreEqual(HCNRHList[1].PROFIT, Convert.ToDecimal(-14342));

            Assert.AreEqual(TCNUDList.Count, 2);
            Assert.AreEqual(TCNUDList[0].DSEQ, "j0343");
            Assert.AreEqual(TCNUDList[0].BQTY, Convert.ToDecimal(500));
            Assert.AreEqual(TCNUDList[0].FEE, Convert.ToDecimal(62));
            Assert.AreEqual(TCNUDList[0].COST, Convert.ToDecimal(43762));
        }

        //有昨日日現股餘額、有今日匯入、今日部分賣出（需部分沖銷）
        [TestMethod()]
        public void currentStockSellTest_3()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "i0268",
                JRNUM = "01337974",
                MTYPE = "T",
                CSEQ = "0131263",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "S",
                STOCK = "2609",
                QTY = Convert.ToDecimal(1000),
                PRICE = Convert.ToDecimal(58.7000),
                SALES = "0056",
                ORGIN = "1",
                MTIME = "112803904",
                TRDATE = "20221017",
                TRTIME = "112804",
                MODDATE = "20221017",
                MODTIME = "112804",
                MODUSER = "REPLY"
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "i0295",
                JRNUM = "01443453",
                MTYPE = "T",
                CSEQ = "0131263",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "S",
                STOCK = "2609",
                QTY = Convert.ToDecimal(500),
                PRICE = Convert.ToDecimal(59.1000),
                SALES = "0056",
                ORGIN = "1",
                MTIME = "115328092",
                TRDATE = "20221017",
                TRTIME = "115328",
                MODDATE = "20221017",
                MODTIME = "115328",
                MODUSER = "REPLY"
            });
            TCNUDList.Add(new TCNUD()
            {
                TDATE = "20220815",
                BHNO = "592S",
                CSEQ = "0131263",
                STOCK = "2609",
                PRICE = Convert.ToDecimal(87.4),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(124.00),
                COST = Convert.ToDecimal(87524.00),
                DSEQ = "t0350",
                DNO = "0000007",
                WTYPE = "0",
                TRDATE = "20220815",
                TRTIME = "190320",
                MODDATE = "20220815",
                MODTIME = "190320",
                MODUSER = "DailyJob"
            });
            TCNUDList.Add(new TCNUD()
            {
                TDATE = "20220815",
                BHNO = "592S",
                CSEQ = "0131263",
                STOCK = "2609",
                PRICE = Convert.ToDecimal(87.4),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(124.00),
                COST = Convert.ToDecimal(87524.00),
                DSEQ = "j0343",
                DNO = "0000008",
                WTYPE = "0",
                TRDATE = "20220815",
                TRTIME = "190320",
                MODDATE = "20220815",
                MODTIME = "190320",
                MODUSER = "DailyJob"
            });
            TCNUDList.Add(new TCNUD()
            {
                TDATE = "20220815",
                BHNO = "592S",
                CSEQ = "0131263",
                STOCK = "2609",
                PRICE = Convert.ToDecimal(87.3000),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(124.00),
                COST = Convert.ToDecimal(87424.00),
                DSEQ = "t0357",
                DNO = "0000009",
                WTYPE = "0",
                TRDATE = "20220815",
                TRTIME = "190320",
                MODDATE = "20220815",
                MODTIME = "190320",
                MODUSER = "DailyJob"
            });
            TCSIOList.Add(new TCSIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "0002A",
                DNO = "00",
                CSEQ = "0131263",
                STOCK = "6111",
                BSTYPE = "B",
                QTY = Convert.ToDecimal(2400),
                IOFLAG = "0167",
                REMARK = "",
                JRNUM = "9111101700004",
                TRDATE = "20221017",
                TRTIME = "180906",
                MODDATE = "20221017",
                MODTIME = "180906",
                MODUSER = "REPLY"
            });
            HCMIOList = ESMPData.getHCMIO(TCSIOList, TMHIOList);
            TCNUDList = ESMPData.addTCSIO(TCNUDList, HCMIOList);
            (HCNRHList, TCNUDList) = ESMPData.currentStockSell(TCNUDList, HCMIOList);
            Assert.AreEqual(HCNRHList.Count, 2);
            Assert.AreEqual(HCNRHList[1].SDNO, "01443453");
            Assert.AreEqual(HCNRHList[1].CQTY, Convert.ToDecimal(500));
            Assert.AreEqual(HCNRHList[1].SFEE, Convert.ToDecimal(42));
            Assert.AreEqual(HCNRHList[1].TAX, Convert.ToDecimal(88));
            Assert.AreEqual(HCNRHList[1].INCOME, Convert.ToDecimal(29420));
            Assert.AreEqual(HCNRHList[1].COST, Convert.ToDecimal(43762));
            Assert.AreEqual(HCNRHList[1].PROFIT, Convert.ToDecimal(-14342));

            Assert.AreEqual(TCNUDList.Count, 3);
            Assert.AreEqual(TCNUDList[0].DSEQ, "j0343");
            Assert.AreEqual(TCNUDList[0].WTYPE, "0");
            Assert.AreEqual(TCNUDList[0].BQTY, Convert.ToDecimal(500));
            Assert.AreEqual(TCNUDList[0].FEE, Convert.ToDecimal(62));
            Assert.AreEqual(TCNUDList[0].COST, Convert.ToDecimal(43762));
            Assert.AreEqual(TCNUDList[2].DSEQ, "0002A");
            Assert.AreEqual(TCNUDList[2].WTYPE, "A");
        }

        //沒有昨日日現股餘額、有今日匯入、今日全部賣出
        [TestMethod()]
        public void currentStockSellTest_4()
        {
            
        }
    }
}