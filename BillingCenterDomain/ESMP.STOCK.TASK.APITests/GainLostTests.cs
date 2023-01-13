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
    public class GainLostTests
    {
        /// <summary>
        /// 預估賣出價金計算結果 單元測試
        /// </summary>
        [TestMethod]
        public void estimateAmtTest_1()
        {
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();
            unoffset_qtype_accsum accsum = new unoffset_qtype_accsum();
            List<TCNUD> dbTCNUD = new List<TCNUD>();
            Dictionary<string, List<Symbol>> Quote_Dic = new Dictionary<string, List<Symbol>>();
            List<Symbol> symbolList = new List<Symbol>();
            #region Quote
            symbolList.Add(new Symbol()
            {
                id = "2108",
                dealprice = Convert.ToDecimal(120.5),
                shortname = "南帝",
                refprice = Convert.ToDecimal(124),
                moddate = "20210809",
                modtime = "133031"
            });
            symbolList.Add(new Symbol()
            {
                id = "3515",
                dealprice = Convert.ToDecimal(160),
                shortname = "華擎",
                refprice = Convert.ToDecimal(162.5),
                moddate = "20210809",
                modtime = "133038"
            });
            symbolList.Add(new Symbol()
            {
                id = "1303",
                dealprice = Convert.ToDecimal(88.3),
                shortname = "南亞",
                refprice = Convert.ToDecimal(88.4),
                moddate = "20210809",
                modtime = "133020"
            });
            Quote_Dic = symbolList.GroupBy(d => d.id).ToDictionary(x => x.Key, x => x.ToList());
            #endregion
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCSRH_Dic;
            _ = BasicData.MsysDict;
            #region 3筆TCNUD資料
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20211203",
                BHNO = "592S",
                CSEQ = "0074647",
                STOCK = "2108",
                PRICE = Convert.ToDecimal(84.5000),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(120.00),
                COST = Convert.ToDecimal(84620.00),
                DSEQ = "r0044",
                DNO = "0000001",
                ADJDATE = "",
                WTYPE = "0",
                TRDATE = "20211203",
                TRTIME = "190048",
                MODDATE = "20211203",
                MODTIME = "190048",
                MODUSER = "DailyJob",
                IOFLAG = ""
            });
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20220913",
                BHNO = "592S",
                CSEQ = "0074647",
                STOCK = "3515",
                PRICE = Convert.ToDecimal(100.0000),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(142.00),
                COST = Convert.ToDecimal(100142.00),
                DSEQ = "o0008",
                DNO = "0000002",
                ADJDATE = "",
                WTYPE = "0",
                TRDATE = "20220913",
                TRTIME = "181250",
                MODDATE = "20220913",
                MODTIME = "181250",
                MODUSER = "DailyJob",
                IOFLAG = ""
            });
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20010101",
                BHNO = "592S",
                CSEQ = "0034661",
                STOCK = "1303",
                PRICE = Convert.ToDecimal(37.4800),
                QTY = Convert.ToDecimal(1601),
                BQTY = Convert.ToDecimal(601),
                FEE = Convert.ToDecimal(0.00),
                COST = Convert.ToDecimal(22525.00),
                DSEQ = "1643B",
                DNO = "506434",
                ADJDATE = "20171213",
                WTYPE = "A",
                TRDATE = "20111212",
                TRTIME = "090101",
                MODDATE = "20191217",
                MODTIME = "180837",
                MODUSER = "DBTrans",
                IOFLAG = ""
            });
            #endregion
            sumList = SubGainLost.SubSearchSum(dbTCNUD, Quote_Dic);
            accsum = SubGainLost.SubSearchAccSum(sumList);
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].estimateAmt, Convert.ToDecimal(120500));
            Assert.AreEqual(accsum.unoffset_qtype_sum[1].estimateAmt, Convert.ToDecimal(160000));
            Assert.AreEqual(accsum.unoffset_qtype_sum[2].estimateAmt, Convert.ToDecimal(53068));
            Assert.AreEqual(accsum.estimateAmt, Convert.ToDecimal(333568));
        }

        /// <summary>
        /// 預估賣出手續費計算結果 單元測試
        /// </summary>
        [TestMethod]
        public void estimateFeeTest_2()
        {
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();
            unoffset_qtype_accsum accsum = new unoffset_qtype_accsum();
            List<TCNUD> dbTCNUD = new List<TCNUD>();
            Dictionary<string, List<Symbol>> Quote_Dic = new Dictionary<string, List<Symbol>>();
            List<Symbol> symbolList = new List<Symbol>();
            #region Quote
            symbolList.Add(new Symbol()
            {
                id = "2108",
                dealprice = Convert.ToDecimal(120.5),
                shortname = "南帝",
                refprice = Convert.ToDecimal(124),
                moddate = "20210809",
                modtime = "133031"
            });
            symbolList.Add(new Symbol()
            {
                id = "3515",
                dealprice = Convert.ToDecimal(160),
                shortname = "華擎",
                refprice = Convert.ToDecimal(162.5),
                moddate = "20210809",
                modtime = "133038"
            });
            symbolList.Add(new Symbol()
            {
                id = "1303",
                dealprice = Convert.ToDecimal(88.3),
                shortname = "南亞",
                refprice = Convert.ToDecimal(88.4),
                moddate = "20210809",
                modtime = "133020"
            });
            Quote_Dic = symbolList.GroupBy(d => d.id).ToDictionary(x => x.Key, x => x.ToList());
            #endregion
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCSRH_Dic;
            _ = BasicData.MsysDict;
            #region 3筆TCNUD資料
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20211203",
                BHNO = "592S",
                CSEQ = "0074647",
                STOCK = "2108",
                PRICE = Convert.ToDecimal(84.5000),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(120.00),
                COST = Convert.ToDecimal(84620.00),
                DSEQ = "r0044",
                DNO = "0000001",
                ADJDATE = "",
                WTYPE = "0",
                TRDATE = "20211203",
                TRTIME = "190048",
                MODDATE = "20211203",
                MODTIME = "190048",
                MODUSER = "DailyJob",
                IOFLAG = ""
            });
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20220913",
                BHNO = "592S",
                CSEQ = "0074647",
                STOCK = "3515",
                PRICE = Convert.ToDecimal(100.0000),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(142.00),
                COST = Convert.ToDecimal(100142.00),
                DSEQ = "o0008",
                DNO = "0000002",
                ADJDATE = "",
                WTYPE = "0",
                TRDATE = "20220913",
                TRTIME = "181250",
                MODDATE = "20220913",
                MODTIME = "181250",
                MODUSER = "DailyJob",
                IOFLAG = ""
            });
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20010101",
                BHNO = "592S",
                CSEQ = "0034661",
                STOCK = "1303",
                PRICE = Convert.ToDecimal(37.4800),
                QTY = Convert.ToDecimal(1601),
                BQTY = Convert.ToDecimal(601),
                FEE = Convert.ToDecimal(0.00),
                COST = Convert.ToDecimal(22525.00),
                DSEQ = "1643B",
                DNO = "506434",
                ADJDATE = "20171213",
                WTYPE = "A",
                TRDATE = "20111212",
                TRTIME = "090101",
                MODDATE = "20191217",
                MODTIME = "180837",
                MODUSER = "DBTrans",
                IOFLAG = ""
            });
            #endregion
            sumList = SubGainLost.SubSearchSum(dbTCNUD, Quote_Dic);
            accsum = SubGainLost.SubSearchAccSum(sumList);
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].estimateFee, Convert.ToDecimal(171));
            Assert.AreEqual(accsum.unoffset_qtype_sum[1].estimateFee, Convert.ToDecimal(228));
            Assert.AreEqual(accsum.unoffset_qtype_sum[2].estimateFee, Convert.ToDecimal(75));
            Assert.AreEqual(accsum.estimateFee, Convert.ToDecimal(474));
        }

        /// <summary>
        /// 預估賣出交易稅計算結果 單元測試
        /// </summary>
        [TestMethod]
        public void estimateTaxTest_3()
        {
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();
            unoffset_qtype_accsum accsum = new unoffset_qtype_accsum();
            List<TCNUD> dbTCNUD = new List<TCNUD>();
            Dictionary<string, List<Symbol>> Quote_Dic = new Dictionary<string, List<Symbol>>();
            List<Symbol> symbolList = new List<Symbol>();
            #region Quote
            symbolList.Add(new Symbol()
            {
                id = "2108",
                dealprice = Convert.ToDecimal(120.5),
                shortname = "南帝",
                refprice = Convert.ToDecimal(124),
                moddate = "20210809",
                modtime = "133031"
            });
            symbolList.Add(new Symbol()
            {
                id = "3515",
                dealprice = Convert.ToDecimal(160),
                shortname = "華擎",
                refprice = Convert.ToDecimal(162.5),
                moddate = "20210809",
                modtime = "133038"
            });
            symbolList.Add(new Symbol()
            {
                id = "1303",
                dealprice = Convert.ToDecimal(88.3),
                shortname = "南亞",
                refprice = Convert.ToDecimal(88.4),
                moddate = "20210809",
                modtime = "133020"
            });
            Quote_Dic = symbolList.GroupBy(d => d.id).ToDictionary(x => x.Key, x => x.ToList());
            #endregion
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCSRH_Dic;
            _ = BasicData.MsysDict;
            #region 3筆TCNUD資料
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20211203",
                BHNO = "592S",
                CSEQ = "0074647",
                STOCK = "2108",
                PRICE = Convert.ToDecimal(84.5000),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(120.00),
                COST = Convert.ToDecimal(84620.00),
                DSEQ = "r0044",
                DNO = "0000001",
                ADJDATE = "",
                WTYPE = "0",
                TRDATE = "20211203",
                TRTIME = "190048",
                MODDATE = "20211203",
                MODTIME = "190048",
                MODUSER = "DailyJob",
                IOFLAG = ""
            });
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20220913",
                BHNO = "592S",
                CSEQ = "0074647",
                STOCK = "3515",
                PRICE = Convert.ToDecimal(100.0000),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(142.00),
                COST = Convert.ToDecimal(100142.00),
                DSEQ = "o0008",
                DNO = "0000002",
                ADJDATE = "",
                WTYPE = "0",
                TRDATE = "20220913",
                TRTIME = "181250",
                MODDATE = "20220913",
                MODTIME = "181250",
                MODUSER = "DailyJob",
                IOFLAG = ""
            });
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20010101",
                BHNO = "592S",
                CSEQ = "0034661",
                STOCK = "1303",
                PRICE = Convert.ToDecimal(37.4800),
                QTY = Convert.ToDecimal(1601),
                BQTY = Convert.ToDecimal(601),
                FEE = Convert.ToDecimal(0.00),
                COST = Convert.ToDecimal(22525.00),
                DSEQ = "1643B",
                DNO = "506434",
                ADJDATE = "20171213",
                WTYPE = "A",
                TRDATE = "20111212",
                TRTIME = "090101",
                MODDATE = "20191217",
                MODTIME = "180837",
                MODUSER = "DBTrans",
                IOFLAG = ""
            });
            #endregion
            sumList = SubGainLost.SubSearchSum(dbTCNUD, Quote_Dic);
            accsum = SubGainLost.SubSearchAccSum(sumList);
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].estimateTax, Convert.ToDecimal(361));
            Assert.AreEqual(accsum.unoffset_qtype_sum[1].estimateTax, Convert.ToDecimal(480));
            Assert.AreEqual(accsum.unoffset_qtype_sum[2].estimateTax, Convert.ToDecimal(159));
            Assert.AreEqual(accsum.estimateTax, Convert.ToDecimal(1000));
        }

        /// <summary>
        /// 預估市值計算結果 單元測試
        /// </summary>
        [TestMethod]
        public void marketvalueTest_4()
        {
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();
            unoffset_qtype_accsum accsum = new unoffset_qtype_accsum();
            List<TCNUD> dbTCNUD = new List<TCNUD>();
            Dictionary<string, List<Symbol>> Quote_Dic = new Dictionary<string, List<Symbol>>();
            List<Symbol> symbolList = new List<Symbol>();
            #region Quote
            symbolList.Add(new Symbol()
            {
                id = "2108",
                dealprice = Convert.ToDecimal(120.5),
                shortname = "南帝",
                refprice = Convert.ToDecimal(124),
                moddate = "20210809",
                modtime = "133031"
            });
            symbolList.Add(new Symbol()
            {
                id = "3515",
                dealprice = Convert.ToDecimal(160),
                shortname = "華擎",
                refprice = Convert.ToDecimal(162.5),
                moddate = "20210809",
                modtime = "133038"
            });
            symbolList.Add(new Symbol()
            {
                id = "1303",
                dealprice = Convert.ToDecimal(88.3),
                shortname = "南亞",
                refprice = Convert.ToDecimal(88.4),
                moddate = "20210809",
                modtime = "133020"
            });
            Quote_Dic = symbolList.GroupBy(d => d.id).ToDictionary(x => x.Key, x => x.ToList());
            #endregion
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCSRH_Dic;
            _ = BasicData.MsysDict;
            #region 3筆TCNUD資料
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20211203",
                BHNO = "592S",
                CSEQ = "0074647",
                STOCK = "2108",
                PRICE = Convert.ToDecimal(84.5000),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(120.00),
                COST = Convert.ToDecimal(84620.00),
                DSEQ = "r0044",
                DNO = "0000001",
                ADJDATE = "",
                WTYPE = "0",
                TRDATE = "20211203",
                TRTIME = "190048",
                MODDATE = "20211203",
                MODTIME = "190048",
                MODUSER = "DailyJob",
                IOFLAG = ""
            });
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20220913",
                BHNO = "592S",
                CSEQ = "0074647",
                STOCK = "3515",
                PRICE = Convert.ToDecimal(100.0000),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(142.00),
                COST = Convert.ToDecimal(100142.00),
                DSEQ = "o0008",
                DNO = "0000002",
                ADJDATE = "",
                WTYPE = "0",
                TRDATE = "20220913",
                TRTIME = "181250",
                MODDATE = "20220913",
                MODTIME = "181250",
                MODUSER = "DailyJob",
                IOFLAG = ""
            });
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20010101",
                BHNO = "592S",
                CSEQ = "0034661",
                STOCK = "1303",
                PRICE = Convert.ToDecimal(37.4800),
                QTY = Convert.ToDecimal(1601),
                BQTY = Convert.ToDecimal(601),
                FEE = Convert.ToDecimal(0.00),
                COST = Convert.ToDecimal(22525.00),
                DSEQ = "1643B",
                DNO = "506434",
                ADJDATE = "20171213",
                WTYPE = "A",
                TRDATE = "20111212",
                TRTIME = "090101",
                MODDATE = "20191217",
                MODTIME = "180837",
                MODUSER = "DBTrans",
                IOFLAG = ""
            });
            #endregion
            sumList = SubGainLost.SubSearchSum(dbTCNUD, Quote_Dic);
            accsum = SubGainLost.SubSearchAccSum(sumList);
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].marketvalue, Convert.ToDecimal(119968));
            Assert.AreEqual(accsum.unoffset_qtype_sum[1].marketvalue, Convert.ToDecimal(159292));
            Assert.AreEqual(accsum.unoffset_qtype_sum[2].marketvalue, Convert.ToDecimal(52834));
            Assert.AreEqual(accsum.marketvalue, Convert.ToDecimal(332094));
        }

        /// <summary>
        /// 預估損益計算結果 單元測試
        /// </summary>
        [TestMethod]
        public void profitTest_5()
        {
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();
            unoffset_qtype_accsum accsum = new unoffset_qtype_accsum();
            List<TCNUD> dbTCNUD = new List<TCNUD>();
            Dictionary<string, List<Symbol>> Quote_Dic = new Dictionary<string, List<Symbol>>();
            List<Symbol> symbolList = new List<Symbol>();
            #region Quote
            symbolList.Add(new Symbol()
            {
                id = "2108",
                dealprice = Convert.ToDecimal(120.5),
                shortname = "南帝",
                refprice = Convert.ToDecimal(124),
                moddate = "20210809",
                modtime = "133031"
            });
            symbolList.Add(new Symbol()
            {
                id = "3515",
                dealprice = Convert.ToDecimal(160),
                shortname = "華擎",
                refprice = Convert.ToDecimal(162.5),
                moddate = "20210809",
                modtime = "133038"
            });
            symbolList.Add(new Symbol()
            {
                id = "1303",
                dealprice = Convert.ToDecimal(88.3),
                shortname = "南亞",
                refprice = Convert.ToDecimal(88.4),
                moddate = "20210809",
                modtime = "133020"
            });
            Quote_Dic = symbolList.GroupBy(d => d.id).ToDictionary(x => x.Key, x => x.ToList());
            #endregion
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCSRH_Dic;
            _ = BasicData.MsysDict;
            #region 3筆TCNUD資料
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20211203",
                BHNO = "592S",
                CSEQ = "0074647",
                STOCK = "2108",
                PRICE = Convert.ToDecimal(84.5000),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(120.00),
                COST = Convert.ToDecimal(84620.00),
                DSEQ = "r0044",
                DNO = "0000001",
                ADJDATE = "",
                WTYPE = "0",
                TRDATE = "20211203",
                TRTIME = "190048",
                MODDATE = "20211203",
                MODTIME = "190048",
                MODUSER = "DailyJob",
                IOFLAG = ""
            });
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20220913",
                BHNO = "592S",
                CSEQ = "0074647",
                STOCK = "3515",
                PRICE = Convert.ToDecimal(100.0000),
                QTY = Convert.ToDecimal(1000),
                BQTY = Convert.ToDecimal(1000),
                FEE = Convert.ToDecimal(142.00),
                COST = Convert.ToDecimal(100142.00),
                DSEQ = "o0008",
                DNO = "0000002",
                ADJDATE = "",
                WTYPE = "0",
                TRDATE = "20220913",
                TRTIME = "181250",
                MODDATE = "20220913",
                MODTIME = "181250",
                MODUSER = "DailyJob",
                IOFLAG = ""
            });
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20010101",
                BHNO = "592S",
                CSEQ = "0034661",
                STOCK = "1303",
                PRICE = Convert.ToDecimal(37.4800),
                QTY = Convert.ToDecimal(1601),
                BQTY = Convert.ToDecimal(601),
                FEE = Convert.ToDecimal(0.00),
                COST = Convert.ToDecimal(22525.00),
                DSEQ = "1643B",
                DNO = "506434",
                ADJDATE = "20171213",
                WTYPE = "A",
                TRDATE = "20111212",
                TRTIME = "090101",
                MODDATE = "20191217",
                MODTIME = "180837",
                MODUSER = "DBTrans",
                IOFLAG = ""
            });
            #endregion
            sumList = SubGainLost.SubSearchSum(dbTCNUD, Quote_Dic);
            accsum = SubGainLost.SubSearchAccSum(sumList);
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].profit, Convert.ToDecimal(35348));
            Assert.AreEqual(accsum.unoffset_qtype_sum[1].profit, Convert.ToDecimal(59150));
            Assert.AreEqual(accsum.unoffset_qtype_sum[2].profit, Convert.ToDecimal(30309));
            Assert.AreEqual(accsum.profit, Convert.ToDecimal(124807));
        }

        /// <summary>
        /// 當日現股買進－價金計算
        /// </summary>
        [TestMethod]
        public void TMHIOTest_6()
        {
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();
            unoffset_qtype_accsum accsum = new unoffset_qtype_accsum();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            Dictionary<string, List<Symbol>> Quote_Dic = new Dictionary<string, List<Symbol>>();
            List<Symbol> symbolList = new List<Symbol>();
            #region Quote
            symbolList.Add(new Symbol()
            {
                id = "2344",
                dealprice = Convert.ToDecimal(33.95),
                shortname = "華邦電",
                refprice = Convert.ToDecimal(34.1),
                moddate = "20210809",
                modtime = "133002"
            });
            symbolList.Add(new Symbol()
            {
                id = "2603",
                dealprice = Convert.ToDecimal(142.5),
                shortname = "長榮",
                refprice = Convert.ToDecimal(142),
                moddate = "20210809",
                modtime = "133001"
            });
            Quote_Dic = symbolList.GroupBy(d => d.id).ToDictionary(x => x.Key, x => x.ToList());
            #endregion
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCSRH_Dic;
            _ = BasicData.MsysDict;
            #region 2筆TMHIO資料
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "592a",
                DSEQ = "30001",
                JRNUM = "00007362",
                MTYPE = "T",
                CSEQ = "0241095",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "B",
                STOCK = "2344",
                QTY = Convert.ToDecimal(2000),
                PRICE = Convert.ToDecimal(19.1000),
                SALES = "0066",
                ORGIN = "",
                MTIME = "090002262",
                TRDATE = "20221017",
                TRTIME = "090004",
                MODDATE = "20221017",
                MODTIME = "090004",
                MODUSER = "REPLY"
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "592a",
                DSEQ = "30014",
                JRNUM = "00520106",
                MTYPE = "T",
                CSEQ = "0241095",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "B",
                STOCK = "2603",
                QTY = Convert.ToDecimal(1000),
                PRICE = Convert.ToDecimal(138.0000),
                SALES = "0085",
                ORGIN = "",
                MTIME = "093231889",
                TRDATE = "20221017",
                TRTIME = "093232",
                MODDATE = "20221017",
                MODTIME = "093232",
                MODUSER = "REPLY"
            });
            #endregion
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            TCNUDList = SubESMPData.SubAddTMHIOBuy(TCNUDList, HCMIOList);
            sumList = SubGainLost.SubSearchSum(TCNUDList, Quote_Dic);
            accsum = SubGainLost.SubSearchAccSum(sumList);
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].estimateAmt, Convert.ToDecimal(67900));
            Assert.AreEqual(accsum.unoffset_qtype_sum[1].estimateAmt, Convert.ToDecimal(142500));
            Assert.AreEqual(accsum.estimateAmt, Convert.ToDecimal(210400));
        }

        /// <summary>
        /// 當日現股買進－手續費計算 單元測試
        /// </summary>
        [TestMethod]
        public void TMHIOTest_7()
        {
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();
            unoffset_qtype_accsum accsum = new unoffset_qtype_accsum();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            Dictionary<string, List<Symbol>> Quote_Dic = new Dictionary<string, List<Symbol>>();
            List<Symbol> symbolList = new List<Symbol>();
            #region Quote
            symbolList.Add(new Symbol()
            {
                id = "2344",
                dealprice = Convert.ToDecimal(33.95),
                shortname = "華邦電",
                refprice = Convert.ToDecimal(34.1),
                moddate = "20210809",
                modtime = "133002"
            });
            symbolList.Add(new Symbol()
            {
                id = "2603",
                dealprice = Convert.ToDecimal(142.5),
                shortname = "長榮",
                refprice = Convert.ToDecimal(142),
                moddate = "20210809",
                modtime = "133001"
            });
            Quote_Dic = symbolList.GroupBy(d => d.id).ToDictionary(x => x.Key, x => x.ToList());
            #endregion
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCSRH_Dic;
            _ = BasicData.MsysDict;
            #region 2筆TMHIO資料
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "592a",
                DSEQ = "30001",
                JRNUM = "00007362",
                MTYPE = "T",
                CSEQ = "0241095",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "B",
                STOCK = "2344",
                QTY = Convert.ToDecimal(2000),
                PRICE = Convert.ToDecimal(19.1000),
                SALES = "0066",
                ORGIN = "",
                MTIME = "090002262",
                TRDATE = "20221017",
                TRTIME = "090004",
                MODDATE = "20221017",
                MODTIME = "090004",
                MODUSER = "REPLY"
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "592a",
                DSEQ = "30014",
                JRNUM = "00520106",
                MTYPE = "T",
                CSEQ = "0241095",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "B",
                STOCK = "2603",
                QTY = Convert.ToDecimal(1000),
                PRICE = Convert.ToDecimal(138.0000),
                SALES = "0085",
                ORGIN = "",
                MTIME = "093231889",
                TRDATE = "20221017",
                TRTIME = "093232",
                MODDATE = "20221017",
                MODTIME = "093232",
                MODUSER = "REPLY"
            });
            #endregion
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            TCNUDList = SubESMPData.SubAddTMHIOBuy(TCNUDList, HCMIOList);
            sumList = SubGainLost.SubSearchSum(TCNUDList, Quote_Dic);
            accsum = SubGainLost.SubSearchAccSum(sumList);
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].estimateFee, Convert.ToDecimal(96));
            Assert.AreEqual(accsum.unoffset_qtype_sum[1].estimateFee, Convert.ToDecimal(203));
            Assert.AreEqual(accsum.estimateFee, Convert.ToDecimal(299));
        }

        /// <summary>
        /// 當日現股買進－成本計算 單元測試
        /// </summary>
        [TestMethod]
        public void TMHIOTest_8()
        {
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();
            unoffset_qtype_accsum accsum = new unoffset_qtype_accsum();
            List<TMHIO> TMHIOList = new List<TMHIO>();
            List<HCMIO> HCMIOList = new List<HCMIO>();
            List<TCNUD> TCNUDList = new List<TCNUD>();
            List<TCSIO> TCSIOList = new List<TCSIO>();
            Dictionary<string, List<Symbol>> Quote_Dic = new Dictionary<string, List<Symbol>>();
            List<Symbol> symbolList = new List<Symbol>();
            #region Quote
            symbolList.Add(new Symbol()
            {
                id = "2344",
                dealprice = Convert.ToDecimal(33.95),
                shortname = "華邦電",
                refprice = Convert.ToDecimal(34.1),
                moddate = "20210809",
                modtime = "133002"
            });
            symbolList.Add(new Symbol()
            {
                id = "2603",
                dealprice = Convert.ToDecimal(142.5),
                shortname = "長榮",
                refprice = Convert.ToDecimal(142),
                moddate = "20210809",
                modtime = "133001"
            });
            Quote_Dic = symbolList.GroupBy(d => d.id).ToDictionary(x => x.Key, x => x.ToList());
            #endregion
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCSRH_Dic;
            _ = BasicData.MsysDict;
            #region 2筆TMHIO資料
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "592a",
                DSEQ = "30001",
                JRNUM = "00007362",
                MTYPE = "T",
                CSEQ = "0241095",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "B",
                STOCK = "2344",
                QTY = Convert.ToDecimal(2000),
                PRICE = Convert.ToDecimal(19.1000),
                SALES = "0066",
                ORGIN = "",
                MTIME = "090002262",
                TRDATE = "20221017",
                TRTIME = "090004",
                MODDATE = "20221017",
                MODTIME = "090004",
                MODUSER = "REPLY"
            });
            TMHIOList.Add(new TMHIO()
            {
                TDATE = "20221017",
                BHNO = "592a",
                DSEQ = "30014",
                JRNUM = "00520106",
                MTYPE = "T",
                CSEQ = "0241095",
                TTYPE = "0",
                ETYPE = "0",
                BSTYPE = "B",
                STOCK = "2603",
                QTY = Convert.ToDecimal(1000),
                PRICE = Convert.ToDecimal(138.0000),
                SALES = "0085",
                ORGIN = "",
                MTIME = "093231889",
                TRDATE = "20221017",
                TRTIME = "093232",
                MODDATE = "20221017",
                MODTIME = "093232",
                MODUSER = "REPLY"
            });
            #endregion
            HCMIOList = SubESMPData.SubGetHCMIO(TCSIOList, TMHIOList);
            TCNUDList = SubESMPData.SubAddTMHIOBuy(TCNUDList, HCMIOList);
            sumList = SubGainLost.SubSearchSum(TCNUDList, Quote_Dic);
            accsum = SubGainLost.SubSearchAccSum(sumList);
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].cost, Convert.ToDecimal(38254));
            Assert.AreEqual(accsum.unoffset_qtype_sum[1].cost, Convert.ToDecimal(138196));
            Assert.AreEqual(accsum.cost, Convert.ToDecimal(176450));
        }

        /// <summary>
        /// (Ram)TCNUD先賣部位 單元測試
        /// </summary>
        [TestMethod]
        public void TCNUDTest_9()
        {
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();
            unoffset_qtype_accsum accsum = new unoffset_qtype_accsum();
            List<TCNUD> dbTCNUD = new List<TCNUD>();
            Dictionary<string, List<Symbol>> Quote_Dic = new Dictionary<string, List<Symbol>>();
            List<Symbol> symbolList = new List<Symbol>();
            symbolList.Add(new Symbol()
            {
                id = "3622",
                dealprice = Convert.ToDecimal(29.3),
                shortname = "洋華",
                refprice = Convert.ToDecimal(30.2),
                moddate = "20210809",
                modtime = "133036"
            });
            Quote_Dic.Add("3622", symbolList);
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCSRH_Dic;
            #region 1筆TCNUD資料
            dbTCNUD.Add(new TCNUD()
            {
                TDATE = "20221017",
                BHNO = "5920",
                CSEQ = "0002141",
                STOCK = "2330",
                PRICE = Convert.ToDecimal(399),
                QTY = Convert.ToDecimal(5000),
                BQTY = Convert.ToDecimal(-3000),
                FEE = Convert.ToDecimal(1705),
                COST = Convert.ToDecimal(0),
                DSEQ = "T0726",
                DNO = "00600970",
                ADJDATE = "",
                WTYPE = "",
                TRDATE = "",
                TRTIME = "",
                MODDATE = "",
                MODTIME = "",
                MODUSER = "",
                IOFLAG = ""
            });
            #endregion
            sumList = SubGainLost.SubSearchSum(dbTCNUD, Quote_Dic);
            accsum = SubGainLost.SubSearchAccSum(sumList);
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].tdate, "20221017");
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].ttypename, "現賣");
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].bstype, "S");
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].bqty, Convert.ToDecimal(-3000));
        }

        [TestMethod]
        public void TCRUDTest_10()
        {
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();
            unoffset_qtype_accsum accsum = new unoffset_qtype_accsum();
            List<TCRUD> dbTCRUD = new List<TCRUD>();
            Dictionary<string, List<Symbol>> Quote_Dic = new Dictionary<string, List<Symbol>>();
            List<Symbol> symbolList = new List<Symbol>();
            symbolList.Add(new Symbol()
            {
                id = "3622",
                dealprice = Convert.ToDecimal(29.3),
                shortname = "洋華",
                refprice = Convert.ToDecimal(30.2),
                moddate = "20210809",
                modtime = "133036"
            });
            Quote_Dic.Add("3622", symbolList);
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCSRH_Dic;
            #region 3筆TCRUD資料
            dbTCRUD.Add(new TCRUD()
            {
                TDATE = "20210331",
                BHNO = "592S",
                DSEQ = "j0553",
                PRICE = Convert.ToDecimal(36.6500),
                DNO = "0000001",
                CSEQ = "0116660",
                STOCK = "3622",
                QTY = Convert.ToDecimal(7000),
                CRAMT = Convert.ToDecimal(153000),
                PAMT = Convert.ToDecimal(103550),
                BQTY = Convert.ToDecimal(2000),
                BCRAMT = Convert.ToDecimal(43000),
                CRINT = Convert.ToDecimal(4050),
                SFCODE = "5920",
                CRRATIO = Convert.ToDecimal(0),
                ASFAMT = Convert.ToDecimal(0),
                FEE = Convert.ToDecimal(365),
                COST = Convert.ToDecimal(0),
                TRDATE = "20220916",
                TRTIME = "174541",
                MODDATE = "",
                MODTIME = "",
                MODUSER = ""
            });
            dbTCRUD.Add(new TCRUD()
            {
                TDATE = "20210331",
                BHNO = "592S",
                DSEQ = "j0553",
                PRICE = Convert.ToDecimal(36.6500),
                DNO = "0000001",
                CSEQ = "0116660",
                STOCK = "3622",
                QTY = Convert.ToDecimal(7000),
                CRAMT = Convert.ToDecimal(153000),
                PAMT = Convert.ToDecimal(103550),
                BQTY = Convert.ToDecimal(2000),
                BCRAMT = Convert.ToDecimal(43000),
                CRINT = Convert.ToDecimal(4050),
                SFCODE = "5920",
                CRRATIO = Convert.ToDecimal(0),
                ASFAMT = Convert.ToDecimal(0),
                FEE = Convert.ToDecimal(365),
                COST = Convert.ToDecimal(0),
                TRDATE = "20220916",
                TRTIME = "174541",
                MODDATE = "",
                MODTIME = "",
                MODUSER = ""
            });
            dbTCRUD.Add(new TCRUD()
            {
                TDATE = "20210331",
                BHNO = "592S",
                DSEQ = "j0553",
                PRICE = Convert.ToDecimal(36.6500),
                DNO = "0000001",
                CSEQ = "0116660",
                STOCK = "3622",
                QTY = Convert.ToDecimal(7000),
                CRAMT = Convert.ToDecimal(153000),
                PAMT = Convert.ToDecimal(103550),
                BQTY = Convert.ToDecimal(2000),
                BCRAMT = Convert.ToDecimal(43000),
                CRINT = Convert.ToDecimal(4050),
                SFCODE = "5920",
                CRRATIO = Convert.ToDecimal(0),
                ASFAMT = Convert.ToDecimal(0),
                FEE = Convert.ToDecimal(365),
                COST = Convert.ToDecimal(0),
                TRDATE = "20220916",
                TRTIME = "174541",
                MODDATE = "",
                MODTIME = "",
                MODUSER = ""
            });
            #endregion
            sumList = SubGainLost.SubSearchSum(dbTCRUD, Quote_Dic);
            accsum = SubGainLost.SubSearchAccSum(sumList);
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].tdate, "20210331");
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].ttypename, "融資");
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].bstype, "B");
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].mamt, Convert.ToDecimal(73300));
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].estimateAmt, Convert.ToDecimal(58600));
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].interest, Convert.ToDecimal(4050));

            Assert.AreEqual(accsum.unoffset_qtype_sum[0].stock, "3622");
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].ttypename, "融資");
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].bstype, "B");
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].estimateAmt, Convert.ToDecimal(175800));
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].interest, Convert.ToDecimal(12150));

            Assert.AreEqual(accsum.unoffset_qtype_sum[0].estimateAmt, Convert.ToDecimal(175800));
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].cramt, Convert.ToDecimal(459000));
        }

        /// <summary>
        /// 融券未實現損益 單元測試
        /// </summary>
        [TestMethod]
        public void TDBUDTest_11()
        {
            List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();
            unoffset_qtype_accsum accsum = new unoffset_qtype_accsum();
            List<TDBUD> dbTDBUD = new List<TDBUD>();
            Dictionary<string, List<Symbol>> Quote_Dic = new Dictionary<string, List<Symbol>>();
            List<Symbol> symbolList = new List<Symbol>();
            symbolList.Add(new Symbol()
            {
                id = "8069",
                dealprice = Convert.ToDecimal(80.9),
                shortname = "元太",
                refprice = Convert.ToDecimal(87.3),
                moddate = "20210809",
                modtime = "133024"
            });
            Quote_Dic.Add("8069", symbolList);
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCSRH_Dic;
            #region 3筆TDBUD資料
            dbTDBUD.Add(new TDBUD()
            {
                TDATE = "20220804",
                BHNO = "592S",
                DSEQ = "i0328",
                DNO = "0000004",
                CSEQ = "0082985",
                STOCK = "8069",
                PRICE = Convert.ToDecimal(184.5000),
                QTY = Convert.ToDecimal(1000),
                DBAMT = Convert.ToDecimal(166100),
                GTAMT = Convert.ToDecimal(166100),
                DNAMT = Convert.ToDecimal(183538),
                BQTY = Convert.ToDecimal(1000),
                BDBAMT = Convert.ToDecimal(349638),
                BGTAMT = Convert.ToDecimal(166100),
                BDNAMT = Convert.ToDecimal(183538),
                GTINT = Convert.ToDecimal(40),
                DNINT = Convert.ToDecimal(44),
                DLFEE = Convert.ToDecimal(0),
                DLINT = Convert.ToDecimal(0),
                DBRATIO = Convert.ToDecimal(0),
                SFCODE = "5920",
                AGTAMT = Convert.ToDecimal(0),
                FEE = Convert.ToDecimal(0),
                TAX = Convert.ToDecimal(0),
                DBFEE = Convert.ToDecimal(0),
                COST = Convert.ToDecimal(0),
                STINTAX = Convert.ToDecimal(0),
                HEALTHFEE = Convert.ToDecimal(0),
                TRDATE = "20220916",
                TRTIME = "174542",
                MODDATE = "",
                MODTIME = "",
                MODUSER = ""
            });
            dbTDBUD.Add(new TDBUD()
            {
                TDATE = "20220804",
                BHNO = "592S",
                DSEQ = "i0328",
                DNO = "0000004",
                CSEQ = "0082985",
                STOCK = "8069",
                PRICE = Convert.ToDecimal(184.5000),
                QTY = Convert.ToDecimal(1000),
                DBAMT = Convert.ToDecimal(166100),
                GTAMT = Convert.ToDecimal(166100),
                DNAMT = Convert.ToDecimal(183538),
                BQTY = Convert.ToDecimal(1000),
                BDBAMT = Convert.ToDecimal(349638),
                BGTAMT = Convert.ToDecimal(166100),
                BDNAMT = Convert.ToDecimal(183538),
                GTINT = Convert.ToDecimal(40),
                DNINT = Convert.ToDecimal(44),
                DLFEE = Convert.ToDecimal(0),
                DLINT = Convert.ToDecimal(0),
                DBRATIO = Convert.ToDecimal(0),
                SFCODE = "5920",
                AGTAMT = Convert.ToDecimal(0),
                FEE = Convert.ToDecimal(0),
                TAX = Convert.ToDecimal(0),
                DBFEE = Convert.ToDecimal(0),
                COST = Convert.ToDecimal(0),
                STINTAX = Convert.ToDecimal(0),
                HEALTHFEE = Convert.ToDecimal(0),
                TRDATE = "20220916",
                TRTIME = "174542",
                MODDATE = "",
                MODTIME = "",
                MODUSER = ""
            });
            dbTDBUD.Add(new TDBUD()
            {
                TDATE = "20220804",
                BHNO = "592S",
                DSEQ = "i0328",
                DNO = "0000004",
                CSEQ = "0082985",
                STOCK = "8069",
                PRICE = Convert.ToDecimal(184.5000),
                QTY = Convert.ToDecimal(1000),
                DBAMT = Convert.ToDecimal(166100),
                GTAMT = Convert.ToDecimal(166100),
                DNAMT = Convert.ToDecimal(183538),
                BQTY = Convert.ToDecimal(1000),
                BDBAMT = Convert.ToDecimal(349638),
                BGTAMT = Convert.ToDecimal(166100),
                BDNAMT = Convert.ToDecimal(183538),
                GTINT = Convert.ToDecimal(40),
                DNINT = Convert.ToDecimal(44),
                DLFEE = Convert.ToDecimal(0),
                DLINT = Convert.ToDecimal(0),
                DBRATIO = Convert.ToDecimal(0),
                SFCODE = "5920",
                AGTAMT = Convert.ToDecimal(0),
                FEE = Convert.ToDecimal(0),
                TAX = Convert.ToDecimal(0),
                DBFEE = Convert.ToDecimal(0),
                COST = Convert.ToDecimal(0),
                STINTAX = Convert.ToDecimal(0),
                HEALTHFEE = Convert.ToDecimal(0),
                TRDATE = "20220916",
                TRTIME = "174542",
                MODDATE = "",
                MODTIME = "",
                MODUSER = ""
            });
            #endregion
            sumList = SubGainLost.SubSearchSum(dbTDBUD, Quote_Dic);
            accsum = SubGainLost.SubSearchAccSum(sumList);
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].tdate, "20220804");
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].ttypename, "融券");
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].bstype, "S");
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].mamt, Convert.ToDecimal(184500));
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].estimateAmt, Convert.ToDecimal(-80900.0));
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].unoffset_qtype_detail[0].interest, Convert.ToDecimal(84));

            Assert.AreEqual(accsum.unoffset_qtype_sum[0].stock, "8069");
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].ttypename, "融券");
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].bstype, "S");
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].estimateAmt, Convert.ToDecimal(-242700));
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].interest, Convert.ToDecimal(252));

            Assert.AreEqual(accsum.unoffset_qtype_sum[0].estimateAmt, Convert.ToDecimal(-242700));
            Assert.AreEqual(accsum.unoffset_qtype_sum[0].gtamt, Convert.ToDecimal(498300));
        }
    }
}