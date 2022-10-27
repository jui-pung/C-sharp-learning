using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class GainPayTests
    {
        [TestMethod()]
        public void searchAccSumTest()
        {
            List<profit_sum> sumList = new List<profit_sum>();
            List<profit_accsum> accsumList = new List<profit_accsum>();

            sumList.Add(new profit_sum() { bhno = "592S", cseq = "0123938", tdate = "20210108", dseq = "j0394", dno = "0000002", ttype = "0", ttypename = "現股", bstype = "S", stock = "2303", stocknm = "聯  電", cqty = Convert.ToDecimal(10000.0), mprice = "47.4500", fee = Convert.ToDecimal(676.00), tax = Convert.ToDecimal(1423.00), cost = Convert.ToDecimal(465560.00), income = Convert.ToDecimal(472401.00), profit = Convert.ToDecimal(6841.00), pl_ratio = "1.47%", ctype = "0", ttypename2 = "現賣" });
            sumList.Add(new profit_sum() { bhno = "592S", cseq = "0123938", tdate = "20210108", dseq = "j0394", dno = "0000002", ttype = "0", ttypename = "現股", bstype = "S", stock = "2303", stocknm = "聯  電", cqty = Convert.ToDecimal(10000.0), mprice = "47.4500", fee = Convert.ToDecimal(676.00), tax = Convert.ToDecimal(1423.00), cost = Convert.ToDecimal(465560.00), income = Convert.ToDecimal(472401.00), profit = Convert.ToDecimal(6841.00), pl_ratio = "1.47%", ctype = "0", ttypename2 = "現賣" });

            foreach (var item in accsumList)
            {
                GainPay gainPay = new GainPay();
                accsumList = gainPay.searchAccSum(sumList);
                Assert.AreEqual(item.cqty, Convert.ToDecimal(20000.0));
                Assert.AreEqual(item.cost, Convert.ToDecimal(931120.00));
                Assert.AreEqual(item.income, Convert.ToDecimal(944802.00));
                Assert.AreEqual(item.profit, Convert.ToDecimal(13682.00));
                Assert.AreEqual(item.fee, Convert.ToDecimal(1352.00));
                Assert.AreEqual(item.tax, Convert.ToDecimal(2846.00));
            }
        }

        [TestMethod()]
        public void searchSumTest()
        {
            List<profit_detail_out> detailOutList = new List<profit_detail_out>();
            List<profit_sum> sumList = new List<profit_sum>();

            detailOutList.Add(new profit_detail_out() { tdate = "20210108", dseq = "j0394", dno = "0000002", mqty = Convert.ToDecimal(10000.0), cqty = Convert.ToDecimal(10000.0), mprice = "47.4500", mamt = "474500.0000", cost = Convert.ToDecimal(465560.00), income = Convert.ToDecimal(472401.00), netamt = Convert.ToDecimal(472401.00), fee = Convert.ToDecimal(676.00), tax = Convert.ToDecimal(1423.00), ttype = "0", ttypename = "現股", bstype = "S", wtype = "0", profit = Convert.ToDecimal(6841.00), pl_ratio = "1.47%", ctype = "0", ttypename2 = "現賣" });

            foreach (var item in sumList)
            {
                GainPay gainPay = new GainPay();
                sumList = gainPay.searchSum(detailOutList);
                Assert.AreEqual(item.cqty, Convert.ToDecimal(10000.0));
                Assert.AreEqual(item.cost, Convert.ToDecimal(465560.00));
                Assert.AreEqual(item.income, Convert.ToDecimal(472401.00));
                Assert.AreEqual(item.profit, Convert.ToDecimal(6841.00));
            }
        }

        [TestMethod()]
        public void searchDetailsTest()
        {
            List<profit_detail_out> detailOutList = new List<profit_detail_out>();
            List<profit_detail_out> ResultList = new List<profit_detail_out>();

            //3筆HCNRH(同TDATE、SDSEQ、SDNO)資料
            detailOutList.Add(new profit_detail_out()
            {
                tdate = "20210108",
                dseq = "i0450",
                dno = "0000003",
                mqty = Convert.ToDecimal(5000.0),
                cqty = Convert.ToDecimal(3000.0),
                mprice = "42.5500",
                cost = Convert.ToDecimal(127080.00),
                income = Convert.ToDecimal(127085.00),
                fee = Convert.ToDecimal(182.00),
                tax = Convert.ToDecimal(383.00),
                ttype = "0",
                ttypename = "現股",
                bstype = "S",
                wtype = "0",
                profit = Convert.ToDecimal(5.00)
            }); 
            detailOutList.Add(new profit_detail_out()
            {
                tdate = "20210108",
                dseq = "i0450",
                dno = "0000003",
                mqty = Convert.ToDecimal(5000.0),
                cqty = Convert.ToDecimal(1000.0),
                mprice = "42.5500",
                cost = Convert.ToDecimal(42360.00),
                income = Convert.ToDecimal(42361.00),
                fee = Convert.ToDecimal(61.00),
                tax = Convert.ToDecimal(128.00),
                ttype = "0",
                ttypename = "現股",
                bstype = "S",
                wtype = "0",
                profit = Convert.ToDecimal(1.00)
            });
            detailOutList.Add(new profit_detail_out()
            {
                tdate = "20210108",
                dseq = "i0450",
                dno = "0000003",
                mqty = Convert.ToDecimal(5000.0),
                cqty = Convert.ToDecimal(1000.0),
                mprice = "42.5500",
                cost = Convert.ToDecimal(42411.00),
                income = Convert.ToDecimal(42363.00),
                fee = Convert.ToDecimal(60.00),
                tax = Convert.ToDecimal(127.00),
                ttype = "0",
                ttypename = "現股",
                bstype = "S",
                wtype = "0",
                profit = Convert.ToDecimal(-48.00)
            });

            //3筆HCNTD(同TDATE、SDSEQ、SDNO)資料
            detailOutList.Add(new profit_detail_out()
            {
                tdate = "20210104",
                dseq = "h0057",
                dno = "0000037",
                mqty = Convert.ToDecimal(5000.0),
                cqty = Convert.ToDecimal(2000.0),
                mprice = "28.4000",
                cost = Convert.ToDecimal(56981.0),
                income = Convert.ToDecimal(56635.0),
                netamt = Convert.ToDecimal(141585.0),
                fee = Convert.ToDecimal(80.0),
                tax = Convert.ToDecimal(85.0),
                ttype = "0",
                ttypename = "現股",
                bstype = "S",
                wtype = "0",
                profit = Convert.ToDecimal(-346.0),
                ctype = "0",
                ttypename2 = "賣沖"
            });
            detailOutList.Add(new profit_detail_out()
            {
                tdate = "20210104",
                dseq = "h0057",
                dno = "0000037",
                mqty = Convert.ToDecimal(5000.0),
                cqty = Convert.ToDecimal(1000.0),
                mprice = "28.4000",
                cost = Convert.ToDecimal(28340.0),
                income = Convert.ToDecimal(28315.0),
                fee = Convert.ToDecimal(42.0),
                tax = Convert.ToDecimal(43.0),
                ttype = "0",
                ttypename = "現股",
                bstype = "S",
                wtype = "0",
                profit = Convert.ToDecimal(-25.0),
                ctype = "0",
                ttypename2 = "賣沖"
            });
            detailOutList.Add(new profit_detail_out()
            {
                tdate = "20210104",
                dseq = "h0057",
                dno = "0000037",
                mqty = Convert.ToDecimal(5000.0),
                cqty = Convert.ToDecimal(2000.0),
                mprice = "28.4000",
                cost = Convert.ToDecimal(56580.0),
                income = Convert.ToDecimal(56635.0),
                fee = Convert.ToDecimal(80.0),
                tax = Convert.ToDecimal(85.0),
                ttype = "0",
                ttypename = "現股",
                bstype = "S",
                wtype = "0",
                profit = Convert.ToDecimal(55.0),
                ctype = "0",
                ttypename2 = "賣沖"
            });
            GainPay gainPay = new GainPay();
            ResultList = gainPay.searchDetails(detailOutList);

            Assert.AreEqual(ResultList[0].cqty, Convert.ToDecimal(5000.0));
            Assert.AreEqual(ResultList[0].cost, Convert.ToDecimal(211851.00));
            Assert.AreEqual(ResultList[0].income, Convert.ToDecimal(211809.00));
            Assert.AreEqual(ResultList[0].fee, Convert.ToDecimal(303.00));
            Assert.AreEqual(ResultList[0].tax, Convert.ToDecimal(638.00));
            Assert.AreEqual(ResultList[0].profit, Convert.ToDecimal(-42.00));

            Assert.AreEqual(ResultList[1].cqty, Convert.ToDecimal(5000.0));
            Assert.AreEqual(ResultList[1].cost, Convert.ToDecimal(141901.00));
            Assert.AreEqual(ResultList[1].income, Convert.ToDecimal(141585.00));
            Assert.AreEqual(ResultList[1].fee, Convert.ToDecimal(202.00));
            Assert.AreEqual(ResultList[1].tax, Convert.ToDecimal(213.00));
            Assert.AreEqual(ResultList[1].profit, Convert.ToDecimal(-316.00));

        }
    }
}