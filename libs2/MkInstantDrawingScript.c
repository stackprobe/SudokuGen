#include "MkInstantDrawingScript.h"

// ---- Cell ----

typedef struct Cell_st
{
	uint X;
	uint Y;
}
Cell_t;

static Cell_t *CreateCell(uint x, uint y)
{
	Cell_t *i = nb(Cell_t);

	i->X = x;
	i->Y = y;

	return i;
}
static void ReleaseCell(Cell_t *i)
{
	memFree(i);
}

// ---- Line ----

/*
	X1 <= X2
	Y1 <= Y2
*/
typedef struct Line_st
{
	uint X1;
	uint Y1;
	uint X2;
	uint Y2;
}
Line_t;

static Line_t *CreateLine(uint x1, uint y1, uint x2, uint y2)
{
	Line_t *i = nb(Line_t);

	i->X1 =x1;
	i->Y1 =y1;
	i->X2 =x2;
	i->Y2 =y2;

	return i;
}
static void ReleaseLine(Line_t *i)
{
	memFree(i);
}

static int CompLine(Line_t *a, Line_t *b)
{
	int ret;

	ret = simpleComp(a->X1, b->X1);
	if(ret)
		return ret;

	ret = simpleComp(a->Y1, b->Y1);
	if(ret)
		return ret;

	ret = simpleComp(a->X2, b->X2);
	if(ret)
		return ret;

	ret = simpleComp(a->Y2, b->Y2);
	if(ret)
		return ret;

	return 0;
}

// ----

static char *FrameFile;
static char *GroupFile;
static char *ConditionFile;
static char *ScriptFile;

static autoList_t *Groups;

static autoList_t *OuterLines;
static autoList_t *GroupLines;
static autoList_t *InnerLines;

static autoList_t *ScrLines;

static void LoadFrameFile(char *file)
{
	FILE *fp = fileOpen(file, "rt");

	for(; ; )
	{
		char *line = readLine(fp);

		if(!line)
			break;

		error(); // TODO -> Groups
	}
	fileClose(fp);
}
static void LoadGroupFile(char *file)
{
	FILE *fp = fileOpen(file, "rt");

	for(; ; )
	{
		char *line = readLine(fp);

		if(!line)
			break;

		error(); // TODO -> Groups
	}
	fileClose(fp);
}
static void GroupsToLines(void)
{
	error(); // TODO Groups -> OuterLines, GroupLines, InnerLines
}
static void EraseOverlapLines(autoList_t *lines, autoList_t *overLines)
{
	error(); // TODO
}
static void ArrangeLines(autoList_t *lines)
{
	error(); // TODO
}
static void GetMaxXYFromLines(autoList_t *lines, uint *p_x, uint *p_y)
{
	Line_t *line;
	uint index;
	uint x = 0;
	uint y = 0;

	foreach(lines, line, index)
	{
		m_maxim(x, line->X2);
		m_maxim(y, line->Y2);
	}
	*p_x = x;
	*p_y = y;
}
static void DrawLines(autoList_t *lines, uint command)
{
	Line_t *line;
	uint index;

	foreach(lines, line, index)
	{
		addElement(ScrLines, (uint)xcout("%u %u %u %u %u", command, line->X1, line->Y1, line->X2, line->Y2));
	}
}
static void ReadConditionGK(void)
{
	autoList_t *rows = readCSVFile(ConditionFile);
	autoList_t *row;
	char *cell;
	uint x;
	uint y;

	foreach(rows, row, y)
	foreach(row, cell, x)
	{
		uint value = toValue(cell);

		if(value == 1 || value == 2)
		{
			addElement(
				ScrLines,
				(uint)xcout("%u %u %u %u %u", 6 + value, x, y, x + 1, y + 1)
				);
		}
	}
	releaseDim(rows, 2);
}
void MkInstantDrawingScript(char *dataDir)
{
	FrameFile = combine(dataDir, "Frame.csv");
	GroupFile = combine(dataDir, "Group.csv");
	ConditionFile = combine(dataDir, "Condition.csv");
	ScriptFile = combine(dataDir, "InnerDrawingScript.txt");

	Groups = newList();

	if(existFile(FrameFile))
		LoadFrameFile(FrameFile);

	if(existFile(GroupFile))
		LoadGroupFile(GroupFile);

	OuterLines = newList();
	GroupLines = newList();
	InnerLines = newList();

	GroupsToLines();

	EraseOverlapLines(GroupLines, OuterLines);
	EraseOverlapLines(InnerLines, OuterLines);
	EraseOverlapLines(InnerLines, GroupLines);

	ArrangeLines(OuterLines);
	ArrangeLines(GroupLines);
	ArrangeLines(InnerLines);

	ScrLines = newList();

	{
		uint x;
		uint y;

		GetMaxXYFromLines(OuterLines, &x, &y);

		addElement(ScrLines, (uint)xcout("0 %u %u", x, y));
	}

	DrawLines(OuterLines, 3);
	DrawLines(GroupLines, 2);
	DrawLines(InnerLines, 1);

	if(existFile(ConditionFile))
		ReadConditionGK();

	writeLines(ScriptFile, ScrLines);

	releaseDim(ScrLines, 1);

	releaseDim_BR(OuterLines, 1, (void (*)(void *))ReleaseLine);
	releaseDim_BR(GroupLines, 1, (void (*)(void *))ReleaseLine);
	releaseDim_BR(InnerLines, 1, (void (*)(void *))ReleaseLine);

	releaseDim_BR(Groups, 2, (void (*)(void *))ReleaseCell);

	memFree(FrameFile);
	memFree(GroupFile);
	memFree(ConditionFile);
	memFree(ScriptFile);
}
