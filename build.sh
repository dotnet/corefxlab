#!/usr/bin/env bash

# adding dotnet to the path. it is needed to run toolset csc.
parent_path=$( cd "$(dirname "${BASH_SOURCE[0]}")" ; pwd -P )
dotnet_path=$parent_path/dotnetcli
export PATH=$PATH:$dotnet_path

usage()
{
    echo "build.sh [ Debug(default) | Release ]"
}

if [ "$1" == "-?" ] || [ "$1" == "-h" ]; then
    usage
	exit
fi

if [ $# -eq 0 ]
  then
    Configuration="Debug"
  else
    Configuration=$1
fi

BuildVersion="e$(date '+%y%m%d')-1"
Version="<default>"
Restore="true"
echo "Configuration=$Configuration."
echo "Restore=$Restore."
echo "Version=$Version."
echo "BuildVersion=$BuildVersion."

if [ ! -d "dotnetcli" ]; then
  echo "dotnet.exe not installed, downloading and installing."
  if [ "$Version" = "<default>" ]; then
    Version=$(head -n 1 "DotnetCLIVersion.txt")
  fi
  ./scripts/install-dotnet.sh -Channel master -Version "$Version" -InstallDir "dotnetcli"
  ret=$?
  if [ $ret -ne 0 ]; then
    echo "Failed to install latest dotnet.exe, exit code $ret, aborting build."
    exit -1
  fi

  # Temporary workaround until CLI, Core-Setup, CoreFx are all in sync with the shared runtime.
  SharedVersion=$(head -n 1 "SharedRuntimeVersion.txt")
  ./scripts/install-dotnet.sh -Channel master -Version "$SharedVersion" -InstallDir "dotnetcli" -SharedRuntime
  ret=$?
  if [ $ret -ne 0 ]; then
    echo "Failed to install latest 2.1.0 shared runtime (version $SharedVersion), exit code $ret, aborting build."
    exit -1
  fi

  ./scripts/install-dotnet.sh -Version 1.0.0 -InstallDir "dotnetcli"
  ret=$?
  if [ $ret -ne 0 ]; then
    echo "Failed to install framework version 1.0.0, exit code $ret, aborting build."
    exit -1
  fi

  ./scripts/install-dotnet.sh -Version 2.0.0 -InstallDir "dotnetcli"
  ret=$?
  if [ $ret -ne 0 ]; then
    echo "Failed to install framework version 2.0.0, exit code $ret, aborting build."
    exit -1
  fi
fi

export DOTNET_SKIP_FIRST_TIME_EXPERIENCE=1
export DOTNET_MULTILEVEL_LOOKUP=0

dotnetExePath="dotnetcli/dotnet"

myFile="corefxlab.sln"

if [ "$Restore" = "true" ]; then
  echo "Restoring all packages"
  ./$dotnetExePath restore $myFile /p:VersionSuffix="$BuildVersion"
  ret=$?
  if [ $ret -ne 0 ]; then
    echo "Failed to restore packages."
    exit -1
  fi
fi

echo "Building solution $myFile..."

./$dotnetExePath build $myFile -c "$Configuration" /p:VersionSuffix="$BuildVersion"
ret=$?
if [ $ret -ne 0 ]; then
  echo "Failed to build solution $myFile"
  exit -1
fi

errorsEncountered=0
declare -a projectsFailed

while read -r testFile;
do
  echo "Building and running tests for project $testFile..."
  ./$dotnetExePath test "$testFile" -c "$Configuration" --no-build -- -notrait category=performance -notrait category=outerloop
  ret=$?
  if [ $ret -ne 0 ]; then
    echo "Some tests failed in project $testFile"
    projectsFailed[${#projectsFailed[@]}]="$testFile"
	((errorsEncountered=errorsEncountered+1))
  fi
done < <(find tests -name "*.csproj")

RED='\033[0;31m'
GREEN='\033[0;32m'
NC='\033[0m'

if [ $errorsEncountered -ne 0 ]; then
  echo -e "${RED}** Build failed. $errorsEncountered projects failed to build or test. **${NC}"
  for i in "${projectsFailed[@]}"
  do
   echo -e "${RED}    $i${NC}"
  done
else
  echo -e "${GREEN}** Build succeeded. **${NC}"
fi

exit $errorsEncountered
