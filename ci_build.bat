for /f "delims=" %%a IN ('dir /b IB_2015.sln') do (
echo "%%a"
.\nuget\nuget.exe restore "%%a"
%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe "%%a"
)