C:\Factory\Tools\RDMD.exe /RC out

COPY /B Sudoku2\Release\Sudoku2.exe out\Numple2.exe
COPY /B MkSudoku2.exe out\MkNumple2.exe
COPY /B SudokuGen\SudokuGen\bin\Release\NumpleGen.exe out\ナンプレ作成印刷プログラム.exe
COPY /B C:\Factory\Resource\JIS0208.txt out

C:\Factory\Tools\xcp.exe doc out

C:\Factory\SubTools\EmbedConfig.exe --factory-dir-disabled out\MkNumple2.exe

C:\Factory\SubTools\zip.exe /O out NumpleGen

IF NOT "%1" == "/-P" PAUSE
