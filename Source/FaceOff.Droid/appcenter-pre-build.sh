#!/usr/bin/env bash
APIScriptPath=`find . -name injectCognitiveServicesAPI.sh | head -1`

CognitiveServicesConstantsFile=`find . -name CognitiveServicesConstants.cs | head -1`

echo APIScriptPath = $APIScriptPath
echo CognitiveServicesConstantsFile = $CognitiveServicesConstantsFile

bash $APIScriptPath $CognitiveServicesConstantsFile $CognitiveServicesAPIKey

echo "Finished Injecting Cognitive Services API Key"