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
        /// <summary>
        /// 現股現賣已實現損益 單元測試
        /// </summary>
        [TestMethod]
        public void HCNRHTest_1()
        {
            List<profit_sum> sumList = new List<profit_sum>();
            List<profit_accsum> accsumList = new List<profit_accsum>();
            List<HCNRH> dbHCNRH = new List<HCNRH>();
            _ = BasicData.MSTMB_Dic;
            #region 3筆HCNRH資料 3筆(同TDATE、SDSEQ、SDNO)
            dbHCNRH.Add(new HCNRH()
            {
                BHNO = "592S",
                TDATE = "20210104",
                RDATE = "20071009",
                CSEQ = "0027432",
                BDSEQ = "2414A",
                BDNO = "000000",
                SDSEQ = "30004",
                SDNO = "0000001",
                STOCK = "2303",
                CQTY = Convert.ToDecimal(218),
                BPRICE = Convert.ToDecimal(0.0000),
                BFEE = Convert.ToDecimal(0.00),
                SPRICE = Convert.ToDecimal(45.6500),
                SFEE = Convert.ToDecimal(14.00),
                TAX = Convert.ToDecimal(29.00),
                INCOME = Convert.ToDecimal(9908.00),
                COST = Convert.ToDecimal(0.00),
                PROFIT = Convert.ToDecimal(9908.00),
                ADJDATE = "",
                WTYPE = "A",
                BQTY = Convert.ToDecimal(2001),
                SQTY = Convert.ToDecimal(1000),
                STINTAX = Convert.ToDecimal(0),
                IOFLAG = "",
                TRDATE = "",
                TRTIME = "",
                MODDATE = "",
                MODTIME = "",
                MODUSER = ""
            });
            dbHCNRH.Add(new HCNRH()
            {
                BHNO = "592S",
                TDATE = "20210104",
                RDATE = "20071009",
                CSEQ = "0027432",
                BDSEQ = "2414A",
                BDNO = "000000",
                SDSEQ = "30004",
                SDNO = "0000001",
                STOCK = "2303",
                CQTY = Convert.ToDecimal(218),
                BPRICE = Convert.ToDecimal(0.0000),
                BFEE = Convert.ToDecimal(0.00),
                SPRICE = Convert.ToDecimal(45.6500),
                SFEE = Convert.ToDecimal(14.00),
                TAX = Convert.ToDecimal(29.00),
                INCOME = Convert.ToDecimal(9908.00),
                COST = Convert.ToDecimal(0.00),
                PROFIT = Convert.ToDecimal(9908.00),
                ADJDATE = "",
                WTYPE = "A",
                BQTY = Convert.ToDecimal(2001),
                SQTY = Convert.ToDecimal(1000),
                STINTAX = Convert.ToDecimal(0),
                IOFLAG = "",
                TRDATE = "",
                TRTIME = "",
                MODDATE = "",
                MODTIME = "",
                MODUSER = ""
            });
            dbHCNRH.Add(new HCNRH()
            {
                BHNO = "592S",
                TDATE = "20210104",
                RDATE = "20071009",
                CSEQ = "0027432",
                BDSEQ = "2414A",
                BDNO = "000000",
                SDSEQ = "30004",
                SDNO = "0000001",
                STOCK = "2303",
                CQTY = Convert.ToDecimal(218),
                BPRICE = Convert.ToDecimal(0.0000),
                BFEE = Convert.ToDecimal(0.00),
                SPRICE = Convert.ToDecimal(45.6500),
                SFEE = Convert.ToDecimal(14.00),
                TAX = Convert.ToDecimal(29.00),
                INCOME = Convert.ToDecimal(9908.00),
                COST = Convert.ToDecimal(0.00),
                PROFIT = Convert.ToDecimal(9908.00),
                ADJDATE = "",
                WTYPE = "A",
                BQTY = Convert.ToDecimal(2001),
                SQTY = Convert.ToDecimal(1000),
                STINTAX = Convert.ToDecimal(0),
                IOFLAG = "",
                TRDATE = "",
                TRTIME = "",
                MODDATE = "",
                MODTIME = "",
                MODUSER = ""
            });
            #endregion
            sumList = SubGainPay.SubSearchSum(dbHCNRH, "5920", "9813603");
            accsumList = SubGainPay.SubSearchAccSum(sumList);
            Assert.AreEqual(accsumList.Count, 1);
            Assert.AreEqual(sumList.Count, 1);
            Assert.AreEqual(sumList[0].profit_detail.Count, 3);
            Assert.AreEqual(sumList[0].ttypename2, "現賣");
            Assert.AreEqual(sumList[0].profit_detail[0].mamt, "0");
            Assert.AreEqual(sumList[0].profit_detail_out.cqty, Convert.ToDecimal(654));
            Assert.AreEqual(sumList[0].profit_detail_out.cost, Convert.ToDecimal(0));
        }

        /// <summary>
        /// 現股當沖已實現損益 單元測試
        /// </summary>
        [TestMethod]
        public void HCNTDTest_2()
        {
            List<profit_sum> sumList = new List<profit_sum>();
            List<profit_accsum> accsumList = new List<profit_accsum>();
            List<HCNTD> dbHCNTD = new List<HCNTD>();
            _ = BasicData.MSTMB_Dic;
            #region 3筆HCNTD資料 3筆(同TDATE、SDSEQ、SDNO)
            dbHCNTD.Add(new HCNTD()
            {
                BHNO = "592S",
                TDATE = "20210104",
                CSEQ = "0006822",
                BDSEQ = "j0558",
                BDNO = "0000003",
                SDSEQ = "j0612",
                SDNO = "0000004",
                STOCK = "2317",
                CQTY = Convert.ToDecimal(5000),
                BPRICE = Convert.ToDecimal(98.3000),
                BFEE = Convert.ToDecimal(700),
                SPRICE = Convert.ToDecimal(99.9000),
                SFEE = Convert.ToDecimal(711),
                TAX = Convert.ToDecimal(749),
                INCOME = Convert.ToDecimal(498040),
                COST = Convert.ToDecimal(492200),
                PROFIT = Convert.ToDecimal(5840),
                BQTY = Convert.ToDecimal(5000),
                SQTY = Convert.ToDecimal(5000),
                TRDATE = "20210104",
                TRTIME = "175407",
                MODDATE = "",
                MODTIME = "",
                MODUSER = ""
            });
            dbHCNTD.Add(new HCNTD()
            {
                BHNO = "592S",
                TDATE = "20210104",
                CSEQ = "0006822",
                BDSEQ = "j0558",
                BDNO = "0000003",
                SDSEQ = "j0612",
                SDNO = "0000004",
                STOCK = "2317",
                CQTY = Convert.ToDecimal(5000),
                BPRICE = Convert.ToDecimal(98.3000),
                BFEE = Convert.ToDecimal(700),
                SPRICE = Convert.ToDecimal(99.9000),
                SFEE = Convert.ToDecimal(711),
                TAX = Convert.ToDecimal(749),
                INCOME = Convert.ToDecimal(498040),
                COST = Convert.ToDecimal(492200),
                PROFIT = Convert.ToDecimal(5840),
                BQTY = Convert.ToDecimal(5000),
                SQTY = Convert.ToDecimal(5000),
                TRDATE = "20210104",
                TRTIME = "175407",
                MODDATE = "",
                MODTIME = "",
                MODUSER = ""
            });
            dbHCNTD.Add(new HCNTD()
            {
                BHNO = "592S",
                TDATE = "20210104",
                CSEQ = "0006822",
                BDSEQ = "j0558",
                BDNO = "0000003",
                SDSEQ = "j0612",
                SDNO = "0000004",
                STOCK = "2317",
                CQTY = Convert.ToDecimal(5000),
                BPRICE = Convert.ToDecimal(98.3000),
                BFEE = Convert.ToDecimal(700),
                SPRICE = Convert.ToDecimal(99.9000),
                SFEE = Convert.ToDecimal(711),
                TAX = Convert.ToDecimal(749),
                INCOME = Convert.ToDecimal(498040),
                COST = Convert.ToDecimal(492200),
                PROFIT = Convert.ToDecimal(5840),
                BQTY = Convert.ToDecimal(5000),
                SQTY = Convert.ToDecimal(5000),
                TRDATE = "20210104",
                TRTIME = "175407",
                MODDATE = "",
                MODTIME = "",
                MODUSER = ""
            });
            #endregion
            sumList = SubGainPay.SubSearchSum(dbHCNTD, "5920", "9813603");
            accsumList = SubGainPay.SubSearchAccSum(sumList);
            Assert.AreEqual(accsumList.Count, 1);
            Assert.AreEqual(sumList.Count, 1);
            Assert.AreEqual(sumList[0].profit_detail.Count, 3);
            Assert.AreEqual(sumList[0].ttypename2, "現沖");
            Assert.AreEqual(sumList[0].profit_detail[0].mamt, "491500.0");
            Assert.AreEqual(sumList[0].profit_detail_out.cqty, Convert.ToDecimal(15000));
            Assert.AreEqual(sumList[0].profit_detail_out.cost, Convert.ToDecimal(1476600));
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
    }
}