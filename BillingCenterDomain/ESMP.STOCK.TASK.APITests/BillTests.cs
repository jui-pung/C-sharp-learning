using Microsoft.VisualStudio.TestTools.UnitTesting;
using ESMP.STOCK.TASK.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESMP.STOCK.FORMAT;
using ESMP.STOCK.DB.TABLE;
using ESMP.STOCK.TASK.APITests;

namespace ESMP.STOCK.TASK.API.Tests
{
    [TestClass()]
    public class BillTests
    {
        [TestMethod()]
        public void searchSumTest()
        {
            List<profile> detailList = new List<profile>();
            billSum billsum = new billSum();
            detailList.Add(new profile()
            {
                bhno = "592S",
                cseq = "0098047",
                stock = "3041",
                stocknm = "揚智",
                mdate = "20221003",
                dseq = "f0021",
                dno = "0000014",
                ttype = "0",
                ttypename = "現買",
                bstype = "B",
                bstypename = "買",
                etype = "0",
                mprice = Convert.ToDecimal(19.9500),
                mqty = Convert.ToDecimal(2000),
                mamt = Convert.ToDecimal(39900.00),
                fee = Convert.ToDecimal(56.00),
                tax = Convert.ToDecimal(0.00),
                netamt = Convert.ToDecimal(-39956.00)
            });
            detailList.Add(new profile()
            {
                bhno = "592S",
                cseq = "0098047",
                stock = "3041",
                stocknm = "揚智",
                mdate = "20221003",
                dseq = "f0021",
                dno = "0000014",
                ttype = "0",
                ttypename = "現買",
                bstype = "B",
                bstypename = "買",
                etype = "0",
                mprice = Convert.ToDecimal(19.9500),
                mqty = Convert.ToDecimal(2000),
                mamt = Convert.ToDecimal(39900.00),
                fee = Convert.ToDecimal(56.00),
                tax = Convert.ToDecimal(0.00),
                netamt = Convert.ToDecimal(-39956.00)
            });
            Bill bill = new Bill();
            billsum = bill.searchSum(detailList);
            Assert.AreEqual(billsum.cnbamt, Convert.ToDecimal(0));
            Assert.AreEqual(billsum.cnsamt, Convert.ToDecimal(0));
            Assert.AreEqual(billsum.cnfee, Convert.ToDecimal(112.0));
            Assert.AreEqual(billsum.cntax, Convert.ToDecimal(0.0));
            Assert.AreEqual(billsum.cnnetamt, Convert.ToDecimal(-79912.0));
            Assert.AreEqual(billsum.bqty, Convert.ToDecimal(4000.0));
            Assert.AreEqual(billsum.sqty, Convert.ToDecimal(0));
        }

        [TestMethod()]
        public void getBillSearchTest_5()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
            List<HCNTD> HCNTDList = new List<HCNTD>();
            List<TCNTD> TCNTDList = new List<TCNTD>();
            List<T210> T210List = new List<T210>();
            List<profile> profileList = new List<profile>();
            billSum billsum = new billSum();
            Bill bill = new Bill();
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "5920",
                DSEQ = "T0726",
                JRNUM = "00600970",
                MTYPE = "T",
                CSEQ = "0002141",
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
                CSEQ = "0002141",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "B",
                STOCK = "2330",
                QTY = Convert.ToDecimal(10000),
                PRICE = Convert.ToDecimal(394)
            });
            string BHNO = TMHIOList[0].BHNO;
            string CSEQ = TMHIOList[0].CSEQ;
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCUMS_Dic;
            (TCNUDList, HCNRHList, HCNTDList, HCMIOList) = ESMPData.GetESMPData(TCNUDList, TMHIOList, TCSIOList, TCNTDList, T210List, BHNO, CSEQ);
            profileList = bill.searchDetails_Today(HCMIOList, HCNTDList, HCNRHList, BHNO, CSEQ);
            Assert.AreEqual(profileList.Count, 2);
            Assert.AreEqual(profileList[0].ttypename, "賣沖");
            Assert.AreEqual(profileList[1].ttypename, "買沖");
            Assert.AreEqual(profileList[0].netamt, Convert.ToDecimal(3978330));
            Assert.AreEqual(profileList[1].netamt, Convert.ToDecimal(-3945614));
        }

        [TestMethod()]
        public void getBillSearchTest_6()
        {
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<HCNRH> HCNRHList = new List<HCNRH>();
            List<HCNTD> HCNTDList = new List<HCNTD>();
            List<TCNTD> TCNTDList = new List<TCNTD>();
            List<T210> T210List = new List<T210>();
            List<profile> profileList = new List<profile>();
            billSum billsum = new billSum();
            Bill bill = new Bill();
            TCNUDList.Add(new TCNUD()
            {
                TDATE = "20160504",
                BHNO = "5920",
                CSEQ = "0002141",
                STOCK = "2330",
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
                CSEQ = "0002141",
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
                CSEQ = "0002141",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "B",
                STOCK = "2330",
                QTY = Convert.ToDecimal(7000),
                PRICE = Convert.ToDecimal(394)
            });
            string BHNO = TMHIOList[0].BHNO;
            string CSEQ = TMHIOList[0].CSEQ;
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCUMS_Dic;
            (TCNUDList, HCNRHList, HCNTDList, HCMIOList) = ESMPData.GetESMPData(TCNUDList, TMHIOList, TCSIOList, TCNTDList, T210List, BHNO, CSEQ);
            profileList = bill.searchDetails_Today(HCMIOList, HCNTDList, HCNRHList, BHNO, CSEQ);
            Assert.AreEqual(profileList.Count, 3);
            Assert.AreEqual(profileList[0].ttypename, "賣沖");
            Assert.AreEqual(profileList[0].netamt, Convert.ToDecimal(2784831));
        }
    }
}