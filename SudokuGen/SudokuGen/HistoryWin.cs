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
	public partial class HistoryWin : Form
	{
		public HistoryWin()
		{
			InitializeComponent();

			this.MinimumSize = this.Size;
		}

		private void HistoryWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void HistoryWin_Shown(object sender, EventArgs e)
		{
			mainSheet_init();

			{
				HistoryData[] histories = HistoryDataUtils.getHistories();

				mainSheet.RowCount = histories.Length;

				for (int index = 0; index < histories.Length; index++)
				{
					mainSheet_setRow(index, histories[index]);
				}
				mainSheet.Rows[0].Selected = true;
			}
		}

		private void HistoryWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void HistoryWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			// noop
		}

		private void mainSheet_init()
		{
			mainSheet.RowCount = 0;
			mainSheet.ColumnCount = 0;
			mainSheet.ColumnCount = 4;

			mainSheet.RowHeadersVisible = false; // 行ヘッダ_非表示

			foreach (DataGridViewColumn column in mainSheet.Columns) // 列ソート_禁止
			{
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
			}

			int colidx = 0;

			{
				DataGridViewColumn column = mainSheet.Columns[colidx++];

				column.HeaderText = "dir";
				column.Visible = false;
			}

			{
				DataGridViewColumn column = mainSheet.Columns[colidx++];

				column.HeaderText = "作成日時";
				column.Width = 150;
			}

			{
				DataGridViewColumn column = mainSheet.Columns[colidx++];

				column.HeaderText = "問題の種類";
				column.Width = 300;
			}

			{
				DataGridViewColumn column = mainSheet.Columns[colidx++];

				column.HeaderText = "難易度";
				column.Width = 100;
			}

			mainSheet.RowCount = 0;
		}

		private HistoryData mainSheet_getRow(int rowidx)
		{
			return new HistoryData(mainSheet.Rows[rowidx].Cells[0].Value.ToString());
		}

		private void mainSheet_setRow(int rowidx, HistoryData hd)
		{
			DataGridViewRow row = mainSheet.Rows[rowidx];
			int cc = 0;

			row.Cells[cc++].Value = hd.getDir();
			row.Cells[cc++].Value = Utils.stampToString(hd.getStamp());
			row.Cells[cc++].Value = hd.getProblemInfo().getName();
			row.Cells[cc++].Value = Utils.levelToString(hd.getLevel());
		}

		private void mainSheet_refreshRow(int rowidx)
		{
			mainSheet_setRow(rowidx, mainSheet_getRow(rowidx));
		}

		private int getSelectedRow()
		{
			for (int rowidx = 0; rowidx < mainSheet.RowCount; rowidx++)
				if (mainSheet.Rows[rowidx].Selected)
					return rowidx;

			return -1;
		}

		private void btnPrint_Click(object sender, EventArgs e)
		{
			HistoryData hd = mainSheet_getRow(getSelectedRow());

			Gnd.i.mkAndPrintWinParentWin = this;
			Gnd.i.mkSudokuProc = null; // 2bs

			using (MkAndPrintWin f = new MkAndPrintWin(hd))
			{
				f.ShowDialog();
			}
			GC.Collect();
		}
	}
}
