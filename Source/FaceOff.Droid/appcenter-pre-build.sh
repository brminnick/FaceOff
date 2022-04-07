#!/usr/bin/env bash
PostBuildScriptFile=`find "$APPCENTER_SOURCE_DIRECTORY" -name appcenter-post-build.sh | grep Droid | head -1`
CognitiveServicesConstantsFile=`find "$APPCENTER_SOURCE_DIRECTORY" -name CognitiveServicesConstants.cs | head -1`

echo PostBuildScriptFile = $PostBuildScriptFile
echo CognitiveServicesConstantsFile = $CognitiveServicesConstantsFile
echo SlnFile = $SlnFile

sed -i '' "s/Add Face API Key Here/$CognitiveServicesAPIKey/g" "$CognitiveServicesConstantsFile"
sed -i '' "s/#error Face API Key Missing/\/\/#error Face API Key Missing/g" "$CognitiveServicesConstantsFile"

sed -i '' "s/Add Face API Base Url Here/$CognitiveServicesBaseUri/g" "$CognitiveServicesConstantsFile"
sed -i '' "s/#error Base Url Missing/\/\/#error Base Url Missing/g" "$CognitiveServicesConstantsFile"

echo "Finished Injecting Cognitive Services API Key"

sed -i '' "s/--token token/--token $AppCenterLoginToken/g" "$PostBuildScriptFile"

echo "Finished Injecting App Center Login Token"