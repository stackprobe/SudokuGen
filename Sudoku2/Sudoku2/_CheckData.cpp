#include "all.h"

void CheckData(void)
{
	Dc.ProcessPhase = "�f�[�^�̃`�F�b�N��";

	errorCase_m(Dc.Groups->GetCount() == 0, "�O���[�v���P�������B");

	for(int index = 0; index < Dc.Groups->GetCount(); index++)
	{
		Group_t *group = Dc.Groups->GetElement(index);

		group->Cells->Sort(CompCellPos);

		for(int cellIndex = 1; cellIndex < group->Cells->GetCount(); cellIndex++)
		{
			Cell_t *cell1 = group->Cells->GetElement(cellIndex - 1);
			Cell_t *cell2 = group->Cells->GetElement(cellIndex);

			if(cell1 == cell2) // ? �Z���̏d��
			{
				AddErrorPos(CellToPos(cell1));
				error_m("�O���[�v���ŃZ�����d�����Ă���B");
			}
		}
		for(int cellIndex = 0; cellIndex < group->Cells->GetCount(); cellIndex++)
		{
			Cell_t *cell = group->Cells->GetElement(cellIndex);

			if(cell->FixedValue)
			{
				AddErrorPos(CellToPos(cell));
				errorCase_m(group->Cells->GetCount() < cell->FixedValue, "�O���[�v�̃Z�����i�ő�l�j���傫�Ȑ������L������Ă���B");

				for(int nearPos = 0; nearPos < cellIndex; nearPos++)
				{
					Cell_t *nearCell = group->Cells->GetElement(nearPos);

					AddErrorPos(CellToPos(nearCell));
					errorCase_m(nearCell->FixedValue == cell->FixedValue, "�O���[�v���ɓ����������Q�ȏ゠��B");
					UnaddErrorPos();
				}
				UnaddErrorPos();
			}
		}
	}
restartDeleteSameGroup:
	for(int ndx2 = 1; ndx2 < Dc.Groups->GetCount(); ndx2++)
	for(int ndx1 = 0; ndx1 < ndx2; ndx1++)
	{
		Group_t *group1 = Dc.Groups->GetElement(ndx1);
		Group_t *group2 = Dc.Groups->GetElement(ndx2);

		if(IsSameGroup(group1, group2)) // ? �O���[�v�̏d�� -> �Е����폜
		{
			Dc.Groups->FastDesertElement(ndx2);
			ReleaseGroup(group2);
			goto restartDeleteSameGroup;
		}
	}
	for(int index = 0; index < Dc.Groups->GetCount(); index++) // init LinkedGroups
	{
		Group_t *group = Dc.Groups->GetElement(index);

		for(int cellIndex = 0; cellIndex < group->Cells->GetCount(); cellIndex++)
		{
			Cell_t *cell = group->Cells->GetElement(cellIndex);
			GroupLink_t *gl = CreateGroupLink();

			gl->Group = group;
			gl->Index = cellIndex;

			cell->LinkedGroups->AddElement(gl);
		}
	}

	// <-- Group

	// Cell -->

	for(int x = 0; x < Dc.Field->GetWidth(); x++)
	for(int y = 0; y < Dc.Field->GetHeight(); y++)
	{
		Cell_t *cell = Dc.Field->GetCell(x, y);

		for(int condIndex = 0; condIndex < cell->Conds->GetCount(); condIndex++)
		{
			CellCond_t *cond = cell->Conds->GetElement(condIndex);
			CellCondKind_t revKind = CCK_DUMMY;

			switch(cond->Kind)
			{
			case CCK_LESSTHAN: revKind = CCK_GREATERTHAN; break;
			case CCK_GREATERTHAN: revKind = CCK_LESSTHAN; break;
			case CCK_JOINTED: revKind = CCK_JOINTED; break;
			case CCK_UNJOINTED: revKind = CCK_UNJOINTED; break;

			default:
				break;
			}
			if(revKind)
			{
				errorCase(!cond->OtherCell);

				CellCond_t *occ = CreateCellCond();

				occ->Kind = revKind;
				occ->OtherCell = cell;

				cond->OtherCell->Conds->AddElement(occ);
			}
		}
		cell->MaxValue = IMAX;

		for(int glIndex = 0; glIndex < cell->LinkedGroups->GetCount(); glIndex++)
		{
			GroupLink_t *gl = cell->LinkedGroups->GetElement(glIndex);

			m_minim(cell->MaxValue, gl->Group->Cells->GetCount());
		}
	}
restartDeleteSameCellCond:
	for(int x = 0; x < Dc.Field->GetWidth(); x++)
	for(int y = 0; y < Dc.Field->GetHeight(); y++)
	{
		Cell_t *cell = Dc.Field->GetCell(x, y);

		for(int ndx2 = 1; ndx2 < cell->Conds->GetCount(); ndx2++)
		for(int ndx1 = 0; ndx1 < ndx2; ndx1++)
		{
			CellCond_t *cond1 = cell->Conds->GetElement(ndx1);
			CellCond_t *cond2 = cell->Conds->GetElement(ndx2);

			if(IsSameCellCond(cond1, cond2)) // �����̏d�� -> �Е����폜
			{
				cell->Conds->FastDesertElement(ndx2);
				ReleaseCellCond(cond2);
				goto restartDeleteSameCellCond;
			}
		}
	}
	for(int x = 0; x < Dc.Field->GetWidth(); x++)
	for(int y = 0; y < Dc.Field->GetHeight(); y++)
	{
		Cell_t *cell = Dc.Field->GetCell(x, y);

		if(cell->LinkedGroups->GetCount() == 0) // ? �g�O
		{
			AddErrorPos(CellToPos(cell));
			errorCase_m(cell->FixedValue, "�g�O�̃Z���ɐ������L������Ă���B");
			errorCase_m(cell->Conds->GetCount(), "�g�O�̃Z���ɏ������ݒ肳��Ă���B");
			UnaddErrorPos();
		}
		if(cell->FixedValue) // ? �K��l
		{
			AddErrorPos(CellToPos(cell));
			errorCase(!m_isRange(cell->FixedValue, 1, IMAX)); // 2bs
			errorCase_m(cell->MaxValue < cell->FixedValue, "����l���傫�����܂��B");

			for(int condIndex = 0; condIndex < cell->Conds->GetCount(); condIndex++)
			{
				CellCond_t *cond = cell->Conds->GetElement(condIndex);

				switch(cond->Kind)
				{
				case CCK_ODDNUM:
					errorCase_m(cell->FixedValue % 2 == 0, "�K��l�͊�łȂ���΂Ȃ�܂���B");
					break;

				case CCK_EVENNUM:
					errorCase_m(cell->FixedValue % 2 == 1, "�K��l�͋����łȂ���΂Ȃ�܂���B");
					break;

				default:
					break;
				}
			}
			UnaddErrorPos();
		}
	}
}
