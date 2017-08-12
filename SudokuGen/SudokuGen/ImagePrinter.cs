using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using System.Drawing;

namespace Charlotte
{
	public class ImagePrinter
	{
		private List<Image> _images = new List<Image>();
		private LTRB _margin = null;
		private string _paperSizeName = null;
		private string _printerName = null;

		public void addImage(Image image)
		{
			_images.Add(image);
		}

		public void setMargin(LTRB margin)
		{
			_margin = margin;
		}

		public void setPaperSizeName(string name)
		{
			_paperSizeName = name;
		}

		public static string[] getPaperSizeNames()
		{
			List<string> dest = new List<string>();

			using (PrintDocument pd = new PrintDocument())
			{
				foreach (PaperSize ps in pd.PrinterSettings.PaperSizes)
				{
					dest.Add(ps.PaperName);
				}
			}
			return dest.ToArray();
		}

		public void setPrinterName(string name)
		{
			_printerName = name;
		}

		public static string[] getPrinterNames()
		{
			List<string> dest = new List<string>();

			foreach (string name in PrinterSettings.InstalledPrinters)
			{
				dest.Add(name);
			}
			return dest.ToArray();
		}

		public void doPrint()
		{
			if (_images.Count < 1)
				throw new Exception("印刷イメージがありません。");

			_imgq = new Queue<Image>(_images);

			using (PrintDocument pd = new PrintDocument())
			{
				if (_margin != null)
				{
					// LRTB 注意！
					pd.DefaultPageSettings.Margins = new Margins(
						_margin.l,
						_margin.r,
						_margin.t,
						_margin.b
						);
				}
				if (_paperSizeName != null)
				{
					pd.DefaultPageSettings.PaperSize = getPaperSize(pd, _paperSizeName);
				}
				if (_printerName != null)
				{
					pd.PrinterSettings.PrinterName = _printerName;
				}
				pd.PrintPage += new PrintPageEventHandler(pd_printPage);
				pd.Print();
			}
		}

		private static PaperSize getPaperSize(PrintDocument pd, string name)
		{
			foreach (PaperSize ps in pd.PrinterSettings.PaperSizes)
			{
				if (ps.PaperName == name)
				{
					return ps;
				}
			}
			throw new Exception("不明な用紙サイズ：" + name);
		}

		private Queue<Image> _imgq;

		private void pd_printPage(object sender, PrintPageEventArgs e)
		{
			using (Image img = _imgq.Dequeue())
			{
				e.Graphics.DrawImage(
					img,
					adjustInside(img.Width, img.Height, e.MarginBounds)
					);
				e.HasMorePages = 1 <= _imgq.Count;
			}
		}

		private static Rectangle adjustInside(int w, int h, Rectangle screen)
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
