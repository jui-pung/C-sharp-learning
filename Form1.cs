using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _1_UnrealizedGainsOrLosses
{
    public partial class Form1 : Form
    {
        string searchStr;                                   //查詢xml格式字串
        SqlTask sqlTask;                                    //自訂sql類別
        List<unoffset_qtype_detail> detailList;             //自訂unoffset_qtype_detail類別List (階層三:個股明細)
        List<unoffset_qtype_sum> sumList;                   //自訂unoffset_qtype_sum類別List    (階層二:個股未實現損益)
        List<unoffset_qtype_accsum> accsumList;             //自訂unoffset_qtype_accsum類別List (階層一:帳戶未實現損益)

        public Form1()
        {
            InitializeComponent();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //取得查詢xml格式字串
            searchStr = searchSerilizer();
            txtSearchContent.Text = searchStr;
            //取得查詢字串Element
            var obj = xmlGetElement(searchStr);
            root SearchElement = obj as root;
            //查詢開始....
            detailList = new List<unoffset_qtype_detail>();
            sumList = new List<unoffset_qtype_sum>();
            accsumList = new List<unoffset_qtype_accsum>();
            sqlTask = new SqlTask();
            detailList =  sqlTask.selectTCNUD(SearchElement);
            detailList = sqlTask.selectMSTMB(SearchElement);
            detailList = searchDetails();
            sumList = searchSum(detailList);
            accsumList = searchAccSum(sumList);
            //呈現階層三查詢結果
            //detailListSerilizer();
            resultListSerilizer();
        }

        //-------------------------------------------------------------------
        //function SearchSerilizer() - 將輸入的查詢資訊序列化為xml格式字串
        //-------------------------------------------------------------------
        public string searchSerilizer()
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(typeof(root));
                serializer.Serialize(stringwriter, new root
                {
                    bhno = txtBHNO.Text,
                    cseq = txtCSEQ.Text
                });
                return stringwriter.ToString();
            }
        }

        //------------------------------------------------------------------------
        // function xmlGetElement() - 取得xml格式字串 Element
        //------------------------------------------------------------------------
        public object xmlGetElement(string xmlContent)
        {
            //建立serializer物件,並指定反序列化物件的型別(root)
            XmlSerializer ser = new XmlSerializer(typeof(root));
            //反序列化XML(obj為反序列化的型別的物件變數)
            var obj = (root)ser.Deserialize(new StringReader(xmlContent));
            return obj;
        }

        //------------------------------------------------------------------------
        // function searchDetails() - 計算取得 查詢回復階層三的個股明細
        //------------------------------------------------------------------------
        public List<unoffset_qtype_detail> searchDetails()
        {
            foreach (var item in detailList)
            {
                item.mamt = item.bqty * item.mprice;
                item.estimateAmt = item.lastprice * item.bqty;
                item.estimateFee = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(item.estimateAmt) * 0.001425));
                if(item.estimateFee < 1)
                    item.estimateFee = 1;
                item.estimateTax = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(item.estimateAmt) * 0.003));
                item.marketvalue = item.estimateAmt - item.estimateFee - item.estimateTax;
                item.profit = item.marketvalue - item.cost;
                item.pl_ratio = decimal.Round(((item.profit / item.cost) * 100), 2).ToString() + "%";
            }
            return detailList;
        }
        //---------------------------------------------------------------------------
        //function detailListSerilizer() - 將查詢回復階層三結果 序列化為xml格式字串
        //---------------------------------------------------------------------------
        public void detailListSerilizer()
        {
            foreach (var item in detailList)
            {
                using (var stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(unoffset_qtype_detail));
                    serializer.Serialize(stringwriter, new unoffset_qtype_detail
                    {
                        tdate = item.tdate,
                        ttype = item.ttype,
                        ttypename = item.ttypename,
                        bstype = item.bstype,
                        dseq = item.dseq,
                        dno = item.dno,
                        bqty = item.bqty,
                        mprice = item.mprice,
                        mamt = item.mamt,
                        lastprice = item.lastprice,
                        marketvalue = item.marketvalue,
                        fee = item.fee,
                        tax = item.tax,
                        cost = item.cost,
                        estimateAmt = item.estimateAmt,
                        estimateFee = item.estimateFee,
                        estimateTax = item.estimateTax,
                        profit = item.profit,
                        pl_ratio = item.pl_ratio
                    });
                    txtSearchResultContent.Text += stringwriter.ToString() + "\r\n";
                    txtSearchResultContent.Text += Environment.NewLine;
                }
            }
        }

        //------------------------------------------------------------------------
        // function searchSum() - 計算取得 查詢回復階層二 個股未實現損益
        //------------------------------------------------------------------------
        public List<unoffset_qtype_sum> searchSum(List<unoffset_qtype_detail> detailList)
        {
            string preStockNO = "";
            int index = -1;
            foreach (var item in detailList)
            {               
                //判斷detailList的Stock名稱與前一次是否相同 是->個股明細加總
                if (item.stock.Equals(preStockNO))
                {
                    sumList[index].bqty += item.bqty;
                    sumList[index].cost += item.cost;
                    sumList[index].marketvalue += item.marketvalue;
                    sumList[index].estimateAmt += item.estimateAmt;
                    sumList[index].estimateFee += item.estimateFee;
                    sumList[index].estimateTax += item.estimateTax;
                    sumList[index].profit += item.profit;
                    sumList[index].fee += item.fee;
                    sumList[index].tax += item.tax;
                    sumList[index].amt += item.mamt;
                }
                else
                {
                    preStockNO = item.stock;
                    index++;
                    var row = new unoffset_qtype_sum();
                    row.stock = item.stock;
                    row.stocknm = sqlTask.selectStockName(item.stock);
                    row.bqty = item.bqty;
                    row.cost = item.cost;
                    row.lastprice = item.lastprice;
                    row.marketvalue = item.marketvalue;
                    row.estimateAmt = item.estimateAmt;
                    row.estimateFee = item.estimateFee;
                    row.estimateTax = item.estimateTax;
                    row.profit = item.profit;
                    row.fee = item.fee;
                    row.tax = item.tax;
                    row.amt = item.mamt;
                    sumList.Add(row);
                }   
            }
            foreach (var item in sumList)
            {
                item.avgprice = decimal.Round((item.cost / item.bqty), 2);
                item.pl_ratio = decimal.Round(((item.profit / item.cost) * 100), 2).ToString() + "%";
                
            }
            return sumList;
        }

        //------------------------------------------------------------------------
        // function searchSum() - 計算取得 查詢回復階層二 個股未實現損益
        //------------------------------------------------------------------------
        public List<unoffset_qtype_accsum> searchAccSum(List<unoffset_qtype_sum> sumList)
        {
            var row = new unoffset_qtype_accsum();
            foreach (var item in sumList)
            {
                row.bqty += item.bqty;
                row.cost += item.cost;
                row.marketvalue += item.marketvalue;
                row.estimateAmt += item.estimateAmt;
                row.estimateFee += item.estimateFee;
                row.estimateTax += item.estimateTax;
                row.profit += item.profit;
                row.fee += item.fee;
                row.tax += item.tax;   
            }
            accsumList.Add(row);
            foreach (var item in accsumList)
            {
                item.pl_ratio = decimal.Round(((item.profit / item.cost) * 100), 2).ToString() + "%";
                item.unoffset_qtype_sum = sumList;
            }
            return accsumList;
        }

        public void resultListSerilizer()
        {
            resultListSerilizer(detailList);
        }

        //---------------------------------------------------------------------------
        //function resultListSerilizer() - 將查詢結果 序列化為xml格式字串
        //---------------------------------------------------------------------------
        public void resultListSerilizer(List<unoffset_qtype_detail> detailList)
        {
            //foreach (var item in sumList)
            //{
            //    item.unoffset_qtype_detail = detailList;
            //    //item.unoffset_qtype_detail.Contains(item.stock);
            //}
                
            foreach (var item in accsumList)
            {
                //List < unoffset_qtype_detail > test = new List<unoffset_qtype_detail>();
                //test = detailList;
                //unoffset_qtype_detail test1 = new unoffset_qtype_detail();
                //test1 = detailList[0];
                //item.unoffset_qtype_sum[0].unoffset_qtype_detail = detailList;
                //item.unoffset_qtype_sum[0].unoffset_qtype_detail[0] = detailList[0];
                //item.unoffset_qtype_sum[0].unoffset_qtype_detail[1] = detailList[0];
                //item.unoffset_qtype_sum[0].unoffset_qtype_detail[2] = detailList[2];
                //item.unoffset_qtype_sum[0].unoffset_qtype_detail[3] = detailList[0];
                //item.unoffset_qtype_sum[0].unoffset_qtype_detail[4] = detailList[0];
                using (var stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(unoffset_qtype_accsum));
                    var accsumSer = new unoffset_qtype_accsum()
                    {
                        bqty = item.bqty,
                        marketvalue = item.marketvalue,
                        fee = item.fee,
                        tax = item.tax,
                        cost = item.cost,
                        estimateAmt = item.estimateAmt,
                        estimateFee = item.estimateFee,
                        estimateTax = item.estimateTax,
                        profit = item.profit,
                        pl_ratio = item.pl_ratio,
                        unoffset_qtype_sum = item.unoffset_qtype_sum
                        
                    };
                    serializer.Serialize(stringwriter, accsumSer);
                    txtSearchResultContent.Text += stringwriter.ToString() + "\r\n";
                }
            }
        }
    }
}
