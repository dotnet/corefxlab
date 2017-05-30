@setlocal

mkdir out\win32 && pushd out\win32
cmake -G "Visual Studio 14 2015" ..\..
popd

mkdir out\win64 && pushd out\win64
cmake -G "Visual Studio 14 2015 Win64" ..\..
popd

cmake --build out\win32 --config RelWithDebInfo
mkdir binaries\windows\x86
copy out\win32\RelWithDebInfo\brotlicommon.dll binaries\windows\x86\
copy out\win32\RelWithDebInfo\brotlicommon.pdb binaries\windows\x86\
copy out\win32\RelWithDebInfo\brotlidec.dll binaries\windows\x86\
copy out\win32\RelWithDebInfo\brotlidec.pdb binaries\windows\x86\
copy out\win32\RelWithDebInfo\brotlienc.dll binaries\windows\x86\
copy out\win32\RelWithDebInfo\brotlienc.pdb binaries\windows\x86\

cmake --build out\win64 --config RelWithDebInfo
mkdir binaries\windows\x64
copy out\win64\RelWithDebInfo\brotlicommon.dll binaries\windows\x64\
copy out\win64\RelWithDebInfo\brotlicommon.pdb binaries\windows\x64\
copy out\win64\RelWithDebInfo\brotlidec.dll binaries\windows\x64\
copy out\win64\RelWithDebInfo\brotlidec.pdb binaries\windows\x64\
copy out\win64\RelWithDebInfo\brotlienc.dll binaries\windows\x64\
copy out\win64\RelWithDebInfo\brotlienc.pdb binaries\windows\x64\

rd /s out
