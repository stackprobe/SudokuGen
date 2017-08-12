using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Charlotte.Tools;

namespace Charlotte
{
	public partial class MkAndPrintWin : Form
	{
		public MkAndPrintWin(ProblemInfo problemInfo, int level)
			: this(problemInfo, level, null)
		{ }

		public MkAndPrintWin(HistoryData historyData)
			: this(historyData.getProblemInfo(), historyData.getLevel(), historyData)
		{ }

		private ProblemInfo _problemInfo;
		private int _level;
		private HistoryData _historyData; // null == 作成未完了

		private MkAndPrintWin(ProblemInfo problemInfo, int level, HistoryData historyData)
		{
			_problemInfo = problemInfo;
			_level = level;
			_historyData = historyData;

			InitializeComponent();

			this.lblProblemTitle.Text = "印刷する問題：" + _problemInfo.getName() + " , 難易度：" + Utils.levelToString(_level);
			this.lblMkStatus.Text = "作成中...";

			this.cmbPrinterName.Items.Clear();
			foreach (string name in ImagePrinter.getPrinterNames())
			{
				this.cmbPrinterName.Items.Add(name);
			}
			this.cmbPrinterName.SelectedIndex = 0;

			this.cmbPaperSizeName.Items.Clear();
			foreach (string name in ImagePrinter.getPaperSizeNames())
			{
				this.cmbPaperSizeName.Items.Add(name);
			}
			this.cmbPaperSizeName.SelectedIndex = 0;

			// 前回のプリンタ選択を復元
			{
				string name = Gnd.i.lastPrinterName;

				if (name != "")
				{
					int index = Utils.getIndex(cmbPrinterName, name);

					if (index != -1)
					{
						cmbPrinterName.SelectedIndex = index;
						cbUseDefaultPrinter.Checked = false;
					}
				}
			}
			// 前回の用紙サイズ選択を復元
			{
				string name = Gnd.i.lastPaperSizeName;

				if (name != "")
				{
					int index = Utils.getIndex(cmbPaperSizeName, name);

					if (index != -1)
					{
						cmbPaperSizeName.SelectedIndex = index;
						cbUseDefaultPaperSize.Checked = false;
					}
				}
			}
			// 前回のマージンを復元
			{
				LTRB margin = Gnd.i.lastMargin;

				if (margin != null)
				{
					marginL.Text = "" + margin.l;
					marginT.Text = "" + margin.t;
					marginR.Text = "" + margin.r;
					marginB.Text = "" + margin.b;
					cbUseDefaultMargin.Checked = false;
				}
				else
				{
					marginL.Text = "" + 100;
					marginT.Text = "" + 100;
					marginR.Text = "" + 100;
					marginB.Text = "" + 100;
				}
			}

			cbPrintProblem.Checked = Gnd.i.lastPrintProblem;
			cbPrintAnswer.Checked = Gnd.i.lastPrintAnswer;

			refreshUI();

			if (_historyData != null)
			{
				historyDataCreated();
			}
		}

		private void refreshUI()
		{
			this.cmbPrinterName.Enabled = this.cbUseDefaultPrinter.Checked == false;
			this.cmbPaperSizeName.Enabled = this.cbUseDefaultPaperSize.Checked == false;

			{
				bool f = this.cbUseDefaultMargin.Checked == false;

				this.marginL.Enabled = f;
				this.marginT.Enabled = f;
				this.marginR.Enabled = f;
				this.marginB.Enabled = f;
			}

			{
				bool f = this.cbPrintProblem.Checked || this.cbPrintAnswer.Checked;

				this.btnMkAndPrint.Enabled = f;
			}
		}

		private void historyDataCreated()
		{
			this.pbMk.Value = this.pbMk.Maximum;
			this.lblMkStatus.Text = "作成完了";
			this.btnPreview.Enabled = true;
		}

		private void MkAndPrintWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void MkAndPrintWin_Shown(object sender, EventArgs e)
		{
			this.mtEnabled = true;
		}

		private void MkAndPrintWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			// プリンタ_保存
			{
				if (cbUseDefaultPrinter.Checked)
					Gnd.i.lastPrinterName = "";
				else
					Gnd.i.lastPrinterName = cmbPrinterName.SelectedItem.ToString();
			}
			// 用紙サイズ_保存
			{
				if (cbUseDefaultPaperSize.Checked)
					Gnd.i.lastPaperSizeName = "";
				else
					Gnd.i.lastPaperSizeName = cmbPaperSizeName.SelectedItem.ToString();
			}
			// マージン_保存
			{
				if (cbUseDefaultMargin.Checked)
					Gnd.i.lastMargin = null;
				else
					Gnd.i.lastMargin = getMaringLTRB();
			}

			Gnd.i.lastPrintProblem = cbPrintProblem.Checked;
			Gnd.i.lastPrintAnswer = cbPrintAnswer.Checked;
		}

		private void MkAndPrintWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.mtEnabled = false;
		}

		private void btnMkAndPrint_Click(object sender, EventArgs e)
		{
			this.mtEnabled = false;
			this.Visible = false;
			Gnd.i.mkAndPrintWinParentWin.Visible = false;

			try
			{
				CancellableBusyDlg.perform(false, delegate
				{
					if (_historyData == null) // ? 作成未完了
					{
						if (Gnd.i.mkSudokuProc == null)
							throw null;

						Gnd.i.cancellableBusyDlg.setTitle("ナンプレを作成しています... (印刷は自動的に開始されます)");

						while (Gnd.i.mkSudokuProc.isEnded() == false)
						{
							if (Gnd.i.cancelled)
							{
								Gnd.i.cancellableBusyDlg.setTitle("キャンセルしています...");
								Gnd.i.mkSudokuProc.bCancel();
								throw new Cancelled();
							}
							Thread.Sleep(2000);
						}
						Gnd.i.cancellableBusyDlg.setTitle("作成した問題を画像に変換しています...");

						_historyData = Gnd.i.mkSudokuProc.getResult();
						//historyDataCreated(); // もうこのフォームを表示しないので、呼ばなくて良い。

						if (Gnd.i.cancelled)
							throw new Cancelled();
					}
					Gnd.i.cancellableBusyDlg.setTitle("ナンプレを印刷しています...");

					{
						ImagePrinter imgPrn = new ImagePrinter();

						if (this.cbUseDefaultMargin.Checked == false)
							imgPrn.setMargin(getMaringLTRB());

						if (this.cbUseDefaultPrinter.Checked == false)
							imgPrn.setPrinterName(this.cmbPrinterName.SelectedItem.ToString());

						if (this.cbUseDefaultPaperSize.Checked == false)
							imgPrn.setPaperSizeName(this.cmbPaperSizeName.SelectedItem.ToString());

						if (this.cbPrintProblem.Checked)
							imgPrn.addImage(_historyData.getProblemImage());

						if (this.cbPrintAnswer.Checked)
							imgPrn.addImage(_historyData.getAnswerImage());

						imgPrn.doPrint();
					}

					throw new Completed("印刷しました。");
				});
			}
			catch (Exception ex)
			{
				FaultOperation.caught(ex);
			}

			Gnd.i.mkAndPrintWinParentWin.Visible = true;
			//this.Visible = true;
			this.Close();
		}

		private LTRB getMaringLTRB()
		{
			return new LTRB(
				IntTools.toInt(this.marginL.Text, 0, 9999, 100),
				IntTools.toInt(this.marginT.Text, 0, 9999, 100),
				IntTools.toInt(this.marginR.Text, 0, 9999, 100),
				IntTools.toInt(this.marginB.Text, 0, 9999, 100)
				);
		}

		private bool mtEnabled;
		private bool mtBusy;
		private long mtCount;

		private void mainTimer_Tick(object sender, EventArgs e)
		{
			if (this.mtEnabled == false || this.mtBusy)
				return;

			this.mtBusy = true;

			try
			{
				if (
					this._historyData == null &&
					Gnd.i.mkSudokuProc != null &&
					Gnd.i.mkSudokuProc.isEnded()
					)
				{
					this._historyData = Gnd.i.mkSudokuProc.getResult();
					this.historyDataCreated();
				}

				if (
					this._historyData == null &&
					Gnd.i.mkSudokuProc != null &&
					Gnd.i.mkSudokuProc.isEnded() == false
					)
				{
					double progressRate;

					lock (Gnd.i.n2Listener.SYNCROOT)
					{
						progressRate = Gnd.i.n2Listener.progressRate;
					}

					// なんちゃってプログレスバー効果
					{
						const long MC_MAX = 3000L; // 5 min
						const double MC_MAX_RATE = 0.3; // 30 pct

						long mc = Math.Min(mtCount, MC_MAX);
						double rate = (mc * MC_MAX_RATE) / MC_MAX;

						progressRate = Math.Max(progressRate, rate);
					}

					progressRate = DoubleTools.toRange(progressRate, 0.0, 1.0);
					progressRate *= 0.9;
					progressRate += 0.05;

					int value = IntTools.toInt(progressRate * IntTools.IMAX);

					if (pbMk.Value != value)
						pbMk.Value = value;
				}
			}
			finally
			{
				this.mtBusy = false;
				this.mtCount++;
			}
		}

		private void btnPreview_Click(object sender, EventArgs e)
		{
			if (_historyData == null)
				return;

			this.mtEnabled = false;

			using (PreviewDlg f = new PreviewDlg(_historyData))
			{
				f.ShowDialog();
			}
			GC.Collect();
			this.mtEnabled = true;
		}

		private void cbUseDefaultPrinter_CheckedChanged(object sender, EventArgs e)
		{
			refreshUI();
		}

		private void cbUseDefaultPaperSize_CheckedChanged(object sender, EventArgs e)
		{
			refreshUI();
		}

		private void cbUseDefaultMargin_CheckedChanged(object sender, EventArgs e)
		{
			refreshUI();
		}

		private void cbPrintProblem_CheckedChanged(object sender, EventArgs e)
		{
			refreshUI();
		}

		private void cbPrintAnswer_CheckedChanged(object sender, EventArgs e)
		{
			refreshUI();
		}
	}
}
