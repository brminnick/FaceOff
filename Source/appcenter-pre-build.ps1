$APIScriptPath = Get-ChildItem -Path "$APPCENTER_SOURCE_DIRECTORY\*" -Filter "injectCognitiveServicesAPI.sh" -Recurse -File | % { $_.FullName }

$CognitiveServicesConstantsFile = Get-ChildItem -Path "$APPCENTER_SOURCE_DIRECTORY\*" -Filter  "CognitiveServicesConstants.cs" -Recurse -File | % { $_.FullName }

Write-Host APIScriptPath = $APIScriptPath
Write-Host CognitiveServicesConstantsFile = $CognitiveServicesConstantsFile

sh $APIScriptPath $CognitiveServicesConstantsFile $CognitiveServicesAPIKey

Write-Host "Finished Injecting Cognitive Services API Key"