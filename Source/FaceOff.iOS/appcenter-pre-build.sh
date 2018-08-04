#!/usr/bin/env bash
APIScriptPath=`find "$APPCENTER_SOURCE_DIRECTORY" -name injectCognitiveServicesAPI.sh | head -1`
PostBuildScriptFile=`find "$APPCENTER_SOURCE_DIRECTORY" -name appcenter-post-build.sh | grep iOS | head -1`
CognitiveServicesConstantsFile=`find "$APPCENTER_SOURCE_DIRECTORY" -name CognitiveServicesConstants.cs | head -1`

echo APIScriptPath = $APIScriptPath
echo PostBuildScriptFile = $PostBuildScriptFile
echo CognitiveServicesConstantsFile = $CognitiveServicesConstantsFile

bash $APIScriptPath $CognitiveServicesConstantsFile $CognitiveServicesAPIKey $CognitiveServicesBaseUri

echo "Finished Injecting Cognitive Services API Key"

sed -i '' "s/--token token/--token $AppCenterLoginToken/g" "$PostBuildScriptFile"

echo "Finished Injecting App Center Login Token"