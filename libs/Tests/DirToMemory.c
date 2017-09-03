#include "C:\Factory\Common\all.h"
#include "..\DirToMemory.h"

#define DEST_DIR "C:\\temp\\DTM_Test"

int main(int argc, char **argv)
{
	DirToMemory(c_dropDir());
	createDirIfNotExist(DEST_DIR);
	MemoryToDir(DEST_DIR);
	coExecute("START \"\" \"" DEST_DIR "\"");
}
