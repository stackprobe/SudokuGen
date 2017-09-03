// use int

#include "MkGroupCsvHB.h"

#define MAP_WH_MAX 100

static int Map[MAP_WH_MAX][MAP_WH_MAX];
static int MapWH; // == グループ数 == グループの升数
static int BlockW;
static int BlockH;

static void InitMap(void)
{
	int x;
	int y;

	for(x = 0; x < MapWH; x++)
	for(y = 0; y < MapWH; y++)
	{
		int g = x / BlockW + (y / BlockH) * BlockH + 1;

		Map[x][y] = g;
	}
}

static int G1Xs[MAP_WH_MAX]; // グループの升数(MAP_WH_MAX)より多くなることは無い。
static int G1Ys[MAP_WH_MAX];
static int G1Cnt;
static int G2Xs[MAP_WH_MAX];
static int G2Ys[MAP_WH_MAX];
static int G2Cnt;

static int GetCell(int x, int y)
{
	if(
		0 <= x && x < MapWH &&
		0 <= y && y < MapWH
		)
		return Map[x][y];

	return 0;
}
static int KM_FindXYs(int g1, int g2, int *xs, int *ys) // ret: 見つけた個数
{
	int x;
	int y;
	int cnt = 0;

	for(x = 0; x < MapWH; x++)
	for(y = 0; y < MapWH; y++)
	{
		if(
			Map[x][y] == g1 &&
			(
				GetCell(x - 1, y    ) == g2 ||
				GetCell(x,     y - 1) == g2 ||
				GetCell(x + 1, y    ) == g2 ||
				GetCell(x,     y + 1) == g2
				)
			)
		{
			xs[cnt] = x;
			ys[cnt] = y;
			cnt++;
		}
	}
	return cnt;
}

static int Fnd_X;
static int Fnd_Y;

static int FindXY(int g) // ret: ? found
{
	int x;
	int y;

	for(x = 0; x < MapWH; x++)
	for(y = 0; y < MapWH; y++)
	{
		if(Map[x][y] == g)
		{
			Fnd_X = x;
			Fnd_Y = y;
			return 1;
		}
	}
	return 0;
}
static void FillG(int x, int y, int gR, int gW)
{
	if(
		0 <= x && x < MapWH &&
		0 <= y && y < MapWH &&
		Map[x][y] == gR
		)
	{
		Map[x][y] = gW;

		FillG(x - 1, y,     gR, gW);
		FillG(x,     y - 1, gR, gW);
		FillG(x + 1, y,     gR, gW);
		FillG(x,     y + 1, gR, gW);
	}
}
static void ChangeG(int gR, int gW)
{
	int x;
	int y;

	for(x = 0; x < MapWH; x++)
	for(y = 0; y < MapWH; y++)
	{
		if(Map[x][y] == gR)
			Map[x][y] = gW;
	}
}
static int IsBundan_G(int g)
{
	int ret;

	errorCase(!FindXY(g));
	FillG(Fnd_X, Fnd_Y, g, -1);
	ret = FindXY(g);
	ChangeG(-1, g);
	return ret;
}
static int IsBundan(void)
{
	int g;

	for(g = 1; g <= MapWH; g++)
		if(IsBundan_G(g))
			return 1;

	return 0;
}
static int IsLinear_G(int g)
{
	int minX = MapWH;
	int minY = MapWH;
	int maxX = -1;
	int maxY = -1;
	int x;
	int y;

	for(x = 0; x < MapWH; x++)
	for(y = 0; y < MapWH; y++)
	{
		if(Map[x][y] == g)
		{
			m_minim(minX, x);
			m_minim(minY, y);
			m_maxim(maxX, x);
			m_maxim(maxY, y);
		}
	}
	return
		minX == 0 && maxX == MapWH - 1 ||
		minY == 0 && maxY == MapWH - 1;
}
static int IsLinear(void)
{
	int g;

	for(g = 1; g <= MapWH; g++)
		if(IsLinear_G(g))
			return 1;

	return 0;
}
static void KonekuriMap_GG(int g1, int g2)
{
	int i1;
	int i2;

	G1Cnt = KM_FindXYs(g1, g2, G1Xs, G1Ys);
	G2Cnt = KM_FindXYs(g2, g1, G2Xs, G2Ys);

	if(!G1Cnt || !G2Cnt)
		return;

	i1 = GetRand_Mod(G1Cnt);
	i2 = GetRand_Mod(G2Cnt);

	// swap
	Map[G1Xs[i1]][G1Ys[i1]] = g2;
	Map[G2Xs[i2]][G2Ys[i2]] = g1;

	if(IsBundan() || IsLinear()) // ? グループの分断が生じた。|| 行・列と同じグループが生じた。
	{
		// revert
		Map[G1Xs[i1]][G1Ys[i1]] = g1;
		Map[G2Xs[i2]][G2Ys[i2]] = g2;
	}
}
static void KonekuriMap(void)
{
	int count;
	int g1;
	int g2;

	for(count = GetRand_Mod(20); count; count--)
	for(g2 = 2; g2 <= MapWH; g2++)
	for(g1 = 1; g1 < g2; g1++)
	{
		KonekuriMap_GG(g1, g2);
	}
}
static void MakeGroupCsvFile(char *dir)
{
	char *file = combine(dir, "Group.csv");
	FILE *fp;
	int g;

	fp = fileOpen(file, "wt");

	writeLine_x(fp, xcout("%d", MapWH));

	for(g = 1; g <= MapWH; g++)
	{
		int x;
		int y;

		writeLine_x(fp, xcout("%d", MapWH));

		for(x = 0; x < MapWH; x++)
		for(y = 0; y < MapWH; y++)
		{
			if(Map[x][y] == g)
			{
				writeLine_x(fp, xcout("%d,%d", x, y));
			}
		}
	}
	fileClose(fp);
	memFree(file);
}
static void MakePresetGroupCsvFile(char *dir)
{
	char *file = combine(dir, "ms2PresetGroup.csv");
	FILE *fp;
	int x;
	int y;

	fp = fileOpen(file, "wt");

	for(y = 0; y < MapWH; y++)
	{
		for(x = 0; x < MapWH; x++)
			writeToken_x(fp, xcout("%s,", Map[x][y] == 1 ? "1" : ""));

		writeLine(fp, "");
	}
	fileClose(fp);
	memFree(file);
}
static void MakeInputCsvFile(char *dir)
{
	char *file = combine(dir, "Input.csv");
	FILE *fp;
	int x;
	int y;
	int n = 1;

	fp = fileOpen(file, "wt");

	for(y = 0; y < MapWH; y++)
	{
		for(x = 0; x < MapWH; x++)
		{
			if(Map[x][y] == 1)
				writeToken_x(fp, xcout("%u", n++));

			writeToken(fp, ",");
		}
		writeLine(fp, "");
	}
	fileClose(fp);
	memFree(file);
}
static MkGroupCsvHB(char *dir, int block_w, int block_h)
{
	MapWH = block_w * block_h;
	errorCase(MAP_WH_MAX < MapWH);
	BlockW = block_w;
	BlockH = block_h;

	InitMap();
	KonekuriMap();

	MakeGroupCsvFile(dir);
	MakePresetGroupCsvFile(dir);
	MakeInputCsvFile(dir);
}
void MkGroupCsvHB_4x4(char *dir) // テスト用に作成
{
	MkGroupCsvHB(dir, 2, 2);
}
void MkGroupCsvHB_9x9(char *dir)
{
	MkGroupCsvHB(dir, 3, 3);
}
