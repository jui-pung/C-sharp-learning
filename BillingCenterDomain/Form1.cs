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
        int _type;                                           //查詢與回覆格式設定
        string _searchStr;                                   //查詢xml或json格式字串
        SqlSearch _sqlSearch;                                //自訂SqlSearch類別 (ESMP.STOCK.TASK.API)
        Dictionary<string, string> _ioflagNameDic;
        public Form1()
        {
            InitializeComponent();
            _sqlSearch = new SqlSearch();
            _ioflagNameDic = new Dictionary<string, string>();
            _ioflagNameDic = _sqlSearch.createIoflagameDic();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtSearchContent.Clear();
            txtSearchResultContent.Clear();
            _sqlSearch = new SqlSearch();

            //未實現損益查詢
            if (comboBoxQTYPE.Text == "0001" && txtBHNO.Text.Length == 4 && txtCSEQ.Text.Length == 7)
            {
                //宣告物件
                GainLost gainLost = new GainLost();                                             //自訂GainLost類別   (ESMP.STOCK.TASK.API)
                List<TCNUD> TCNUDList = new List<TCNUD>();                                      //自訂TCNUD類別List (ESMP.STOCK.DB.TABLE.API)
                List<TMHIO> TMHIOList = new List<TMHIO>();                                      //自訂TMHIO類別List (ESMP.STOCK.DB.TABLE.API)
                List<TCSIO> TCSIOList = new List<TCSIO>();                                      //自訂TCSIO類別List (ESMP.STOCK.DB.TABLE.API)
                List<unoffset_qtype_detail> detailList = new List<unoffset_qtype_detail>();     //自訂unoffset_qtype_detail類別List (階層三:個股明細)
                List<unoffset_qtype_sum> sumList = new List<unoffset_qtype_sum>();              //自訂unoffset_qtype_sum類別List    (階層二:個股未實現損益)
                List<unoffset_qtype_accsum> accsumList = new List<unoffset_qtype_accsum>();     //自訂unoffset_qtype_accsum類別List (階層一:帳戶未實現損益)
                
                //取得查詢xml或json格式字串
                _searchStr = gainLost.searchSerilizer(comboBoxQTYPE.Text, txtBHNO.Text, txtCSEQ.Text, txtStockSymbol.Text, _type);
                txtSearchContent.Text = _searchStr;
                //取得查詢字串Element
                var obj = gainLost.GetElement(_searchStr, _type);
                root SearchElement = obj as root;
                //查詢開始...
                TCNUDList = _sqlSearch.selectTCNUD(SearchElement);
                TMHIOList = _sqlSearch.selectTMHIO(SearchElement);
                TCSIOList = _sqlSearch.selectTCSIO(SearchElement);
                //新增提供今日買進現股資料TMHIOList
                TCNUDList = gainLost.getTMHIO(TCNUDList, TMHIOList);
                //新增當日現股匯入資料TCSIOList
                TCNUDList = gainLost.getTCSIO(TCNUDList, TCSIOList);
                if (TCNUDList.Count > 0)
                {
                    sumList = gainLost.searchSum(TCNUDList);
                    accsumList = gainLost.searchAccSum(sumList);
                    //呈現查詢結果
                    txtSearchResultContent.Text = gainLost.resultListSerilizer(accsumList, _type);
                }
                else
                {
                    txtSearchResultContent.Text = gainLost.resultErrListSerilizer(_type);
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
                _searchStr = gainPay.searchSerilizer(comboBoxQTYPE.Text, txtBHNO.Text, txtCSEQ.Text, txtSDATE.Text, txtEDATE.Text, txtStockSymbol.Text, _type);
                txtSearchContent.Text = _searchStr;
                //取得查詢字串Element
                var obj = gainPay.GetElement(_searchStr, _type);
                root SearchElement = obj as root;
                //查詢開始...
                HCNRHList = _sqlSearch.selectHCNRH(SearchElement);
                HCNTDList = _sqlSearch.selectHCNTD(SearchElement);
                if (HCNRHList.Count > 0 || HCNTDList.Count > 0)
                {
                    sumProfitList_HCNRH = gainPay.searchSum_HCNRH(HCNRHList, txtBHNO.Text, txtCSEQ.Text);
                    sumProfitList_HCNTD = gainPay.searchSum_HCNTD(HCNTDList, txtBHNO.Text, txtCSEQ.Text);
                    //合併沖銷與當沖資料
                    sumProfitList = sumProfitList_HCNRH.Concat(sumProfitList_HCNTD).ToList();
                    accsumProfitList = gainPay.searchAccSum(sumProfitList);
                    //呈現查詢結果
                    txtSearchResultContent.Text = gainPay.resultListSerilizer(accsumProfitList, _type);
                }
                else
                {
                    txtSearchResultContent.Text = gainPay.resultErrListSerilizer(_type);
                }
            }
            //對帳單查詢
            else if (comboBoxQTYPE.Text == "0003" && txtBHNO.Text.Length == 4 && txtCSEQ.Text.Length == 7 && txtEDATE.Text.Length == 8 && txtSDATE.Text.Length == 8)
            {
                //宣告物件
                Bill bill = new Bill();                                     //自訂Bill類別                  (ESMP.STOCK.TASK.API)
                List<HCMIO> HCMIOList = new List<HCMIO>();                  //自訂HCMIO類別List             (ESMP.STOCK.DB.TABLE.API)
                List<TMHIO> TMHIOList = new List<TMHIO>();                  //自訂TMHIO類別List             (ESMP.STOCK.DB.TABLE.API)
                List<profile> profileList = new List<profile>();            //自訂profile類別List           (階層二:對帳單明細資料)  
                billSum billsum = new billSum();                            //自訂billSum類別class          (階層二:對帳單匯總資料)  
                profile_sum profileSum = new profile_sum();                 //自訂profile_sum類別Class      (階層一:對帳單彙總資料)  

                //取得查詢xml或json格式字串
                _searchStr = bill.searchSerilizer(comboBoxQTYPE.Text, txtBHNO.Text, txtCSEQ.Text, txtSDATE.Text, txtEDATE.Text, txtStockSymbol.Text, _type);
                txtSearchContent.Text = _searchStr;
                //取得查詢字串Element
                var obj = bill.GetElement(_searchStr, _type);
                root SearchElement = obj as root;
                //查詢開始...
                HCMIOList = _sqlSearch.selectHCMIO(SearchElement);
                TMHIOList = _sqlSearch.selectTMHIO(SearchElement);
                if (HCMIOList.Count > 0 || TMHIOList.Count > 0)
                {
                    profileList = bill.searchDetails(HCMIOList, TMHIOList, txtBHNO.Text, txtCSEQ.Text);
                    billsum = bill.searchSum(profileList);
                    profileSum = bill.searchProfileSum(profileList, billsum);
                    //呈現查詢結果
                    txtSearchResultContent.Text = bill.resultListSerilizer(profileSum, _type);
                }
                else
                {
                    txtSearchResultContent.Text = bill.resultErrListSerilizer(_type);
                }
            }
            else
                MessageBox.Show("輸入格式錯誤 請重新輸入");
        
        }

        private void radioBtnXml_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnXml.Checked)
                _type = 0;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioBtnJson.Checked)
                _type = 1;
        }

        private void comboBoxQTYPE_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxQTYPE.Text == "0001")
            {
                txtBHNO.Text = "592S";
                txtCSEQ.Text = "0057758";
                txtSDATE.Text = "";
                txtEDATE.Text = "";
                txtSDATE.Enabled = false;
                txtEDATE.Enabled = false;
                txtStockSymbol.Text = "";
            }
            if (comboBoxQTYPE.Text == "0002")
            {
                txtBHNO.Text = "592S";
                txtCSEQ.Text = "0123938";
                txtSDATE.Text = "20210101";
                txtEDATE.Text = "20210121";
                txtSDATE.Enabled = true;
                txtEDATE.Enabled = true;
                txtStockSymbol.Text = "";
            }
            if (comboBoxQTYPE.Text == "0003")
            {
                txtBHNO.Text = "592S";
                txtCSEQ.Text = "0098047";
                txtSDATE.Text = "20221001";
                txtEDATE.Text = "20221031";
                txtSDATE.Enabled = true;
                txtEDATE.Enabled = true;
                txtStockSymbol.Text = "3041";
            }
        }
    }
}
