using _1_UnrealizedGainsOrLosses.Properties;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _1_UnrealizedGainsOrLosses
{
    public partial class Form1 : Form
    {
        int type = 1;                                       //查詢與回覆格式設定
        string searchStr;                                   //查詢xml或json格式字串
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
            txtSearchContent.Clear();
            txtSearchResultContent.Clear();
            
            if(txtBHNO.Text.Length == 4 && txtCSEQ.Text.Length == 7)
            {
                //取得查詢xml或json格式字串
                searchStr = searchSerilizer();
                txtSearchContent.Text = searchStr;
                //取得查詢字串Element
                var obj = GetElement(searchStr);
                root SearchElement = obj as root;
                //查詢開始...
                detailList = new List<unoffset_qtype_detail>();
                sumList = new List<unoffset_qtype_sum>();
                accsumList = new List<unoffset_qtype_accsum>();
                sqlTask = new SqlTask();
                detailList = sqlTask.selectTCNUD(SearchElement);
                detailList = sqlTask.selectMSTMB(SearchElement);
                detailList = searchDetails();
                sumList = searchSum(detailList);
                accsumList = searchAccSum(sumList);
                //呈現查詢結果
                resultListSerilizer();
            }
            else
                MessageBox.Show("輸入格式錯誤 請重新輸入");
        }

        //-------------------------------------------------------------------
        //function SearchSerilizer() - 將輸入的查詢資訊序列化為xml格式字串
        //-------------------------------------------------------------------
        public string searchSerilizer()
        {
            if(type == 0)
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
            else if (type == 1)
            {
                var root = new root
                {
                    bhno = txtBHNO.Text,
                    cseq = txtCSEQ.Text
                };
                string jsonString = JsonConvert.SerializeObject(root);
                return jsonString;
            }
            return "";
        }

        //------------------------------------------------------------------------
        // function GetElement() - 取得xml格式字串 Element
        //------------------------------------------------------------------------
        public object GetElement(string Content)
        {
            root root = new root();
            if (type == 0)
            {
                //建立serializer物件,並指定反序列化物件的型別(root)
                XmlSerializer ser = new XmlSerializer(typeof(root));
                //反序列化XML(obj為反序列化的型別的物件變數)
                root obj = (root)ser.Deserialize(new StringReader(Content));
                return obj;
            }
            else if (type == 1)
            {
                root obj = JsonConvert.DeserializeObject<root>(Content);
                return obj;
            }
            return root;
        }

        //------------------------------------------------------------------------
        // function searchDetails() - 計算取得 查詢回復階層三的個股明細
        //------------------------------------------------------------------------
        public List<unoffset_qtype_detail> searchDetails()
        {
            foreach (var item in detailList)
            {
                item.mamt = item.bqty * item.mprice;
                item.estimateAmt = decimal.Truncate(item.lastprice * item.bqty);
                item.estimateFee = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(item.estimateAmt) * 0.001425));
                if(item.estimateFee < 20)
                    item.estimateFee = 20;
                item.estimateTax = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(item.estimateAmt) * 0.003));
                item.marketvalue = item.estimateAmt - item.estimateFee - item.estimateTax;
                item.profit = item.marketvalue - item.cost;
                item.pl_ratio = decimal.Round(((item.profit / item.cost) * 100), 2).ToString() + "%";
            }
            return detailList;
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

        //---------------------------------------------------------------------------
        //function resultListSerilizer() - 將查詢結果 序列化為xml格式字串
        //---------------------------------------------------------------------------
        public void resultListSerilizer()
        {
            foreach (var item in accsumList)
            {
                //處理第三階層反序列內容 
                int index = 0;
                for (int i = 0; i < item.unoffset_qtype_sum.Count; i++)
                {
                    List<unoffset_qtype_detail> initialization = new List<unoffset_qtype_detail>();
                    int count = 0;
                    while(count < detailList.Count)
                    {
                        initialization.Add(null);
                        count++;
                    }
                    item.unoffset_qtype_sum[i].unoffset_qtype_detail = initialization;
                    
                    for (int j = index; j < detailList.Count; j++)
                    {
                        if (item.unoffset_qtype_sum[i].stock.Equals(detailList[j].stock))
                        {
                            item.unoffset_qtype_sum[i].unoffset_qtype_detail[j] = detailList[j];
                        }
                        else
                        {
                            index = j;
                            break;
                        }      
                    }
                }
                //透過unoffset_qtype_accsum自訂類別反序列
                if (type == 0)
                {
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
                else if (type == 1)
                {
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
                    JsonSerializerSettings settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    string jsonString = JsonConvert.SerializeObject(accsumSer);
                    txtSearchResultContent.Text += jsonString.ToString() + "\r\n";
                }
            }
        }

        private void radioBtnXml_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnXml.Checked)
                type = 0;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnJson.Checked)
                type = 1;
        }
    }
}
