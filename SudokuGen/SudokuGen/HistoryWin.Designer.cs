namespace Charlotte
{
	partial class HistoryWin
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HistoryWin));
			this.btnPrint = new System.Windows.Forms.Button();
			this.mainSheet = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.mainSheet)).BeginInit();
			this.SuspendLayout();
			// 
			// btnPrint
			// 
			this.btnPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnPrint.Font = new System.Drawing.Font("メイリオ", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.btnPrint.Location = new System.Drawing.Point(484, 453);
			this.btnPrint.Name = "btnPrint";
			this.btnPrint.Size = new System.Drawing.Size(124, 62);
			this.btnPrint.TabIndex = 3;
			this.btnPrint.Text = "印刷";
			this.btnPrint.UseVisualStyleBackColor = true;
			this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
			// 
			// mainSheet
			// 
			this.mainSheet.AllowUserToAddRows = false;
			this.mainSheet.AllowUserToDeleteRows = false;
			this.mainSheet.AllowUserToResizeRows = false;
			this.mainSheet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mainSheet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.mainSheet.Location = new System.Drawing.Point(12, 12);
			this.mainSheet.MultiSelect = false;
			this.mainSheet.Name = "mainSheet";
			this.mainSheet.ReadOnly = true;
			this.mainSheet.RowTemplate.Height = 21;
			this.mainSheet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.mainSheet.Size = new System.Drawing.Size(596, 435);
			this.mainSheet.TabIndex = 4;
			// 
			// HistoryWin
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(620, 527);
			this.Controls.Add(this.mainSheet);
			this.Controls.Add(this.btnPrint);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.Name = "HistoryWin";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "過去に作成した問題の一覧";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HistoryWin_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.HistoryWin_FormClosed);
			this.Load += new System.EventHandler(this.HistoryWin_Load);
			this.Shown += new System.EventHandler(this.HistoryWin_Shown);
			((System.ComponentModel.ISupportInitialize)(this.mainSheet)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnPrint;
		private System.Windows.Forms.DataGridView mainSheet;
	}
}
