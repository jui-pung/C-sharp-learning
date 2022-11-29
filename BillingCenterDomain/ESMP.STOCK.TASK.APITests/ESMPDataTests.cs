using Microsoft.VisualStudio.TestTools.UnitTesting;
using ESMP.STOCK.TASK.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESMP.STOCK.DB.TABLE;
using ESMP.STOCK.FORMAT;
using ESMP.STOCK.TASK.APITests;

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
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            (HCNRHList,TCNUDList) = SubESMPData.SubCurrentStockSell(TCNUDList, HCMIOList);
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
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            (HCNRHList, TCNUDList) = SubESMPData.SubCurrentStockSell(TCNUDList, HCMIOList);
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
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            TCNUDList = SubESMPData.SubAddTCSIO(TCNUDList, HCMIOList);
            (HCNRHList, TCNUDList) = SubESMPData.SubCurrentStockSell(TCNUDList, HCMIOList);
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

        //沒有昨日現股餘額、有今日匯入、今日全部賣出
        [TestMethod()]
        public void currentStockSellTest_4()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
            TCSIOList.Add(new TCSIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "0002A",
                DNO = "00",
                CSEQ = "0105354",
                STOCK = "6111",
                BSTYPE = "B",
                QTY = Convert.ToDecimal(300),
                IOFLAG = "0167",
                REMARK = "",
                JRNUM = "9111101700003",
                TRDATE = "20221017",
                TRTIME = "180906",
                MODDATE = "20221017",
                MODTIME = "180906",
                MODUSER = "REPLY"
            });
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
                STOCK = "6111",
                QTY = Convert.ToDecimal(300),
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
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            TCNUDList = SubESMPData.SubAddTCSIO(TCNUDList, HCMIOList);
            (HCNRHList, TCNUDList) = SubESMPData.SubCurrentStockSell(TCNUDList, HCMIOList);
            Assert.AreEqual(HCNRHList.Count, 1);
            Assert.AreEqual(HCNRHList[0].SDNO, "01337974");
            Assert.AreEqual(HCNRHList[0].CQTY, Convert.ToDecimal(300));
            Assert.AreEqual(HCNRHList[0].SFEE, Convert.ToDecimal(25));
            Assert.AreEqual(HCNRHList[0].TAX, Convert.ToDecimal(52));
            Assert.AreEqual(HCNRHList[0].INCOME, Convert.ToDecimal(17533));
            Assert.AreEqual(HCNRHList[0].COST, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].PROFIT, Convert.ToDecimal(17533));

            Assert.AreEqual(TCNUDList.Count, 0);
        }

        //沒有昨日現股餘額、有今日匯入、今日部分賣出（需部分沖銷）
        [TestMethod()]
        public void currentStockSellTest_5()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
            TCSIOList.Add(new TCSIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "0002A",
                DNO = "00",
                CSEQ = "0105354",
                STOCK = "6111",
                BSTYPE = "B",
                QTY = Convert.ToDecimal(300),
                IOFLAG = "0167",
                REMARK = "",
                JRNUM = "9111101700003",
                TRDATE = "20221017",
                TRTIME = "180906",
                MODDATE = "20221017",
                MODTIME = "180906",
                MODUSER = "REPLY"
            });
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
                STOCK = "6111",
                QTY = Convert.ToDecimal(100),
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
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            TCNUDList = SubESMPData.SubAddTCSIO(TCNUDList, HCMIOList);
            (HCNRHList, TCNUDList) = SubESMPData.SubCurrentStockSell(TCNUDList, HCMIOList);
            Assert.AreEqual(HCNRHList.Count, 1);
            Assert.AreEqual(HCNRHList[0].SDNO, "01337974");
            Assert.AreEqual(HCNRHList[0].CQTY, Convert.ToDecimal(100));
            Assert.AreEqual(HCNRHList[0].SFEE, Convert.ToDecimal(8));
            Assert.AreEqual(HCNRHList[0].TAX, Convert.ToDecimal(17));
            Assert.AreEqual(HCNRHList[0].INCOME, Convert.ToDecimal(5845));
            Assert.AreEqual(HCNRHList[0].COST, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].PROFIT, Convert.ToDecimal(5845));

            Assert.AreEqual(TCNUDList.Count, 1);
            Assert.AreEqual(TCNUDList[0].DSEQ, "0002A");
            Assert.AreEqual(TCNUDList[0].WTYPE, "A");
            Assert.AreEqual(TCNUDList[0].BQTY, Convert.ToDecimal(200));
            Assert.AreEqual(TCNUDList[0].FEE, Convert.ToDecimal(0));
            Assert.AreEqual(TCNUDList[0].COST, Convert.ToDecimal(0));
        }

        //有昨日現股餘額、今日全部匯出
        [TestMethod()]
        public void currentStockSellTest_6()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
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
            TCSIOList.Add(new TCSIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "0002A",
                DNO = "00",
                CSEQ = "0105354",
                STOCK = "2609",
                BSTYPE = "S",
                QTY = Convert.ToDecimal(1000),
                IOFLAG = "0167",
                REMARK = "",
                JRNUM = "9111101700003",
                TRDATE = "20221017",
                TRTIME = "180906",
                MODDATE = "20221017",
                MODTIME = "180906",
                MODUSER = "REPLY"
            });
            //將TMHIO List與TCSIO List資料轉入(Ram)HCMIO中
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            //今日匯入（TCSIO）加入現股餘額
            TCNUDList = SubESMPData.SubAddTCSIO(TCNUDList, HCMIOList);
            //今日賣出 匯出現股扣除現股餘額資料
            (HCNRHList, TCNUDList) = SubESMPData.SubCurrentStockSell(TCNUDList, HCMIOList);

            Assert.AreEqual(HCNRHList.Count, 1);
            Assert.AreEqual(HCNRHList[0].SDNO, "00");
            Assert.AreEqual(HCNRHList[0].CQTY, Convert.ToDecimal(1000));
            Assert.AreEqual(HCNRHList[0].SFEE, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].TAX, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].INCOME, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].PROFIT, Convert.ToDecimal(-87524));

            Assert.AreEqual(TCNUDList.Count, 0);
        }

        //有昨日現股餘額、今日部分匯出（需部分沖銷）
        [TestMethod()]
        public void currentStockSellTest_7()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
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
            TCSIOList.Add(new TCSIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "0002A",
                DNO = "00",
                CSEQ = "0105354",
                STOCK = "2609",
                BSTYPE = "S",
                QTY = Convert.ToDecimal(500),
                IOFLAG = "0167",
                REMARK = "",
                JRNUM = "9111101700003",
                TRDATE = "20221017",
                TRTIME = "180906",
                MODDATE = "20221017",
                MODTIME = "180906",
                MODUSER = "REPLY"
            });
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            (HCNRHList, TCNUDList) = SubESMPData.SubCurrentStockSell(TCNUDList, HCMIOList);
            Assert.AreEqual(HCNRHList.Count, 1);
            Assert.AreEqual(HCNRHList[0].SDNO, "00");
            Assert.AreEqual(HCNRHList[0].CQTY, Convert.ToDecimal(500));
            Assert.AreEqual(HCNRHList[0].SFEE, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].TAX, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].INCOME, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].PROFIT, Convert.ToDecimal(-43762));

            Assert.AreEqual(TCNUDList.Count, 1);
            Assert.AreEqual(TCNUDList[0].DSEQ, "t0350");
            Assert.AreEqual(TCNUDList[0].WTYPE, "0");
            Assert.AreEqual(TCNUDList[0].BQTY, Convert.ToDecimal(500));
            Assert.AreEqual(TCNUDList[0].FEE, Convert.ToDecimal(62));
            Assert.AreEqual(TCNUDList[0].COST, Convert.ToDecimal(43762));
        }

        //有昨日現股餘額、有今日匯入、今日部分匯出（需部分沖銷）
        [TestMethod()]
        public void currentStockSellTest_8()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
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
            TCSIOList.Add(new TCSIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "0002A",
                DNO = "00",
                CSEQ = "0105354",
                STOCK = "2609",
                BSTYPE = "B",
                QTY = Convert.ToDecimal(500),
                IOFLAG = "0167",
                REMARK = "",
                JRNUM = "9111101700003",
                TRDATE = "20221017",
                TRTIME = "180906",
                MODDATE = "20221017",
                MODTIME = "180906",
                MODUSER = "REPLY"
            });
            TCSIOList.Add(new TCSIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "0004A",
                DNO = "00",
                CSEQ = "0105354",
                STOCK = "2609",
                BSTYPE = "S",
                QTY = Convert.ToDecimal(500),
                IOFLAG = "0167",
                REMARK = "",
                JRNUM = "9111101700003",
                TRDATE = "20221017",
                TRTIME = "180906",
                MODDATE = "20221017",
                MODTIME = "180906",
                MODUSER = "REPLY"
            });
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            TCNUDList = SubESMPData.SubAddTCSIO(TCNUDList, HCMIOList);
            (HCNRHList, TCNUDList) = SubESMPData.SubCurrentStockSell(TCNUDList, HCMIOList);
            Assert.AreEqual(HCNRHList.Count, 1);
            Assert.AreEqual(HCNRHList[0].SDNO, "00");
            Assert.AreEqual(HCNRHList[0].CQTY, Convert.ToDecimal(500));
            Assert.AreEqual(HCNRHList[0].SFEE, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].TAX, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].INCOME, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].PROFIT, Convert.ToDecimal(-43762));

            Assert.AreEqual(TCNUDList.Count, 2);
            Assert.AreEqual(TCNUDList[0].DSEQ, "t0350");
            Assert.AreEqual(TCNUDList[0].WTYPE, "0");
            Assert.AreEqual(TCNUDList[0].BQTY, Convert.ToDecimal(500));
            Assert.AreEqual(TCNUDList[0].FEE, Convert.ToDecimal(62));
            Assert.AreEqual(TCNUDList[0].COST, Convert.ToDecimal(43762));
            Assert.AreEqual(TCNUDList[1].DSEQ, "0002A");
            Assert.AreEqual(TCNUDList[1].WTYPE, "A");
            Assert.AreEqual(TCNUDList[1].BQTY, Convert.ToDecimal(500));
            Assert.AreEqual(TCNUDList[1].FEE, Convert.ToDecimal(0));
            Assert.AreEqual(TCNUDList[1].COST, Convert.ToDecimal(0));
        }

        //沒有昨日現股餘額、有今日匯入、今日全部匯出
        [TestMethod()]
        public void currentStockSellTest_9()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
            TCSIOList.Add(new TCSIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "0002A",
                DNO = "00",
                CSEQ = "0105354",
                STOCK = "6111",
                BSTYPE = "B",
                QTY = Convert.ToDecimal(300),
                IOFLAG = "0167",
                REMARK = "",
                JRNUM = "9111101700003",
                TRDATE = "20221017",
                TRTIME = "180906",
                MODDATE = "20221017",
                MODTIME = "180906",
                MODUSER = "REPLY"
            });
            TCSIOList.Add(new TCSIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "0005A",
                DNO = "00",
                CSEQ = "0105354",
                STOCK = "6111",
                BSTYPE = "S",
                QTY = Convert.ToDecimal(300),
                IOFLAG = "0256",
                REMARK = "",
                JRNUM = "9111101700087",
                TRDATE = "20221017",
                TRTIME = "180906",
                MODDATE = "20221017",
                MODTIME = "180906",
                MODUSER = "REPLY"
            });
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            TCNUDList = SubESMPData.SubAddTCSIO(TCNUDList, HCMIOList);
            (HCNRHList, TCNUDList) = SubESMPData.SubCurrentStockSell(TCNUDList, HCMIOList);
            Assert.AreEqual(HCNRHList.Count, 1);
            Assert.AreEqual(HCNRHList[0].SDNO, "00");
            Assert.AreEqual(HCNRHList[0].CQTY, Convert.ToDecimal(300));
            Assert.AreEqual(HCNRHList[0].SFEE, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].TAX, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].INCOME, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].COST, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].PROFIT, Convert.ToDecimal(0));

            Assert.AreEqual(TCNUDList.Count, 0);
        }

        //沒有昨日現股餘額、有今日匯入、今日部分匯出（需部分沖銷）
        [TestMethod()]
        public void currentStockSellTest_10()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
            TCSIOList.Add(new TCSIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "0004A",
                DNO = "00",
                CSEQ = "0105354",
                STOCK = "6111",
                BSTYPE = "B",
                QTY = Convert.ToDecimal(300),
                IOFLAG = "0167",
                REMARK = "",
                JRNUM = "9111101700003",
                TRDATE = "20221017",
                TRTIME = "180906",
                MODDATE = "20221017",
                MODTIME = "180906",
                MODUSER = "REPLY"
            });
            TCSIOList.Add(new TCSIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "0005A",
                DNO = "00",
                CSEQ = "0105354",
                STOCK = "6111",
                BSTYPE = "S",
                QTY = Convert.ToDecimal(100),
                IOFLAG = "0256",
                REMARK = "",
                JRNUM = "9111101700087",
                TRDATE = "20221017",
                TRTIME = "180906",
                MODDATE = "20221017",
                MODTIME = "180906",
                MODUSER = "REPLY"
            });
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            TCNUDList = SubESMPData.SubAddTCSIO(TCNUDList, HCMIOList);
            (HCNRHList, TCNUDList) = SubESMPData.SubCurrentStockSell(TCNUDList, HCMIOList);
            Assert.AreEqual(HCNRHList.Count, 1);
            Assert.AreEqual(HCNRHList[0].SDNO, "00");
            Assert.AreEqual(HCNRHList[0].CQTY, Convert.ToDecimal(100));
            Assert.AreEqual(HCNRHList[0].SFEE, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].TAX, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].INCOME, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].COST, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[0].PROFIT, Convert.ToDecimal(0));

            Assert.AreEqual(TCNUDList.Count, 1);
            Assert.AreEqual(TCNUDList[0].DSEQ, "0004A");
            Assert.AreEqual(TCNUDList[0].WTYPE, "A");
            Assert.AreEqual(TCNUDList[0].BQTY, Convert.ToDecimal(200));
            Assert.AreEqual(TCNUDList[0].FEE, Convert.ToDecimal(0));
            Assert.AreEqual(TCNUDList[0].COST, Convert.ToDecimal(0));
        }

        //有昨日現股餘額、今日部分匯出（需部分沖銷）、今日部分賣出（需部分沖銷）
        [TestMethod()]
        public void currentStockSellTest_11()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
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
                STOCK = "6111",
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
            TCSIOList.Add(new TCSIO()
            {
                TDATE = "20221017",
                BHNO = "592S",
                DSEQ = "0005A",
                DNO = "00",
                CSEQ = "0105354",
                STOCK = "2609",
                BSTYPE = "S",
                QTY = Convert.ToDecimal(100),
                IOFLAG = "0256",
                REMARK = "",
                JRNUM = "9111101700087",
                TRDATE = "20221017",
                TRTIME = "180906",
                MODDATE = "20221017",
                MODTIME = "180906",
                MODUSER = "REPLY"
            });
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
                STOCK = "6111",
                QTY = Convert.ToDecimal(100),
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
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            (HCNRHList, TCNUDList) = SubESMPData.SubCurrentStockSell(TCNUDList, HCMIOList);
            Assert.AreEqual(HCNRHList.Count, 2);
            Assert.AreEqual(HCNRHList[0].SDNO, "01337974");
            Assert.AreEqual(HCNRHList[0].CQTY, Convert.ToDecimal(100));
            Assert.AreEqual(HCNRHList[0].SFEE, Convert.ToDecimal(8));
            Assert.AreEqual(HCNRHList[0].TAX, Convert.ToDecimal(17));
            Assert.AreEqual(HCNRHList[0].INCOME, Convert.ToDecimal(5845));
            Assert.AreEqual(HCNRHList[0].PROFIT, Convert.ToDecimal(-2907));
            Assert.AreEqual(HCNRHList[1].SDNO, "00");
            Assert.AreEqual(HCNRHList[1].CQTY, Convert.ToDecimal(100));
            Assert.AreEqual(HCNRHList[1].SFEE, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[1].TAX, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[1].INCOME, Convert.ToDecimal(0));
            Assert.AreEqual(HCNRHList[1].PROFIT, Convert.ToDecimal(-8752));

            Assert.AreEqual(TCNUDList.Count, 2);
            Assert.AreEqual(TCNUDList[0].DSEQ, "j0343");
        }

        //［現股當沖資格：X］今日賣出10張、今日買進10張
        [TestMethod()]
        public void currentStockSellTest_12()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
            List<HCNTD> HCNTDList = new List<HCNTD>();
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "T0726",
                JRNUM = "00600970",
                MTYPE = "T",
                CSEQ = "0126687",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "S",
                STOCK = "2330",
                QTY = Convert.ToDecimal(10000),
                PRICE = Convert.ToDecimal(399)
            });

            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "B0726",
                JRNUM = "00601350",
                MTYPE = "T",
                CSEQ = "0126687",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "B",
                STOCK = "2330",
                QTY = Convert.ToDecimal(10000),
                PRICE = Convert.ToDecimal(394)
            });
            ESMPData.CreateDic();
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            (HCMIOList, HCNTDList) = SubESMPData.SubDayTrade(HCMIOList);
            Assert.AreEqual(HCNTDList.Count, 0);

            Assert.AreEqual(HCMIOList.Count, 2);
        }

        //［現股當沖資格：X］今日賣出10張、今日買進10張
        [TestMethod()]
        public void currentStockSellTest_13()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
            List<HCNTD> HCNTDList = new List<HCNTD>();
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "T0726",
                JRNUM = "00600970",
                MTYPE = "T",
                CSEQ = "0126687",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "S",
                STOCK = "2330",
                QTY = Convert.ToDecimal(10000),
                PRICE = Convert.ToDecimal(399)
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "B0726",
                JRNUM = "00601350",
                MTYPE = "T",
                CSEQ = "0126687",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "B",
                STOCK = "2330",
                QTY = Convert.ToDecimal(10000),
                PRICE = Convert.ToDecimal(394)
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "B0726",
                JRNUM = "00601351",
                MTYPE = "T",
                CSEQ = "0126687",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "B",
                STOCK = "2330",
                QTY = Convert.ToDecimal(2000),
                PRICE = Convert.ToDecimal(394)
            });
            ESMPData.CreateDic();
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            (HCMIOList, HCNTDList) = SubESMPData.SubDayTrade(HCMIOList);
            Assert.AreEqual(HCNTDList.Count, 0);

            Assert.AreEqual(HCMIOList.Count, 3);
        }

        //［現股當沖資格：X］有昨日現股餘額、今日賣出10張、今日買進7張（需含部分沖銷）
        [TestMethod()]
        public void currentStockSellTest_14()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
            List<HCNTD> HCNTDList = new List<HCNTD>();
            TCNUDList.Add(new TCNUD()
            {
                TDATE = "20160504",
                BHNO = "5920",
                CSEQ = "0126687",
                STOCK = "2330",
                PRICE = Convert.ToDecimal(147),
                QTY = Convert.ToDecimal(12000),
                BQTY = Convert.ToDecimal(12000),
                FEE = Convert.ToDecimal(2513),
                COST = Convert.ToDecimal(1766513),
                DSEQ = "k8563",
                DNO = "002713",
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "T0726",
                JRNUM = "00600970",
                MTYPE = "T",
                CSEQ = "0126687",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "S",
                STOCK = "2330",
                QTY = Convert.ToDecimal(10000),
                PRICE = Convert.ToDecimal(399)
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "B0726",
                JRNUM = "00601350",
                MTYPE = "T",
                CSEQ = "0126687",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "B",
                STOCK = "2330",
                QTY = Convert.ToDecimal(7000),
                PRICE = Convert.ToDecimal(394)
            });
            ESMPData.CreateDic();
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            (HCMIOList, HCNTDList) = SubESMPData.SubDayTrade(HCMIOList);
            (HCNRHList, TCNUDList) = SubESMPData.SubCurrentStockSell(TCNUDList, HCMIOList);
            Assert.AreEqual(TCNUDList.Count, 1);
            Assert.AreEqual(TCNUDList[0].CSEQ, "0126687");
            Assert.AreEqual(TCNUDList[0].BQTY, Convert.ToDecimal(2000));
            Assert.AreEqual(TCNUDList[0].FEE, Convert.ToDecimal(419));
            Assert.AreEqual(TCNUDList[0].COST, Convert.ToDecimal(294419));

            Assert.AreEqual(HCNRHList.Count, 1);
            Assert.AreEqual(HCNRHList[0].CQTY, Convert.ToDecimal(10000));
            Assert.AreEqual(HCNRHList[0].BFEE, Convert.ToDecimal(2094));
            Assert.AreEqual(HCNRHList[0].SFEE, Convert.ToDecimal(5685));
            Assert.AreEqual(HCNRHList[0].TAX, Convert.ToDecimal(11970));
            Assert.AreEqual(HCNRHList[0].INCOME, Convert.ToDecimal(3972345));
            Assert.AreEqual(HCNRHList[0].PROFIT, Convert.ToDecimal(2500251));

            Assert.AreEqual(HCNTDList.Count, 0);
        }

        //［現股當沖資格：X］有昨日現股餘額、今日賣出10張、今日買進5張（需含部分沖銷）－其中一筆賣單須有現股當沖與沖銷
        [TestMethod()]
        public void currentStockSellTest_15()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
            List<HCNTD> HCNTDList = new List<HCNTD>();
            TCNUDList.Add(new TCNUD()
            {
                TDATE = "20160504",
                BHNO = "5920",
                CSEQ = "0126687",
                STOCK = "2330",
                PRICE = Convert.ToDecimal(147),
                QTY = Convert.ToDecimal(12000),
                BQTY = Convert.ToDecimal(12000),
                FEE = Convert.ToDecimal(2513),
                COST = Convert.ToDecimal(1766513),
                DSEQ = "k8563",
                DNO = "002713",
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "T0726",
                JRNUM = "00600970",
                MTYPE = "T",
                CSEQ = "0126687",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "S",
                STOCK = "2330",
                QTY = Convert.ToDecimal(10000),
                PRICE = Convert.ToDecimal(399)
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "B0726",
                JRNUM = "00601350",
                MTYPE = "T",
                CSEQ = "0126687",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "B",
                STOCK = "2330",
                QTY = Convert.ToDecimal(5000),
                PRICE = Convert.ToDecimal(394)
            });
            ESMPData.CreateDic();
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            (HCMIOList, HCNTDList) = SubESMPData.SubDayTrade(HCMIOList);
            (HCNRHList, TCNUDList) = SubESMPData.SubCurrentStockSell(TCNUDList, HCMIOList);
            Assert.AreEqual(TCNUDList.Count, 1);
            Assert.AreEqual(TCNUDList[0].CSEQ, "0126687");
            Assert.AreEqual(TCNUDList[0].BQTY, Convert.ToDecimal(2000));
            Assert.AreEqual(TCNUDList[0].FEE, Convert.ToDecimal(419));
            Assert.AreEqual(TCNUDList[0].COST, Convert.ToDecimal(294419));

            Assert.AreEqual(HCNRHList.Count, 1);
            Assert.AreEqual(HCNRHList[0].CQTY, Convert.ToDecimal(10000));
            Assert.AreEqual(HCNRHList[0].BFEE, Convert.ToDecimal(2094));
            Assert.AreEqual(HCNRHList[0].SFEE, Convert.ToDecimal(5685));
            Assert.AreEqual(HCNRHList[0].TAX, Convert.ToDecimal(11970));
            Assert.AreEqual(HCNRHList[0].INCOME, Convert.ToDecimal(3972345));
            Assert.AreEqual(HCNRHList[0].PROFIT, Convert.ToDecimal(2500251));

            Assert.AreEqual(HCNTDList.Count, 0);
        }

        //［現股當沖資格：Y］有昨日現股餘額、今日賣出10張（其中3張買進早）、今日買進7張（需含部分沖銷）
        [TestMethod()]
        public void currentStockSellTest_16()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
            List<HCNTD> HCNTDList = new List<HCNTD>();
            TCNUDList.Add(new TCNUD()
            {
                TDATE = "20160504",
                BHNO = "5920",
                CSEQ = "0003027",
                STOCK = "2327",
                PRICE = Convert.ToDecimal(380),
                QTY = Convert.ToDecimal(8000),
                BQTY = Convert.ToDecimal(8000),
                FEE = Convert.ToDecimal(4332),
                COST = Convert.ToDecimal(3044332),
                DSEQ = "k8563",
                DNO = "002713",
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "T0726",
                JRNUM = "00600970",
                MTYPE = "T",
                CSEQ = "0003027",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "S",
                STOCK = "2327",
                QTY = Convert.ToDecimal(3000),
                PRICE = Convert.ToDecimal(399)
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "B0726",
                JRNUM = "00601350",
                MTYPE = "T",
                CSEQ = "0003027",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "B",
                STOCK = "2327",
                QTY = Convert.ToDecimal(7000),
                PRICE = Convert.ToDecimal(394)
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "T0726",
                JRNUM = "00602000",
                MTYPE = "T",
                CSEQ = "0003027",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "S",
                STOCK = "2327",
                QTY = Convert.ToDecimal(7000),
                PRICE = Convert.ToDecimal(400)
            });
            ESMPData.CreateDic();
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            (HCMIOList, HCNTDList) = SubESMPData.SubDayTrade(HCMIOList);
            HCNTDList = SubESMPData.SubCalculateHCNTD(HCNTDList);
            (HCNRHList, TCNUDList) = SubESMPData.SubCurrentStockSell(TCNUDList, HCMIOList);
            Assert.AreEqual(TCNUDList.Count, 1);
            Assert.AreEqual(TCNUDList[0].CSEQ, "0003027");
            Assert.AreEqual(TCNUDList[0].BQTY, Convert.ToDecimal(5000));
            Assert.AreEqual(TCNUDList[0].FEE, Convert.ToDecimal(2708));
            Assert.AreEqual(TCNUDList[0].COST, Convert.ToDecimal(1902708));

            Assert.AreEqual(HCNRHList.Count, 1);
            Assert.AreEqual(HCNRHList[0].CQTY, Convert.ToDecimal(3000));
            Assert.AreEqual(HCNRHList[0].BFEE, Convert.ToDecimal(1624));
            Assert.AreEqual(HCNRHList[0].SFEE, Convert.ToDecimal(1705));
            Assert.AreEqual(HCNRHList[0].TAX, Convert.ToDecimal(3591));
            Assert.AreEqual(HCNRHList[0].INCOME, Convert.ToDecimal(1191704));
            Assert.AreEqual(HCNRHList[0].PROFIT, Convert.ToDecimal(50080));

            Assert.AreEqual(HCNTDList.Count, 1);
            Assert.AreEqual(HCNTDList[0].CQTY, Convert.ToDecimal(7000));
            Assert.AreEqual(HCNTDList[0].BFEE, Convert.ToDecimal(3930));
            Assert.AreEqual(HCNTDList[0].SFEE, Convert.ToDecimal(3990));
            Assert.AreEqual(HCNTDList[0].TAX, Convert.ToDecimal(4200));
            Assert.AreEqual(HCNTDList[0].INCOME, Convert.ToDecimal(2791810));
            Assert.AreEqual(HCNTDList[0].PROFIT, Convert.ToDecimal(29880));
        }

        //［現股當沖資格：Y］股票資格為Y、客戶資格為X 有昨日現股餘額、今日賣出10張（其中4張買進早）、今日買進10張（需含部分沖銷）
        [TestMethod()]
        public void currentStockSellTest_17()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
            List<HCNTD> HCNTDList = new List<HCNTD>();
            TCNUDList.Add(new TCNUD()
            {
                TDATE = "20160504",
                BHNO = "5920",
                CSEQ = "0003027",
                STOCK = "2330",
                PRICE = Convert.ToDecimal(380),
                QTY = Convert.ToDecimal(4000),
                BQTY = Convert.ToDecimal(4000),
                FEE = Convert.ToDecimal(2166),
                COST = Convert.ToDecimal(1522166),
                DSEQ = "k8563",
                DNO = "002713",
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "T0726",
                JRNUM = "00600970",
                MTYPE = "T",
                CSEQ = "0003027",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "S",
                STOCK = "2330",
                QTY = Convert.ToDecimal(4000),
                PRICE = Convert.ToDecimal(399)
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "B0726",
                JRNUM = "00601350",
                MTYPE = "T",
                CSEQ = "0003027",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "B",
                STOCK = "2330",
                QTY = Convert.ToDecimal(10000),
                PRICE = Convert.ToDecimal(394)
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "T0726",
                JRNUM = "00602000",
                MTYPE = "T",
                CSEQ = "0003027",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "S",
                STOCK = "2330",
                QTY = Convert.ToDecimal(6000),
                PRICE = Convert.ToDecimal(400)
            });
            ESMPData.CreateDic();
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            (HCMIOList, HCNTDList) = SubESMPData.SubDayTrade(HCMIOList);
            HCNTDList = SubESMPData.SubCalculateHCNTD(HCNTDList);
            (HCNRHList, TCNUDList) = SubESMPData.SubCurrentStockSell(TCNUDList, HCMIOList);
            Assert.AreEqual(TCNUDList.Count, 0);
            
            Assert.AreEqual(HCNRHList.Count, 1);
            Assert.AreEqual(HCNRHList[0].CQTY, Convert.ToDecimal(4000));
            Assert.AreEqual(HCNRHList[0].BFEE, Convert.ToDecimal(2166));
            Assert.AreEqual(HCNRHList[0].SFEE, Convert.ToDecimal(2274));
            Assert.AreEqual(HCNRHList[0].TAX, Convert.ToDecimal(4788));
            Assert.AreEqual(HCNRHList[0].INCOME, Convert.ToDecimal(1588938));
            Assert.AreEqual(HCNRHList[0].PROFIT, Convert.ToDecimal(66772));

            Assert.AreEqual(HCNTDList.Count, 1);
            Assert.AreEqual(HCNTDList[0].CQTY, Convert.ToDecimal(6000));
            Assert.AreEqual(HCNTDList[0].BFEE, Convert.ToDecimal(3368));
            Assert.AreEqual(HCNTDList[0].SFEE, Convert.ToDecimal(3420));
            Assert.AreEqual(HCNTDList[0].TAX, Convert.ToDecimal(3600));
            Assert.AreEqual(HCNTDList[0].INCOME, Convert.ToDecimal(2392980));
            Assert.AreEqual(HCNTDList[0].PROFIT, Convert.ToDecimal(25612));
        }
    }
}