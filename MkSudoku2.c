/*
	MkSudoku2.exe [/N2] [/Lv:1 | /Lv:2 | /Lv:3 | /Lv:4] [入力DIR]

	----
	2017.2.3

	/-F1 で作成した問題は、/-F1 では総当り不要、/-F2 では総当りになる。
	/-F2 で作成した問題は、/-F1, /-F2 で総当り不要。

	なので、難しさは /-F2 < /-F1 なんだろうなぁ～と思った。
*/

#include "C:\Factory\Common\all.h"
#include "C:\Factory\Common\Options\csv.h"
#include "C:\Factory\Common\Options\CryptoRand_MS.h"
#include "C:\Factory\SubTools\libs\Nectar2.h"
#include "libs\Mk9x9.h"
#include "libs\KnownPosNumb.h"
#include "libs2\MkGroupCsvHB.h"
#include "libs2\MkConditionCsvGK.h"
#include "libs2\MkInstantDrawingScript.h"

#define EV_BRUTE_FORCE_STARTED "{5a2e723d-aece-44a2-8c23-9bdbdfa6d277}" // shared_uuid
#define EV_TOM_STARTED "{fb2c4966-d2e2-49b7-803c-ac3eb791f098}"
#define EV_TOM_STOP    "{af9132ed-48f0-41d9-87f3-e3d1d4ca88c5}"
#define EV_TOM_STOPPED "{8f097143-2a54-4925-945a-79664543d2b9}"
#define EV_TIMED_OUT   "{a95dec28-559a-4510-ac26-132e573704fe}"

#define TRUSTED_TIMEOUT_MILLIS 30000 // タイムアウト前にシグナルが必ず来る「はず」の待ち用
#define PUT_RAND_TIMEOUT_SEC 3600
#define BRUTE_FORCE_TIMEOUT_SEC 30
#define RAND_BUFF_SIZE 8000

static int UseN2;
static Nectar2_t *N2Sender;
static int Gkbnso; // 合体ナンプレの重なった部分に成丈数字を置かない。

enum
{
	DIFFICULTY_EASY_1 = 1, // Fix.2 抑止
	DIFFICULTY_EASY_2 = 2, // Fix.1 抑止
	DIFFICULTY_NORMAL = 3,
	DIFFICULTY_HARD = 4,
};

static int Difficulty = DIFFICULTY_EASY_1;

static char *DataDir;
static char *InputFile;
static char *OutputFile;
static char *ErrorFile;
static char *LoadedDataFile;
static char *ExtraPresetGroupFile;
static char *ExtraKezuruPriorityFile;
static char *ExtraGenDataFile;

static uint EvCancel;
static autoList_t *AnswerTable;
static uint Ans_W;
static uint Ans_H;
static uint NumbMax;
static autoList_t *InputTable;

static uint Cnt_P;
static uint Cnt_K;
static uint Cnt_X;

enum
{
	RET_SUCCESSFUL = 1,
	RET_FAILED,
	RET_TIMED_OUT,
};

static uint GetNumb(autoList_t *table, uint x, uint y)
{
	return toValue(getLine(getList(table, y), x));
}
static uint RefNumb(autoList_t *table, uint x, uint y)
{
	if(getCount(table) <= y)
		return 0;

	if(getCount(getList(table, y)) <= x)
		return 0;

	return GetNumb(table, x, y);
}
static void SetNumb(autoList_t *table, uint x, uint y, uint numb)
{
	char **p_cell = (char **)directGetPoint(getList(table, y), x);

	errorCase(!*p_cell); // 2bs

	if(numb)
		strzp_x(p_cell, xcout("%u", numb));
	else
		**p_cell = '\0';
}
static uint NextRand(void)
{
	static uchar buff[RAND_BUFF_SIZE];
	static uint index = RAND_BUFF_SIZE;

	if(index == RAND_BUFF_SIZE)
	{
		getCryptoBlock_MS(buff, RAND_BUFF_SIZE);
		index = 0;
	}
	return buff[index++];
}
static uint GetRand_Mod(uint modulo)
{
	uint64 value;

	((uchar *)&value)[0] = NextRand();
	((uchar *)&value)[1] = NextRand();
	((uchar *)&value)[2] = NextRand();
	((uchar *)&value)[3] = NextRand();
	((uchar *)&value)[4] = NextRand();
	((uchar *)&value)[5] = NextRand();
	((uchar *)&value)[6] = NextRand();
	((uchar *)&value)[7] = NextRand();

	return (uint)(value % (uint64)modulo);
}
static uint GetRand32(void)
{
	uint value;

	getCryptoBlock_MS((uchar *)&value, sizeof(value));

	return value;
}
static uint GetRand(uint minval, uint maxval)
{
	errorCase(maxval < minval);

	if(minval == 0 && maxval == UINTMAX)
		return GetRand32();

	return GetRand_Mod(maxval - minval + 1) + minval;
}
static void PrintProgPct(double pct)
{
	cout("PROGRESS == %f PCT OK -- P=%u K=%u X=%u\n", pct, Cnt_P, Cnt_K, Cnt_X);

	cmdTitle_x(xcout("MkNumple2 - %f PCT OK", pct));

	if(UseN2)
	{
		Nectar2SendLine_x(N2Sender, xcout("Progress=%d;", d2i(pct * IMAX / 100.0)));
	}
}
static char *GetSudokuExe(void)
{
	static char *file;

	if(!file)
	{
		file = combine(getSelfDir(), "Numple2.exe");

		if(!existFile(file)) // ? dev env
		{
			memFree(file);
			file = combine(getSelfDir(), "Sudoku2\\Release\\Sudoku2.exe");
			errorCase(!existFile(file));
		}
	}
	return file;
}

static char *CS_ExtraArgs = "";

static int CallSudoku(void)
{
	uint evTomStarted = eventOpen(EV_TOM_STARTED);
	uint evTimedOut = eventOpen(EV_TIMED_OUT);
	int ret;

	removeFileIfExist(OutputFile);
	removeFileIfExist(ErrorFile);
	removeFileIfExist(LoadedDataFile);

	// memo: コマンドラインに、ダブルクォートで囲われた箇所 ( "", "hoge" など ) が2つ以上あれば START "" /B /WAIT そうれなければ何もいらないっぽい。
	// <- 本当か？

	// **1 テストの時やかましいから > NUL 入れたんだと思う。

	execute_x(xcout("START \"\" /B \"%s\" /TIME-OUT-MONITOR", getSelfFile()));
	handleWaitForMillis(evTomStarted, TRUSTED_TIMEOUT_MILLIS); // TOM の開始を待つ。
	execute_x(xcout("START \"\" /B /WAIT /HIGH \"%s\" /D \"%s\"%s > NUL", GetSudokuExe(), DataDir, CS_ExtraArgs)); // **1
	execute_x(xcout("\"%s\" /TIME-OUT-MONITOR-STOP", getSelfFile()));

	handleClose(evTomStarted);

	if(handleWaitForMillis(evTimedOut, 0))
	{
		ret = RET_TIMED_OUT;
	}
	else if(existFile(OutputFile) && !existFile(ErrorFile)) // ? 成功
	{
		ret = RET_SUCCESSFUL;
	}
	else
	{
		ret = RET_FAILED;
	}
	handleClose(evTimedOut);
	cout("CallNumple_ret: %d\n", ret);

	if(handleWaitForMillis(EvCancel, 0)) // ★★★ ここで中断処理してるよ！ ★★★
	{
		cout("+---------------+\n");
		cout("| 中断しました。|\n");
		cout("+---------------+\n");
		termination(1);
	}
	// ★★★ restore
	{
		CS_ExtraArgs = "";
	}
	return ret;
}
static int CallSudoku_I(void)
{
	writeCSVFile(InputFile, InputTable);
	return CallSudoku();
}
static uint GetNumbMax(void)
{
	uint ret = 0;
	uint x;
	uint y;

	for(x = 0; x < Ans_W; x++)
	for(y = 0; y < Ans_H; y++)
	{
		m_maxim(ret, GetNumb(AnswerTable, x, y));
	}
	return ret;
}
static void MkInputTable(void)
{
	uint x;
	uint y;

	InputTable = newList();

	for(y = 0; y < Ans_H; y++)
	{
		autoList_t *row = newList();

		for(x = 0; x < Ans_W; x++)
		{
			addElement(row, (uint)strx(""));
		}
		addElement(InputTable, (uint)row);
	}
}
static autoList_t *MkGroup(uint count)
{
	autoList_t *grp = newList();
	uint index;

	for(index = 0; index < count; index++)
		addElement(grp, index + 1);

	for(index = 0; index < count * 100; index++) // fixme
	{
		uint a = GetRand_Mod(count);
		uint b = GetRand_Mod(count);

		swapElement(grp, a, b);
	}
	return grp;
}
static uint Proc_PresetGroup_No(uint grp_no)
{
	autoList_t *csv;
	uint rr;
	uint cc;
	uint gCount = 0;
	autoList_t *grp;

	cout("grp_no: %u\n", grp_no);

	csv = readCSVFileTR(ExtraPresetGroupFile);

	for(rr = 0; rr < getCount(csv); rr++)
	for(cc = 0; cc < getCount(getList(csv, rr)); cc++)
	{
		if(toValue(getLine(getList(csv, rr), cc)) == grp_no) // ? trg_grp
		{
			gCount++;
		}
	}
	grp = MkGroup(gCount);
	gCount = 0;

	for(rr = 0; rr < getCount(csv); rr++)
	for(cc = 0; cc < getCount(getList(csv, rr)); cc++)
	{
		if(toValue(getLine(getList(csv, rr), cc)) == grp_no) // ? trg_grp
		{
			cout("Preset: (%u, %u) = %u\n", cc, rr, getElement(grp, gCount));

			SetNumb(InputTable, cc, rr, getElement(grp, gCount));
			gCount++;
		}
	}
	releaseDim(csv, 2);
	releaseAutoList(grp);

	cout("gCount: %u\n", gCount);

	return gCount;
}
static void Proc_PresetGroup(void)
{
	uint grp_no;

	LOGPOS();

	if(!existFile(ExtraPresetGroupFile))
		return;

	LOGPOS();

	for(grp_no = 1; ; grp_no++)
		if(!Proc_PresetGroup_No(grp_no))
			break;

	LOGPOS();
}
static int TPR_IsPuttable(uint x, uint y)
{
	return GetNumb(AnswerTable, x, y) && !GetNumb(InputTable, x, y);
}
static int TPR_IsPutted(uint x, uint y)
{
	return GetNumb(InputTable, x, y);
}
static double TPR_GetPuttedRate(void)
{
	uint a = 0;
	uint d = 0;
	uint x;
	uint y;

	for(x = 0; x < Ans_W; x++)
	for(y = 0; y < Ans_H; y++)
	{
		if(TPR_IsPuttable(x, y))
		{
			a++;
		}
		else if(TPR_IsPutted(x, y))
		{
			d++;
		}
	}
	return d * 1.0 / (a + d);
}
static int TryPutRand(void) // ret: ? まだ追加出来ると思う。
{
	uint refindCount = 0;
	uint count;
	uint numb;
	uint x;
	uint y;

refindPosNumb:

	// Puttable の個数を数える。-> count

	count = 0;

	for(x = 0; x < Ans_W; x++)
	for(y = 0; y < Ans_H; y++)
	{
		if(TPR_IsPuttable(x, y))
			count++;
	}
	if(!count)
	{
		cout("もう追加出来ない！\n");
		return 0;
	}

	// count -> 0 ～ (count - 1)

	cout("%u -> ", count);
	count = GetRand_Mod(count);
	cout("%u\n", count);

	// (count + 1) 番目の Puttable を採用する。

	for(x = 0; x < Ans_W; x++)
	for(y = 0; y < Ans_H; y++)
	{
		if(TPR_IsPuttable(x, y))
		{
			if(!count)
				goto foundPos;

			count--;
		}
	}
	error();

foundPos:
	numb = GetRand(1, NumbMax);

	if(KPN_IsKnown(x, y, numb))
	{
		if(refindCount < 10) // fixme: 上限適当
		{
			refindCount++;
			goto refindPosNumb;
		}
		KPN_Reset();
	}
	KPN_Add(x, y, numb);

	cout("TryPutRand: x=%u, y=%u, numb=%u\n", x, y, numb);

	SetNumb(InputTable, x, y, numb);

	switch(CallSudoku_I())
	{
	case RET_SUCCESSFUL:
		Cnt_P++;
		break;

	case RET_FAILED:
	case RET_TIMED_OUT:
		SetNumb(InputTable, x, y, 0);
		break;

	default:
		error();
	}
	return 1;
}
static void PutRand(void)
{
	uint startTime = now();

	do
	{
		uint e = now() - startTime;

		cout("PR_Elapsed: %u (%u)\n", e, PUT_RAND_TIMEOUT_SEC);

		if(PUT_RAND_TIMEOUT_SEC < e)
			break;

		PrintProgPct(TPR_GetPuttedRate() * 50.0);
	}
	while(TryPutRand());

	switch(CallSudoku_I())
	{
	case RET_SUCCESSFUL:
		break;

	default:
		error_m("Please retry later."); // 解いた事があるはずなので、解けるはず。タイムアウトぎりぎりだったのかも。XXX
	}
	releaseDim(AnswerTable, 2);
	releaseDim(InputTable, 2);
	AnswerTable = NULL;
	InputTable = readCSVFileTR(OutputFile);

	errorCase(Ans_H != getCount(InputTable));
	errorCase(Ans_W != getCount(getList(InputTable, 0)));
}
static void PrintPairs(autoList_t *pairs)
{
	uint *pair;
	uint index;

	foreach(pairs, pair, index)
	{
		cout("[%u] %u, %u, %u\n", index, pair[0], pair[1], pair[2]);
	}
}
static void K_Shuffle(autoList_t *list)
{
	uint index;

	for(index = 0; index + 1 < getCount(list); index++)
	{
		swapElement(list, index, GetRand(index, getCount(list) - 1));
	}
}

static autoList_t *Ko_KpCsv;

static sint Ko_Comp(uint ua, uint ub)
{
	uint *a = (uint *)ua;
	uint *b = (uint *)ub;
	sint kpa;
	sint kpb;
	sint ret;

	kpa = (sint)RefNumb(Ko_KpCsv, a[0], a[1]);
	kpb = (sint)RefNumb(Ko_KpCsv, b[0], b[1]);

	ret = kpb - kpa; // (b - a) <- 大きい方を先！

	return ret;
}
static void K_Order(autoList_t *pairs, autoList_t *kpCsv)
{
	LOGPOS();

	PrintPairs(pairs);

	LOGPOS();

	Ko_KpCsv = kpCsv;
	gnomeSort(pairs, Ko_Comp);
	Ko_KpCsv = NULL;

	LOGPOS();

	PrintPairs(pairs);

	LOGPOS();
}
static int TryKezuru(uint x, uint y) // ret: ? タイムアウトだった。
{
	uint ansNumb = GetNumb(InputTable, x, y);
	uint numb;
	int ret = 0;
	char *dChkEa;

	for(numb = 1; numb <= NumbMax; numb++)
	{
		if(numb == ansNumb)
			continue;

		SetNumb(InputTable, x, y, numb);

		switch(CallSudoku_I())
		{
		case RET_FAILED:
			break;

		case RET_TIMED_OUT:
			ret = 1;
			// fall through

		case RET_SUCCESSFUL:
			goto restore;

		default:
			error();
		}
	}
	cout("削った: x=%u, y=%u, numb=%u\n", x, y, ansNumb);
	SetNumb(InputTable, x, y, 0);

	switch(Difficulty)
	{
	case DIFFICULTY_EASY_1:
		dChkEa = " /-BF /-F2";
		break;

	case DIFFICULTY_EASY_2:
		dChkEa = " /-BF /-F1";
		break;

	case DIFFICULTY_NORMAL:
		dChkEa = " /-BF";
		break;

	case DIFFICULTY_HARD:
		dChkEa = NULL;
		break;

	default:
		error();
	}
	if(dChkEa)
	{
		cout("CHECK-DIFFICULTY\n");

		CS_ExtraArgs = dChkEa;

		switch(CallSudoku_I()) // 難しくなり過ぎてないかチェック
		{
		case RET_FAILED:
		case RET_TIMED_OUT:
			cout("難しくなり過ぎた様なので却下！\n");
			goto restore;

		case RET_SUCCESSFUL:
			break;

		default:
			error();
		}
	}
	Cnt_K++;
	goto endFunc;

restore:
	cout("削れなかった。\n");
	SetNumb(InputTable, x, y, ansNumb);

endFunc:
	cout("TK_ret: %d\n", ret);
	return ret;
}
static void Kezuru(void)
{
	autoList_t *pairs = newList();
	uint *pair;
	uint pair_index;
	uint x;
	uint y;

	for(x = 0; x < Ans_W; x++)
	for(y = 0; y < Ans_H; y++)
	{
		if(GetNumb(InputTable, x, y))
		{
			pair = (uint *)memAlloc(3 * sizeof(uint));

			pair[0] = x;
			pair[1] = y;
			pair[2] = 0; // リトライ回数

			addElement(pairs, (uint)pair);
		}
	}
	K_Shuffle(pairs);

	cout("Ko_1: %d\n", Gkbnso);
	cout("Ko_2: %d\n", existFile(ExtraKezuruPriorityFile));

	if(Gkbnso && existFile(ExtraKezuruPriorityFile))
	{
		autoList_t *kpCsv = readCSVFileTR(ExtraKezuruPriorityFile);

		K_Order(pairs, kpCsv);

		releaseDim(kpCsv, 2);
	}
	foreach(pairs, pair, pair_index)
	{
		cout("K: %u (%u) %u %u %u\n", pair_index, getCount(pairs), pair[0], pair[1], pair[2]);

		PrintProgPct(50.0 + pair_index * 50.0 / getCount(pairs));

		x = pair[0];
		y = pair[1];

		if(TryKezuru(x, y) && pair[2] < 1) // ? タイムアウトだった。&& リトライしていない。
		{
			// どんどん削って行くのだから、リトライしても意味無いかもしれないけど、ワンチャンあるかなくらいの気持ちで..
			// <- ワンチャン無いだろ。

			/*
			pair = (uint *)memClone(pair, 3 * sizeof(uint));
			pair[2]++;

			cout("タイムアウトだったので後でリトライします: x=%u, y=%u, tried=%u\n", pair[0], pair[1], pair[2]);

			addElement(pairs, (uint)pair); // リトライのため、追加
			*/
		}
	}
	releaseDim(pairs, 1);
}
static void GenData(autoList_t *commands)
{
	char *command;
	uint command_index;

	foreach(commands, command, command_index)
	{
		if(!strcmp(command, "HB_9x9"))
		{
			// done
			// 不定形ブロック
			// ms2PresetGroup.csv Group.csv を作成
			// 中断も受け付けること！

			LOGPOS();

			do
			{
				LOGPOS();
				MkGroupCsvHB_9x9(DataDir);
				LOGPOS();
			}
			while(CallSudoku() != RET_SUCCESSFUL);

			LOGPOS();
			continue;
		}
		if(!strcmp(command, "GK_9x9_25"))
		{
			// done
			// 偶数・奇数の配置 25 %
			// ms2PresetGroup.csv を削除 <- 元から無いので削除しない。
			// Condition.csv を作成
			// 中断も受け付けること！

			LOGPOS();
			Mk9x9InputCsvFile(DataDir);
			LOGPOS();
			MkConditionCsvGK(DataDir, 25);
			LOGPOS();
			continue;
		}
		if(!strcmp(command, "GK_9x9_100"))
		{
			// done
			// 偶数・奇数の配置 100 %
			// ms2PresetGroup.csv を削除 <- 元から無いので削除しない。
			// Condition.csv を作成
			// 中断も受け付けること！

			LOGPOS();
			Mk9x9InputCsvFile(DataDir);
			LOGPOS();
			MkConditionCsvGK(DataDir, 100);
			LOGPOS();
			continue;
		}
		if(!strcmp(command, "MKIDS_HB_9x9"))
		{
			// done
			// InstantDrawingScript.txt を作成
			// Group.csv から..
			// 中断も受け付けること！

			LOGPOS();
			MkInstantDrawingScript(DataDir);
			LOGPOS();
			continue;
		}
		if(!strcmp(command, "MKIDS_GK_9x9"))
		{
			// done
			// InstantDrawingScript.txt を作成
			// Condition.csv から..
			// 中断も受け付けること！

			LOGPOS();
			MkInstantDrawingScript(DataDir);
			LOGPOS();
			continue;
		}
		if(!strcmp(command, "MKIDS_HBGK_9x9"))
		{
			// done
			// InstantDrawingScript.txt を作成
			// Group.csv, Condition.csv から..
			// 中断も受け付けること！

			LOGPOS();
			MkInstantDrawingScript(DataDir);
			LOGPOS();
			continue;
		}
		error(); // Unknown command
	}
}
static void MkSudoku(char *dir)
{
	DataDir = makeFullPath(dir);

	errorCase_m(!existDir(DataDir), "データディレクトリにアクセス出来ません。");

	InputFile = combine(DataDir, "Input.csv");
	OutputFile = combine(DataDir, "Output.csv");
	ErrorFile = combine(DataDir, "Error.csv");
	LoadedDataFile = combine(DataDir, "LoadedData.csv");
	ExtraPresetGroupFile = combine(DataDir, "ms2PresetGroup.csv"); // ms2 固有
	ExtraKezuruPriorityFile = combine(DataDir, "ms2kPriority.csv"); // ms2 固有
	ExtraGenDataFile = combine(DataDir, "ms2GenData.txt"); // ms2 固有

	if(existFile(ExtraGenDataFile))
	{
		autoList_t *lines = readLines(ExtraGenDataFile);

		LOGPOS();
		GenData(lines);
		LOGPOS();
		releaseDim(lines, 1);
	}
	removeFileIfExist(InputFile);

	cout("問題を作成しています...\n");

	switch(CallSudoku())
	{
	case RET_SUCCESSFUL:
		break;

	case RET_FAILED:
		error_m("入力データに誤りがあるようです。");

	case RET_TIMED_OUT:
		error_m("入力データが難し過ぎるか、誤りがあるようです。");

	default:
		error();
	}
	AnswerTable = readCSVFileTR(OutputFile);
	Ans_H = getCount(AnswerTable);
	errorCase(!Ans_H);
	Ans_W = getCount(getList(AnswerTable, 0));
	errorCase(!Ans_W);
	NumbMax = GetNumbMax();

	// set Cnt_X
	{
		uint x;
		uint y;

//		Cnt_X = 0;

		for(x = 0; x < Ans_W; x++)
		for(y = 0; y < Ans_H; y++)
		{
			if(GetNumb(AnswerTable, x, y))
				Cnt_X++;
		}
	}

	// ---- 問題作成メイン ----

	if(Is9x9(DataDir))
	{
		Mk9x9InputCsvFile(DataDir);
		InputTable = readCSVFileTR(InputFile);

		errorCase(Ans_H != getCount(InputTable));
		errorCase(Ans_W != getCount(getList(InputTable, 0)));

		Cnt_P = IMAX;
	}
	else
	{
		MkInputTable();
		Proc_PresetGroup();
		PutRand();
	}
	Kezuru();

	// ----

	PrintProgPct(100.0);

	writeCSVFile(InputFile, InputTable);

	removeFileIfExist(OutputFile);
	removeFileIfExist(ErrorFile);
	removeFileIfExist(LoadedDataFile);

	cout("+---------------------+\n");
	cout("| 問題を作成しました。|\n");
	cout("+---------------------+\n");

	// 解答 (Output.csv) を作成する。
	{
		switch(CallSudoku())
		{
		case RET_SUCCESSFUL:
			break;

		default:
			error_m("Please retry later. 2");
		}

		// 2bs
		errorCase(!existFile(OutputFile));
		errorCase(existFile(ErrorFile));
		errorCase(existFile(LoadedDataFile));
	}

	cout("解答も作成しました。\n");
}
static void Main2(void)
{
	uint mtxProc;

	if(argIs("/TIME-OUT-MONITOR"))
	{
		uint evBfStarted = eventOpen(EV_BRUTE_FORCE_STARTED);
		uint evTomStop = eventOpen(EV_TOM_STOP);
		uint timeout = 0;

		eventWakeup(EV_TOM_STARTED);

		while(!handleWaitForMillis(evTomStop, 2000))
		{
			if(!timeout)
			{
				if(handleWaitForMillis(evBfStarted, 0))
					timeout = now() + BRUTE_FORCE_TIMEOUT_SEC;
			}
			else if(timeout < now())
			{
				execute_x(xcout("\"%s\" /S", GetSudokuExe()));
				eventWakeup(EV_TIMED_OUT);
				handleWaitForMillis(evTomStop, TRUSTED_TIMEOUT_MILLIS); // EV_TOM_STOPPED をセットしたいので、EV_TOM_STOP を待つ。
				break;
			}
		}
		handleClose(evTomStop);
		eventWakeup(EV_TOM_STOPPED);
		return;
	}
	if(argIs("/TIME-OUT-MONITOR-STOP"))
	{
		uint evTomStopped = eventOpen(EV_TOM_STOPPED);
		eventWakeup(EV_TOM_STOP);
		handleWaitForMillis(evTomStopped, TRUSTED_TIMEOUT_MILLIS);
		handleClose(evTomStopped);
		return;
	}

	EvCancel = eventOpen("{ae49c005-b59e-4843-8a08-58e86e8a1c7c}");

	if(argIs("/S"))
	{
		eventWakeupHandle(EvCancel);
		return;
	}

	mtxProc = mutexTryProcLock("{900b20f9-a86f-48f3-a6e3-8312efc0bbf9}");

	// 2bs: ゾンビ TOM を止める。
	{
		eventWakeup(EV_TOM_STOP);
		sleep(100);
		eventWakeup(EV_TOM_STOP);
		sleep(100);
		eventWakeup(EV_TOM_STOP);
		sleep(100);
	}

	if(argIs("/N2"))
	{
		UseN2 = 1;
	}
	if(argIs("/K-"))
	{
		// noop
	}
	if(argIs("/K+"))
	{
		Gkbnso = 1;
	}
	if(argIs("/Lv:1"))
	{
		Difficulty = DIFFICULTY_EASY_1;
	}
	if(argIs("/Lv:2"))
	{
		Difficulty = DIFFICULTY_EASY_2;
	}
	if(argIs("/Lv:3"))
	{
		Difficulty = DIFFICULTY_NORMAL;
	}
	if(argIs("/Lv:4"))
	{
		Difficulty = DIFFICULTY_HARD;
	}
	errorCase_m(hasArgs(2), "Unknown arg!"); // 次の arg は入力DIRの場合アリ

	cout("DIFFICULTY: %u\n", Difficulty);

	if(UseN2)
		N2Sender = CreateNectar2("{aaa1d63e-fc8b-488b-a92f-d7617b70f514}"); // shared_uuid

	if(hasArgs(1))
	{
		MkSudoku(nextArg());
	}
	else
	{
		MkSudoku(dropDir());
	}
	cmdTitle("MkNumple2");

	if(UseN2)
		ReleaseNectar2(N2Sender);

	mutexUnlock(mtxProc);

	handleClose(EvCancel);
}
int main(int argc, char **argv)
{
	Main2();
	termination(0);
}
