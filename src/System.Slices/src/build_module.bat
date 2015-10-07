set projectFolder=%~dp0
set binFolder=%projectFolder%\..\..\..\bin
set debugFolder=%binFolder%\Windows_NT.AnyCPU.Debug\System.Slices
set releaseFolder=%binFolder%\Windows_NT.AnyCPU.Release\System.Slices
set moduleReleaseOutput=%releaseFolder%\System.Slices.netmodule
set moduleDebugOutput=%DebugFolder%\System.Slices.netmodule
mkdir %debugFolder%
mkdir %releaseFolder%

ilasm /dll %~dp0\System\PtrUtils.il /out:%moduleReleaseOutput%
copy %moduleReleaseOutput% %moduleDebugOutput%



