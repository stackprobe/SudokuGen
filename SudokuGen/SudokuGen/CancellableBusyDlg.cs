using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Security.Permissions;
using System.Threading;

namespace Charlotte
{
	public partial class CancellableBusyDlg : Form
	{
		#region ALT_F4 抑止

		private static bool _xPressed = false;

		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override void WndProc(ref Message m)
		{
			const int WM_SYSCOMMAND = 0x112;
			const long SC_CLOSE = 0xF060L;

			if (m.Msg == WM_SYSCOMMAND && (m.WParam.ToInt64() & 0xFFF0L) == SC_CLOSE)
			{
				_xPressed = true;
				return;
			}
			base.WndProc(ref m);
		}

		#endregion

		public CancellableBusyDlg(operation_d operation)
		{
			_operation = operation;

			InitializeComponent();
		}

		private void CancellableBusyDlg_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void CancellableBusyDlg_Shown(object sender, EventArgs e)
		{
			this.startTh();
			this.mtEnabled = true;
		}

		private void CancellableBusyDlg_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void CancellableBusyDlg_FormClosed(object sender, FormClosedEventArgs e)
		{
			this.mtEnabled = false;
		}

		public static void perform(bool antiCancel, operation_d operation)
		{
			using (CancellableBusyDlg f = new CancellableBusyDlg(operation))
			{
				Gnd.i.cancellableBusyDlg = f;
				Gnd.i.cancelled = false;

				if (antiCancel)
					f.btnCancel.Enabled = false;

				f.ShowDialog();

				Gnd.i.cancellableBusyDlg = null;

				if (f._e != null)
				{
					throw f._e;
				}
			}
			GC.Collect();
		}

		public delegate void operation_d();
		private operation_d _operation;
		private Exception _e;
		private Thread _th;

		private void startTh()
		{
			_e = null;
			_th = new Thread((ThreadStart)delegate
			{
				try
				{
					_operation();
				}
				catch (Exception e)
				{
					_e = e;
				}
			});
			_th.Start();
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
				if (5 < this.mtCount && _th.IsAlive == false)
				{
					this.mtEnabled = false;
					this.Close();
					return;
				}
				if (_xPressed)
				{
					this.btnCancel_Click(null, null);
					_xPressed = false;
					return; // 2bs
				}
				lock (SYNCROOT)
				{
					if (_titleNew != null)
					{
						this.Text = _titleNew;
						_titleNew = null;
					}
				}
			}
			finally
			{
				this.mtBusy = false;
				this.mtCount++;
			}
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			if (this.btnCancel.Enabled)
			{
				Gnd.i.cancelled = true;
				this.btnCancel.Enabled = false;
			}
		}

		private object SYNCROOT = new object();
		private string _titleNew = null;

		public void setTitle(string title)
		{
			lock (SYNCROOT)
			{
				_titleNew = title;
			}
		}
	}
}
