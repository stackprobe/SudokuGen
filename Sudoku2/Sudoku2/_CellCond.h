typedef enum CellCondKind_et
{
	CCK_DUMMY,       // �����l
	CCK_ODDNUM,      // �ڑ��i�V
	CCK_EVENNUM,     // �ڑ��i�V
	CCK_LESSTHAN,    // �ڑ��A��
	CCK_GREATERTHAN, // �ڑ��A��
	CCK_JOINTED,     // �ڑ��A��
	CCK_UNJOINTED,   // �ڑ��A��
}
CellCondKind_t;

typedef struct CellCond_st
{
	CellCondKind_t Kind;
	Cell_t *OtherCell; // NULL == �ڑ��i�V, ref

	// <-- LoadData()
}
CellCond_t;

CellCond_t *CreateCellCond(void);
void ReleaseCellCond(CellCond_t *i);

int IsSameCellCond(CellCond_t *cond1, CellCond_t *cond2);
int IsPossibleValueInCellCond(CellCond_t *cond, int value);
int IsPossibleValueInCellConds(autoList<CellCond_t *> *conds, int value);
