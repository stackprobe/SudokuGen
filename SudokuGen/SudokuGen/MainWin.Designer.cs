namespace Charlotte
{
	partial class MainWin
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox4 = new System.Windows.Forms.GroupBox();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.imgPreview = new System.Windows.Forms.PictureBox();
			this.lbKind = new System.Windows.Forms.ListBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.アプリAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.終了XToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ツールTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.設定SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.履歴RToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.過去に作成した問題を全てクリアするCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.btnMkAndPrint = new System.Windows.Forms.Button();
			this.cmbDifficulty = new System.Windows.Forms.ComboBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.groupBox4.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.imgPreview)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.groupBox4);
			this.groupBox1.Controls.Add(this.groupBox2);
			this.groupBox1.Controls.Add(this.lbKind);
			this.groupBox1.Location = new System.Drawing.Point(12, 29);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(744, 416);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "問題の種類";
			// 
			// groupBox4
			// 
			this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox4.Controls.Add(this.txtDescription);
			this.groupBox4.Location = new System.Drawing.Point(278, 26);
			this.groupBox4.Name = "groupBox4";
			this.groupBox4.Size = new System.Drawing.Size(460, 98);
			this.groupBox4.TabIndex = 2;
			this.groupBox4.TabStop = false;
			this.groupBox4.Text = "説明";
			// 
			// txtDescription
			// 
			this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtDescription.Location = new System.Drawing.Point(6, 26);
			this.txtDescription.Multiline = true;
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ReadOnly = true;
			this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtDescription.Size = new System.Drawing.Size(448, 66);
			this.txtDescription.TabIndex = 0;
			this.txtDescription.Text = "準備しています... (1行目)\r\n準備しています... (2行目)\r\n準備しています... (3行目)";
			this.txtDescription.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDescription_KeyPress);
			// 
			// groupBox2
			// 
			this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox2.Controls.Add(this.imgPreview);
			this.groupBox2.Location = new System.Drawing.Point(278, 130);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(460, 280);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "サンプル・プレビュー";
			// 
			// imgPreview
			// 
			this.imgPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.imgPreview.Location = new System.Drawing.Point(6, 26);
			this.imgPreview.Name = "imgPreview";
			this.imgPreview.Size = new System.Drawing.Size(448, 248);
			this.imgPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.imgPreview.TabIndex = 0;
			this.imgPreview.TabStop = false;
			// 
			// lbKind
			// 
			this.lbKind.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.lbKind.FormattingEnabled = true;
			this.lbKind.ItemHeight = 20;
			this.lbKind.Items.AddRange(new object[] {
            "00_準備しています...",
            "01_普通のナンプレ (9x9)",
            "02_大きなナンプレ (256x256)",
            "03_対角線上にも数字が揃う問題 (9x9)"});
			this.lbKind.Location = new System.Drawing.Point(6, 26);
			this.lbKind.Name = "lbKind";
			this.lbKind.ScrollAlwaysVisible = true;
			this.lbKind.Size = new System.Drawing.Size(266, 384);
			this.lbKind.TabIndex = 0;
			this.lbKind.SelectedIndexChanged += new System.EventHandler(this.lbKind_SelectedIndexChanged);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.アプリAToolStripMenuItem,
            this.ツールTToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(768, 24);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// アプリAToolStripMenuItem
			// 
			this.アプリAToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.終了XToolStripMenuItem});
			this.アプリAToolStripMenuItem.Name = "アプリAToolStripMenuItem";
			this.アプリAToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.アプリAToolStripMenuItem.Text = "アプリ(&A)";
			// 
			// 終了XToolStripMenuItem
			// 
			this.終了XToolStripMenuItem.Name = "終了XToolStripMenuItem";
			this.終了XToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
			this.終了XToolStripMenuItem.Text = "終了(&X)";
			this.終了XToolStripMenuItem.Click += new System.EventHandler(this.終了XToolStripMenuItem_Click);
			// 
			// ツールTToolStripMenuItem
			// 
			this.ツールTToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.設定SToolStripMenuItem,
            this.toolStripMenuItem1,
            this.履歴RToolStripMenuItem,
            this.過去に作成した問題を全てクリアするCToolStripMenuItem});
			this.ツールTToolStripMenuItem.Name = "ツールTToolStripMenuItem";
			this.ツールTToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
			this.ツールTToolStripMenuItem.Text = "ツール(&T)";
			// 
			// 設定SToolStripMenuItem
			// 
			this.設定SToolStripMenuItem.Name = "設定SToolStripMenuItem";
			this.設定SToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
			this.設定SToolStripMenuItem.Text = "設定(&S)";
			this.設定SToolStripMenuItem.Click += new System.EventHandler(this.設定SToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(259, 6);
			// 
			// 履歴RToolStripMenuItem
			// 
			this.履歴RToolStripMenuItem.Name = "履歴RToolStripMenuItem";
			this.履歴RToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
			this.履歴RToolStripMenuItem.Text = "過去に作成した問題の一覧(&R)";
			this.履歴RToolStripMenuItem.Click += new System.EventHandler(this.履歴RToolStripMenuItem_Click);
			// 
			// 過去に作成した問題を全てクリアするCToolStripMenuItem
			// 
			this.過去に作成した問題を全てクリアするCToolStripMenuItem.Name = "過去に作成した問題を全てクリアするCToolStripMenuItem";
			this.過去に作成した問題を全てクリアするCToolStripMenuItem.Size = new System.Drawing.Size(262, 22);
			this.過去に作成した問題を全てクリアするCToolStripMenuItem.Text = "過去に作成した問題を全てクリアする(&C)";
			this.過去に作成した問題を全てクリアするCToolStripMenuItem.Click += new System.EventHandler(this.過去に作成した問題を全てクリアするCToolStripMenuItem_Click);
			// 
			// btnMkAndPrint
			// 
			this.btnMkAndPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMkAndPrint.Font = new System.Drawing.Font("メイリオ", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnMkAndPrint.Location = new System.Drawing.Point(497, 461);
			this.btnMkAndPrint.Name = "btnMkAndPrint";
			this.btnMkAndPrint.Size = new System.Drawing.Size(259, 62);
			this.btnMkAndPrint.TabIndex = 2;
			this.btnMkAndPrint.Text = "問題を作成して印刷";
			this.btnMkAndPrint.UseVisualStyleBackColor = true;
			this.btnMkAndPrint.Click += new System.EventHandler(this.btnMkAndPrint_Click);
			// 
			// cmbDifficulty
			// 
			this.cmbDifficulty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.cmbDifficulty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cmbDifficulty.FormattingEnabled = true;
			this.cmbDifficulty.Items.AddRange(new object[] {
            "普通",
            "普通＋",
            "難しい",
            "とても難しい"});
			this.cmbDifficulty.Location = new System.Drawing.Point(6, 31);
			this.cmbDifficulty.Name = "cmbDifficulty";
			this.cmbDifficulty.Size = new System.Drawing.Size(260, 28);
			this.cmbDifficulty.TabIndex = 3;
			// 
			// groupBox3
			// 
			this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.groupBox3.Controls.Add(this.cmbDifficulty);
			this.groupBox3.Location = new System.Drawing.Point(12, 451);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(272, 73);
			this.groupBox3.TabIndex = 4;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "問題の難易度";
			// 
			// MainWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(768, 535);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.btnMkAndPrint);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.menuStrip1);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.menuStrip1;
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "MainWin";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "いろいろなナンプレの問題を作成して印刷するプログラム";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWin_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWin_FormClosed);
			this.Load += new System.EventHandler(this.MainWin_Load);
			this.Shown += new System.EventHandler(this.MainWin_Shown);
			this.groupBox1.ResumeLayout(false);
			this.groupBox4.ResumeLayout(false);
			this.groupBox4.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.imgPreview)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem アプリAToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 終了XToolStripMenuItem;
		private System.Windows.Forms.ListBox lbKind;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.PictureBox imgPreview;
		private System.Windows.Forms.Button btnMkAndPrint;
		private System.Windows.Forms.ComboBox cmbDifficulty;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.ToolStripMenuItem ツールTToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 履歴RToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem 設定SToolStripMenuItem;
		private System.Windows.Forms.GroupBox groupBox4;
		private System.Windows.Forms.TextBox txtDescription;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem 過去に作成した問題を全てクリアするCToolStripMenuItem;
	}
}

