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
	public partial class PreviewDlg : Form
	{
		private HistoryData _historyData;

		public PreviewDlg(HistoryData historyData)
		{
			_historyData = historyData;

			InitializeComponent();
		}

		private void PreviewDlg_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void PreviewDlg_Shown(object sender, EventArgs e)
		{
			imgProblem.Image = _historyData.getProblemImage();
			imgAnswer.Image = _historyData.getAnswerImage();
		}

		private void PreviewDlg_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void PreviewDlg_FormClosed(object sender, FormClosedEventArgs e)
		{
			// noop
		}
	}
}
