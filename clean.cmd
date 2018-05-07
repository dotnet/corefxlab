@echo off
setlocal

if not exist dotnetcli GOTO PROMPT

:: Since the dotnetcli folder exists, the dotnet.exe, if running, is probably from here
tasklist /FI "IMAGENAME eq dotnet.exe" 2>NUL | find /I /N "dotnet.exe">NUL
if "%ERRORLEVEL%"=="0" (
    echo Killing all instances of dotnet.exe that are running.
    taskkill /F /IM dotnet.exe /T
)

:PROMPT
SET /P AREYOUSURE=Are you sure you want to delete all unstaged files from the repo (Y/[N])? 
IF /I "%AREYOUSURE%" NEQ "Y" GOTO END

echo Running "git clean -fdx".
git clean -fdx

:END
endlocal