namespace BillingCenterDomain
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.comboBoxQTYPE = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtEDATE = new System.Windows.Forms.TextBox();
            this.txtSDATE = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtStockSymbol = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCSEQ = new System.Windows.Forms.TextBox();
            this.txtBHNO = new System.Windows.Forms.TextBox();
            this.radioBtnJson = new System.Windows.Forms.RadioButton();
            this.radioBtnXml = new System.Windows.Forms.RadioButton();
            this.btnSearch = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtSearchResultContent = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.txtSearchContent = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // comboBoxQTYPE
            // 
            this.comboBoxQTYPE.FormattingEnabled = true;
            this.comboBoxQTYPE.Items.AddRange(new object[] {
            "0001",
            "0002",
            "0003"});
            this.comboBoxQTYPE.Location = new System.Drawing.Point(72, 22);
            this.comboBoxQTYPE.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxQTYPE.Name = "comboBoxQTYPE";
            this.comboBoxQTYPE.Size = new System.Drawing.Size(58, 20);
            this.comboBoxQTYPE.TabIndex = 32;
            this.comboBoxQTYPE.SelectedIndexChanged += new System.EventHandler(this.comboBoxQTYPE_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(244, 58);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 12);
            this.label5.TabIndex = 31;
            this.label5.Text = "EDATE :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(135, 58);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(47, 12);
            this.label6.TabIndex = 30;
            this.label6.Text = "SDATE :";
            // 
            // txtEDATE
            // 
            this.txtEDATE.Location = new System.Drawing.Point(292, 54);
            this.txtEDATE.Margin = new System.Windows.Forms.Padding(2);
            this.txtEDATE.Name = "txtEDATE";
            this.txtEDATE.Size = new System.Drawing.Size(60, 22);
            this.txtEDATE.TabIndex = 29;
            // 
            // txtSDATE
            // 
            this.txtSDATE.Location = new System.Drawing.Point(183, 54);
            this.txtSDATE.Margin = new System.Windows.Forms.Padding(2);
            this.txtSDATE.Name = "txtSDATE";
            this.txtSDATE.Size = new System.Drawing.Size(60, 22);
            this.txtSDATE.TabIndex = 28;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(358, 26);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 12);
            this.label4.TabIndex = 27;
            this.label4.Text = "stockSymbol:";
            // 
            // txtStockSymbol
            // 
            this.txtStockSymbol.Location = new System.Drawing.Point(425, 22);
            this.txtStockSymbol.Margin = new System.Windows.Forms.Padding(2);
            this.txtStockSymbol.Name = "txtStockSymbol";
            this.txtStockSymbol.Size = new System.Drawing.Size(60, 22);
            this.txtStockSymbol.TabIndex = 26;
            this.txtStockSymbol.Text = " ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 26);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 25;
            this.label3.Text = "QTYPE :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(244, 26);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 12);
            this.label2.TabIndex = 24;
            this.label2.Text = "CSEQ :";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(135, 26);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 12);
            this.label1.TabIndex = 23;
            this.label1.Text = "BHNO :";
            // 
            // txtCSEQ
            // 
            this.txtCSEQ.Location = new System.Drawing.Point(292, 22);
            this.txtCSEQ.Margin = new System.Windows.Forms.Padding(2);
            this.txtCSEQ.Name = "txtCSEQ";
            this.txtCSEQ.Size = new System.Drawing.Size(60, 22);
            this.txtCSEQ.TabIndex = 22;
            // 
            // txtBHNO
            // 
            this.txtBHNO.Location = new System.Drawing.Point(183, 22);
            this.txtBHNO.Margin = new System.Windows.Forms.Padding(2);
            this.txtBHNO.Name = "txtBHNO";
            this.txtBHNO.Size = new System.Drawing.Size(60, 22);
            this.txtBHNO.TabIndex = 21;
            // 
            // radioBtnJson
            // 
            this.radioBtnJson.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioBtnJson.AutoSize = true;
            this.radioBtnJson.Font = new System.Drawing.Font("新細明體", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioBtnJson.Location = new System.Drawing.Point(375, 90);
            this.radioBtnJson.Margin = new System.Windows.Forms.Padding(2);
            this.radioBtnJson.Name = "radioBtnJson";
            this.radioBtnJson.Size = new System.Drawing.Size(47, 15);
            this.radioBtnJson.TabIndex = 35;
            this.radioBtnJson.TabStop = true;
            this.radioBtnJson.Text = "JSON";
            this.radioBtnJson.UseVisualStyleBackColor = true;
            this.radioBtnJson.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioBtnXml
            // 
            this.radioBtnXml.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.radioBtnXml.AutoSize = true;
            this.radioBtnXml.Checked = true;
            this.radioBtnXml.Font = new System.Drawing.Font("新細明體", 7.875F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.radioBtnXml.Location = new System.Drawing.Point(334, 90);
            this.radioBtnXml.Margin = new System.Windows.Forms.Padding(2);
            this.radioBtnXml.Name = "radioBtnXml";
            this.radioBtnXml.Size = new System.Drawing.Size(45, 15);
            this.radioBtnXml.TabIndex = 34;
            this.radioBtnXml.TabStop = true;
            this.radioBtnXml.Text = "XML";
            this.radioBtnXml.UseVisualStyleBackColor = true;
            this.radioBtnXml.CheckedChanged += new System.EventHandler(this.radioBtnXml_CheckedChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.Location = new System.Drawing.Point(425, 82);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(2);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(57, 22);
            this.btnSearch.TabIndex = 33;
            this.btnSearch.Text = "search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(20, 120);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(472, 291);
            this.tabControl1.TabIndex = 36;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtSearchResultContent);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(464, 265);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Search Result Content   ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtSearchResultContent
            // 
            this.txtSearchResultContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchResultContent.Font = new System.Drawing.Font("微軟正黑體", 10.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtSearchResultContent.Location = new System.Drawing.Point(2, 4);
            this.txtSearchResultContent.Margin = new System.Windows.Forms.Padding(2);
            this.txtSearchResultContent.Multiline = true;
            this.txtSearchResultContent.Name = "txtSearchResultContent";
            this.txtSearchResultContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSearchResultContent.Size = new System.Drawing.Size(458, 257);
            this.txtSearchResultContent.TabIndex = 6;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtSearchContent);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(464, 265);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Search Content   ";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // txtSearchContent
            // 
            this.txtSearchContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearchContent.Font = new System.Drawing.Font("微軟正黑體", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtSearchContent.Location = new System.Drawing.Point(2, 2);
            this.txtSearchContent.Margin = new System.Windows.Forms.Padding(2);
            this.txtSearchContent.Multiline = true;
            this.txtSearchContent.Name = "txtSearchContent";
            this.txtSearchContent.Size = new System.Drawing.Size(463, 266);
            this.txtSearchContent.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 426);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.radioBtnJson);
            this.Controls.Add(this.radioBtnXml);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.comboBoxQTYPE);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtEDATE);
            this.Controls.Add(this.txtSDATE);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtStockSymbol);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCSEQ);
            this.Controls.Add(this.txtBHNO);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxQTYPE;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtEDATE;
        private System.Windows.Forms.TextBox txtSDATE;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtStockSymbol;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCSEQ;
        private System.Windows.Forms.TextBox txtBHNO;
        private System.Windows.Forms.RadioButton radioBtnJson;
        private System.Windows.Forms.RadioButton radioBtnXml;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txtSearchResultContent;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtSearchContent;
    }
}

