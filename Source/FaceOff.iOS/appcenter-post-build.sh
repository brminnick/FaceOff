#!/usr/bin/env bash
if [ "$APPCENTER_XAMARIN_CONFIGURATION" == "Debug" ];then

    echo "Post Build Script Started"
    
    SolutionFile=`find "$APPCENTER_SOURCE_DIRECTORY" -name FaceOff.sln`
    SolutionFileFolder=`dirname $SolutionFile`

    UITestProject=`find "$APPCENTER_SOURCE_DIRECTORY" -name FaceOff.UITests.csproj`

    echo SolutionFile: $SolutionFile
    echo SolutionFileFolder: $SolutionFileFolder
    echo UITestProject: $UITestProject

    chmod -R 777 $SolutionFileFolder

    msbuild "$UITestProject" /property:Configuration=$APPCENTER_XAMARIN_CONFIGURATION

    UITestDLL=`find "$APPCENTER_SOURCE_DIRECTORY" -name "FaceOff.UITests.dll" | grep bin`
    echo UITestDLL: $UITestDLL

    UITestBuildDir=`dirname $UITestDLL`
    echo UITestBuildDir: $UITestBuildDir

    UITestVersionNumber=`grep '[0-9]' $UITestProject | grep Xamarin.UITest|grep -o '[0-9]\{1,3\}\.[0-9]\{1,3\}\.[0-9]\{1,3\}'`
    echo UITestVersionNumber: $UITestVersionNumber

    TestCloudExe=`find ~/.nuget | grep test-cloud.exe | grep $UITestVersionNumber | head -1`
    echo TestCloudExe: $TestCloudExe

    TestCloudExeDirectory=`dirname $TestCloudExe`
    echo TestCloudExeDirectory: $TestCloudExeDirectory

    IPAFile=`find "$APPCENTER_SOURCE_DIRECTORY" -name *.ipa | head -1`
    echo IPAFile: $IPAFile

    DSYMFile=`find "$APPCENTER_SOURCE_DIRECTORY" -name *.dsym | head -1`
    DSYMDirectory=`dirname $DSYMFile`

    npm install -g appcenter-cli@1.2.2

    appcenter login --token token

    appcenter test run uitest --app "CDA-Global-Beta/FaceOff-iOS" --devices "CDA-Global-Beta/current-ios-minus-1" --app-path $IPAFile --test-series "master" --locale "en_US" --build-dir $UITestBuildDir --dsym-dir $DSYMDirectory --uitest-tools-dir $TestCloudExeDirectory
fi