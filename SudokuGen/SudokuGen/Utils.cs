using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Charlotte.Tools;
using System.IO;

namespace Charlotte
{
	public static class Utils
	{
		public static int getIndex(ComboBox combo, string item)
		{
			for (int index = 0; index < combo.Items.Count; index++)
				if (combo.Items[index].ToString() == item)
					return index;

			return -1; // not found
		}

		public static string levelToString(int level)
		{
			switch (level)
			{
				case 0: return "普通";
				case 1: return "普通＋";
				case 2: return "難しい";
				case 3: return "とても難しい";
			}
			throw null; // unknown level
		}

		public static Color levelToDigitColor(int level)
		{
			switch (level)
			{
				case 0: return Gnd.i.lv1Color;
				case 1: return Gnd.i.lv2Color;
				case 2: return Gnd.i.lv3Color;
				case 3: return Gnd.i.lv4Color;
			}
			throw null; // unknown level
		}

		public static string stampToString(long stamp)
		{
			string ret = "" + stamp;

			ret = ret.Insert(12, ":");
			ret = ret.Insert(10, ":");
			ret = ret.Insert(8, " ");
			ret = ret.Insert(6, "/");
			ret = ret.Insert(4, "/");

			return ret;
		}

		public static int[] toInts(string[] src)
		{
			int[] dest = new int[src.Length];

			for (int index = 0; index < src.Length; index++)
			{
				try
				{
					dest[index] = int.Parse(src[index]);
				}
				catch
				{
					dest[index] = 0; // def
				}
			}
			return dest;
		}

		public static void drawLine(Bmp bmp, int x1, int y1, int x2, int y2, Bmp.Dot dot, int r)
		{
			for (int bx = -r; bx <= r; bx++)
			{
				for (int by = -r; by <= r; by++)
				{
					bmp.drawLine(x1 + bx, y1 + by, x2 + bx, y2 + by, dot);
				}
			}
		}

		public static RectTable<int> readDigCsvFile(string csvFile)
		{
			string[] lines = File.ReadAllLines(csvFile);
			RectTable<int> dest = new RectTable<int>(0, 0, 0);

			for (int rr = 0; rr < lines.Length; rr++)
			{
				int[] digs = toInts(StringTools.tokenize(lines[rr], ",", false, false).ToArray());

				for (int cc = 0; cc < digs.Length; cc++)
				{
					dest.set(cc, rr, digs[cc]);
				}
			}
			return dest;
		}

		public static void drawDigit(Bmp dest, int l, int t, Color digColor, int dig)
		{
			for (int x = 0; x < 20; x++)
			{
				for (int y = 0; y < 20; y++)
				{
					Bmp.Dot dot = getBmpDigits().table.get(dig * 20 + x, y);

					if (dot.r != 255) // ? 数字の部分
					{
						dest.table.set(l + x, t + y, Bmp.Dot.getDot(digColor));
					}
				}
			}
		}

		private static Bmp _bmpDigits;

		public static Bmp getBmpDigits()
		{
			if (_bmpDigits == null)
				_bmpDigits = Bmp.create(readImageFile(Gnd.i.digitsImageFile));

			return _bmpDigits;
		}

		public static Bitmap readImageFile(string file)
		{
			return new Bitmap(new MemoryStream(File.ReadAllBytes(file)));
		}

		public static string lFileToResFile(string lFile)
		{
			string file = lFile;

			if (File.Exists(file) == false)
				file = Path.Combine(@"..\..\..\..\doc", lFile); // devenv

			file = FileTools.makeFullPath(file);
			return file;
		}

		private static StreamWriter LogWriter = null;

		public static void WriteLog(object message)
		{
			if (LogWriter == null)
			{
				//string logFile = Path.Combine(Program.selfDir, Path.GetFileNameWithoutExtension(Program.selfFile) + ".log");
				string logFile = Path.Combine(Program.selfDir, "NumpleGen.log");

				LogWriter = new StreamWriter(logFile, false, Encoding.UTF8);
			}
			LogWriter.WriteLine("[" + DateTime.Now + "] " + message);
			LogWriter.Flush();
		}

		// sync > @ AntiWindowsDefenderSmartScreen

		public static void AntiWindowsDefenderSmartScreen()
		{
			WriteLog("awdss_1");

			if (Is初回起動())
			{
				WriteLog("awdss_2");

				foreach (string exeFile in Directory.GetFiles(BootTools.SelfDir, "*.exe", SearchOption.TopDirectoryOnly))
				{
					try
					{
						WriteLog("awdss_exeFile: " + exeFile);

						if (exeFile.ToLower() == BootTools.SelfFile.ToLower())
						{
							WriteLog("awdss_self_noop");
						}
						else
						{
							byte[] exeData = File.ReadAllBytes(exeFile);
							File.Delete(exeFile);
							File.WriteAllBytes(exeFile, exeData);
						}
						WriteLog("awdss_OK");
					}
					catch (Exception e)
					{
						WriteLog(e);
					}
				}
				WriteLog("awdss_3");
			}
			WriteLog("awdss_4");
		}

		// < sync

		public static bool Is初回起動()
		{
			return Gnd.i.is初回起動();
		}

		// sync > @ PostShown

		public static void PostShown_GetAllControl(Form f, Action<Control> reaction)
		{
			Queue<Control.ControlCollection> controlTable = new Queue<Control.ControlCollection>();

			controlTable.Enqueue(f.Controls);

			while (1 <= controlTable.Count)
			{
				foreach (Control control in controlTable.Dequeue())
				{
					GroupBox gb = control as GroupBox;

					if (gb != null)
					{
						controlTable.Enqueue(gb.Controls);
					}
					TabControl tc = control as TabControl;

					if (tc != null)
					{
						foreach (TabPage tp in tc.TabPages)
						{
							controlTable.Enqueue(tp.Controls);
						}
					}
					SplitContainer sc = control as SplitContainer;

					if (sc != null)
					{
						controlTable.Enqueue(sc.Panel1.Controls);
						controlTable.Enqueue(sc.Panel2.Controls);
					}
					Panel p = control as Panel;

					if (p != null)
					{
						controlTable.Enqueue(p.Controls);
					}
					reaction(control);
				}
			}
		}

		public static void PostShown(Form f)
		{
			PostShown_GetAllControl(f, control =>
			{
				Control c = new Control[]
				{
					control as TextBox,
					control as NumericUpDown,
				}
				.FirstOrDefault(v => v != null);

				if (c != null)
				{
					if (c.ContextMenuStrip == null)
					{
						ContextMenuStrip menu = new ContextMenuStrip();

#if true
						{
							ToolStripMenuItem item = new ToolStripMenuItem();

							item.Text = "内容をコピー";
							item.Click += (sdr, ev) =>
							{
								try
								{
									Clipboard.SetText(c.Text ?? "");
								}
								catch
								{ }
							};

							menu.Items.Add(item);
						}
#else
						{
							ToolStripMenuItem item = new ToolStripMenuItem();

							item.Text = "項目なし";
							item.Enabled = false;

							menu.Items.Add(item);
						}
#endif

						c.ContextMenuStrip = menu;
					}
				}
			});
		}

		// < sync
	}
}
