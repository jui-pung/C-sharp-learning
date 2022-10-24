using ESMP.STOCK.FORMAT.API;
using ESMP.STOCK.TASK.API;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BillingCenterDomain
{
    public partial class Form1 : Form
    {
        int type;                                           //查詢與回覆格式設定
        string searchStr;                                   //查詢xml或json格式字串
        SqlSearch sqlSearch;                                //自訂SqlSearch類別 (ESMP.STOCK.TASK.API)
        GainLost gainLost;                                  //自訂GainLost類別 (ESMP.STOCK.TASK.API)

                                                            //未實現損益
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
            sqlSearch = new SqlSearch();

            if (comboBoxQTYPE.Text == "0001" && txtBHNO.Text.Length == 4 && txtCSEQ.Text.Length == 7)
            {
                gainLost = new GainLost();
                //取得查詢xml或json格式字串
                gainLost.getFormField(comboBoxQTYPE.Text, txtBHNO.Text, txtCSEQ.Text);
                searchStr = gainLost.searchSerilizer(type);
                txtSearchContent.Text = searchStr;
                //取得查詢字串Element
                var obj = gainLost.GetElement(searchStr, type);
                root SearchElement = obj as root;
                //查詢開始...
                detailList = new List<unoffset_qtype_detail>();
                sumList = new List<unoffset_qtype_sum>();
                accsumList = new List<unoffset_qtype_accsum>();
                detailList = sqlSearch.selectTCNUD(SearchElement);
                if (detailList.Count > 0)
                {
                    detailList = sqlSearch.selectMSTMB(SearchElement);
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    // the code that you want to measure comes here
                    detailList = gainLost.searchDetails(detailList);
                    sumList = gainLost.searchSum(detailList);
                    accsumList = gainLost.searchAccSum(sumList);
                    watch.Stop();
                    var elapsedMs = watch.ElapsedMilliseconds;
                    Console.WriteLine(elapsedMs);
                    //呈現查詢結果
                    txtSearchResultContent.Text = gainLost.resultListSerilizer(detailList, type);
                }
                else
                {
                    txtSearchResultContent.Text = gainLost.resultErrListSerilizer(type);
                }
            }
            //else if (comboBoxQTYPE.Text == "0002" && txtBHNO.Text.Length == 4 && txtCSEQ.Text.Length == 7)
            //{
            //    offsetTask = new offset();
            //    //取得查詢xml或json格式字串
            //    searchStr = offsetTask.searchSerilizer(type);
            //    txtSearchContent.Text = searchStr;
            //    //取得查詢字串Element
            //    var obj = offsetTask.GetElement(searchStr, type);
            //    root SearchElement = obj as root;
            //    //查詢開始...
            //    detailOutList = new List<profit_detail_out>();
            //    detailBuyList = new List<profit_detail>();
            //    sumProfitList = new List<profit_sum>();
            //    accsumProfitList = new List<profit_accsum>();
            //    detailOutList = sqlTask.selectHCNRH(SearchElement);
            //    detailOutList = sqlTask.selectHCNTD(SearchElement);
            //    detailBuyList = sqlTask.selectHCNRH_B(SearchElement);
            //    detailBuyList = sqlTask.selectHCNTD_B(SearchElement);
            //    if (detailOutList.Count > 0)
            //    {
            //        detailOutList = offsetTask.searchDetails(detailOutList);
            //        detailBuyList = offsetTask.searchDetails_B(detailBuyList);
            //        sumProfitList = offsetTask.searchSum(detailOutList);
            //        accsumProfitList = offsetTask.searchAccSum(sumProfitList);
            //        //呈現查詢結果
            //        resultListType2Serilizer();
            //    }
            //    else
            //    {
            //        resultErrListSerilizer();
            //    }
        
            else
                MessageBox.Show("輸入格式錯誤 請重新輸入");
        
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
