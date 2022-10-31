using ESMP.STOCK.DB.TABLE.API;
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
        GainLost gainLost;                                  //自訂GainLost類別  (ESMP.STOCK.TASK.API)
        GainPay gainPay;                                    //自訂GainPay類別   (ESMP.STOCK.TASK.API)
        Bill bill;                                          //自訂Bill類別   (ESMP.STOCK.TASK.API)

        //未實現損益
        
        

        //對帳單
        List<profile> profileList;                          //自訂profile類別List               (階層二:對帳單明細資料)  
        billSum billsum;                                    //自訂billSum類別class              (階層二:對帳單匯總資料)  
        profile_sum profileSum;                             //自訂profile_sum類別Class          (階層一:對帳單彙總資料)  

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtSearchContent.Clear();
            txtSearchResultContent.Clear();
            sqlSearch = new SqlSearch();

            //未實現損益查詢
            if (comboBoxQTYPE.Text == "0001" && txtBHNO.Text.Length == 4 && txtCSEQ.Text.Length == 7)
            {
                //宣告物件
                GainLost gainLost = new GainLost();                                             //自訂GainLost類別   (ESMP.STOCK.TASK.API)
                List<TCNUD> TCNUDList = new List<TCNUD>();                                      //自訂TCNUD類別List (ESMP.STOCK.DB.TABLE.API)
                List<unoffset_qtype_detail> detailList = new List<unoffset_qtype_detail>();     //自訂unoffset_qtype_detail類別List (階層三:個股明細)
                List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();              //自訂unoffset_qtype_sum類別List    (階層二:個股未實現損益)
                List<unoffset_qtype_accsum> accsumList = new List<unoffset_qtype_accsum>();     //自訂unoffset_qtype_accsum類別List (階層一:帳戶未實現損益)

                //取得查詢xml或json格式字串
                searchStr = gainLost.searchSerilizer(comboBoxQTYPE.Text, txtBHNO.Text, txtCSEQ.Text, type);
                txtSearchContent.Text = searchStr;
                //取得查詢字串Element
                var obj = gainLost.GetElement(searchStr, type);
                root SearchElement = obj as root;
                //查詢開始...
                TCNUDList = sqlSearch.selectTCNUD(SearchElement);
                if (TCNUDList.Count > 0)
                {
                    sumList = gainLost.searchSum(TCNUDList);
                    //accsumList = gainLost.searchAccSum(sumList);
                    //watch.Stop();
                    //var elapsedMs = watch.ElapsedMilliseconds;
                    //Console.WriteLine(elapsedMs);
                    //呈現查詢結果
                    txtSearchResultContent.Text = gainLost.resultListSerilizer(detailList, type);
                }
                else
                {
                    txtSearchResultContent.Text = gainLost.resultErrListSerilizer(type);
                }
            }
            //已實現損益查詢
            else if (comboBoxQTYPE.Text == "0002" && txtBHNO.Text.Length == 4 && txtCSEQ.Text.Length == 7 && txtEDATE.Text.Length == 8 && txtSDATE.Text.Length == 8)
            {
                //宣告物件
                GainPay gainPay = new GainPay();                                                //自訂GainPay類別   (ESMP.STOCK.TASK.API)
                List<HCNRH> HCNRHList = new List<HCNRH>();                                      //自訂HCNRH類別List (ESMP.STOCK.DB.TABLE.API)
                List<HCNTD> HCNTDList = new List<HCNTD>();                                      //自訂HCNTD類別List (ESMP.STOCK.DB.TABLE.API)
                List<profit_sum> sumProfitList_HCNRH = new List<profit_sum>();                  //自訂profit_sum類別List            (階層二:個股已實現損益)  
                List<profit_sum> sumProfitList_HCNTD = new List<profit_sum>();                  //自訂profit_sum類別List            (階層二:個股已實現損益)  
                List<profit_sum> sumProfitList = new List<profit_sum>();                        //自訂profit_sum類別List            (階層二:個股已實現損益)  
                List<profit_accsum> accsumProfitList = new List<profit_accsum>();               //自訂profit_accsum類別List         (階層一:帳戶已實現損益)  

                //取得查詢xml或json格式字串
                searchStr = gainPay.searchSerilizer(comboBoxQTYPE.Text, txtBHNO.Text, txtCSEQ.Text, txtSDATE.Text, txtEDATE.Text, type);
                txtSearchContent.Text = searchStr;
                //取得查詢字串Element
                var obj = gainPay.GetElement(searchStr, type);
                root SearchElement = obj as root;
                //查詢開始...
                HCNRHList = sqlSearch.selectHCNRH(SearchElement);
                HCNTDList = sqlSearch.selectHCNTD(SearchElement);
                if (HCNRHList.Count > 0 || HCNTDList.Count > 0)
                {
                    sumProfitList_HCNRH = gainPay.searchSum_HCNRH(HCNRHList, txtBHNO.Text, txtCSEQ.Text);
                    sumProfitList_HCNTD = gainPay.searchSum_HCNTD(HCNTDList, txtBHNO.Text, txtCSEQ.Text);
                    //合併沖銷與當沖資料
                    sumProfitList = sumProfitList_HCNRH.Concat(sumProfitList_HCNTD).ToList();
                    accsumProfitList = gainPay.searchAccSum(sumProfitList);
                    //呈現查詢結果
                    txtSearchResultContent.Text = gainPay.resultListSerilizer(accsumProfitList, type);
                }
                else
                {
                    txtSearchResultContent.Text = gainPay.resultErrListSerilizer(type);
                }
            }
            //對帳單查詢
            else if (comboBoxQTYPE.Text == "0003" && txtBHNO.Text.Length == 4 && txtCSEQ.Text.Length == 7 && txtEDATE.Text.Length == 8 && txtSDATE.Text.Length == 8)
            {
                bill = new Bill();
                //取得查詢xml或json格式字串
                searchStr = bill.searchSerilizer(comboBoxQTYPE.Text, txtBHNO.Text, txtCSEQ.Text, txtSDATE.Text, txtEDATE.Text, txtStockSymbol.Text, type);
                txtSearchContent.Text = searchStr;
                //取得查詢字串Element
                var obj = bill.GetElement(searchStr, type);
                root SearchElement = obj as root;
                //查詢開始...
                profileList = new List<profile>();
                billsum = new billSum();
                profileSum = new profile_sum();
                profileList = sqlSearch.selectHCMIO(SearchElement);
                profileList = sqlSearch.selectTMHIO(SearchElement);
                if (profileList.Count > 0)
                {
                    profileList = bill.searchDetails(profileList, txtBHNO.Text, txtCSEQ.Text);
                    billsum = bill.searchSum(profileList);
                    profileSum = bill.searchProfileSum(profileList);
                    //呈現查詢結果
                    txtSearchResultContent.Text = bill.resultListSerilizer(type);
                }
                else
                {
                    txtSearchResultContent.Text = bill.resultErrListSerilizer(type);
                }
            }
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

        private void comboBoxQTYPE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxQTYPE.Text == "0003")
            {
                txtBHNO.Text = "592S";
                txtCSEQ.Text = "0098047";
                txtSDATE.Text = "20221001";
                txtEDATE.Text = "20221031";
                txtStockSymbol.Text = "2330";
            }
        }
    }
}
