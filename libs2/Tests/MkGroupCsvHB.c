#include "C:\Factory\Common\all.h"
#include "..\MkGroupCsvHB.h"

int main(int argc, char **argv)
{
	if(argIs("/4"))
	{
		char *dataDir = getOutFile("MkGroupCsvHB_Test");

		createDir(dataDir);
		MkGroupCsvHB_4x4(dataDir);
		execute_x(xcout("START \"\" \"%s\"", dataDir));
		memFree(dataDir);
		return;
	}

//	if(argIs("/9"))
	{
		char *dataDir = getOutFile("MkGroupCsvHB_Test");

		createDir(dataDir);
		MkGroupCsvHB_9x9(dataDir);
		execute_x(xcout("START \"\" \"%s\"", dataDir));
		memFree(dataDir);
		return;
	}
}
