namespace Charlotte
{
	partial class MkAndPrintWin
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MkAndPrintWin));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.cbUseDefaultPrinter = new System.Windows.Forms.CheckBox();
			this.cmbPrinterName = new System.Windows.Forms.ComboBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.cbUseDefaultPaperSize = new System.Windows.Forms.CheckBox();
			this.cmbPaperSizeName = new System.Windows.Forms.ComboBox();
			this.cbPrintProblem = new System.Windows.Forms.CheckBox();
			this.cbPrintAnswer = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.pbMk = new System.Windows.Forms.ProgressBar();
			this.lblMkStatus = new System.Windows.Forms.Label();
			this.btnMkAndPrint = new System.Windows.Forms.Button();
			this.lblProblemTitle = new System.Windows.Forms.Label();
			this.mainTimer = new System.Windows.Forms.Timer(this.components);
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.marginB = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.marginR = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.marginT = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.marginL = new System.Windows.Forms.TextBox();
			this.cbUseDefaultMargin = new System.Windows.Forms.CheckBox();
			this.btnPreview = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.cbUseDefaultPrinter);
			this.groupBox1.Controls.Add(this.cmbPrinterName);
			this.groupBox1.Location = new System.Drawing.Point(12, 51);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(577, 99);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "使用するプリンタ";
			// 
			// cbUseDefaultPrinter
			// 
			this.cbUseDefaultPrinter.AutoSize = true;
			this.cbUseDefaultPrinter.Checked = true;
			this.cbUseDefaultPrinter.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbUseDefaultPrinter.Location = new System.Drawing.Point(26, 65);
			this.cbUseDefaultPrinter.Name = "cbUseDefaultPrinter";
			this.cbUseDefaultPrinter.Size = new System.Drawing.Size(236, 24);
			this.cbUseDefaultPrinter.TabIndex = 1;
			this.cbUseDefaultPrinter.Text = "デフォルトのプリンタを使用する。";
			this.cbUseDefaultPrinter.UseVisualStyleBackColor = true;
			this.cbUseDefaultPrinter.CheckedChanged += new System.EventHandler(this.cbUseDefaultPrinter_CheckedChanged);
			// 
			// cmbPrinterName
			// 
			this.cmbPrinterName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbPrinterName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPrinterName.FormattingEnabled = true;
			this.cmbPrinterName.Location = new System.Drawing.Point(18, 28);
			this.cmbPrinterName.Name = "cmbPrinterName";
			this.cmbPrinterName.Size = new System.Drawing.Size(543, 28);
			this.cmbPrinterName.TabIndex = 0;
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.cbUseDefaultPaperSize);
			this.groupBox2.Controls.Add(this.cmbPaperSizeName);
			this.groupBox2.Location = new System.Drawing.Point(12, 156);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(577, 99);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "用紙サイズ";
			// 
			// cbUseDefaultPaperSize
			// 
			this.cbUseDefaultPaperSize.AutoSize = true;
			this.cbUseDefaultPaperSize.Checked = true;
			this.cbUseDefaultPaperSize.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbUseDefaultPaperSize.Location = new System.Drawing.Point(26, 65);
			this.cbUseDefaultPaperSize.Name = "cbUseDefaultPaperSize";
			this.cbUseDefaultPaperSize.Size = new System.Drawing.Size(249, 24);
			this.cbUseDefaultPaperSize.TabIndex = 1;
			this.cbUseDefaultPaperSize.Text = "デフォルトの用紙サイズで印刷する。";
			this.cbUseDefaultPaperSize.UseVisualStyleBackColor = true;
			this.cbUseDefaultPaperSize.CheckedChanged += new System.EventHandler(this.cbUseDefaultPaperSize_CheckedChanged);
			// 
			// cmbPaperSizeName
			// 
			this.cmbPaperSizeName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbPaperSizeName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbPaperSizeName.FormattingEnabled = true;
			this.cmbPaperSizeName.Location = new System.Drawing.Point(18, 28);
			this.cmbPaperSizeName.Name = "cmbPaperSizeName";
			this.cmbPaperSizeName.Size = new System.Drawing.Size(543, 28);
			this.cmbPaperSizeName.TabIndex = 0;
			// 
			// cbPrintProblem
			// 
			this.cbPrintProblem.AutoSize = true;
			this.cbPrintProblem.Checked = true;
			this.cbPrintProblem.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbPrintProblem.Location = new System.Drawing.Point(38, 387);
			this.cbPrintProblem.Name = "cbPrintProblem";
			this.cbPrintProblem.Size = new System.Drawing.Size(132, 24);
			this.cbPrintProblem.TabIndex = 4;
			this.cbPrintProblem.Text = "問題を印刷する。";
			this.cbPrintProblem.UseVisualStyleBackColor = true;
			this.cbPrintProblem.CheckedChanged += new System.EventHandler(this.cbPrintProblem_CheckedChanged);
			// 
			// cbPrintAnswer
			// 
			this.cbPrintAnswer.AutoSize = true;
			this.cbPrintAnswer.Location = new System.Drawing.Point(38, 426);
			this.cbPrintAnswer.Name = "cbPrintAnswer";
			this.cbPrintAnswer.Size = new System.Drawing.Size(132, 24);
			this.cbPrintAnswer.TabIndex = 5;
			this.cbPrintAnswer.Text = "解答を印刷する。";
			this.cbPrintAnswer.UseVisualStyleBackColor = true;
			this.cbPrintAnswer.CheckedChanged += new System.EventHandler(this.cbPrintAnswer_CheckedChanged);
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox3.Controls.Add(this.pbMk);
			this.groupBox3.Controls.Add(this.lblMkStatus);
			this.groupBox3.Location = new System.Drawing.Point(203, 366);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(386, 100);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "問題の作成状況";
			// 
			// pbMk
			// 
			this.pbMk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pbMk.Location = new System.Drawing.Point(18, 26);
			this.pbMk.Maximum = 1000000000;
			this.pbMk.Name = "pbMk";
			this.pbMk.Size = new System.Drawing.Size(352, 25);
			this.pbMk.TabIndex = 0;
			// 
			// lblMkStatus
			// 
			this.lblMkStatus.AutoSize = true;
			this.lblMkStatus.Location = new System.Drawing.Point(22, 64);
			this.lblMkStatus.Name = "lblMkStatus";
			this.lblMkStatus.Size = new System.Drawing.Size(115, 20);
			this.lblMkStatus.TabIndex = 1;
			this.lblMkStatus.Text = "準備しています...";
			// 
			// btnMkAndPrint
			// 
			this.btnMkAndPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMkAndPrint.Font = new System.Drawing.Font("メイリオ", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnMkAndPrint.Location = new System.Drawing.Point(459, 472);
			this.btnMkAndPrint.Name = "btnMkAndPrint";
			this.btnMkAndPrint.Size = new System.Drawing.Size(130, 62);
			this.btnMkAndPrint.TabIndex = 7;
			this.btnMkAndPrint.Text = "印刷";
			this.btnMkAndPrint.UseVisualStyleBackColor = true;
			this.btnMkAndPrint.Click += new System.EventHandler(this.btnMkAndPrint_Click);
			// 
			// lblProblemTitle
			// 
			this.lblProblemTitle.AutoSize = true;
			this.lblProblemTitle.Location = new System.Drawing.Point(26, 18);
			this.lblProblemTitle.Name = "lblProblemTitle";
			this.lblProblemTitle.Size = new System.Drawing.Size(358, 20);
			this.lblProblemTitle.TabIndex = 0;
			this.lblProblemTitle.Text = "印刷する問題：01_普通のナンプレ (9x9) , 難易度：普通";
			// 
			// mainTimer
			// 
			this.mainTimer.Enabled = true;
			this.mainTimer.Tick += new System.EventHandler(this.mainTimer_Tick);
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.label5);
			this.groupBox4.Controls.Add(this.label4);
			this.groupBox4.Controls.Add(this.marginB);
			this.groupBox4.Controls.Add(this.label3);
			this.groupBox4.Controls.Add(this.marginR);
			this.groupBox4.Controls.Add(this.label2);
			this.groupBox4.Controls.Add(this.marginT);
			this.groupBox4.Controls.Add(this.label1);
			this.groupBox4.Controls.Add(this.marginL);
			this.groupBox4.Controls.Add(this.cbUseDefaultMargin);
			this.groupBox4.Location = new System.Drawing.Point(12, 261);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(577, 99);
			this.groupBox4.TabIndex = 3;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "余白";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.label5.Location = new System.Drawing.Point(381, 66);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(180, 20);
			this.label5.TabIndex = 9;
			this.label5.Text = "単位は 1 / 100 インチです。";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(425, 29);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(35, 20);
			this.label4.TabIndex = 6;
			this.label4.Text = "下：";
			// 
			// marginB
			// 
			this.marginB.Location = new System.Drawing.Point(466, 26);
			this.marginB.MaxLength = 4;
			this.marginB.Name = "marginB";
			this.marginB.Size = new System.Drawing.Size(68, 27);
			this.marginB.TabIndex = 7;
			this.marginB.Text = "9999";
			this.marginB.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(288, 29);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(35, 20);
			this.label3.TabIndex = 4;
			this.label3.Text = "右：";
			// 
			// marginR
			// 
			this.marginR.Location = new System.Drawing.Point(329, 26);
			this.marginR.MaxLength = 4;
			this.marginR.Name = "marginR";
			this.marginR.Size = new System.Drawing.Size(68, 27);
			this.marginR.TabIndex = 5;
			this.marginR.Text = "9999";
			this.marginR.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(151, 29);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(35, 20);
			this.label2.TabIndex = 2;
			this.label2.Text = "上：";
			// 
			// marginT
			// 
			this.marginT.Location = new System.Drawing.Point(192, 26);
			this.marginT.MaxLength = 4;
			this.marginT.Name = "marginT";
			this.marginT.Size = new System.Drawing.Size(68, 27);
			this.marginT.TabIndex = 3;
			this.marginT.Text = "9999";
			this.marginT.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(14, 29);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "左：";
			// 
			// marginL
			// 
			this.marginL.Location = new System.Drawing.Point(55, 26);
			this.marginL.MaxLength = 4;
			this.marginL.Name = "marginL";
			this.marginL.Size = new System.Drawing.Size(68, 27);
			this.marginL.TabIndex = 1;
			this.marginL.Text = "9999";
			this.marginL.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// cbUseDefaultMargin
			// 
			this.cbUseDefaultMargin.AutoSize = true;
			this.cbUseDefaultMargin.Checked = true;
			this.cbUseDefaultMargin.CheckState = System.Windows.Forms.CheckState.Checked;
			this.cbUseDefaultMargin.Location = new System.Drawing.Point(26, 65);
			this.cbUseDefaultMargin.Name = "cbUseDefaultMargin";
			this.cbUseDefaultMargin.Size = new System.Drawing.Size(210, 24);
			this.cbUseDefaultMargin.TabIndex = 8;
			this.cbUseDefaultMargin.Text = "デフォルトの余白で印刷する。";
			this.cbUseDefaultMargin.UseVisualStyleBackColor = true;
			this.cbUseDefaultMargin.CheckedChanged += new System.EventHandler(this.cbUseDefaultMargin_CheckedChanged);
			// 
			// btnPreview
			// 
			this.btnPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPreview.Enabled = false;
			this.btnPreview.Font = new System.Drawing.Font("メイリオ", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnPreview.Location = new System.Drawing.Point(294, 472);
			this.btnPreview.Name = "btnPreview";
			this.btnPreview.Size = new System.Drawing.Size(159, 62);
			this.btnPreview.TabIndex = 8;
			this.btnPreview.Text = "プレビュー";
			this.btnPreview.UseVisualStyleBackColor = true;
			this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
			// 
			// MkAndPrintWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(601, 546);
			this.Controls.Add(this.btnPreview);
			this.Controls.Add(this.groupBox4);
			this.Controls.Add(this.lblProblemTitle);
			this.Controls.Add(this.btnMkAndPrint);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.cbPrintAnswer);
			this.Controls.Add(this.cbPrintProblem);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MkAndPrintWin";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "印刷";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MkAndPrintWin_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MkAndPrintWin_FormClosed);
			this.Load += new System.EventHandler(this.MkAndPrintWin_Load);
			this.Shown += new System.EventHandler(this.MkAndPrintWin_Shown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.CheckBox cbUseDefaultPrinter;
		private System.Windows.Forms.ComboBox cmbPrinterName;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox cbUseDefaultPaperSize;
		private System.Windows.Forms.ComboBox cmbPaperSizeName;
		private System.Windows.Forms.CheckBox cbPrintProblem;
		private System.Windows.Forms.CheckBox cbPrintAnswer;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ProgressBar pbMk;
		private System.Windows.Forms.Label lblMkStatus;
		private System.Windows.Forms.Button btnMkAndPrint;
		private System.Windows.Forms.Label lblProblemTitle;
		private System.Windows.Forms.Timer mainTimer;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.TextBox marginL;
		private System.Windows.Forms.CheckBox cbUseDefaultMargin;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox marginB;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox marginR;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox marginT;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnPreview;
		private System.Windows.Forms.Label label5;
	}
}
