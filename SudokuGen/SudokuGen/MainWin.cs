using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Charlotte
{
	public partial class MainWin : Form
	{
		public MainWin()
		{
			InitializeComponent();

			this.MinimumSize = this.Size;

			this.lbKind.Items.Clear();
			foreach (ProblemInfo pi in Gnd.i.problemInfos)
			{
				this.lbKind.Items.Add(pi.getName());
			}
			this.lbKind.SelectedIndex = 0;
			this.lbKindChanged();

			this.txtDescription.ForeColor = new TextBox().ForeColor;
			this.txtDescription.BackColor = new TextBox().BackColor;

			this.cmbDifficulty.SelectedIndex = 0;
		}

		private void MainWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void MainWin_Shown(object sender, EventArgs e)
		{
			this.Height++; // フォームを最小化して元に戻したとき、lbKind のサイズがおかしくなる問題の対策
			this.Height--;

			// 設定復元
			{
				lbKind.SelectedIndex = Gnd.i.lastPIIndex;
				cmbDifficulty.SelectedIndex = Gnd.i.lastDifficulty;
			}
		}

		private void MainWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			// 設定保存
			{
				Gnd.i.lastPIIndex = lbKind.SelectedIndex;
				Gnd.i.lastDifficulty = cmbDifficulty.SelectedIndex;
			}
		}

		private void MainWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.Visible = false;

			// Programs.cs から移動してきた終了処理 ----->

			CancellableBusyDlg.perform(true, delegate
			{
				Gnd.i.cancellableBusyDlg.setTitle("終了しています...");

				Gnd.i.n2Listener.Dispose(); // n2Recver を使っているので、こっちを先に！
				Gnd.i.n2Listener = null;

				Gnd.i.n2Recver.Dispose();
				Gnd.i.n2Recver = null;
			});
		}

		private void 終了XToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void txtDescription_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)1) // ? ctrl_a
			{
				this.txtDescription.SelectAll();
				e.Handled = true;
			}
		}

		private void 設定SToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (SettingWin f = new SettingWin())
			{
				f.ShowDialog();
			}
			GC.Collect();
		}

		private void btnMkAndPrint_Click(object sender, EventArgs e)
		{
			ProblemInfo pi = Gnd.i.problemInfos[lbKind.SelectedIndex];
			int level = cmbDifficulty.SelectedIndex;

			Gnd.i.mkAndPrintWinParentWin = this;
			Gnd.i.mkSudokuProc = new MkSudokuProc(pi, level);

			using (MkAndPrintWin f = new MkAndPrintWin(pi, level))
			{
				f.ShowDialog();
			}
			if (Gnd.i.mkSudokuProc.isEnded() == false)
			{
				CancellableBusyDlg.perform(true, delegate
				{
					Gnd.i.cancellableBusyDlg.setTitle("問題作成プロセスを停止しています...");
					Gnd.i.mkSudokuProc.bCancel();
				});
			}
			Gnd.i.mkSudokuProc.Dispose();
			Gnd.i.mkSudokuProc = null;
			Gnd.i.mkAndPrintWinParentWin = null;

			HistoryDataUtils.trimHistories();

			GC.Collect();
		}

		private void lbKind_SelectedIndexChanged(object sender, EventArgs e)
		{
			lbKindChanged();
		}

		private void lbKindChanged()
		{
			ProblemInfo pi = Gnd.i.problemInfos[this.lbKind.SelectedIndex];

			this.txtDescription.Text = pi.getDescription();
			this.imgPreview.Image = pi.getPreview();

			GC.Collect();
		}

		private void 履歴RToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (HistoryDataUtils.getHistories().Length == 0)
			{
				MessageBox.Show(
					"過去に作成した問題はありません。",
					"続行できません",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
					);
				return;
			}
			this.Visible = false;

			using (HistoryWin f = new HistoryWin())
			{
				f.ShowDialog();
			}
			this.Visible = true; // restore

			GC.Collect();
		}

		private void 過去に作成した問題を全てクリアするCToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (HistoryDataUtils.getHistories().Length == 0)
			{
				MessageBox.Show(
					"過去に作成した問題はありません。",
					"続行できません",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning
					);
				return;
			}
			if (MessageBox.Show(
				"過去に作成した問題を全てクリアします。\n宜しいですか？",
				"確認",
				MessageBoxButtons.OKCancel,
				MessageBoxIcon.Warning
				) == DialogResult.OK
				)
			{
				HistoryDataUtils.clearHistories();

				MessageBox.Show(
					"クリアしました。",
					"情報",
					MessageBoxButtons.OK,
					MessageBoxIcon.Information
					);
			}
		}
	}
}
