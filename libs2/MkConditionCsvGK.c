#include "MkConditionCsvGK.h"

static uint GetNumbCount(autoList_t *rows)
{
	autoList_t *row;
	char *cell;
	uint rowidx;
	uint colidx;
	uint count = 0;

	foreach(rows, row, rowidx)
	foreach(row, cell, colidx)
	{
		if(*cell)
			count++;
	}
	return count;
}
static char *FindCellByIndex(autoList_t *rows, uint index)
{
	autoList_t *row;
	char *cell;
	uint rowidx;
	uint colidx;

	foreach(rows, row, rowidx)
	foreach(row, cell, colidx)
	{
		if(*cell)
		{
			if(!index)
				return cell;

			index--;
		}
	}
	error(); // never
	return NULL; // dummy
}
static void EraseNumbs(autoList_t *rows, uint count, uint targetCount)
{
	for(; targetCount < count; count--)
	{
		*FindCellByIndex(rows, GetRand_Mod(count)) = '\0';
	}
}
static void ToOddEvens(autoList_t *rows)
{
	autoList_t *row;
	char *cell;
	uint rowidx;
	uint colidx;

	foreach(rows, row, rowidx)
	foreach(row, cell, colidx)
	{
		if(*cell)
		{
			cell[0] = '2' - (strchr(cell, '\0')[-1] & 1);
			cell[1] = '\0';
		}
	}
}
static void MkPriorityFile(char *dataDir)
{
	char *rFile = combine(dataDir, "Condition.csv");
	char *wFile = combine(dataDir, "ms2kPriority.csv");
	autoList_t *rows;
	autoList_t *row;
	char *cell;
	uint rowidx;
	uint colidx;

	LOGPOS();

	rows = readCSVFileTR(rFile);

	foreach(rows, row, rowidx)
	foreach(row, cell, colidx)
	{
		if(*cell)
			strcpy(cell, "1");
	}
	writeCSVFile(wFile, rows);

	releaseDim(rows, 1);
	memFree(rFile);
	memFree(wFile);

	LOGPOS();
}
void MkConditionCsvGK(char *dataDir, uint incidencePct)
{
	char *rFile = combine(dataDir, "Input.csv");
	char *wFile = combine(dataDir, "Condition.csv");
	char *delFile = combine(dataDir, "ms2PresetGroup.csv");
	autoList_t *rows;
	uint numbCnt;
	uint targetNumbCnt;

	rows = readCSVFileTR(rFile);

	numbCnt = GetNumbCount(rows);
	targetNumbCnt = divRndOff(numbCnt * incidencePct, 100);
	EraseNumbs(rows, numbCnt, targetNumbCnt);
	ToOddEvens(rows);

	writeCSVFile(wFile, rows);

	removeFileIfExist(delFile);

	memFree(rFile);
	memFree(wFile);
	memFree(delFile);
	releaseDim(rows, 2);

	if(incidencePct < 100)
	{
		MkPriorityFile(dataDir);
	}
}
