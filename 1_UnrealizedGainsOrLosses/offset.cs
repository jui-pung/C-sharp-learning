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
    //處理已實現損益查詢類別
    public class offset
    {
        System.Windows.Forms.ComboBox comboBoxQTYPE = Application.OpenForms["Form1"].Controls["comboBoxQTYPE"] as System.Windows.Forms.ComboBox;
        System.Windows.Forms.TextBox txtBHNO = Application.OpenForms["Form1"].Controls["txtBHNO"] as System.Windows.Forms.TextBox;
        System.Windows.Forms.TextBox txtCSEQ = Application.OpenForms["Form1"].Controls["txtCSEQ"] as System.Windows.Forms.TextBox;
        System.Windows.Forms.TextBox txtSDATE = Application.OpenForms["Form1"].Controls["txtSDATE"] as System.Windows.Forms.TextBox;
        System.Windows.Forms.TextBox txtEDATE = Application.OpenForms["Form1"].Controls["txtEDATE"] as System.Windows.Forms.TextBox;
        List<profit_sum> sumList = new List<profit_sum>();
        List<profit_accsum> accsumList = new List<profit_accsum>();

        //-------------------------------------------------------------------
        //function SearchSerilizer() - 將輸入的查詢資訊序列化為xml格式字串
        //-------------------------------------------------------------------
        public string searchSerilizer(int type)
        {
            var root = new root()
            {
                qtype = comboBoxQTYPE.Text,
                bhno = txtBHNO.Text,
                cseq = txtCSEQ.Text,
                sdate = txtSDATE.Text,
                edate = txtEDATE.Text
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
        // function searchDetails() - 計算取得 查詢回復階層三的個股明細資料 (賣出)
        //------------------------------------------------------------------------
        public List<profit_detail_out> searchDetails(List<profit_detail_out> detailList)
        {
            foreach (var item in detailList)
            {
                item.mamt = (item.cqty * Convert.ToDecimal(item.mprice)).ToString();
                item.pl_ratio = decimal.Round(((item.profit / item.cost) * 100), 2).ToString() + "%";
            }
            return detailList;
        }

        //------------------------------------------------------------------------
        // function searchDetails_B() - 計算取得 查詢回復階層三的個股明細資料 (買入)
        //------------------------------------------------------------------------
        public List<profit_detail> searchDetails_B(List<profit_detail> detailList)
        {
            foreach (var item in detailList)
            {
                item.mamt = (item.cqty * Convert.ToDecimal(item.mprice)).ToString();
                item.pl_ratio = decimal.Round(((item.profit / item.cost) * 100), 2).ToString() + "%";
            }
            return detailList;
        }

        //------------------------------------------------------------------------
        // function searchSum() - 計算取得 查詢回復階層二 個股已實現損益
        //------------------------------------------------------------------------
        public List<profit_sum> searchSum(List<profit_detail_out> detailList)
        {
            SqlTask sqlTask = new SqlTask();
            string preStockNO = "";
            int index = -1;
            foreach (var item in detailList)
            {
                //判斷detailList的Stock名稱與前一次是否相同 是->個股明細加總
                if (item.stock.Equals(preStockNO))
                {
                    sumList[index].tdate = item.tdate;
                    sumList[index].dseq = item.dseq;
                    sumList[index].dno = item.dno;
                    sumList[index].cqty += item.cqty;
                    sumList[index].fee += item.fee;
                    sumList[index].tax += item.tax;
                    sumList[index].cost += item.cost;
                    sumList[index].income += item.income;
                    sumList[index].profit += item.profit;
                }
                else
                {
                    preStockNO = item.stock;
                    index++;
                    var row = new profit_sum();
                    row.tdate = item.tdate;
                    row.dseq = item.dseq;
                    row.dno = item.dno;
                    row.stock = item.stock;
                    row.stocknm = sqlTask.selectStockName(item.stock);
                    row.cqty = item.cqty;
                    row.mprice = item.mprice;
                    row.fee = item.fee;
                    row.tax = item.tax;
                    row.cost = item.cost;
                    row.income = item.income;
                    row.profit = item.profit;
                    row.ttypename2 = item.ttypename2;
                    sumList.Add(row);
                }
            }
            foreach (var item in sumList)
            {
                item.bhno = txtBHNO.Text;
                item.cseq = txtCSEQ.Text;
                item.pl_ratio = decimal.Round(((item.profit / item.cost) * 100), 2).ToString() + "%";
            }
            return sumList;
        }

        //------------------------------------------------------------------------
        // function searchAccSum() - 計算取得 查詢回復階層三 帳戶已實現損益
        //------------------------------------------------------------------------
        public List<profit_accsum> searchAccSum(List<profit_sum> sumList)
        {
            var row = new profit_accsum();
            foreach (var item in sumList)
            {
                row.cqty += item.cqty;
                row.cost += item.cost;
                row.income += item.income;
                row.profit += item.profit;
                row.fee += item.fee;
                row.tax += item.tax;
            }
            accsumList.Add(row);
            foreach (var item in accsumList)
            {
                item.pl_ratio = decimal.Round(((item.profit / item.cost) * 100), 2).ToString() + "%";
                item.profit_sum = sumList;
            }
            return accsumList;
        }
    }
}
