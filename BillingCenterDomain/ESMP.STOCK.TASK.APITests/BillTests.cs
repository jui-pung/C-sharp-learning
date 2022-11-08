using Microsoft.VisualStudio.TestTools.UnitTesting;
using ESMP.STOCK.TASK.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESMP.STOCK.FORMAT;

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
            Assert.AreEqual(billsum.cnbamt, Convert.ToDecimal(79800.0));
            Assert.AreEqual(billsum.cnsamt, Convert.ToDecimal(0));
            Assert.AreEqual(billsum.cnfee, Convert.ToDecimal(112.0));
            Assert.AreEqual(billsum.cntax, Convert.ToDecimal(0.0));
            Assert.AreEqual(billsum.cnnetamt, Convert.ToDecimal(-79912.0));
            Assert.AreEqual(billsum.bqty, Convert.ToDecimal(4000.0));
            Assert.AreEqual(billsum.sqty, Convert.ToDecimal(0));
        }
    }
}