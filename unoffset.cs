using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Shell;
using System.Xml.Serialization;

namespace _1_UnrealizedGainsOrLosses
{
    //處理未實現損益查詢類別
    public class unoffset
    {
        System.Windows.Forms.ComboBox comboBoxQTYPE = Application.OpenForms["Form1"].Controls["comboBoxQTYPE"] as System.Windows.Forms.ComboBox;
        System.Windows.Forms.TextBox txtBHNO = Application.OpenForms["Form1"].Controls["txtBHNO"] as System.Windows.Forms.TextBox;
        System.Windows.Forms.TextBox txtCSEQ = Application.OpenForms["Form1"].Controls["txtCSEQ"] as System.Windows.Forms.TextBox;


        SqlTask sqlTask = new SqlTask();
        List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();
        List<unoffset_qtype_accsum> accsumList = new List<unoffset_qtype_accsum>();

        //-------------------------------------------------------------------
        //function SearchSerilizer() - 將輸入的查詢資訊序列化為xml格式字串
        //-------------------------------------------------------------------
        public string searchSerilizer(int type)
        {
            var root = new root()
            {
                qtype = comboBoxQTYPE.Text,
                bhno = txtBHNO.Text,
                cseq = txtCSEQ.Text
            };
            if (type == 0)
            {
                using (var stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(root));
                    serializer.Serialize(stringwriter, root);
                    return stringwriter.ToString();
                }
            }
            else if (type == 1)
            {
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting = Formatting.Indented;
                string jsonString = JsonConvert.SerializeObject(root, settings);
                return jsonString;
            }
            return "";
        }

        //------------------------------------------------------------------------
        // function GetElement() - 取得xml格式字串 Element
        //------------------------------------------------------------------------
        public object GetElement(string Content, int type)
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
        public List<unoffset_qtype_detail> searchDetails(List<unoffset_qtype_detail> detailList)
        {
            foreach (var item in detailList)
            {
                item.mamt = item.bqty * item.mprice;
                item.estimateAmt = decimal.Truncate(item.lastprice * item.bqty);
                item.estimateFee = decimal.Truncate(Convert.ToDecimal(decimal.ToDouble(item.estimateAmt) * 0.001425));
                if (item.estimateFee < 20)
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
    }
}
