using System;
using System.Collections.Generic;
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
        string searchStr;               //查詢xml格式字串
        SqlTask sqlTask;                //自訂sql類別
        public Form1()
        {
            InitializeComponent();
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            //取得查詢xml格式字串
            searchStr = SearchSerilizer();
            txtSearchContent.Text = searchStr;
            //取得查詢字串Element
            var obj = xmlGetElement(searchStr);
            root SearchElement = obj as root;
            //查詢開始....
            sqlTask = new SqlTask();
            sqlTask.qtype_detail(SearchElement);

        }

        //-------------------------------------------------------------------
        //function SearchSerilizer() - 將輸入的查詢資訊序列化為xml格式字串
        //-------------------------------------------------------------------
        public string SearchSerilizer()
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
    }
}
