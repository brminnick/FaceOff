#!/usr/bin/env bash
PostBuildScriptFile=`find "$APPCENTER_SOURCE_DIRECTORY" -name appcenter-post-build.sh | grep Droid | head -1`
CognitiveServicesConstantsFile=`find "$APPCENTER_SOURCE_DIRECTORY" -name CognitiveServicesConstants.cs | head -1`
SlnFile=`find "$APPCENTER_SOURCE_DIRECTORY" -name FaceOff.sln | head -1`

echo PostBuildScriptFile = $PostBuildScriptFile
echo CognitiveServicesConstantsFile = $CognitiveServicesConstantsFile
echo SlnFile = $SlnFile

sed -i '' "s/Add Face API Key Here/$CognitiveServicesAPIKey/g" "$CognitiveServicesConstantsFile"
sed -i '' "s/#error Face API Key Missing/\/\/#error Face API Key Missing/g" "$CognitiveServicesConstantsFile"

sed -i '' "s/Add Face API Base Url Here/$CognitiveServicesBaseUri/g" "$CognitiveServicesConstantsFile"
sed -i '' "s/#error Base Url Missing/\/\/#error Base Url Missing/g" "$CognitiveServicesConstantsFile"

echo "Finished Injecting Cognitive Services API Key"

sed -i '' "s/--token token/--token $AppCenterLoginToken/g" "$PostBuildScriptFile"

echo "Removing UWP Project"
sed -i '' "Project(\"{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}\") = \"FaceOff.UWP\", \"FaceOff.UWP\FaceOff.UWP.csproj\", \"{7E3407D5-2430-4086-8852-4C20F5B6F78F}\"" "$SlnFile"

echo "Finished Injecting App Center Login Token"