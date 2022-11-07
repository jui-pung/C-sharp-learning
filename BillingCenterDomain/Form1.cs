﻿using ESMP.STOCK.DB.TABLE;
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
        int _type;                                           //查詢與回覆格式設定
        string _searchStr;                                   //查詢xml或json格式字串
        SqlSearch _sqlSearch;                                //自訂SqlSearch類別 (ESMP.STOCK.TASK.API)
        public Form1()
        {
            InitializeComponent();
            _ = BasicData.MsysDict;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtSearchContent.Clear();
            txtSearchResultContent.Clear();
            _sqlSearch = new SqlSearch();

            //未實現損益查詢
            if (comboBoxQTYPE.Text == "0001" && txtBHNO.Text.Length == 4 && txtCSEQ.Text.Length == 7)
            {
                GainLost gainLost = new GainLost();             //自訂GainLost類別   (ESMP.STOCK.TASK.API)
                //呈現查詢結果
                (txtSearchContent.Text,txtSearchResultContent.Text) = gainLost.getGainLostSearch(comboBoxQTYPE.Text, txtBHNO.Text, txtCSEQ.Text, txtStockSymbol.Text, _type);
            }
            //已實現損益查詢
            else if (comboBoxQTYPE.Text == "0002" && txtBHNO.Text.Length == 4 && txtCSEQ.Text.Length == 7 && txtEDATE.Text.Length == 8 && txtSDATE.Text.Length == 8)
            {
                GainPay gainPay = new GainPay();                //自訂GainPay類別   (ESMP.STOCK.TASK.API)
                //呈現查詢結果
                (txtSearchContent.Text, txtSearchResultContent.Text) = gainPay.getGainPaySearch(comboBoxQTYPE.Text, txtBHNO.Text, txtCSEQ.Text, txtSDATE.Text, txtEDATE.Text, txtStockSymbol.Text, _type);
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
