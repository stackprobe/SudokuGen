#include "all.h"

FILE *rfopen(char *file, char *mode)
{
	for(int retry = 0; ; retry++)
	{
		// fopen() �͈����ɖ�肪�����Ă��A�H�Ɏ��s���邱�Ƃ�����B
		// �K���ɊԊu���󂯂ĉ��x�����g���C����B
		FILE *fp = fopen(file, mode); // file == "" �̂Ƃ� NULL ��Ԃ��B

		if(fp != NULL)
		{
			if(retry)
				cout("File opened at %u-th retrial.\n", retry);

			return fp;
		}
		if(retry == 20)
		{
			cout("Can not open file \"%s\" as \"%s\" mode.\n", file, mode);
			return NULL;
		}
		Sleep(100);
	}
}
FILE *fileOpen(char *file, char *mode)
{
	FILE *fp = rfopen(file, mode);

	errorCase(!fp);
	return fp;
}
void fileClose(FILE *fp)
{
	errorCase(fclose(fp)); // ? ���s
}

int readChar(FILE *fp)
{
	int chr = fgetc(fp);

	errorCase(chr == EOF && ferror(fp)); // ? ���s
	return chr;
}
void writeChar(FILE *fp, int chr)
{
	errorCase(fputc(chr, fp) == EOF); // ? ���s
}
void writeToken(FILE *fp, char *token)
{
	for(char *p = token; *p; p++)
	{
		writeChar(fp, *p);
	}
}
