Instructions for building LibGD binaries for Windows

Clone the source from here: https://github.com/libgd/libgd
Open the VS2013 x64 Native Tools Command Prompt. 2012 will *not* work.
Change to the gd-libgd folder.
git branch gd-2.1.1 2912c0a2e0a246318f41bf1997f34ce1dc3e5e42
git checkoutgd-2.1.1
git clone https://github.com/imazen/gd-win-dependencies into the folder. 
Run 
nmake /f windows/Makefile.vc all
nmake /f windows/Makefile.vc check

Before rebuilding, run:
nmake /f windows/Makefile.vc clean 