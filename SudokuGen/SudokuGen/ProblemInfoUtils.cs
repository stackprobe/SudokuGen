using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Charlotte.Tools;
using System.IO;

namespace Charlotte
{
	public class ProblemInfoUtils
	{
		public static ProblemInfo[] loadProblemInfos()
		{
			List<ProblemInfo> dest = new List<ProblemInfo>();

			foreach (string dir in FileTools.lsDirs(Gnd.i.problemInfosDir))
			{
				dest.Add(new ProblemInfo(Path.GetFileName(dir)));
			}
			return dest.ToArray();
		}

		public static ProblemInfo get(string name)
		{
			foreach (ProblemInfo pi in Gnd.i.problemInfos)
				if (pi.getName() == name)
					return pi;

			return null; // not found
		}
	}
}
