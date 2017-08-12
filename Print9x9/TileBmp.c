#include "C:\Factory\Common\all.h"
#include "C:\Factory\SubTools\libs\bmptbl.h"

static autoTable_t *Bmp;

static void Draw(uint l, uint t, autoTable_t *src)
{
	uint x;
	uint y;

	for(x = 0; x < 250; x++)
	for(y = 0; y < 250; y++)
	{
		setTableCell(Bmp, l + x, t + y, getTableCell(src, x, y));
	}
}
int main(int argc, char **argv)
{
	Bmp = newTable(getZero, noop_u);

	resizeTable(Bmp, 550, 850);

	{
		uint x;
		uint y;

		for(x = 0; x < 550; x++)
		for(y = 0; y < 850; y++)
		{
			setTableCell(Bmp, x, y, 0xffffff);
		}
	}

	Draw(0,     0, tReadBMPFile("tmp\\L1.bmp"));
	Draw(300,   0, tReadBMPFile("tmp\\R1.bmp"));
	Draw(0,   300, tReadBMPFile("tmp\\L2.bmp"));
	Draw(300, 300, tReadBMPFile("tmp\\R2.bmp"));
	Draw(0,   600, tReadBMPFile("tmp\\L3.bmp"));
	Draw(300, 600, tReadBMPFile("tmp\\R3.bmp"));

	tWriteBMPFile("tmp\\dest.bmp", Bmp);
}
