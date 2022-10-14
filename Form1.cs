using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace _1_UnrealizedGainsOrLosses
{
    public partial class Form1 : Form
    {
        string searchStr;                                   //查詢xml格式字串
        SqlTask sqlTask;                                    //自訂sql類別
        List<unoffset_qtype_detail> detailList;             //自訂unoffset_qtype_detail類別List (階層三:未實現損益–個股明細)
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
            sqlTask = new SqlTask();
            detailList =  sqlTask.selectTCNUD(SearchElement);
            detailList = sqlTask.selectMSTMB(SearchElement);
            detailList = searchDetails();
            //呈現階層三查詢結果
            detailListSerilizer();
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
    }
}
