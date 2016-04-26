rem SET DIR_SOURCE="C:\Bus.Business2015\Agg\iBUpdate"
rem SET DIR_DEST="c:\temp\testIBUPDATE"

SET DIR_SOURCE="{0}"
SET DIR_DEST="{1}"

SET DLL=BNIEIBUS.DLL
SET ATTEMPTS=20

@echo off
for /L %%n in (1, 1, %ATTEMPTS%) do (
echo I try to update %DLL% %%n of 10
ping 127.0.0.1 -n 5 > null

if exist %DIR_DEST%\%DLL% (
2>nul (
  >> %DIR_DEST%\%DLL%  (call )
) && (goto OK) || (set a = 1) ) else ( exit )

)

goto END

rem =============================

:OK

echo "Eseguo XCOPY"
xcopy %DIR_SOURCE%"\*.*" /s /y /q /i %DIR_DEST%

:END
echo "FINE"