$APIScriptPath = Get-ChildItem -Path "$APPCENTER_SOURCE_DIRECTORY\*" -Filter "CognitiveServicesConstants.cs" -Recurse -File

$CognitiveServicesConstantsFile = Get-ChildItem -Path "$APPCENTER_SOURCE_DIRECTORY\*" -Filter "injectCognitiveServicesAPI.sh" -Recurse -File

Write-Host APIScriptPath = $APIScriptPath
Write-Host CognitiveServicesConstantsFile = $CognitiveServicesConstantsFile

sh $APIScriptPath $CognitiveServicesConstantsFile $CognitiveServicesAPIKey

Write-Host "Finished Injecting Cognitive Services API Key"