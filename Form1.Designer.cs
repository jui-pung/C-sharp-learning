namespace _1_UnrealizedGainsOrLosses
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
            this.txtBHNO = new System.Windows.Forms.TextBox();
            this.txtCSEQ = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.txtSearchContent = new System.Windows.Forms.TextBox();
            this.txtSearchResultContent = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtBHNO
            // 
            this.txtBHNO.Location = new System.Drawing.Point(143, 34);
            this.txtBHNO.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtBHNO.Name = "txtBHNO";
            this.txtBHNO.Size = new System.Drawing.Size(125, 36);
            this.txtBHNO.TabIndex = 0;
            this.txtBHNO.Text = "592S";
            // 
            // txtCSEQ
            // 
            this.txtCSEQ.Location = new System.Drawing.Point(143, 108);
            this.txtCSEQ.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtCSEQ.Name = "txtCSEQ";
            this.txtCSEQ.Size = new System.Drawing.Size(125, 36);
            this.txtCSEQ.TabIndex = 1;
            this.txtCSEQ.Text = "0000527";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "BHNO :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 118);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "CSEQ :";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(143, 177);
            this.btnSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(124, 44);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearchContent
            // 
            this.txtSearchContent.Location = new System.Drawing.Point(311, 75);
            this.txtSearchContent.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSearchContent.Multiline = true;
            this.txtSearchContent.Name = "txtSearchContent";
            this.txtSearchContent.Size = new System.Drawing.Size(751, 149);
            this.txtSearchContent.TabIndex = 5;
            // 
            // txtSearchResultContent
            // 
            this.txtSearchResultContent.Font = new System.Drawing.Font("微軟正黑體", 10.875F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtSearchResultContent.Location = new System.Drawing.Point(39, 290);
            this.txtSearchResultContent.Margin = new System.Windows.Forms.Padding(4);
            this.txtSearchResultContent.Multiline = true;
            this.txtSearchResultContent.Name = "txtSearchResultContent";
            this.txtSearchResultContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSearchResultContent.Size = new System.Drawing.Size(1023, 360);
            this.txtSearchResultContent.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(306, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(160, 24);
            this.label3.TabIndex = 7;
            this.label3.Text = "Search Content :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 252);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(223, 24);
            this.label4.TabIndex = 8;
            this.label4.Text = "Search Result Content :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1105, 684);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtSearchResultContent);
            this.Controls.Add(this.txtSearchContent);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCSEQ);
            this.Controls.Add(this.txtBHNO);
            this.Font = new System.Drawing.Font("新細明體", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtBHNO;
        private System.Windows.Forms.TextBox txtCSEQ;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.TextBox txtSearchContent;
        private System.Windows.Forms.TextBox txtSearchResultContent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}

