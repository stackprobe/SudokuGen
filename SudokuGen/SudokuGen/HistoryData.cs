using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Charlotte.Tools;

namespace Charlotte
{
	public class HistoryData
	{
		private string _dir;

		public HistoryData(string dir)
		{
			_dir = dir;
		}

		public string getDir()
		{
			return _dir;
		}

		public long getStamp()
		{
			return long.Parse(Path.GetFileName(_dir));
		}

		private XNode getDataXml()
		{
			return XNode.load(Path.Combine(_dir, "Data.xml"));
		}

		public string getPIName()
		{
#if true
			return getDataXml().get("PIName").value;
#else // old
			return File.ReadAllText(Path.Combine(_dir, "PIName.dat"), StringTools.ENCODING_SJIS);
#endif
		}

		public ProblemInfo getProblemInfo()
		{
			return ProblemInfoUtils.get(getPIName());
		}

		public int getLevel()
		{
#if true
			return int.Parse(getDataXml().get("Level").value);
#else // old
			return int.Parse(File.ReadAllText(Path.Combine(_dir, "Level.dat"), StringTools.ENCODING_SJIS));
#endif
		}

		public Bitmap getProblemImage()
		{
			return Utils.readImageFile(Path.Combine(_dir, "Problem.png"));
		}

		public Bitmap getAnswerImage()
		{
			return Utils.readImageFile(Path.Combine(_dir, "Answer.png"));
		}

		/// <summary>
		/// 要 getHistories() 再取得
		/// </summary>
		public void remove()
		{
			FileTools.deletePath(_dir);
		}
	}
}
