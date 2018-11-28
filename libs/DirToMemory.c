#include "DirToMemory.h"

static autoBlock_t *Memory;
static uint RPos;

static void DTM_Writer(uchar *block, uint size)
{
	ab_addBlock(Memory, block, size);
}
static void MTD_Reader(uchar *block, uint size)
{
	errorCase(getSize(Memory) - RPos < size);

	memcpy(block, b_(Memory) + RPos, size);
	RPos += size;
}
void DirToMemory(char *dir)
{
	errorCase(!existDir(dir));

	LOGPOS();

	if(!Memory)
		Memory = newBlock();

	setSize(Memory, 0);
	DirToStream(dir, DTM_Writer);

	LOGPOS();
}
void MemoryToDir(char *dir)
{
	errorCase(!existDir(dir));
	errorCase(!Memory);

	LOGPOS();

	recurClearDir(dir);
	RPos = 0;
	StreamToDir(dir, MTD_Reader);

	LOGPOS();
}
