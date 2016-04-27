@echo off
setlocal

:: Invoke VS Developer Command Prompt batch file.
:: This sets up some environment variables needed to use ILDasm and ILAsm.
if not defined VisualStudioVersion (
    if defined VS140COMNTOOLS (
        call "%VS140COMNTOOLS%\VsDevCmd.bat"
        goto :Build
    )

    echo Error: cibuild.cmd requires Visual Studio 2015.
    exit /b 1
)

:Build
powershell -NoProfile %~dp0scripts\build.ps1 %*