@echo off
setlocal

if not exist "%~dp0dotnet\dotnet.exe" (
  echo ERROR: Please make sure to build the repository first using build.cmd
  exit /b 1
)

%~dp0dotnet\dotnet.exe restore %~dp0scripts\PerfHarness
%~dp0dotnet\dotnet.exe run -c Release -p %~dp0scripts\PerfHarness
