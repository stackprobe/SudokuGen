using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Charlotte.Tools;

namespace Charlotte
{
	public class ImageCreator
	{
		private ProblemInfo _pi;
		private Color _digitColor;
		private string _csvFile;
		private Surface _surface;

		public class Surface
		{
			public Color digitColor;
			public string csvFile;
		}

		public ImageCreator(ProblemInfo pi, Color digitColor, string csvFile, Surface surface = null)
		{
			_pi = pi;
			_digitColor = digitColor;
			_csvFile = csvFile;
			_surface = surface;
		}

		private const int BMP_MARGIN = 7;
		private const int BMP_MARGIN_RB = BMP_MARGIN + 1;
		private const int BMP_CELLSTEP = 27;
		private const int BMP_DIGIT_MARGIN = 4;

		public Bitmap getImage()
		{
			Bmp bmp = new Bmp(0, 0, Bmp.Dot.getDot(Color.White));
			int l = 0;
			int t = 0;
			string optionBmpLFile = null;

			foreach (string line in _pi.getDrawingScript())
			{
				int[] prms = Utils.toInts(StringTools.tokenize(line, StringTools.DIGIT, true, true).ToArray());

				if (prms.Length < 1) // ? 空行
					continue;

				switch (prms[0])
				{
					case 0:
						bmp.table.w = BMP_MARGIN + (l + prms[1]) * BMP_CELLSTEP + BMP_MARGIN_RB;
						bmp.table.h = BMP_MARGIN + (t + prms[2]) * BMP_CELLSTEP + BMP_MARGIN_RB;
						break;

					// 枠線
					case 1:
					case 2:
					case 3:
						Utils.drawLine(
							bmp,
							BMP_MARGIN + (l + prms[1]) * BMP_CELLSTEP,
							BMP_MARGIN + (t + prms[2]) * BMP_CELLSTEP,
							BMP_MARGIN + (l + prms[3]) * BMP_CELLSTEP,
							BMP_MARGIN + (t + prms[4]) * BMP_CELLSTEP,
							Bmp.Dot.getDot(Color.Black),
							prms[0] - 1
							);
						break;

					// 対角線
					case 4:
						Utils.drawLine(
							bmp,
							BMP_MARGIN + (l + prms[1]) * BMP_CELLSTEP,
							BMP_MARGIN + (t + prms[2]) * BMP_CELLSTEP,
							BMP_MARGIN + (l + prms[3]) * BMP_CELLSTEP,
							BMP_MARGIN + (t + prms[4]) * BMP_CELLSTEP,
							new Bmp.Dot(255, 255, 200, 200),
							2
							);
						break;

					// 重複部分
					case 5:
						bmp.fillRect(
							BMP_MARGIN + (l + prms[1]) * BMP_CELLSTEP,
							BMP_MARGIN + (t + prms[2]) * BMP_CELLSTEP,
							BMP_MARGIN + (l + prms[3]) * BMP_CELLSTEP,
							BMP_MARGIN + (t + prms[4]) * BMP_CELLSTEP,
							new Bmp.Dot(255, 255, 255, 200)
							);
						break;

					// 座標原点変更
					case 6:
						l = prms[1];
						t = prms[2];
						break;

					// 奇数部分
					case 7:
						bmp.fillRect(
							BMP_MARGIN + (l + prms[1]) * BMP_CELLSTEP,
							BMP_MARGIN + (t + prms[2]) * BMP_CELLSTEP,
							BMP_MARGIN + (l + prms[3]) * BMP_CELLSTEP,
							BMP_MARGIN + (t + prms[4]) * BMP_CELLSTEP,
							new Bmp.Dot(
								255,
								(255 + 255) / 2,
								(255 + 201) / 2,
								(255 + 14) / 2
								)
							);
						break;

					// 偶数部分
					case 8:
						bmp.fillRect(
							BMP_MARGIN + (l + prms[1]) * BMP_CELLSTEP,
							BMP_MARGIN + (t + prms[2]) * BMP_CELLSTEP,
							BMP_MARGIN + (l + prms[3]) * BMP_CELLSTEP,
							BMP_MARGIN + (t + prms[4]) * BMP_CELLSTEP,
							new Bmp.Dot(
								255,
								(255 + 153) / 2,
								(255 + 217) / 2,
								(255 + 234) / 2
								)
							);
						break;

					case 9:
						optionBmpLFile = "even_odd.bmp";
						break;

					default:
						throw null;
				}
			}
			RectTable<int> csv = Utils.readDigCsvFile(_csvFile);
			RectTable<int> surfaceCsv = null;

			if (_surface != null)
				surfaceCsv = Utils.readDigCsvFile(_surface.csvFile);

			for (int x = 0; x < csv.w; x++)
			{
				for (int y = 0; y < csv.h; y++)
				{
					int dig = csv.get(x, y);
					Color color = _digitColor;

					if (surfaceCsv != null && surfaceCsv.get(x, y) != 0)
						color = _surface.digitColor;

					Utils.drawDigit(
						bmp,
						BMP_MARGIN + x * BMP_CELLSTEP + BMP_DIGIT_MARGIN,
						BMP_MARGIN + y * BMP_CELLSTEP + BMP_DIGIT_MARGIN,
						color,
						dig
						);
				}
			}
			if (optionBmpLFile != null)
			{
				string file = Utils.lFileToResFile(optionBmpLFile);

				using (Bitmap optBmp = (Bitmap)Bitmap.FromFile(file))
				{
					bmp.putOptionBmp(optBmp);
				}
			}
			return bmp.getBitmap();
		}
	}
}
