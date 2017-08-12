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
	public partial class MainWin : Form
	{
		public MainWin()
		{
			InitializeComponent();
		}

		private void MainWin_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void MainWin_Shown(object sender, EventArgs e)
		{
			// noop
		}

		private void MainWin_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void MainWin_FormClosed(object sender, FormClosedEventArgs e)
		{
			// noop
		}

		private void Go(string batFile)
		{
			this.Visible = false;

			using (BusyDlg f = new BusyDlg(batFile))
			{
				f.ShowDialog();
			}
			this.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Go("_Go2.bat");
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Go("_Go.bat");
		}

		private void button3_Click(object sender, EventArgs e)
		{
			Go("_Go-G.bat");
		}

		private void button4_Click(object sender, EventArgs e)
		{
			Go("_Go-R.bat");
		}
	}
}
