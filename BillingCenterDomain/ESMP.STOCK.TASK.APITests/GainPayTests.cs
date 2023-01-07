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
                accsumList = SubGainPay.SubSearchAccSum(sumList);
                Assert.AreEqual(item.cqty, Convert.ToDecimal(20000.0));
                Assert.AreEqual(item.cost, Convert.ToDecimal(931120.00));
                Assert.AreEqual(item.income, Convert.ToDecimal(944802.00));
                Assert.AreEqual(item.profit, Convert.ToDecimal(13682.00));
                Assert.AreEqual(item.fee, Convert.ToDecimal(1352.00));
                Assert.AreEqual(item.tax, Convert.ToDecimal(2846.00));
            }
        }

        /// <summary>
        /// 融資已實現損益 單元測試
        /// </summary>
        [TestMethod]
        public void HCRRHTest_3()
        {
            List<profit_sum> sumList = new List<profit_sum>();
            List<profit_accsum> accsumList = new List<profit_accsum>();
            List<HCRRH> dbHCRRH = new List<HCRRH>();
            _ = BasicData.MSTMB_Dic;
            #region 3筆HCRRH資料 3筆(同TDATE、DBDSEQ、DBDNO)
            dbHCRRH.Add(new HCRRH()
            {
                BHNO = "5920",
                TDATE = "20221203",
                DSEQ = "f0193",
                DNO = "0000007",
                RDATE = "20211222",
                RDSEQ = "o0043",
                RDNO = "0000002",
                CSEQ = "9813603",
                RCODE = "1",
                STOCK = "8081",
                BPRICE = Convert.ToDecimal(262.0000),
                BQTY = Convert.ToDecimal(9000),
                CQTY = Convert.ToDecimal(9000),
                CCRAMT = Convert.ToDecimal(1414000),
                CCRINT = Convert.ToDecimal(2964),
                BFEE = Convert.ToDecimal(3360),
                SPRICE = Convert.ToDecimal(256.5000),
                SQTY = Convert.ToDecimal(9000),
                SFEE = Convert.ToDecimal(3185),
                TAX = Convert.ToDecimal(6925),
                INCOME = Convert.ToDecimal(0),
                COST = Convert.ToDecimal(0),
                PROFIT = Convert.ToDecimal(0),
                SFCODE = "5920",
                STINTAX = Convert.ToDecimal(0),
                TRDATE = "20220106",
                TRTIME = "173854",
                MODDATE = "20220225",
                MODTIME = "202802",
                MODUSER = "DBTRIN"
            });
            dbHCRRH.Add(new HCRRH()
            {
                BHNO = "5920",
                TDATE = "20221203",
                DSEQ = "f0193",
                DNO = "0000007",
                RDATE = "20211222",
                RDSEQ = "o0043",
                RDNO = "0000002",
                CSEQ = "9813603",
                RCODE = "1",
                STOCK = "8081",
                BPRICE = Convert.ToDecimal(262.0000),
                BQTY = Convert.ToDecimal(9000),
                CQTY = Convert.ToDecimal(9000),
                CCRAMT = Convert.ToDecimal(1414000),
                CCRINT = Convert.ToDecimal(2964),
                BFEE = Convert.ToDecimal(3360),
                SPRICE = Convert.ToDecimal(256.5000),
                SQTY = Convert.ToDecimal(9000),
                SFEE = Convert.ToDecimal(3185),
                TAX = Convert.ToDecimal(6925),
                INCOME = Convert.ToDecimal(0),
                COST = Convert.ToDecimal(0),
                PROFIT = Convert.ToDecimal(0),
                SFCODE = "5920",
                STINTAX = Convert.ToDecimal(0),
                TRDATE = "20220106",
                TRTIME = "173854",
                MODDATE = "20220225",
                MODTIME = "202802",
                MODUSER = "DBTRIN"
            });
            dbHCRRH.Add(new HCRRH()
            {
                BHNO = "5920",
                TDATE = "20221203",
                DSEQ = "f0193",
                DNO = "0000007",
                RDATE = "20211222",
                RDSEQ = "o0043",
                RDNO = "0000002",
                CSEQ = "9813603",
                RCODE = "1",
                STOCK = "8081",
                BPRICE = Convert.ToDecimal(262.0000),
                BQTY = Convert.ToDecimal(9000),
                CQTY = Convert.ToDecimal(9000),
                CCRAMT = Convert.ToDecimal(1414000),
                CCRINT = Convert.ToDecimal(2964),
                BFEE = Convert.ToDecimal(3360),
                SPRICE = Convert.ToDecimal(256.5000),
                SQTY = Convert.ToDecimal(9000),
                SFEE = Convert.ToDecimal(3185),
                TAX = Convert.ToDecimal(6925),
                INCOME = Convert.ToDecimal(0),
                COST = Convert.ToDecimal(0),
                PROFIT = Convert.ToDecimal(0),
                SFCODE = "5920",
                STINTAX = Convert.ToDecimal(0),
                TRDATE = "20220106",
                TRTIME = "173854",
                MODDATE = "20220225",
                MODTIME = "202802",
                MODUSER = "DBTRIN"
            });
            #endregion
            sumList = SubGainPay.SubSearchSum(dbHCRRH, "5920", "9813603");
            accsumList = SubGainPay.SubSearchAccSum(sumList);
            Assert.AreEqual(accsumList.Count, 1);
            Assert.AreEqual(sumList.Count, 1);
            Assert.AreEqual(sumList[0].profit_detail.Count, 3);
            Assert.AreEqual(sumList[0].ttypename2, "資賣");
            Assert.AreEqual(sumList[0].profit_detail[0].mamt, "2358000");
            Assert.AreEqual(sumList[0].profit_detail_out.cqty, Convert.ToDecimal(27000));
            Assert.AreEqual(sumList[0].profit_detail_out.cost, Convert.ToDecimal(0));
        }
        /// <summary>
        /// 融券已實現損益 單元測試
        /// </summary>
        [TestMethod]
        public void HDBRHTest_4()
        {
            List<profit_sum> sumList = new List<profit_sum>();
            List<profit_accsum> accsumList = new List<profit_accsum>();
            List<HDBRH> dbHDBRH = new List<HDBRH>();
            _ = BasicData.MSTMB_Dic;
            #region 3筆HDBRH資料 3筆(同TDATE、DBDSEQ、DBDNO)
            dbHDBRH.Add(new HDBRH()
            {
                BHNO = "5920",
                TDATE = "20221206",
                DSEQ = "j1885",
                DNO = "0000013",
                RDATE = "20220104",
                RDSEQ = "i0656",
                RDNO = "0000008",
                CSEQ = "9813603",
                RCODE = "1",
                STOCK = "8450",
                SPRICE = Convert.ToDecimal(61.2000),
                SQTY = Convert.ToDecimal(1000),
                CQTY = Convert.ToDecimal(1000),
                CDBAMT = Convert.ToDecimal(122082),
                CGTAMT = Convert.ToDecimal(61200),
                CGTINT = Convert.ToDecimal(1),
                CDNAMT = Convert.ToDecimal(60882),
                CDNINT = Convert.ToDecimal(1),
                CDLFEE = Convert.ToDecimal(0),
                LBFINT = Convert.ToDecimal(0),
                SFEE = Convert.ToDecimal(87),
                TAX = Convert.ToDecimal(92),
                DBFEE = Convert.ToDecimal(0),
                BPRICE = Convert.ToDecimal(67.6000),
                BQTY = Convert.ToDecimal(2000),
                BFEE = Convert.ToDecimal(96),
                INCOME = Convert.ToDecimal(0),
                COST = Convert.ToDecimal(0),
                PROFIT = Convert.ToDecimal(0),
                SFCODE = "5920",
                STINTAX = Convert.ToDecimal(0),
                HEALTHFEE = Convert.ToDecimal(0),
                TRDATE = "20220106",
                TRTIME = "173650",
                MODDATE = "20220225",
                MODTIME = "203942",
                MODUSER = "DBTRIN"
            });
            dbHDBRH.Add(new HDBRH()
            {
                BHNO = "5920",
                TDATE = "20221206",
                DSEQ = "j1885",
                DNO = "0000013",
                RDATE = "20220104",
                RDSEQ = "i0656",
                RDNO = "0000008",
                CSEQ = "9813603",
                RCODE = "1",
                STOCK = "8450",
                SPRICE = Convert.ToDecimal(61.2000),
                SQTY = Convert.ToDecimal(1000),
                CQTY = Convert.ToDecimal(1000),
                CDBAMT = Convert.ToDecimal(122082),
                CGTAMT = Convert.ToDecimal(61200),
                CGTINT = Convert.ToDecimal(1),
                CDNAMT = Convert.ToDecimal(60882),
                CDNINT = Convert.ToDecimal(1),
                CDLFEE = Convert.ToDecimal(0),
                LBFINT = Convert.ToDecimal(0),
                SFEE = Convert.ToDecimal(87),
                TAX = Convert.ToDecimal(92),
                DBFEE = Convert.ToDecimal(0),
                BPRICE = Convert.ToDecimal(67.6000),
                BQTY = Convert.ToDecimal(2000),
                BFEE = Convert.ToDecimal(96),
                INCOME = Convert.ToDecimal(0),
                COST = Convert.ToDecimal(0),
                PROFIT = Convert.ToDecimal(0),
                SFCODE = "5920",
                STINTAX = Convert.ToDecimal(0),
                HEALTHFEE = Convert.ToDecimal(0),
                TRDATE = "20220106",
                TRTIME = "173650",
                MODDATE = "20220225",
                MODTIME = "203942",
                MODUSER = "DBTRIN"
            });
            dbHDBRH.Add(new HDBRH()
            {
                BHNO = "5920",
                TDATE = "20221206",
                DSEQ = "j1885",
                DNO = "0000013",
                RDATE = "20220104",
                RDSEQ = "i0656",
                RDNO = "0000008",
                CSEQ = "9813603",
                RCODE = "1",
                STOCK = "8450",
                SPRICE = Convert.ToDecimal(61.2000),
                SQTY = Convert.ToDecimal(1000),
                CQTY = Convert.ToDecimal(1000),
                CDBAMT = Convert.ToDecimal(122082),
                CGTAMT = Convert.ToDecimal(61200),
                CGTINT = Convert.ToDecimal(1),
                CDNAMT = Convert.ToDecimal(60882),
                CDNINT = Convert.ToDecimal(1),
                CDLFEE = Convert.ToDecimal(0),
                LBFINT = Convert.ToDecimal(0),
                SFEE = Convert.ToDecimal(87),
                TAX = Convert.ToDecimal(92),
                DBFEE = Convert.ToDecimal(0),
                BPRICE = Convert.ToDecimal(67.6000),
                BQTY = Convert.ToDecimal(2000),
                BFEE = Convert.ToDecimal(96),
                INCOME = Convert.ToDecimal(0),
                COST = Convert.ToDecimal(0),
                PROFIT = Convert.ToDecimal(0),
                SFCODE = "5920",
                STINTAX = Convert.ToDecimal(0),
                HEALTHFEE = Convert.ToDecimal(0),
                TRDATE = "20220106",
                TRTIME = "173650",
                MODDATE = "20220225",
                MODTIME = "203942",
                MODUSER = "DBTRIN"
            });
            #endregion
            sumList = SubGainPay.SubSearchSum(dbHDBRH, "5920", "9813603");
            accsumList = SubGainPay.SubSearchAccSum(sumList);
            Assert.AreEqual(accsumList.Count, 1);
            Assert.AreEqual(sumList.Count, 1);
            Assert.AreEqual(sumList[0].profit_detail.Count, 3);
            Assert.AreEqual(sumList[0].ttypename2, "券買");
            Assert.AreEqual(sumList[0].profit_detail[0].mamt, "61200.0");
            Assert.AreEqual(sumList[0].profit_detail_out.cqty, Convert.ToDecimal(3000));
            Assert.AreEqual(sumList[0].profit_detail_out.cost, Convert.ToDecimal(0));
        }
        /// <summary>
        /// 信用當沖已實現損益 單元測試
        /// </summary>
        [TestMethod]
        public void HCDTDTest_5()
        {
            List<profit_sum> sumList = new List<profit_sum>();
            List<profit_accsum> accsumList = new List<profit_accsum>();
            List<HCDTD> dbHCDTD = new List<HCDTD>();
            _ = BasicData.MSTMB_Dic;
            #region 4筆HCDTD資料 其中3筆(同TDATE、DBDSEQ、DBDNO)
            dbHCDTD.Add(new HCDTD() {
                TDATE = "20221202",
                BHNO = "5920",
                CRDSEQ = "10006",
                CRDNO = "000007",
                DBDSEQ = "55001",
                DBDNO = "000008",
                CSEQ = "9813603",
                STOCK = "1234",
                QTY = Convert.ToDecimal(26000),
                BPRICE = Convert.ToDecimal(16.7000),
                BQTY = Convert.ToDecimal(26000),
                BFEE = Convert.ToDecimal(618),
                SPRICE = Convert.ToDecimal(17.4500),
                SQTY = Convert.ToDecimal(26000),
                SFEE = Convert.ToDecimal(646),
                TAX = Convert.ToDecimal(1361),
                DBFEE = Convert.ToDecimal(362),
                INCOME = Convert.ToDecimal(451331),
                COST = Convert.ToDecimal(434818),
                PROFIT = Convert.ToDecimal(16513),
                SFCODE = "5920",
                STINTAX = Convert.ToDecimal(0),
                TRDATE = "20111125",
                TRTIME = "091001",
                MODDATE = "20120206",
                MODTIME = "141404",
                MODUSER = "DBTrans"
            });
            dbHCDTD.Add(new HCDTD()
            {
                TDATE = "20221206",
                BHNO = "5920",
                CRDSEQ = "10006",
                CRDNO = "000035",
                DBDSEQ = "R0107",
                DBDNO = "000033",
                CSEQ = "9813603",
                STOCK = "8105",
                QTY = Convert.ToDecimal(1000),
                BPRICE = Convert.ToDecimal(24.9000),
                BQTY = Convert.ToDecimal(1000),
                BFEE = Convert.ToDecimal(35),
                SPRICE = Convert.ToDecimal(26.3000),
                SQTY = Convert.ToDecimal(1000),
                SFEE = Convert.ToDecimal(37),
                TAX = Convert.ToDecimal(78),
                DBFEE = Convert.ToDecimal(7),
                INCOME = Convert.ToDecimal(26178),
                COST = Convert.ToDecimal(24935),
                PROFIT = Convert.ToDecimal(1243),
                SFCODE = "5920",
                STINTAX = Convert.ToDecimal(0),
                TRDATE = "20111125",
                TRTIME = "091001",
                MODDATE = "20120206",
                MODTIME = "141404",
                MODUSER = "DBTrans"
            });
            dbHCDTD.Add(new HCDTD()
            {
                TDATE = "20221206",
                BHNO = "5920",
                CRDSEQ = "10006",
                CRDNO = "000035",
                DBDSEQ = "R0107",
                DBDNO = "000033",
                CSEQ = "9813603",
                STOCK = "8105",
                QTY = Convert.ToDecimal(1000),
                BPRICE = Convert.ToDecimal(24.9000),
                BQTY = Convert.ToDecimal(1000),
                BFEE = Convert.ToDecimal(35),
                SPRICE = Convert.ToDecimal(26.3000),
                SQTY = Convert.ToDecimal(1000),
                SFEE = Convert.ToDecimal(37),
                TAX = Convert.ToDecimal(78),
                DBFEE = Convert.ToDecimal(7),
                INCOME = Convert.ToDecimal(26178),
                COST = Convert.ToDecimal(24935),
                PROFIT = Convert.ToDecimal(1243),
                SFCODE = "5920",
                STINTAX = Convert.ToDecimal(0),
                TRDATE = "20111125",
                TRTIME = "091001",
                MODDATE = "20120206",
                MODTIME = "141404",
                MODUSER = "DBTrans"
            });
            dbHCDTD.Add(new HCDTD()
            {
                TDATE = "20221206",
                BHNO = "5920",
                CRDSEQ = "10006",
                CRDNO = "000035",
                DBDSEQ = "R0107",
                DBDNO = "000033",
                CSEQ = "9813603",
                STOCK = "8105",
                QTY = Convert.ToDecimal(1000),
                BPRICE = Convert.ToDecimal(24.9000),
                BQTY = Convert.ToDecimal(1000),
                BFEE = Convert.ToDecimal(35),
                SPRICE = Convert.ToDecimal(26.3000),
                SQTY = Convert.ToDecimal(1000),
                SFEE = Convert.ToDecimal(37),
                TAX = Convert.ToDecimal(78),
                DBFEE = Convert.ToDecimal(7),
                INCOME = Convert.ToDecimal(26178),
                COST = Convert.ToDecimal(24935),
                PROFIT = Convert.ToDecimal(1243),
                SFCODE = "5920",
                STINTAX = Convert.ToDecimal(0),
                TRDATE = "20111125",
                TRTIME = "091001",
                MODDATE = "20120206",
                MODTIME = "141404",
                MODUSER = "DBTrans"
            });
            #endregion
            sumList = SubGainPay.SubSearchSum(dbHCDTD, "5920", "9813603");
            accsumList = SubGainPay.SubSearchAccSum(sumList);
            Assert.AreEqual(accsumList.Count, 1);
            Assert.AreEqual(sumList.Count, 2);
            Assert.AreEqual(sumList[0].profit_detail.Count, 1);
            Assert.AreEqual(sumList[1].profit_detail.Count, 3);
            Assert.AreEqual(sumList[0].ttypename2, "信沖");
            Assert.AreEqual(sumList[0].profit_detail[0].mamt, "434200.0");
            Assert.AreEqual(sumList[1].profit_detail_out.cqty, Convert.ToDecimal(3000));
            Assert.AreEqual(sumList[1].profit_detail_out.cost, Convert.ToDecimal(74805));
        }

        //[TestMethod()]
        //public void searchSumTest()
        //{
        //    List<profit_detail_out> detailOutList = new List<profit_detail_out>();
        //    List<profit_sum> sumList = new List<profit_sum>();

        //    detailOutList.Add(new profit_detail_out() { tdate = "20210108", dseq = "j0394", dno = "0000002", mqty = Convert.ToDecimal(10000.0), cqty = Convert.ToDecimal(10000.0), mprice = "47.4500", mamt = "474500.0000", cost = Convert.ToDecimal(465560.00), income = Convert.ToDecimal(472401.00), netamt = Convert.ToDecimal(472401.00), fee = Convert.ToDecimal(676.00), tax = Convert.ToDecimal(1423.00), ttype = "0", ttypename = "現股", bstype = "S", wtype = "0", profit = Convert.ToDecimal(6841.00), pl_ratio = "1.47%", ctype = "0", ttypename2 = "現賣" });

        //    foreach (var item in sumList)
        //    {
        //        GainPay gainPay = new GainPay();
        //        sumList = gainPay.searchSum(detailOutList);
        //        Assert.AreEqual(item.cqty, Convert.ToDecimal(10000.0));
        //        Assert.AreEqual(item.cost, Convert.ToDecimal(465560.00));
        //        Assert.AreEqual(item.income, Convert.ToDecimal(472401.00));
        //        Assert.AreEqual(item.profit, Convert.ToDecimal(6841.00));
        //    }
        //}

        //[TestMethod()]
        //public void searchDetailsTest()
        //{
        //    List<profit_detail_out> detailOutList = new List<profit_detail_out>();
        //    List<profit_detail_out> ResultList = new List<profit_detail_out>();

        //    //3筆HCNRH(同TDATE、SDSEQ、SDNO)資料
        //    detailOutList.Add(new profit_detail_out()
        //    {
        //        tdate = "20210108",
        //        dseq = "i0450",
        //        dno = "0000003",
        //        mqty = Convert.ToDecimal(5000.0),
        //        cqty = Convert.ToDecimal(3000.0),
        //        mprice = "42.5500",
        //        cost = Convert.ToDecimal(127080.00),
        //        income = Convert.ToDecimal(127085.00),
        //        fee = Convert.ToDecimal(182.00),
        //        tax = Convert.ToDecimal(383.00),
        //        ttype = "0",
        //        ttypename = "現股",
        //        bstype = "S",
        //        wtype = "0",
        //        profit = Convert.ToDecimal(5.00)
        //    }); 
        //    detailOutList.Add(new profit_detail_out()
        //    {
        //        tdate = "20210108",
        //        dseq = "i0450",
        //        dno = "0000003",
        //        mqty = Convert.ToDecimal(5000.0),
        //        cqty = Convert.ToDecimal(1000.0),
        //        mprice = "42.5500",
        //        cost = Convert.ToDecimal(42360.00),
        //        income = Convert.ToDecimal(42361.00),
        //        fee = Convert.ToDecimal(61.00),
        //        tax = Convert.ToDecimal(128.00),
        //        ttype = "0",
        //        ttypename = "現股",
        //        bstype = "S",
        //        wtype = "0",
        //        profit = Convert.ToDecimal(1.00)
        //    });
        //    detailOutList.Add(new profit_detail_out()
        //    {
        //        tdate = "20210108",
        //        dseq = "i0450",
        //        dno = "0000003",
        //        mqty = Convert.ToDecimal(5000.0),
        //        cqty = Convert.ToDecimal(1000.0),
        //        mprice = "42.5500",
        //        cost = Convert.ToDecimal(42411.00),
        //        income = Convert.ToDecimal(42363.00),
        //        fee = Convert.ToDecimal(60.00),
        //        tax = Convert.ToDecimal(127.00),
        //        ttype = "0",
        //        ttypename = "現股",
        //        bstype = "S",
        //        wtype = "0",
        //        profit = Convert.ToDecimal(-48.00)
        //    });

        //    //3筆HCNTD(同TDATE、SDSEQ、SDNO)資料
        //    detailOutList.Add(new profit_detail_out()
        //    {
        //        tdate = "20210104",
        //        dseq = "h0057",
        //        dno = "0000037",
        //        mqty = Convert.ToDecimal(5000.0),
        //        cqty = Convert.ToDecimal(2000.0),
        //        mprice = "28.4000",
        //        cost = Convert.ToDecimal(56981.0),
        //        income = Convert.ToDecimal(56635.0),
        //        netamt = Convert.ToDecimal(141585.0),
        //        fee = Convert.ToDecimal(80.0),
        //        tax = Convert.ToDecimal(85.0),
        //        ttype = "0",
        //        ttypename = "現股",
        //        bstype = "S",
        //        wtype = "0",
        //        profit = Convert.ToDecimal(-346.0),
        //        ctype = "0",
        //        ttypename2 = "賣沖"
        //    });
        //    detailOutList.Add(new profit_detail_out()
        //    {
        //        tdate = "20210104",
        //        dseq = "h0057",
        //        dno = "0000037",
        //        mqty = Convert.ToDecimal(5000.0),
        //        cqty = Convert.ToDecimal(1000.0),
        //        mprice = "28.4000",
        //        cost = Convert.ToDecimal(28340.0),
        //        income = Convert.ToDecimal(28315.0),
        //        fee = Convert.ToDecimal(42.0),
        //        tax = Convert.ToDecimal(43.0),
        //        ttype = "0",
        //        ttypename = "現股",
        //        bstype = "S",
        //        wtype = "0",
        //        profit = Convert.ToDecimal(-25.0),
        //        ctype = "0",
        //        ttypename2 = "賣沖"
        //    });
        //    detailOutList.Add(new profit_detail_out()
        //    {
        //        tdate = "20210104",
        //        dseq = "h0057",
        //        dno = "0000037",
        //        mqty = Convert.ToDecimal(5000.0),
        //        cqty = Convert.ToDecimal(2000.0),
        //        mprice = "28.4000",
        //        cost = Convert.ToDecimal(56580.0),
        //        income = Convert.ToDecimal(56635.0),
        //        fee = Convert.ToDecimal(80.0),
        //        tax = Convert.ToDecimal(85.0),
        //        ttype = "0",
        //        ttypename = "現股",
        //        bstype = "S",
        //        wtype = "0",
        //        profit = Convert.ToDecimal(55.0),
        //        ctype = "0",
        //        ttypename2 = "賣沖"
        //    });
        //    GainPay gainPay = new GainPay();
        //    //ResultList = gainPay.searchDetails(detailOutList);

        //    Assert.AreEqual(ResultList[0].cqty, Convert.ToDecimal(5000.0));
        //    Assert.AreEqual(ResultList[0].cost, Convert.ToDecimal(211851.00));
        //    Assert.AreEqual(ResultList[0].income, Convert.ToDecimal(211809.00));
        //    Assert.AreEqual(ResultList[0].fee, Convert.ToDecimal(303.00));
        //    Assert.AreEqual(ResultList[0].tax, Convert.ToDecimal(638.00));
        //    Assert.AreEqual(ResultList[0].profit, Convert.ToDecimal(-42.00));

        //    Assert.AreEqual(ResultList[1].cqty, Convert.ToDecimal(5000.0));
        //    Assert.AreEqual(ResultList[1].cost, Convert.ToDecimal(141901.00));
        //    Assert.AreEqual(ResultList[1].income, Convert.ToDecimal(141585.00));
        //    Assert.AreEqual(ResultList[1].fee, Convert.ToDecimal(202.00));
        //    Assert.AreEqual(ResultList[1].tax, Convert.ToDecimal(213.00));
        //    Assert.AreEqual(ResultList[1].profit, Convert.ToDecimal(-316.00));

        //}
    }
}