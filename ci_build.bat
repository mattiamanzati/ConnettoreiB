for /f "delims=" %%a IN ('dir /b /s .\Standard\IB*.sln') do (
echo "%%a"
nuget.exe restore "%%a"
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe "%%a"
)