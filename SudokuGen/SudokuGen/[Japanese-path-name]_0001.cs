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
	public partial class Setting詳細Win : Form
	{
		public Setting詳細Win()
		{
			InitializeComponent();
		}

		private void Setting詳細Win_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void Setting詳細Win_Shown(object sender, EventArgs e)
		{
			// load
			{
				cb合体ナンプレの重なった部分に成丈数字を置かない.Checked = Gnd.i._合体ナンプレの重なった部分に成丈数字を置かない;
			}
		}

		private void Setting詳細Win_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void Setting詳細Win_FormClosed(object sender, FormClosedEventArgs e)
		{
			// noop
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			// save
			{
				Gnd.i._合体ナンプレの重なった部分に成丈数字を置かない = cb合体ナンプレの重なった部分に成丈数字を置かない.Checked;
			}
			this.Close();
		}
	}
}
