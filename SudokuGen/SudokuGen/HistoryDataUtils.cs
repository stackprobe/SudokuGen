using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Charlotte.Tools;
using System.IO;
using System.Drawing.Imaging;

namespace Charlotte
{
	public class HistoryDataUtils
	{
		public static HistoryData createHistory(ProblemInfo pi, int level, Bitmap imgProblem, Bitmap imgAnswer)
		{
			long lStamp = DateTimeToSec.Now.getDateTime();
			string dir;

			for (; ; )
			{
				dir = Path.Combine(Gnd.i.historyDir, "" + lStamp);

				if (Directory.Exists(dir) == false)
					break;

				lStamp++;
			}
			Directory.CreateDirectory(dir);

#if true
			{
				XNode root = new XNode("Root");

				root.children.Add(new XNode("PIName", pi.getName()));
				root.children.Add(new XNode("Level", "" + level));

				root.save(Path.Combine(dir, "Data.xml"));
			}
#else // old
			File.WriteAllText(Path.Combine(dir, "PIName.dat"), pi.getName(), StringTools.ENCODING_SJIS);
			File.WriteAllText(Path.Combine(dir, "Level.dat"), "" + level, StringTools.ENCODING_SJIS);
#endif
			imgProblem.Save(Path.Combine(dir, "Problem.png"), ImageFormat.Png);
			imgAnswer.Save(Path.Combine(dir, "Answer.png"), ImageFormat.Png);

			return new HistoryData(dir);
		}

		public static HistoryData[] getHistories()
		{
			List<HistoryData> dest = new List<HistoryData>();

			foreach (string dir in FileTools.lsDirs(Gnd.i.historyDir))
			{
				dest.Add(new HistoryData(dir));
			}

			// 新しい -> 古い
			ArrayTools.sort<HistoryData>(dest, delegate(HistoryData a, HistoryData b)
			{
				return LongTools.comp(a.getStamp(), b.getStamp()) * -1;
			});

			return dest.ToArray();
		}

		public static void trimHistories()
		{
			for (; ; )
			{
				HistoryData[] histories = getHistories();

				if (histories.Length <= 100)
					break;

				getOldest(histories).remove();
			}
		}

		private static HistoryData getOldest(HistoryData[] histories)
		{
			return histories[histories.Length - 1];
		}

		public static void clearHistories()
		{
			foreach (HistoryData hd in getHistories())
			{
				hd.remove();
			}
		}
	}
}
