C:\Factory\Tools\RDMD.exe /RC out

COPY /B Sudoku2\Release\Sudoku2.exe out\Numple2.exe
COPY /B MkSudoku2.exe out\MkNumple2.exe
COPY /B Print9x9\digits.bmp out
COPY /B Print9x9\Draw9x9.exe out
COPY /B Print9x9\TileBmp.exe out
COPY /B Print9x9\PrintImage\PrintImage\bin\Release\PrintImage.exe out
COPY /B Print9x9\WPrint9x9\WPrint9x9\bin\Release\WPrint9x9.exe out

C:\Factory\Tools\xcp.exe Print9x9_doc out
C:\Factory\Tools\DirFltr.exe /EF out

C:\Factory\SubTools\EmbedConfig.exe --factory-dir-disabled out\MkNumple2.exe

C:\Factory\SubTools\zip.exe /O out SudokuGen_Print9x9

PAUSE
