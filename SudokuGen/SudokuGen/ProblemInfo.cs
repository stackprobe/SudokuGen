using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Charlotte.Tools;
using System.Drawing;

namespace Charlotte
{
	public class ProblemInfo
	{
		private string _dir;

		public ProblemInfo(string name)
		{
			_dir = Path.Combine(Gnd.i.problemInfosDir, name);
		}

		public string getName()
		{
			return Path.GetFileName(_dir);
		}

		public string getDescription()
		{
			return File.ReadAllText(Path.Combine(_dir, "Description.txt"), StringTools.ENCODING_SJIS);
		}

		public Bitmap getPreview()
		{
			return Utils.readImageFile(Path.Combine(_dir, "Preview.png"));
		}

		public string getInputDataDir()
		{
			return Path.Combine(_dir, "InputData");
		}

		public string[] getDrawingScript()
		{
			string file;

			if (Gnd.i.instantDrawingScriptFile == null)
				file = Path.Combine(_dir, "DrawingScript.txt");
			else
				file = Gnd.i.instantDrawingScriptFile;

			return File.ReadAllLines(file, StringTools.ENCODING_SJIS);
		}
	}
}
