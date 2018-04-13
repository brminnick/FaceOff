#!/usr/bin/env bash
APIScriptPath=`find . -name injectCognitiveServicesAPI.sh | head -1`

CognitiveServicesConstantsFile=`find . -name CognitiveServicesConstants.cs | head -1`

bash $APIScriptPath $CognitiveServicesConstantsFile $CognitiveServicesAPIKey