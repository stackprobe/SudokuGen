/*
	Draw9x9.exe Input.csv Output.bmp
*/

#include "C:\Factory\Common\all.h"
#include "C:\Factory\Common\Options\csv.h"
#include "C:\Factory\SubTools\libs\bmptbl.h"

#define BMP_W 250
#define BMP_H 250

#define DR_L 12
#define DR_T 12
#define CELLSPAN 25
#define DR_DIG_L 3
#define DR_DIG_T 3

static autoTable_t *Bmp;
static autoTable_t *DigBmp;
static uint DigColor = UINTMAX;

// ====

static void DrawLine(int x1, int y1, int x2, int y2, int w)
{
	int ax;
	int ay;

	for(ax = -w; ax <= w; ax++)
	for(ay = -w; ay <= w; ay++)
	{
		int x;
		int y;

		for(x = x1; x <= x2; x++)
		for(y = y1; y <= y2; y++)
		{
			int xx = x + ax;
			int yy = y + ay;

			setTableCell(Bmp, xx, yy, 0);
		}
	}
}

// ====

static void DrawDigit(uint l, uint t, uint dig)
{
	uint x;
	uint y;

	for(x = 0; x < 20; x++)
	for(y = 0; y < 20; y++)
	{
		uint color = getTableCell(DigBmp, dig * 20 + x, y);

		if(color != 0xffffff && DigColor != UINTMAX)
			color = DigColor;

		setTableCell(Bmp, l + x, t + y, color);
	}
}
static void Main2(char *rCsvFile, char *wBmpFile)
{
	autoList_t *csv = readCSVFileTR(rCsvFile);
	uint c;
	uint x;
	uint y;

	Bmp = newTable(getZero, noop_u);
	DigBmp = tReadBMPFile(innerResPathFltr("digits.bmp"));

	resizeTable(Bmp, BMP_W, BMP_H);

	for(x = 0; x < BMP_W; x++)
	for(y = 0; y < BMP_H; y++)
	{
		setTableCell(Bmp, x, y, 0xffffff);
	}
	for(c = 0; c <= 9; c += 3)
	{
		DrawLine(
			DR_L + c * CELLSPAN,
			DR_T + 0,
			DR_L + c * CELLSPAN,
			DR_T + 9 * CELLSPAN,
			1
			);

		DrawLine(
			DR_L + 0,
			DR_T + c * CELLSPAN,
			DR_L + 9 * CELLSPAN,
			DR_T + c * CELLSPAN,
			1
			);
	}
	for(c = 1; c < 9; c++)
	{
		DrawLine(
			DR_L + c * CELLSPAN,
			DR_T + 0,
			DR_L + c * CELLSPAN,
			DR_T + 9 * CELLSPAN,
			0
			);

		DrawLine(
			DR_L + 0,
			DR_T + c * CELLSPAN,
			DR_L + 9 * CELLSPAN,
			DR_T + c * CELLSPAN,
			0
			);
	}
	for(x = 0; x < 9; x++)
	for(y = 0; y < 9; y++)
	{
		uint dig = toValue(refLine(refList(csv, y), x));

//		cout("%u %u = %u\n", x, y, dig);

		DrawDigit(
			DR_L + x * CELLSPAN + DR_DIG_L,
			DR_T + y * CELLSPAN + DR_DIG_T,
			dig
			);
	}
	tWriteBMPFile(wBmpFile, Bmp);
}
int main(int argc, char **argv)
{
	char *rCsvFile;
	char *wBmpFile;

	if(argIs("/DG"))
	{
		DigColor = 0x007f00;
	}
	if(argIs("/DR"))
	{
		DigColor = 0x7f0000;
	}

	rCsvFile = nextArg();
	wBmpFile = nextArg();

	Main2(rCsvFile, wBmpFile);
}
