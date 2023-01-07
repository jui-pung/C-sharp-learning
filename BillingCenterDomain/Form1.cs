using ESMP.STOCK.DB.TABLE;
using ESMP.STOCK.FORMAT;
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
        int _type;                                              //查詢與回覆格式設定

        public Form1()
        {
            InitializeComponent();
            _ = BasicData.MsysDict;
            _ = BasicData.MSTMB_Dic;
            _ = BasicData.MCSRH_Dic;
            _ = BasicData.MCUMS_Dic;
            //BasicData.GetDBToXml();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtSearchContent.Clear();
            txtSearchResultContent.Clear();
            string strComboBoxTTYPE = comboBoxTTYPE.Text;
            if (String.IsNullOrEmpty(strComboBoxTTYPE))
                strComboBoxTTYPE = "";
            else
                strComboBoxTTYPE = comboBoxTTYPE.Text.Substring(0, 1);

            //未實現損益查詢
            if (comboBoxQTYPE.Text == "0001" && txtBHNO.Text.Length == 4 && txtCSEQ.Text.Length == 7)
            {
                GainLost gainLost = new GainLost();             //自訂GainLost類別   (ESMP.STOCK.TASK.API)
                //呈現查詢結果
                (txtSearchContent.Text,txtSearchResultContent.Text) = gainLost.getGainLostSearch(comboBoxQTYPE.Text, txtBHNO.Text, txtCSEQ.Text, txtStockSymbol.Text, strComboBoxTTYPE, _type);
            }
            //已實現損益查詢
            else if (comboBoxQTYPE.Text == "0002" && txtBHNO.Text.Length == 4 && txtCSEQ.Text.Length == 7 && txtEDATE.Text.Length == 8 && txtSDATE.Text.Length == 8)
            {
                GainPay gainPay = new GainPay();                //自訂GainPay類別   (ESMP.STOCK.TASK.API)
                //呈現查詢結果
                (txtSearchContent.Text, txtSearchResultContent.Text) = gainPay.getGainPaySearch(comboBoxQTYPE.Text, txtBHNO.Text, txtCSEQ.Text, txtSDATE.Text, txtEDATE.Text, txtStockSymbol.Text, strComboBoxTTYPE, _type);
            }
            //對帳單查詢
            else if (comboBoxQTYPE.Text == "0003" && txtBHNO.Text.Length == 4 && txtCSEQ.Text.Length == 7 && txtEDATE.Text.Length == 8 && txtSDATE.Text.Length == 8)
            {
                Bill bill = new Bill();                         //自訂Bill類別      (ESMP.STOCK.TASK.API)
                //呈現查詢結果
                (txtSearchContent.Text, txtSearchResultContent.Text) = bill.getBillSearch(comboBoxQTYPE.Text, txtBHNO.Text, txtCSEQ.Text, txtSDATE.Text, txtEDATE.Text, txtStockSymbol.Text, _type); 
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
                txtCSEQ.Text = "0107938";
                txtSDATE.Text = "";
                txtEDATE.Text = "";
                comboBoxTTYPE.Enabled = true;
                txtSDATE.Enabled = false;
                txtEDATE.Enabled = false;
                txtStockSymbol.Text = "";
            }
            if (comboBoxQTYPE.Text == "0002")
            {
                txtBHNO.Text = "5920";
                txtCSEQ.Text = "9813603";
                txtSDATE.Text = "20210101";
                txtEDATE.Text = "20221231";
                comboBoxTTYPE.Enabled = true;
                txtSDATE.Enabled = true;
                txtEDATE.Enabled = true;
                txtStockSymbol.Text = "";
            }
            if (comboBoxQTYPE.Text == "0003")
            {
                txtBHNO.Text = "592S";
                txtCSEQ.Text = "0105354";
                txtSDATE.Text = "20221001";
                txtEDATE.Text = "20221031";
                comboBoxTTYPE.Enabled = false;
                txtSDATE.Enabled = true;
                txtEDATE.Enabled = true;
                txtStockSymbol.Text = "3041";
            }
        }
    }
}
