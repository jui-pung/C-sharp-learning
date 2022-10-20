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
        List<profit_detail_out> detailList = new List<profit_detail_out>();

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
                item.netamt = item.income;
                item.pl_ratio = decimal.Round(((item.profit / item.cost) * 100), 2).ToString() + "%";
                item.ttypename2 = "現賣";
            }
            return detailList;
        }
    }
}
