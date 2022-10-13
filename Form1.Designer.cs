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
            this.SuspendLayout();
            // 
            // txtBHNO
            // 
            this.txtBHNO.Location = new System.Drawing.Point(143, 34);
            this.txtBHNO.Name = "txtBHNO";
            this.txtBHNO.Size = new System.Drawing.Size(124, 36);
            this.txtBHNO.TabIndex = 0;
            this.txtBHNO.Text = "592S";
            // 
            // txtCSEQ
            // 
            this.txtCSEQ.Location = new System.Drawing.Point(143, 107);
            this.txtCSEQ.Name = "txtCSEQ";
            this.txtCSEQ.Size = new System.Drawing.Size(124, 36);
            this.txtCSEQ.TabIndex = 1;
            this.txtCSEQ.Text = "0000527";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "BHNO :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "CSEQ :";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(120, 185);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(124, 45);
            this.btnSearch.TabIndex = 4;
            this.btnSearch.Text = "search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // txtSearchContent
            // 
            this.txtSearchContent.Location = new System.Drawing.Point(38, 259);
            this.txtSearchContent.Multiline = true;
            this.txtSearchContent.Name = "txtSearchContent";
            this.txtSearchContent.Size = new System.Drawing.Size(940, 356);
            this.txtSearchContent.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1020, 643);
            this.Controls.Add(this.txtSearchContent);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtCSEQ);
            this.Controls.Add(this.txtBHNO);
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
    }
}

