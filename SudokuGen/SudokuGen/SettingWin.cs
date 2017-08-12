using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Charlotte
{
	public partial class SettingWin : Form
	{
		public SettingWin()
		{
			InitializeComponent();
		}

		private void SettingWin_Load(object sender, EventArgs e)
		{
			this.pnlLv1Color.BackColor = Gnd.i.lv1Color;
			this.pnlLv2Color.BackColor = Gnd.i.lv2Color;
			this.pnlLv3Color.BackColor = Gnd.i.lv3Color;
			this.pnlLv4Color.BackColor = Gnd.i.lv4Color;
			this.pnlAnsColor.BackColor = Gnd.i.ansColor;
		}

		private void SettingWin_Shown(object sender, EventArgs e)
		{
			// noop
		}

		private void SettingWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void SettingWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			// noop
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			Gnd.i.lv1Color = pnlLv1Color.BackColor;
			Gnd.i.lv2Color = pnlLv2Color.BackColor;
			Gnd.i.lv3Color = pnlLv3Color.BackColor;
			Gnd.i.lv4Color = pnlLv4Color.BackColor;
			Gnd.i.ansColor = pnlAnsColor.BackColor;
			this.Close();
		}

		private void btnLv1Color_Click(object sender, EventArgs e)
		{
			editColor(pnlLv1Color);
		}

		private void btnLv2Color_Click(object sender, EventArgs e)
		{
			editColor(pnlLv2Color);
		}

		private void btnLv3Color_Click(object sender, EventArgs e)
		{
			editColor(pnlLv3Color);
		}

		private void btnLv4Color_Click(object sender, EventArgs e)
		{
			editColor(pnlLv4Color);
		}

		private void btnAnsColor_Click(object sender, EventArgs e)
		{
			editColor(pnlAnsColor);
		}

		private void editColor(Panel pnl)
		{
			//ColorDialogクラスのインスタンスを作成
			using (ColorDialog cd = new ColorDialog())
			{
				//はじめに選択されている色を設定
				cd.Color = pnl.BackColor;
				//色の作成部分を表示可能にする
				//デフォルトがTrueのため必要はない
				cd.AllowFullOpen = true;
				//純色だけに制限しない
				//デフォルトがFalseのため必要はない
				cd.SolidColorOnly = false;
				//[作成した色]に指定した色（RGB値）を表示する
				cd.CustomColors = new int[]
				{
					0x33, 0x66, 0x99, 0xCC, 0x3300, 0x3333,
					0x3366, 0x3399, 0x33CC, 0x6600, 0x6633,
					0x6666, 0x6699, 0x66CC, 0x9900, 0x9933,
				};

				//ダイアログを表示する
				if (cd.ShowDialog() == DialogResult.OK)
				{
					//選択された色の取得
					pnl.BackColor = cd.Color;
				}
			}
		}

		private void btn詳細_Click(object sender, EventArgs e)
		{
			using (Setting詳細Win f = new Setting詳細Win())
			{
				f.ShowDialog();
			}
		}
	}
}
