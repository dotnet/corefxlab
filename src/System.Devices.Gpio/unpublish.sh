#!/bin/sh

rm -rf ./publish/

unzip -d ./publish/ publish.zip > /dev/null

rm publish.zip

chmod +x ./publish/System.Devices.Gpio.Samples
clear
