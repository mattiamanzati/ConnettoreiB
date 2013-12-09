del ConnettoreBusinessSRC.zip
rmdir /S /Q ConnettoreBusinessSRC
mkdir ConnettoreBusinessSRC

xcopy /s ..\BDIEIBUS\*.* .\ConnettoreBusinessSRC\BDIEIBUS\ 
xcopy /s ..\BEIEIBUS\*.* .\ConnettoreBusinessSRC\BEIEIBUS\
xcopy /s ..\BNIEIBUS\*.* .\ConnettoreBusinessSRC\BNIEIBUS\

rmdir /S /Q .\ConnettoreBusinessSRC\BDIEIBUS\obj
rmdir /S /Q .\ConnettoreBusinessSRC\BEIEIBUS\obj
rmdir /S /Q .\ConnettoreBusinessSRC\BNIEIBUS\obj

"%ProgramFiles%\7-zip\7z.exe" a -tzip -r "ConnettoreBusinessSRC.zip" .\ConnettoreBusinessSRC

ren ConnettoreBusinessSRC.zip ConnettoreBusinessSRC.RenameInZIP
rmdir /S /Q ConnettoreBusinessSRC


del iBUpdateVertical.zip
rmdir /S /Q iBUpdateVertical
mkdir iBUpdateVertical

copy ..\TEST\BOIEIBUS.dll .\iBUpdateVertical
copy ..\TEST\BHIEIBUS.dll .\iBUpdateVertical
copy ..\TEST\BFIEIBUS.dll .\iBUpdateVertical

copy ..\TEST\BOIEIBUS.pdb .\iBUpdateVertical
copy ..\TEST\BHIEIBUS.pdb .\iBUpdateVertical
copy ..\TEST\BFIEIBUS.pdb .\iBUpdateVertical

"%ProgramFiles%\7-zip\7z.exe" a -tzip -r "iBUpdateVertical.zip" .\iBUpdateVertical

rmdir /S /Q iBUpdateVertical






del iBUpdate.zip
rmdir /S /Q iBUpdate
mkdir iBUpdate

copy ..\TEST\BNIEIBUS.dll .\iBUpdate
copy ..\TEST\BEIEIBUS.dll .\iBUpdate
copy ..\TEST\BDIEIBUS.dll .\iBUpdate


copy ..\TEST\BNIEIBUS.pdb .\iBUpdate
copy ..\TEST\BEIEIBUS.pdb .\iBUpdate
copy ..\TEST\BDIEIBUS.pdb .\iBUpdate

copy ..\TEST\ApexNet*.dll .\iBUpdate
copy ..\TEST\ApexNet*.pbd .\iBUpdate

copy ..\lib\*.* .\iBUpdate
copy .\AssistenzaApex.exe .\iBUpdate

"%ProgramFiles%\7-zip\7z.exe" a -tzip -r "iBUpdate.zip" .\iBUpdate

rmdir /S /Q iBUpdate

del ConnettoreBusiness.zip
rmdir /S /Q ConnettoreBusiness
mkdir ConnettoreBusiness

copy ..\TEST\ApexNet*.dll .\ConnettoreBusiness
copy ..\TEST\ApexNet*.pbd .\ConnettoreBusiness

copy ..\TEST\BNIEIBUS.dll .\ConnettoreBusiness
copy ..\TEST\BEIEIBUS.dll .\ConnettoreBusiness
copy ..\TEST\BDIEIBUS.dll .\ConnettoreBusiness
copy ..\TEST\BNIEIBUS.pdb .\ConnettoreBusiness
copy ..\TEST\BEIEIBUS.pdb .\ConnettoreBusiness
copy ..\TEST\BDIEIBUS.pdb .\ConnettoreBusiness


copy ..\TEST\iBUpdate.exe .\ConnettoreBusiness
copy ..\TEST\iBUpdate.pdb .\ConnettoreBusiness

copy ..\TEST\Ionic.Utils.Zip.dll .\ConnettoreBusiness

cd .\ConnettoreBusiness
"%ProgramFiles%\7-zip\7z.exe" a -tzip -r "ConnettoreBusiness.zip" *.*
move .\ConnettoreBusiness.zip ..
cd ..

rmdir /S /Q ConnettoreBusiness
