@echo off

echo - Publishing...

	cd System.Devices.Gpio.Samples
	dotnet publish -r linux-arm >NUL
	cd ..

echo - Creating publish.zip...

	del publish.zip >NUL
	cd System.Devices.Gpio.Samples\bin\Debug\netcoreapp2.1\linux-arm\publish
	7z a ..\..\..\..\..\..\publish.zip * >NUL
	cd ..\..\..\..\..\..\

echo - Uploading publish.zip to device...

	sftpc pi@pi3 -pw=pi < publish_upload.txt >NUL
	pause

echo - Connecting to device...

	stermc pi@pi3 -pw=pi
