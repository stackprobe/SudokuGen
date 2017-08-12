#include "GenRand.h"

#define RAND_BUFF_SIZE 1024

uint NextRand(void)
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
uint GetRand_Mod(uint modulo)
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
