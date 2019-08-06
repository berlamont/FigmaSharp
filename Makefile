#General vars
CONFIG?=Debug
ARGS:=/p:Configuration="${CONFIG}" $(ARGS)

all:
	echo "Building FigmaSharp..."
	msbuild FigmaSharp.LiteForms.sln $(ARGS)

clean:
	find . -type d -name bin -exec rm -rf {} \;
	find . -type d -name obj -exec rm -rf {} \;
	find . -type d -name packages -exec rm -rf {} \;

pack:
	msbuild FigmaSharp.LiteForms.sln $(ARGS) /p:CreatePackage=true

install:
	msbuild FigmaSharp.LiteForms.sln $(ARGS) /p:InstallAddin=true

check-dependencies:
	#updating the submodules
	git submodule update --init --recursive

	if [ ! -f ./nuget.exe ]; then \
	    echo "nuget.exe not found! downloading latest version" ; \
	    curl -O https://dist.nuget.org/win-x86-commandline/latest/nuget.exe ; \
	fi

submodules: nuget-download
	echo "Restoring FigmaSharp..."
	msbuild FigmaSharp.LiteForms.sln /t:Restore

sdk: nuget-download
	mono src/.nuget/nuget.exe pack FigmaSharp.nuspec

package: check-dependencies
	msbuild FigmaSharp/FigmaSharp.csproj /p:Configuration=Release /t:Restore
	msbuild FigmaSharp.Cocoa/FigmaSharp.Cocoa.csproj /p:Configuration=Release /t:Restore
	msbuild FigmaSharp.Forms/FigmaSharp.Forms.csproj /p:Configuration=Release /t:Restore

	mono nuget.exe restore FigmaSharp.sln
	msbuild FigmaSharp/FigmaSharp.csproj /p:Configuration=Release
	msbuild FigmaSharp.Cocoa/FigmaSharp.Cocoa.csproj /p:Configuration=Release
	msbuild FigmaSharp.Forms/FigmaSharp.Forms.csproj /p:Configuration=Release

	mono nuget.exe pack FigmaSharp.nuspec
	mono nuget.exe pack FigmaSharp.Cocoa.nuspec
	mono nuget.exe pack FigmaSharp.Forms.nuspec

.PHONY: all clean pack install submodules sdk nuget-download check-dependencies
