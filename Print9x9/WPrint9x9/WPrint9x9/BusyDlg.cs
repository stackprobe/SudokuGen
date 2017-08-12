using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Permissions;

namespace Charlotte
{
	public partial class BusyDlg : Form
	{
		#region ALT_F4 抑止

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			const int WM_SYSCOMMAND = 0x112;
			const long SC_CLOSE = 0xF060L;

			if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE)
				return;

			base.WndProc(ref m);
		}

		#endregion

		private string _batFile;
		private Process _batProc;

		public BusyDlg(string batFile)
		{
			_batFile = batFile;

			{
				ProcessStartInfo psi = new ProcessStartInfo();

				psi.FileName = "cmd.exe";
				psi.Arguments = "/C " + _batFile;
				psi.CreateNoWindow = true;
				psi.UseShellExecute = false;

				_batProc = Process.Start(psi);
			}

			InitializeComponent();
		}

		private void BusyDlg_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void BusyDlg_Shown(object sender, EventArgs e)
		{
			this.MT_Enabled = true;
		}

		private void BusyDlg_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void BusyDlg_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.MT_Enabled = false;
		}

		private bool MT_Enabled;
		private bool MT_Busy;
		private long MT_Count;

		private void MainTimer_Tick(object sender, EventArgs e)
		{
			if (this.MT_Enabled == false || this.MT_Busy)
				return;

			this.MT_Busy = true;

			try
			{
				if (_batProc.HasExited)
				{
					this.MT_Enabled = false;
					this.Close();
					return;
				}

				// ----

				{
					string message = "プリンタを起動して、お待ち下さい";

					switch (this.MT_Count % 4)
					{
						case 0: message += "... "; break;
						case 1: message += ".. ."; break;
						case 2: message += ". .."; break;
						case 3: message += " ..."; break;
					}
					this.Message.Text = message;
				}
			}
			finally
			{
				this.MT_Busy = false;
				this.MT_Count++;
			}
		}
	}
}
