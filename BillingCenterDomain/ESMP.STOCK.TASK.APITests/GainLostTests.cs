using Microsoft.VisualStudio.TestTools.UnitTesting;
using ESMP.STOCK.TASK.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESMP.STOCK.FORMAT;
using ESMP.STOCK.DB.TABLE;

namespace ESMP.STOCK.TASK.API.Tests
{
    [TestClass()]
    public class GainLostTests
    {
        [TestMethod()]
        public void searchSumTest()
        {
            List<unoffset_qtype_detail> detailList = new List<unoffset_qtype_detail>();
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();
            List<TCNUD> dbTCNUD = new List<TCNUD>();
            dbTCNUD.Add(new TCNUD() { STOCK = "1702", TDATE = "20220308", DSEQ = "i0602", DNO = "0000001", BQTY = Convert.ToDecimal(1000.0), PRICE = Convert.ToDecimal(89.1000), FEE = Convert.ToDecimal(126.00), COST = Convert.ToDecimal(89226.00) });
            dbTCNUD.Add(new TCNUD() { STOCK = "1702", TDATE = "20220427", DSEQ = "t0493", DNO = "0000001", BQTY = Convert.ToDecimal(1000.0), PRICE = Convert.ToDecimal(83.0000), FEE = Convert.ToDecimal(118.00), COST = Convert.ToDecimal(83118.00) });
            //detailList.Add(new unoffset_qtype_detail() { tdate = "20220308", ttype = "0", ttypename = "現買", bstype = "B", dseq = "i0602", dno = "0000001", bqty = Convert.ToDecimal(1000.0), mprice = Convert.ToDecimal(89.1000), mamt = Convert.ToDecimal(89100.0000), lastprice = Convert.ToDecimal(65.0000), marketvalue = Convert.ToDecimal(64713.0), fee = Convert.ToDecimal(126.00), tax = Convert.ToDecimal(0.0), cost = Convert.ToDecimal(89226.00), estimateAmt = Convert.ToDecimal(65000.0), estimateFee = Convert.ToDecimal(92.0), estimateTax = Convert.ToDecimal(195.0), profit = Convert.ToDecimal(-24513.00), pl_ratio = "-27.47%" });
            //detailList.Add(new unoffset_qtype_detail() { tdate = "20220427", ttype = "0", ttypename = "現買", bstype = "B", dseq = "t0493", dno = "0000001", bqty = Convert.ToDecimal(1000.0), mprice = Convert.ToDecimal(83.0000), mamt = Convert.ToDecimal(83000.0000), lastprice = Convert.ToDecimal(65.0000), marketvalue = Convert.ToDecimal(64713.0), fee = Convert.ToDecimal(118.00), tax = Convert.ToDecimal(0.0), cost = Convert.ToDecimal(83118.00), estimateAmt = Convert.ToDecimal(65000.0), estimateFee = Convert.ToDecimal(92.0), estimateTax = Convert.ToDecimal(195.0), profit = Convert.ToDecimal(-18405.00), pl_ratio = "-22.14%" });
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCSRH_Dic;
            GainLost gainLost = new GainLost();
            //sumList = gainLost.searchSum(dbTCNUD);
            detailList = sumList[0].unoffset_qtype_detail;
            foreach (var item in detailList)
            {
                //49.5 * 1000
                Assert.AreEqual(item.estimateAmt, Convert.ToDecimal(49500));
                //49500 * 0.001425
                Assert.AreEqual(item.estimateFee, Convert.ToDecimal(70));
                //49500 * 0.003
                Assert.AreEqual(item.estimateTax, Convert.ToDecimal(148));
                Assert.AreEqual(item.marketvalue, Convert.ToDecimal(49282));
            }
            foreach (var item in sumList)
            {
                Assert.AreEqual(item.estimateAmt, Convert.ToDecimal(99000));
                Assert.AreEqual(item.estimateFee, Convert.ToDecimal(140));
                Assert.AreEqual(item.estimateTax, Convert.ToDecimal(296));
                Assert.AreEqual(item.marketvalue, Convert.ToDecimal(98564));
                Assert.AreEqual(item.profit, Convert.ToDecimal(-73780));
            }
        }

        [TestMethod()]
        public void searchAccSumTest()
        {
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();
            unoffset_qtype_accsum accsum = new unoffset_qtype_accsum();

            sumList.Add(new unoffset_qtype_sum() { stock = "4721", stocknm = "美琪瑪", ttype = "0", ttypename = "現買", bstype = "B", bqty = Convert.ToDecimal(40.0), cost = Convert.ToDecimal(734.00), avgprice = Convert.ToDecimal(18.35), lastprice = Convert.ToDecimal(105.5000), marketvalue = Convert.ToDecimal(4188.0), profit = Convert.ToDecimal(3454.00), pl_ratio = "470.57%", fee = Convert.ToDecimal(2.00), tax = Convert.ToDecimal(0.0), estimateAmt = Convert.ToDecimal(4220.0), estimateFee = Convert.ToDecimal(20.0), estimateTax = Convert.ToDecimal(12.0), amt = Convert.ToDecimal(732.0000) });
            sumList.Add(new unoffset_qtype_sum() { stock = "5478", stocknm = "智  冠", ttype = "0", ttypename = "現買", bstype = "B", bqty = Convert.ToDecimal(152.0), cost = Convert.ToDecimal(5342.00), avgprice = Convert.ToDecimal(35.14), lastprice = Convert.ToDecimal(76.3000), marketvalue = Convert.ToDecimal(11543.0), profit = Convert.ToDecimal(6201.00), pl_ratio = "116.08%", fee = Convert.ToDecimal(7.00), tax = Convert.ToDecimal(0.0), estimateAmt = Convert.ToDecimal(11597.0), estimateFee = Convert.ToDecimal(20.0), estimateTax = Convert.ToDecimal(34.0), amt = Convert.ToDecimal(5335.2000) });
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCSRH_Dic;
            GainLost gainLost = new GainLost();
            accsum = gainLost.searchAccSum(sumList);
            Assert.AreEqual(accsum.estimateAmt, Convert.ToDecimal(15817.0));
            Assert.AreEqual(accsum.estimateFee, Convert.ToDecimal(40.0));
            Assert.AreEqual(accsum.estimateTax, Convert.ToDecimal(46.0));
            Assert.AreEqual(accsum.marketvalue, Convert.ToDecimal(15731.0));
            Assert.AreEqual(accsum.profit, Convert.ToDecimal(9655.00));
        }
    }
}