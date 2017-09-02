#include "C:\Factory\Common\all.h"
#include "..\GenRand.h"
#include "..\MkConditionCsvGK.h"

static void MkDummyInputCsv(char *dataDir, uint w, uint h)
{
	char *inputCsvFile = combine(dataDir, "Input.csv");
	FILE *fp;
	uint x;
	uint y;

	fp = fileOpen(inputCsvFile, "wt");

	for(y = 0; y < h; y++)
	{
		for(x = 0; x < w; x++)
			writeToken_x(fp, xcout("%u,", GetRand_Mod(20) + 1)); // ”Žš‚Í“K“–

		writeToken(fp, "\n");
	}
	fileClose(fp);
	memFree(inputCsvFile);
}
int main(int argc, char **argv)
{
	char *dataDir = getOutFile("MkConditionCsvGK_Test");

	createDir(dataDir);
	MkDummyInputCsv(dataDir, 9, 9);
	MkConditionCsvGK(dataDir, 25);
	openOutDir();
	memFree(dataDir);
}
