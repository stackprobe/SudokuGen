using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace PrintImage
{
	class Program
	{
		private static string _imgFile;

		static void Main(string[] args)
		{
			try
			{
				_imgFile = args[0];

				//PrintDocumentオブジェクトの作成
				System.Drawing.Printing.PrintDocument pd = new System.Drawing.Printing.PrintDocument();

				pd.DefaultPageSettings.Margins = new System.Drawing.Printing.Margins(100, 100, 100, 100);

				//PrintPageイベントハンドラの追加
				pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);
				//印刷を開始する
				pd.Print();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
		}

		private static void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			//画像を読み込む
			Image img = Image.FromFile(_imgFile);
			//画像を描画する
			e.Graphics.DrawImage(img, AdjustInside(img.Width, img.Height, e.MarginBounds));
			//次のページがないことを通知する
			e.HasMorePages = false;
			//後始末をする
			img.Dispose();
		}

		private static Rectangle AdjustInside(int w, int h, Rectangle screen)
		{
			int screen_l = screen.Left;
			int screen_t = screen.Top;
			int screen_w = screen.Width;
			int screen_h = screen.Height;

			{
				int new_w = screen_w;
				int new_h = (h * screen_w) / w;

				if (screen_h < new_h)
				{
					new_w = (w * screen_h) / h;
					new_h = screen_h;

					if (screen_w < new_w)
						throw null;
				}
				w = new_w;
				h = new_h;
			}

			int l = screen_l + (screen_w - w) / 2;
			int t = screen_t + (screen_h - h) / 2;

			return new Rectangle(l, t, w, h);
		}
	}
}
