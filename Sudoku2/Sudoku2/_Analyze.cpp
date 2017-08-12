#include "all.h"

static int PC_GetNumb(int rr, int cc) // ret: -1 == ���O, 0 == ��, 1�` == �m��
{
	for(int index = 0; index < Dc.Cells->GetCount(); index++)
	{
		if(
			Dc.Cells->GetElement(index)->Pos.X == cc &&
			Dc.Cells->GetElement(index)->Pos.Y == rr
			)
			return Dc.Cells->GetElement(index)->FixedValue;
	}
	return -1; // ���O
}
static void PrintCells(void)
{
	int rr_max = 0;
	int cc_max = 0;

	cout("---- Cells ----\n");

	for(int index = 0; index < Dc.Cells->GetCount(); index++)
	{
		m_maxim(cc_max, Dc.Cells->GetElement(index)->Pos.X);
		m_maxim(rr_max, Dc.Cells->GetElement(index)->Pos.Y);
	}
	for(int rr = 0; rr <= rr_max; rr++)
	{
		for(int cc = 0; cc < cc_max; cc++)
		{
			int numb = PC_GetNumb(rr, cc);

			if(numb == -1)
				cout("�@");
			else if(numb == 0)
				cout("��");
			else
				cout("%2d", numb);
		}
		cout("#\n");
	}
	cout("----\n");
}
static void CheckChuudan(void)
{
	DWORD ret = WaitForSingleObject(Dc.EvStop, 0);

	if(ret == WAIT_OBJECT_0)
	{
		error_m("���f���܂����B");
	}
}
static int BFSC_GetScore(Cell_t *cell)
{
	int score = 0;

	for(int index = 0; index < cell->LinkedGroups->GetCount(); index++)
	{
		GroupLink_t *gl = cell->LinkedGroups->GetElement(index);

		for(int cellIndex = 0; cellIndex < gl->Group->Cells->GetCount(); cellIndex++)
		{
			if(cellIndex != gl->Index) // ? �����ł͂Ȃ��B
			{
				Cell_t *glCell = gl->Group->Cells->GetElement(cellIndex);

				if(glCell->BFSC_Ordered)
				{
					score += 10000;
				}
			}
		}
	}
	for(int value = 1; value <= cell->MaxValue; value++)
	{
		if(IsPossibleValueInCell(cell, value))
		{
			score -= 100;
		}
	}
	return score + cell->Conds->GetCount();
}
static void BF_SortCells(void)
{
	autoList<Cell_t *> *newOrder = new autoList<Cell_t *>();

	while(Dc.Cells->GetCount())
	{
		int bestPos = 0;
		int bestScore = BFSC_GetScore(Dc.Cells->GetElement(0));

		for(int index = 1; index < Dc.Cells->GetCount(); index++)
		{
			int score = BFSC_GetScore(Dc.Cells->GetElement(index));

			if(bestScore < score)
			{
				bestPos = index;
				bestScore = score;
			}
		}

		{
			Cell_t *cell = Dc.Cells->FastDesertElement(bestPos);
			newOrder->AddElement(cell);
			cell->BFSC_Ordered = 1;
		}
	}
	Dc.Cells->Change(newOrder);
	delete newOrder;
}
static void BruteForceSearch(void)
{
	if(Dc.OutputBeforeBruteForce)
		PrintCells();

	Dc.ProcessPhase = "��̓t�F�[�Y3(BRUTE_FORCE)";

	cout("Cells.1: %d\n", Dc.Cells->GetCount());

	for(int index = 0; index < Dc.Cells->GetCount(); index++)
	{
		Cell_t *cell = Dc.Cells->GetElement(index);

		if(cell->FixedValue) // ? �m�� -> ���O
		{
			Dc.Cells->FastDesertElement(index);
			index--;
		}
	}
	cout("Cells.2: %d\n", Dc.Cells->GetCount());

	errorCase_m(Dc.NoBruteForce && Dc.Cells->GetCount() != 0, "�������͕K�v_�I�v�V�����ɂ��G���[");

	BF_SortCells();

	for(int index = 0; index < Dc.Cells->GetCount(); index++)
	{
		Cell_t *cell = Dc.Cells->GetElement(index);

		cell->BF_KnownPossibles = na(uchar, cell->MaxValue);

		for(int value = 1; value <= cell->MaxValue; value++)
		{
			cell->BF_KnownPossibles[value - 1] = IsPossibleValueInCell(cell, value);
		}
	}
	int seekPos = 0;
	int maxSeekPos = 0;
	int answerCount = 0;

	SetEvent(Dc.EvBruteForceStarted);

startSearch:
	while(seekPos < Dc.Cells->GetCount())
	{
		Cell_t *cell = Dc.Cells->GetElement(seekPos);
		int value = cell->FixedValue;

		cell->FixedValue = 0;

		for(; ; )
		{
			value++;

			if(cell->MaxValue < value)
			{
				seekPos--;

				if(seekPos < 0) // ? ���i�V
				{
					AddErrorPos(CellToPos(Dc.Cells->GetElement(maxSeekPos)));

					if(Dc.CountAnswerMode)
						goto endSearch;

					error_m("�𖳂��B");
				}
				break;
			}
			if(cell->BF_KnownPossibles[value - 1] && IsPossibleValueInCell(cell, value))
			{
				cell->FixedValue = value;
				seekPos++;

				if(maxSeekPos < seekPos)
					cout("%.2f percent completed.\n", 50.0 + seekPos * 49.99 / Dc.Cells->GetCount());

				m_maxim(maxSeekPos, seekPos);
				break;
			}

			{
				static uint c;

				if((c++ & 0x3ffffff) == 0)
				{
					CheckChuudan();
				}
			}
		}
	}
	if(Dc.CountAnswerMode)
	{
		cout("Found Answer!\n");
		answerCount++;

		if(answerCount < 2 && 1 <= Dc.Cells->GetCount())
		{
			seekPos--;
			goto startSearch;
		}
	}
endSearch:
	if(Dc.CountAnswerMode)
	{
		cout("+------------------+\n");
		cout("| ANSWER COUNT : %u |\n", answerCount);
		cout("+------------------+\n");
	}
	cout("100.00 percent completed.\n");
}
void Analyze(void)
{
	Dc.ProcessPhase = "��̓t�F�[�Y";

	Dc.Cells = new autoList<Cell_t *>();
	Dc.MaxValueMax = 0;

	for(int x = 0; x < Dc.Field->GetWidth(); x++)
	for(int y = 0; y < Dc.Field->GetHeight(); y++)
	{
		Cell_t *cell = Dc.Field->GetCell(x, y);

		if(cell->LinkedGroups->GetCount()) // ? �g��
		{
			Dc.Cells->AddElement(cell);
			m_maxim(Dc.MaxValueMax, cell->MaxValue);
		}
	}
	Dc.Possibles = na(int, Dc.MaxValueMax);

	int presume = 0;

	for(; ; )
	{
		{
			int fixedCnt = 0;

			for(int index = 0; index < Dc.Cells->GetCount(); index++)
				if(Dc.Cells->GetElement(index)->FixedValue) // ? �m��
					fixedCnt++;

			cout("%.2f percent completed.\n", fixedCnt * 50.0 / Dc.Cells->GetCount());
		}

		int modified = 0;

		for(int index = 0; index < Dc.Cells->GetCount(); index++)
		{
			if(Dc.NoFix1)
			{
				cout("Fix.1_BLOCKED!\n");
				break;
			}
			Cell_t *cell = Dc.Cells->GetElement(index);

			if(cell->FixedValue == 0) // ? ���m��
			{
				memset(Dc.Possibles, 0xff, cell->MaxValue * sizeof(int));

				for(int glIndex = 0; glIndex < cell->LinkedGroups->GetCount(); glIndex++)
				{
					GroupLink_t *gl = cell->LinkedGroups->GetElement(glIndex);

					for(int cellIndex = 0; cellIndex < gl->Group->Cells->GetCount(); cellIndex++)
					{
						if(cellIndex != gl->Index) // ? �����ł͂Ȃ��B
						{
							Cell_t *glCell = gl->Group->Cells->GetElement(cellIndex);

							if(glCell->FixedValue) // ? �m��
							{
								Dc.Possibles[glCell->FixedValue - 1] = 0; // cell->MaxValue �ȍ~�ɂ������Ă��܂����A�o�b�t�@�͂���̂Ŗ�薳���B
							}
						}
					}
				}
				for(int possPos = 0; possPos < cell->MaxValue; possPos++)
				{
					if(Dc.Possibles[possPos])
					{
						Dc.Possibles[possPos] = IsPossibleValueInCellConds(cell->Conds, possPos + 1);
					}
				}

				{
					int possCnt = 0;
					int possPos;

					for(int posIndex = 0; posIndex < cell->MaxValue; posIndex++)
					{
						if(Dc.Possibles[posIndex])
						{
							possCnt++;

							if(possCnt == 2)
								break;

							possPos = posIndex;
						}
					}
					if(possCnt == 0) // ? ���̃Z���ɓ���l�������B== ���i�V
					{
						AddErrorPos(CellToPos(cell));
						error_m("�𖳂��B(���̏��ɓ���l������)");
					}
					if(possCnt == 1)
					{
						cell->FixedValue = possPos + 1;
						modified = 1;

						cout("Fix.1 (%d, %d) == %d\n", cell->Pos.X, cell->Pos.Y, cell->FixedValue);
					}
				}
			}
		}
		for(int index = 0; index < Dc.Groups->GetCount(); index++)
		{
			if(Dc.NoFix2)
			{
				cout("Fix.2_BLOCKED!\n");
				break;
			}
			Group_t *group = Dc.Groups->GetElement(index);

			for(int value = 1; value <= group->Cells->GetCount(); value++)
			{
				if(!IsFixedValueInGroup(group, value))
				{
					int possCnt = 0;
					int possPos;

					for(int cellIndex = 0; cellIndex < group->Cells->GetCount(); cellIndex++)
					{
						Cell_t *cell = group->Cells->GetElement(cellIndex);

						if(cell->FixedValue == 0 && IsPossibleValueInCell(cell, value) && (!presume || IsSolvableValueInCell(cell, value)))
						{
							possCnt++;

							if(possCnt == 2)
								break;

							possPos = cellIndex;
						}
					}
					if(possCnt == 0) // ? value ������Z���� group �ɑ��݂��Ȃ��B
					{
						for(int cellIndex = 0; cellIndex < group->Cells->GetCount(); cellIndex++)
						{
							AddErrorPos(CellToPos(group->Cells->GetElement(cellIndex)));
						}
						error_m("�𖳂��B(���̃O���[�v�ɓ���Ȃ�����������)");
					}
					if(possCnt == 1)
					{
						Cell_t *cell = group->Cells->GetElement(possPos);

						cell->FixedValue = value;
						modified = 1;

						cout("Fix.2 (%d, %d) == %d\n", cell->Pos.X, cell->Pos.Y, cell->FixedValue);
					}
				}
			}
		}
		if(!modified)
		{
			if(presume)
				break;

			Dc.ProcessPhase = "��̓t�F�[�Y2(PRESUME)";
			presume = 1;
		}
		CheckChuudan();
	}
	BruteForceSearch();
}
