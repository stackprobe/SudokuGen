#include "C:\Factory\Common\all.h"
#include "..\MkGroupCsvHB.h"
#include "..\MkInstantDrawingScript.h"

int main(int argc, char **argv)
{
	char *dataDir = getOutFile("MkInstantDrawingScript_Test");

	createDir(dataDir);
	MkGroupCsvHB_9x9(dataDir);
	MkInstantDrawingScript(dataDir);
	openOutDir();
	memFree(dataDir);
}
