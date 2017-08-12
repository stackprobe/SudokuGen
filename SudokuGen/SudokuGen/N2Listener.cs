using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Charlotte.Tools;

namespace Charlotte
{
	public class N2Listener : IDisposable
	{
		private Thread _th;
		private bool _death;

		public object SYNCROOT = new object();

		// ---- 要 lock ----

		public double progressRate = 0.0;

		// ----

		public N2Listener()
		{
			_th = new Thread((ThreadStart)delegate
			{
				int millis = 0;

				while (_death == false)
				{
					byte[] bLine = Gnd.i.n2Recver.recv();

					if (bLine != null)
					{
						string line = StringTools.ENCODING_SJIS.GetString(bLine);

						if (line.StartsWith("Progress="))
						{
							int value = int.Parse(line.Substring(9));

							lock (SYNCROOT)
							{
								progressRate = (double)value / IntTools.IMAX;
							}
						}
						millis = 0;
					}
					else
					{
						if (millis < 2000)
							millis++;

						Thread.Sleep(millis);
					}
				}
			});
			_th.Start();
		}

		public void Dispose()
		{
			_death = true;

			_th.Join();
			_th = null;
		}
	}
}
