/*
	{
		aaa

		<-- AAA()

		bbb

		<-- BBB()

		ccc

		<-- CCC()

		ddd
	}

	aaa �� AAA() �I�����܂łɏ�����(�f�[�^���Z�b�g)�����B
	bbb �� BBB() �I�����܂łɏ�����(�f�[�^���Z�b�g)�����B
	ccc �� CCC() �I�����܂łɏ�����(�f�[�^���Z�b�g)�����B

	... �Ƃ������ɋL�q�����������B
*/

typedef struct Dc_st
{
	HANDLE EvStop;
	HANDLE EvBruteForceStarted;

	char *ProcessPhase;

	char *FrameCsvFile;
	char *SquareCsvFile;
	char *GroupCsvFile;
	char *XInequalCsvFile;
	char *YInequalCsvFile;
	char *XJointCsvFile;
	char *YJointCsvFile;
	char *CondCsvFile;
	char *RelCsvFile;
	char *InputCsvFile;
	char *OutputCsvFile;
	char *ErrorCsvFile;
	char *LoadedDataCsvFile;

	int ForceOutputLoadedData;
	int NoBruteForce;
	int NoFix1;
	int NoFix2;
	int CountAnswerMode;
	int OutputBeforeBruteForce;

	// <-- main()

	autoTable<Cell_t *> *Field;
	autoList<Group_t *> *Groups;

	// <-- LoadData()

	autoList<Cell_t *> *Cells;
	int MaxValueMax;
	int *Possibles;
}
Dc_t;

extern Dc_t Dc;
