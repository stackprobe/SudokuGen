using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using Charlotte.Tools;
using System.Threading;
using System.Drawing;

namespace Charlotte
{
	public class MkSudokuProc : IDisposable
	{
		private ProblemInfo _pi;
		private int _level;

		public MkSudokuProc(ProblemInfo pi, int level)
		{
			_pi = pi;
			_level = level;

			this.startProc();
		}

		private string _inputDataDir;
		private Process _proc;

		private void startProc()
		{
			_inputDataDir = Path.Combine(FileTools.getTMP(), StringTools.getUUID());
			FileTools.copyDir(_pi.getInputDataDir(), _inputDataDir);

			{
				ProcessStartInfo psi = new ProcessStartInfo();

				psi.FileName = Gnd.i.mkSudokuFile;
				psi.Arguments =
					"//O MkNumple2.log /N2 /K" +
					(Gnd.i._合体ナンプレの重なった部分に成丈数字を置かない ? "+" : "-") +
					" /Lv:" +
					(_level + 1) +
					" " +
					_inputDataDir;
#if DEBUG == false
				psi.CreateNoWindow = true;
				psi.UseShellExecute = false;
#endif
				psi.WorkingDirectory = Path.GetDirectoryName(Gnd.i.mkSudokuFile);

				_proc = Process.Start(psi);
			}

			lock (Gnd.i.n2Listener.SYNCROOT)
			{
				Gnd.i.n2Listener.progressRate = 0.0; // リセット
			}
		}

		/// <summary>
		/// blocking
		/// </summary>
		public void bCancel()
		{
			while (isEnded() == false)
			{
				cancel();
				Thread.Sleep(2000);
			}
		}

		public void cancel()
		{
			{
				ProcessStartInfo psi = new ProcessStartInfo();

				psi.FileName = Gnd.i.mkSudokuFile;
				psi.Arguments = "/S";
				psi.CreateNoWindow = true;
				psi.UseShellExecute = false;
				psi.WorkingDirectory = Path.GetDirectoryName(Gnd.i.mkSudokuFile);

				Process.Start(psi).WaitForExit();
			}

			// ついでに Sudoku2.exe も止めに行ってみる。
			{
				ProcessStartInfo psi = new ProcessStartInfo();

				psi.FileName = Gnd.i.sudokuFile;
				psi.Arguments = "/S";
				psi.CreateNoWindow = true;
				psi.UseShellExecute = false;
				psi.WorkingDirectory = Path.GetDirectoryName(Gnd.i.sudokuFile);

				Process.Start(psi).WaitForExit();
			}
		}

		public bool isEnded()
		{
			if (_proc != null && _proc.HasExited)
				_proc = null;

			return _proc == null;
		}

		private HistoryData _result = null;

		public HistoryData getResult()
		{
			if (_result == null)
			{
				if (_proc != null)
					throw null;

				string inputCsvFile = Path.Combine(_inputDataDir, "Input.csv");
				string outputCsvFile = Path.Combine(_inputDataDir, "Output.csv");

				{
					string file = Path.Combine(_inputDataDir, "InstantDrawingScript.txt");

					if (File.Exists(file))
						Gnd.i.instantDrawingScriptFile = file;
				}

				Bitmap imgProblem = new ImageCreator(
					_pi,
					Utils.levelToDigitColor(_level),
					inputCsvFile
					)
					.getImage();

				Bitmap imgAnswer = new ImageCreator(
					_pi,
					Gnd.i.ansColor,
					outputCsvFile,
					new ImageCreator.Surface()
					{
						digitColor = Utils.levelToDigitColor(_level),
						csvFile = inputCsvFile,
					}
					)
					.getImage();

				Gnd.i.instantDrawingScriptFile = null;

				_result = HistoryDataUtils.createHistory(_pi, _level, imgProblem, imgAnswer);
			}
			return _result;
		}

		public void Dispose()
		{
			if (_inputDataDir != null)
			{
				FileTools.deletePath(_inputDataDir);
				_inputDataDir = null;
			}
		}
	}
}
