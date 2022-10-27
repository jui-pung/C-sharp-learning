﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using ESMP.STOCK.TASK.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESMP.STOCK.FORMAT.API;

namespace ESMP.STOCK.TASK.API.Tests
{
    [TestClass()]
    public class GainLostTests
    {
        [TestMethod()]
        public void searchDetailsTest()
        {
            List<unoffset_qtype_detail> detailList = new List<unoffset_qtype_detail>();
            foreach (var item in detailList)
            {
                item.bqty = 2000;
                item.mprice = Convert.ToDecimal(79.250);
                item.lastprice = Convert.ToDecimal(24.5);
                item.cost = Convert.ToDecimal(10000);
                GainLost gainLost = new GainLost();
                detailList = gainLost.searchDetails(detailList);
                //Assert.AreEqual(item.mamt, 158500);
                Assert.AreEqual(item.estimateAmt, Convert.ToDecimal(49000));
                Assert.AreEqual(item.estimateFee, Convert.ToDecimal(69.82));
                Assert.AreEqual(item.estimateTax, Convert.ToDecimal(147));
                Assert.AreEqual(item.marketvalue, Convert.ToDecimal(48783.18));
                Assert.AreEqual(item.profit, Convert.ToDecimal(38783.18));
            }
        }

        [TestMethod()]
        public void searchSumTest()
        {
            List<unoffset_qtype_detail> detailList = new List<unoffset_qtype_detail>();
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();

            detailList.Add(new unoffset_qtype_detail() { tdate = "20220308", ttype = "0", ttypename = "現買", bstype = "B", dseq = "i0602", dno = "0000001", bqty = Convert.ToDecimal(1000.0), mprice = Convert.ToDecimal(89.1000), mamt = Convert.ToDecimal(89100.0000), lastprice = Convert.ToDecimal(65.0000), marketvalue = Convert.ToDecimal(64713.0), fee = Convert.ToDecimal(126.00), tax = Convert.ToDecimal(0.0), cost = Convert.ToDecimal(89226.00), estimateAmt = Convert.ToDecimal(65000.0), estimateFee = Convert.ToDecimal(92.0), estimateTax = Convert.ToDecimal(195.0), profit = Convert.ToDecimal(-24513.00), pl_ratio = "-27.47%" });
            detailList.Add(new unoffset_qtype_detail() { tdate = "20220427", ttype = "0", ttypename = "現買", bstype = "B", dseq = "t0493", dno = "0000001", bqty = Convert.ToDecimal(1000.0), mprice = Convert.ToDecimal(83.0000), mamt = Convert.ToDecimal(83000.0000), lastprice = Convert.ToDecimal(65.0000), marketvalue = Convert.ToDecimal(64713.0), fee = Convert.ToDecimal(118.00), tax = Convert.ToDecimal(0.0), cost = Convert.ToDecimal(83118.00), estimateAmt = Convert.ToDecimal(65000.0), estimateFee = Convert.ToDecimal(92.0), estimateTax = Convert.ToDecimal(195.0), profit = Convert.ToDecimal(-18405.00), pl_ratio = "-22.14%" });

            foreach (var item in sumList)
            {
                GainLost gainLost = new GainLost();
                sumList = gainLost.searchSum(detailList);
                Assert.AreEqual(item.estimateAmt, Convert.ToDecimal(130000.0));
                Assert.AreEqual(item.estimateFee, Convert.ToDecimal(184.0));
                Assert.AreEqual(item.estimateTax, Convert.ToDecimal(390.0));
                Assert.AreEqual(item.marketvalue, Convert.ToDecimal(129426.0));
                Assert.AreEqual(item.profit, Convert.ToDecimal(-42918.00));
            }
        }

        [TestMethod()]
        public void searchAccSumTest()
        {
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();
            List<unoffset_qtype_accsum> accsumList = new List<unoffset_qtype_accsum>();

            sumList.Add(new unoffset_qtype_sum() { stock = "4721", stocknm = "美琪瑪", ttype = "0", ttypename = "現買", bstype = "B", bqty = Convert.ToDecimal(40.0), cost = Convert.ToDecimal(734.00), avgprice = Convert.ToDecimal(18.35), lastprice = Convert.ToDecimal(105.5000), marketvalue = Convert.ToDecimal(4188.0), profit = Convert.ToDecimal(3454.00), pl_ratio = "470.57%", fee = Convert.ToDecimal(2.00), tax = Convert.ToDecimal(0.0), estimateAmt = Convert.ToDecimal(4220.0), estimateFee = Convert.ToDecimal(20.0), estimateTax = Convert.ToDecimal(12.0), amt = Convert.ToDecimal(732.0000) });
            sumList.Add(new unoffset_qtype_sum() { stock = "5478", stocknm = "智  冠", ttype = "0", ttypename = "現買", bstype = "B", bqty = Convert.ToDecimal(152.0), cost = Convert.ToDecimal(5342.00), avgprice = Convert.ToDecimal(35.14), lastprice = Convert.ToDecimal(76.3000), marketvalue = Convert.ToDecimal(11543.0), profit = Convert.ToDecimal(6201.00), pl_ratio = "116.08%", fee = Convert.ToDecimal(7.00), tax = Convert.ToDecimal(0.0), estimateAmt = Convert.ToDecimal(11597.0), estimateFee = Convert.ToDecimal(20.0), estimateTax = Convert.ToDecimal(34.0), amt = Convert.ToDecimal(5335.2000) });

            foreach (var item in accsumList)
            {
                GainLost gainLost = new GainLost();
                accsumList = gainLost.searchAccSum(sumList);
                Assert.AreEqual(item.estimateAmt, Convert.ToDecimal(15817.0));
                Assert.AreEqual(item.estimateFee, Convert.ToDecimal(40.0));
                Assert.AreEqual(item.estimateTax, Convert.ToDecimal(46.0));
                Assert.AreEqual(item.marketvalue, Convert.ToDecimal(15731.0));
                Assert.AreEqual(item.profit, Convert.ToDecimal(9655.00));
            }
        }
    }
}