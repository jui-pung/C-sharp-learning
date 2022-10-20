using _1_UnrealizedGainsOrLosses.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace _1_UnrealizedGainsOrLosses
{
    public partial class Form1 : Form
    {
        int type;                                           //查詢與回覆格式設定
        string searchStr;                                   //查詢xml或json格式字串
        SqlTask sqlTask;                                    //自訂sql類別
        unoffset unoffsetTask;                              //自訂unoffset類別
        offset offsetTask;                                  //自訂offset類別
                                                            //未實現損益
        List<unoffset_qtype_detail> detailList;             //自訂unoffset_qtype_detail類別List (階層三:個股明細)
        List<unoffset_qtype_sum> sumList;                   //自訂unoffset_qtype_sum類別List    (階層二:個股未實現損益)
        List<unoffset_qtype_accsum> accsumList;             //自訂unoffset_qtype_accsum類別List (階層一:帳戶未實現損益)
                                                            //已實現損益
        List<profit_detail_out> detailOutList;              //自訂profit_detail_out類別List     (階層三:個股明細資料 (賣出))  
        public Form1()
        {
            InitializeComponent();
            txtSearchResultContent.AutoSize = true;
        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtSearchContent.Clear();
            txtSearchResultContent.Clear();
            sqlTask = new SqlTask();

            if (comboBoxQTYPE.Text == "0001" && txtBHNO.Text.Length == 4 && txtCSEQ.Text.Length == 7)
            {
                unoffsetTask = new unoffset();
                //取得查詢xml或json格式字串
                searchStr = unoffsetTask.searchSerilizer(type);
                txtSearchContent.Text = searchStr;
                //取得查詢字串Element
                var obj = unoffsetTask.GetElement(searchStr, type);
                root SearchElement = obj as root;
                //查詢開始...
                detailList = new List<unoffset_qtype_detail>();
                sumList = new List<unoffset_qtype_sum>();
                accsumList = new List<unoffset_qtype_accsum>();
                detailList = sqlTask.selectTCNUD(SearchElement);
                if(detailList.Count > 0)
                {
                    detailList = sqlTask.selectMSTMB(SearchElement);
                    detailList = unoffsetTask.searchDetails(detailList);
                    sumList = unoffsetTask.searchSum(detailList);
                    accsumList = unoffsetTask.searchAccSum(sumList);
                    //呈現查詢結果
                    resultListSerilizer();
                }
                else
                {
                    resultErrListSerilizer();
                }
            }
            else if (comboBoxQTYPE.Text == "0002" && txtBHNO.Text.Length == 4 && txtCSEQ.Text.Length == 7)
            {
                offsetTask = new offset();
                //取得查詢xml或json格式字串
                searchStr = offsetTask.searchSerilizer(type);
                txtSearchContent.Text = searchStr;
                //取得查詢字串Element
                var obj = offsetTask.GetElement(searchStr, type);
                root SearchElement = obj as root;
                ////查詢開始...
                detailOutList = new List<profit_detail_out>();
                //sumList = new List<unoffset_qtype_sum>();
                //accsumList = new List<unoffset_qtype_accsum>();
                detailOutList = sqlTask.selectHCNRH(SearchElement);
                if (detailOutList.Count > 0)
                {
                    detailOutList = offsetTask.searchDetails(detailOutList);
                    //sumList = unoffsetTask.searchSum(detailList);
                    //accsumList = unoffsetTask.searchAccSum(sumList);
                    ////呈現查詢結果
                    //resultListSerilizer();
                }
                else
                {
                    //resultErrListSerilizer();
                }
            }
            else
                MessageBox.Show("輸入格式錯誤 請重新輸入");
        }

        //---------------------------------------------------------------------------
        //function resultListSerilizer() - 將查詢結果 序列化為xml或json格式字串
        //---------------------------------------------------------------------------
        public void resultListSerilizer()
        {
            foreach (var item in accsumList)
            {
                //處理第三階層反序列內容 
                int index = 0;
                for (int i = 0; i < item.unoffset_qtype_sum.Count; i++)
                {
                    //初始化 unoffset_qtype_sum[].unoffset_qtype_detail 清單長度為detaillist長度
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
                        //合併第三階層相同股票代號到第二階層
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
                    item.unoffset_qtype_sum[i].unoffset_qtype_detail.RemoveAll(s => s == null);
                }

                //透過unoffset_qtype_accsum自訂類別反序列 -> accsumSer
                var accsumSer = new unoffset_qtype_accsum()
                {
                    errcode = "0000",
                    errmsg = "成功",
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
                //序列化為xml格式字串
                if (type == 0)
                {
                    using (var stringwriter = new System.IO.StringWriter())
                    {
                        var serializer = new XmlSerializer(typeof(unoffset_qtype_accsum));
                        serializer.Serialize(stringwriter, accsumSer);
                        txtSearchResultContent.Text += stringwriter.ToString();
                    }
                }
                //序列化為json格式字串
                else if (type == 1)
                {
                    var settings = new JsonSerializerSettings();
                    settings.NullValueHandling = NullValueHandling.Ignore;
                    settings.Formatting = Formatting.Indented;
                    string jsonString = JsonConvert.SerializeObject(accsumSer, settings);
                    txtSearchResultContent.Text += jsonString.ToString();
                }
            }
        }

        //---------------------------------------------------------------------------
        //function resultErrListSerilizer() - 將查詢失敗結果 序列化為xml或json格式字串
        //---------------------------------------------------------------------------
        public void resultErrListSerilizer()
        {
            //透過unoffset_qtype_accsum自訂類別反序列 -> accsumSer
            var accsumSer = new unoffset_qtype_accsum()
            {
                errcode = "0001",
                errmsg = "查詢失敗",
            };
            //序列化為xml格式字串
            if (type == 0)
            {
                using (var stringwriter = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(unoffset_qtype_accsum));
                    serializer.Serialize(stringwriter, accsumSer);
                    txtSearchResultContent.Text += stringwriter.ToString();
                }
            }
            //序列化為json格式字串
            else if (type == 1)
            {
                var settings = new JsonSerializerSettings();
                settings.NullValueHandling = NullValueHandling.Ignore;
                settings.Formatting = Formatting.Indented;
                string jsonString = JsonConvert.SerializeObject(accsumSer, settings);
                txtSearchResultContent.Text += jsonString.ToString();
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
