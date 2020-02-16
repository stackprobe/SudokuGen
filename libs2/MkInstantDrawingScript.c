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
	Cell_t *i = nb_(Cell_t);

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
	Line_t *i = nb_(Line_t);

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

static sint CompLine(Line_t *a, Line_t *b)
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
	char *line;
	uint frmnum;
	uint frmidx;

	line = neReadLine(fp);
	frmnum = toValue(line);
	memFree(line);

	for(frmidx = 0; frmidx < frmnum; frmidx++)
	{
		char *s1;
		char *s2;
		uint bL;
		uint bT;
		uint bW;
		uint bH;
		uint xB;
		uint yB;

		line = neReadLine(fp);

		s1 = toknext(line, ",");
		s2 = toknext(NULL, NULL);
		errorCase(!s2);

		bL = toValue(s1);
		bT = toValue(s2);

		memFree(line);

		line = neReadLine(fp);

		s1 = toknext(line, ",");
		s2 = toknext(NULL, NULL);
		errorCase(!s2);

		bW = toValue(s1);
		bH = toValue(s2);

		memFree(line);

		for(xB = 0; xB < bH; xB++) // X •ûŒü‚É bH ŒÂ
		for(yB = 0; yB < bW; yB++) // Y •ûŒü‚É bW ŒÂ
		{
			autoList_t *group = newList();
			uint x;
			uint y;

			for(x = 0; x < bW; x++)
			for(y = 0; y < bH; y++)
			{
				addElement(group, (uint)CreateCell(xB * bW + x, yB * bH + y));
			}
			addElement(Groups, (uint)group);
		}
	}
	fileClose(fp);
}
static void LoadGroupFile(char *file)
{
	FILE *fp = fileOpen(file, "rt");
	char *line;
	uint grpnum;
	uint grpidx;

	line = neReadLine(fp);
	grpnum = toValue(line);
	memFree(line);

	for(grpidx = 0; grpidx < grpnum; grpidx++)
	{
		autoList_t *group = newList();
		uint cellnum;
		uint cellidx;

		line = neReadLine(fp);
		cellnum = toValue(line);
		memFree(line);

		for(cellidx = 0; cellidx < cellnum; cellidx++)
		{
			char *sx;
			char *sy;
			uint x;
			uint y;

			line = neReadLine(fp);

			sx = toknext(line, ",");
			sy = toknext(NULL, NULL);
			errorCase(!sy);

			x = toValue(sx);
			y = toValue(sy);

			memFree(line);

			addElement(group, (uint)CreateCell(x, y));
		}
		addElement(Groups, (uint)group);
	}
	fileClose(fp);
}

// ==== use int ====

static int HasCellInGroup(autoList_t *group, int x, int y)
{
	Cell_t *cell;
	int index;

	if(x < 0 || y < 0)
		return 0;

	foreach(group, cell, index)
		if(cell->X == x && cell->Y == y)
			return 1;

	return 0;
}
static int ExistsCell(int x, int y)
{
	autoList_t *group;
	int index;

	foreach(Groups, group, index)
		if(HasCellInGroup(group, x, y))
			return 1;

	return 0;
}
static void CellToLines(autoList_t *group, Cell_t *cell, int x, int y, int x1, int y1, int x2, int y2)
{
	Line_t *line = CreateLine(x1, y1, x2, y2);

	if(HasCellInGroup(group, x, y))
		addElement(InnerLines, (uint)line);
	else if(ExistsCell(x, y))
		addElement(GroupLines, (uint)line);
	else
		addElement(OuterLines, (uint)line);
}
static void GroupsToLines(void)
{
	autoList_t *group;
	Cell_t *cell;
	int grpidx;
	int cellidx;

	foreach(Groups, group, grpidx)
	foreach(group, cell, cellidx)
	{
		int x = cell->X;
		int y = cell->Y;

		CellToLines(group, cell, x - 1, y,     x,     y,     x,     y + 1);
		CellToLines(group, cell, x,     y - 1, x,     y,     x + 1, y    );
		CellToLines(group, cell, x + 1, y,     x + 1, y,     x + 1, y + 1);
		CellToLines(group, cell, x,     y + 1, x,     y + 1, x + 1, y + 1);
	}
}

// ====

static int HasLine(autoList_t *lines, Line_t *target)
{
	Line_t *line;
	uint index;

	foreach(lines, line, index)
		if(!CompLine(line, target))
			return 1;

	return 0;
}
static void EraseOverlapLines(autoList_t *lines, autoList_t *overLines)
{
	Line_t *line;
	uint index;

	foreach(lines, line, index)
	{
		if(HasLine(overLines, line))
		{
			ReleaseLine(line);
			setElement(lines, index, 0);
		}
	}
	removeZero(lines);
}
static void ArrangeLines(autoList_t *lines)
{
	Line_t *a;
	Line_t *b;
	uint a_index;
	uint b_index;

restart:
	foreach(lines, a, a_index)
	foreach(lines, b, b_index)
	if(a != b)
	{
		if(
			a->X1 == a->X2 &&
			a->X1 == b->X1 &&
			a->X1 == b->X2 &&
			a->Y2 == b->Y1
			)
		{
			a->Y2 = b->Y2;
			ReleaseLine(b);
			fastDesertElement(lines, b_index);
			goto restart;
		}
		if(
			a->Y1 == a->Y2 &&
			a->Y1 == b->Y1 &&
			a->Y1 == b->Y2 &&
			a->X2 == b->X1
			)
		{
			a->X2 = b->X2;
			ReleaseLine(b);
			fastDesertElement(lines, b_index);
			goto restart;
		}
	}
	distinct2(lines, (sint (*)(uint, uint))CompLine, (void (*)(uint))ReleaseLine);
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
	int gk = 0;

	LOGPOS();

	FrameFile = combine(dataDir, "Frame.csv");
	GroupFile = combine(dataDir, "Group.csv");
	ConditionFile = combine(dataDir, "Condition.csv");
	ScriptFile = combine(dataDir, "InstantDrawingScript.txt");

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

	if(existFile(ConditionFile))
	{
		ReadConditionGK();
		gk = 1;
	}
	DrawLines(OuterLines, 3);
	DrawLines(GroupLines, 2);
	DrawLines(InnerLines, 1);

	if(gk)
		addElement(ScrLines, (uint)strx("9"));

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

	LOGPOS();
}
