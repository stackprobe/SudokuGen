typedef struct Cell_st
{
	/*
		0 == ���m��
		1 �` IMAX == �m��l
		1 �` ���̃Z����������ŏ��̃O���[�v�̃Z���� == �m��l @ CheckData()
	*/
	int FixedValue;

	autoList<struct CellCond_st *> *Conds; // �����̏d���i�V
	autoList<struct GroupLink_st *> *LinkedGroups; // ����: 0 == �g�O

	int MaxValue; // ���̃Z����������ŏ��̃O���[�v�̃Z���� @ CheckData()

	// <-- LoadData()

	i2D_t Pos;
	int PosGot;

	int BFSC_Ordered;

	uchar *BF_KnownPossibles;
}
Cell_t;

Cell_t *CreateCell(void);
void ReleaseCell(Cell_t *cell);

i2D_t CellToPos(Cell_t *cell);
int CompCellPos(Cell_t *cell1, Cell_t *cell2);
int IsPossibleValueInCell(Cell_t *cell, int value);
int IsSolvableValueInCell(Cell_t *cell, int value);
autoList<struct Group_st *> *GetAroundGroups(Cell_t *cell);
