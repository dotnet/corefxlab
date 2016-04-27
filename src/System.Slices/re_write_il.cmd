@echo off
setlocal

:: Invoke VS Developer Command Prompt batch file.
:: This sets up some environment variables needed to use ILDasm and ILAsm.
if not defined VisualStudioVersion (
    if defined VS140COMNTOOLS (
        call "%VS140COMNTOOLS%\VsDevCmd.bat"
        goto :Rewrite
    )

    echo Error: re_write_il.cmd requires Visual Studio 2015.
    exit /b 1
)

:Rewrite
@echo on
tools\ildasm.exe /caverbal /linenum /out:%1\system.slices.beforerewrite.il /nobar %2
tools\ILSub\ILSub.exe %1\system.slices.beforerewrite.il %1\system.slices.rewritten.il
tools\ilasm.exe /quiet /pdb /dll /out:%2 /nologo %1\system.slices.rewritten.il