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

copy ..\libraries\RestSharp*.* .\iBUpdate
copy ..\libraries\Ionic*.* .\iBUpdate
copy ..\libraries\AMHelper*.* .\iBUpdate
copy ..\libraries\Newtonsoft* .\iBUpdate

copy .\AssistenzaApex.exe .\iBUpdate

"%ProgramFiles%\7-zip\7z.exe" a -tzip -r "iBUpdate.zip" .\iBUpdate
 

rmdir /S /Q iBUpdate

del ConnettoreBusiness.zip
rmdir /S /Q ConnettoreBusiness
mkdir ConnettoreBusiness

copy ..\TEST\BNIEIBUS.dll .\ConnettoreBusiness
copy ..\TEST\BNIEIBUS.pdb .\ConnettoreBusiness

copy ..\TEST\BEIEIBUS.dll .\ConnettoreBusiness
copy ..\TEST\BEIEIBUS.pdb .\ConnettoreBusiness

copy ..\TEST\BDIEIBUS.dll .\ConnettoreBusiness
copy ..\TEST\BDIEIBUS.pdb .\ConnettoreBusiness

copy ..\TEST\ApexNet*.dll .\ConnettoreBusiness
copy ..\TEST\ApexNet*.pbd .\ConnettoreBusiness

copy ..\libraries\RestSharp*.* .\ConnettoreBusiness
copy ..\libraries\Ionic*.* .\ConnettoreBusiness
copy ..\libraries\AMHelper*.* .\ConnettoreBusiness
copy ..\libraries\Newtonsoft* .\ConnettoreBusiness

copy .\AssistenzaApex.exe .\ConnettoreBusiness

rem ci sono differenze solo a partire da qui
copy ..\TEST\iBUpdate.exe .\ConnettoreBusiness
copy ..\TEST\iBUpdate.pdb .\ConnettoreBusiness

cd .\ConnettoreBusiness
"%ProgramFiles%\7-zip\7z.exe" a -tzip -r "ConnettoreBusiness.zip" *.*
move .\ConnettoreBusiness.zip ..
cd ..

rmdir /S /Q ConnettoreBusiness
