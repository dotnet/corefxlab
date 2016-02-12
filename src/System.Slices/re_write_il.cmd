tools\ildasm.exe /linenum /out:%1\system.slices.beforerewrite.il /nobar %2
tools\ILSub\ILSub.exe %1\system.slices.beforerewrite.il %1\system.slices.rewritten.il
tools\ilasm.exe /quiet /pdb /dll /out:%2 /nologo %1\system.slices.rewritten.il