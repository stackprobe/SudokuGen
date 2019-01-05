using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace Charlotte
{
	public class Gnd
	{
		private static Gnd _i = null;

		public static Gnd i
		{
			get
			{
				if (_i == null)
					_i = new Gnd();

				return _i;
			}
		}

		private Gnd()
		{ }

		// ---- conf data ----

		public string dummyConfString = "default";

		public void loadConf()
		{
			try
			{
				List<string> lines = new List<string>();

				foreach (string line in FileTools.readAllLines(getConfFile(), StringTools.ENCODING_SJIS))
					if (line != "" && line[0] != ';')
						lines.Add(line);

				int c = 0;

				// items >

				dummyConfString = lines[c++];

				// < items
			}
			catch
			{ }
		}

		private string getConfFile()
		{
			return Path.Combine(Program.selfDir, Path.GetFileNameWithoutExtension(Program.selfFile) + ".conf");
		}

		// ---- saved data ----

		public int lastPIIndex = 0;
		public int lastDifficulty = 0;
		public string lastPrinterName = ""; // "" == デフォルトのプリンタ
		public string lastPaperSizeName = ""; // "" == デフォルトの用紙サイズ
		public LTRB lastMargin = null; // null == デフォルトの余白
		public bool lastPrintProblem = true;
		public bool lastPrintAnswer = false;
		public Color lv1Color = Color.Blue;
		public Color lv2Color = Color.DarkGreen;
		public Color lv3Color = Color.DarkRed;
		public Color lv4Color = Color.DarkViolet;
		public Color ansColor = Color.DarkGray;
		public bool _合体ナンプレの重なった部分に成丈数字を置かない = false;

		public void loadData()
		{
			try
			{
				string[] lines = File.ReadAllLines(getDataFile(), Encoding.UTF8);
				int c = 0;

				// items >

				lastPIIndex = int.Parse(lines[c++]);
				lastDifficulty = int.Parse(lines[c++]);
				lastPrinterName = lines[c++];
				lastPaperSizeName = lines[c++];

				{
					string line = lines[c++];

					if (line != "")
					{
						List<string> tokens = StringTools.tokenize(line, StringTools.DIGIT, true, true);

						lastMargin = new LTRB(
							IntTools.toInt(tokens[0], 0, 9999),
							IntTools.toInt(tokens[1], 0, 9999),
							IntTools.toInt(tokens[2], 0, 9999),
							IntTools.toInt(tokens[3], 0, 9999)
							);
					}
					else
						lastMargin = null;
				}

				lastPrintProblem = StringTools.toFlag(lines[c++]);
				lastPrintAnswer = StringTools.toFlag(lines[c++]);
				lv1Color = IntTools.toColor(int.Parse(lines[c++]));
				lv2Color = IntTools.toColor(int.Parse(lines[c++]));
				lv3Color = IntTools.toColor(int.Parse(lines[c++]));
				lv4Color = IntTools.toColor(int.Parse(lines[c++]));
				ansColor = IntTools.toColor(int.Parse(lines[c++]));
				_合体ナンプレの重なった部分に成丈数字を置かない = StringTools.toFlag(lines[c++]);

				// < items
			}
			catch
			{ }
		}

		public void saveData()
		{
			try
			{
				List<string> lines = new List<string>();

				// items >

				lines.Add("" + lastPIIndex);
				lines.Add("" + lastDifficulty);
				lines.Add(lastPrinterName);
				lines.Add(lastPaperSizeName);

				{
					string line;

					if (lastMargin != null)
						line = String.Join(", ", lastMargin.l, lastMargin.t, lastMargin.r, lastMargin.b);
					else
						line = "";

					lines.Add(line);
				}

				lines.Add(StringTools.toString(lastPrintProblem));
				lines.Add(StringTools.toString(lastPrintAnswer));
				lines.Add("" + IntTools.toInt(lv1Color));
				lines.Add("" + IntTools.toInt(lv2Color));
				lines.Add("" + IntTools.toInt(lv3Color));
				lines.Add("" + IntTools.toInt(lv4Color));
				lines.Add("" + IntTools.toInt(ansColor));
				lines.Add(StringTools.toString(_合体ナンプレの重なった部分に成丈数字を置かない));

				// < items

				File.WriteAllLines(getDataFile(), lines, Encoding.UTF8);
			}
			catch
			{ }
		}

		private string getDataFile()
		{
			return Path.Combine(Program.selfDir, Path.GetFileNameWithoutExtension(Program.selfFile) + ".dat");
		}

		// ----

		public Nectar2.Recver n2Recver = new Nectar2.Recver("{aaa1d63e-fc8b-488b-a92f-d7617b70f514}", ';'); // shared_uuid
		public N2Listener n2Listener;

		public string problemInfosDir
		{
			get
			{
				string dir = "問題の種類";

				if (Directory.Exists(dir) == false)
					dir = @"..\..\..\..\doc\問題の種類"; // dev-env

				dir = FileTools.makeFullPath(dir);
				return dir;
			}
		}

		public string historyDir
		{
			get
			{
				string dir = "作成した問題";

				if (Directory.Exists(dir) == false)
					dir = @"..\..\..\..\doc\作成した問題"; // dev-env

				dir = FileTools.makeFullPath(dir);
				return dir;
			}
		}

		public string mkSudokuFile
		{
			get
			{
				string file = "MkNumple2.exe";

				if (File.Exists(file) == false)
					file = @"..\..\..\..\MkSudoku2.exe"; // dev-env

				file = FileTools.makeFullPath(file);
				return file;
			}
		}

		public string sudokuFile
		{
			get
			{
				string file = "Numple2.exe";

				if (File.Exists(file) == false)
					file = @"..\..\..\..\Sudoku2\Release\Sudoku2.exe"; // dev-env

				file = FileTools.makeFullPath(file);
				return file;
			}
		}

		public string digitsImageFile
		{
			get
			{
				string file = "digits.bmp";

				if (File.Exists(file) == false)
					file = @"..\..\..\..\doc\digits.bmp"; // dev-env

				file = FileTools.makeFullPath(file);
				return file;
			}
		}

		public Form mkAndPrintWinParentWin; // MainWin か HistoryWin
		public ProblemInfo[] problemInfos;
		public CancellableBusyDlg cancellableBusyDlg;
		public bool cancelled;
		public MkSudokuProc mkSudokuProc = null; // null == 未実行
		public string instantDrawingScriptFile = null; // null == 未定義

		public bool is初回起動()
		{
			return File.Exists(getDataFile()) == false; // ? saveData()を1度も実行していない。
		}
	}
}
