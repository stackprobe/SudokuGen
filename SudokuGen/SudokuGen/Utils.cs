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
	public class Utils
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

		public static void antiWindowsDefenderSmartScreen()
		{
			WriteLog("awdss_1");

			if (Gnd.i.is初回起動())
			{
				WriteLog("awdss_2");

				foreach (string exeFile in Directory.GetFiles(Program.selfDir, "*.exe", SearchOption.AllDirectories))
				{
					try
					{
						WriteLog("awdss_exeFile: " + exeFile);

						if (StringTools.equalsIgnoreCase(exeFile, Program.selfFile))
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
	}
}
