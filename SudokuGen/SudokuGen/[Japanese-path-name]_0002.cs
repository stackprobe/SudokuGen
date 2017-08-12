namespace Charlotte
{
	partial class Setting詳細Win
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Setting詳細Win));
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.cb合体ナンプレの重なった部分に成丈数字を置かない = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(287, 67);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(101, 36);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(394, 67);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(101, 36);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "キャンセル";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// cb合体ナンプレの重なった部分に成丈数字を置かない
			// 
			this.cb合体ナンプレの重なった部分に成丈数字を置かない.AutoSize = true;
			this.cb合体ナンプレの重なった部分に成丈数字を置かない.Location = new System.Drawing.Point(25, 24);
			this.cb合体ナンプレの重なった部分に成丈数字を置かない.Name = "cb合体ナンプレの重なった部分に成丈数字を置かない";
			this.cb合体ナンプレの重なった部分に成丈数字を置かない.Size = new System.Drawing.Size(470, 24);
			this.cb合体ナンプレの重なった部分に成丈数字を置かない.TabIndex = 0;
			this.cb合体ナンプレの重なった部分に成丈数字を置かない.Text = "合体ナンプレの重なった部分や、条件のある升になるべく数字を置かない。";
			this.cb合体ナンプレの重なった部分に成丈数字を置かない.UseVisualStyleBackColor = true;
			// 
			// Setting詳細Win
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(507, 115);
			this.Controls.Add(this.cb合体ナンプレの重なった部分に成丈数字を置かない);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "Setting詳細Win";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "詳細";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Setting詳細Win_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Setting詳細Win_FormClosed);
			this.Load += new System.EventHandler(this.Setting詳細Win_Load);
			this.Shown += new System.EventHandler(this.Setting詳細Win_Shown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.CheckBox cb合体ナンプレの重なった部分に成丈数字を置かない;
	}
}
