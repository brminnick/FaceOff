$CognitiveServicesConstantsFile = Get-ChildItem -Path "$APPCENTER_SOURCE_DIRECTORY\*" -Filter  "CognitiveServicesConstants.cs" -Recurse -File | % { $_.FullName }

Write-Host CognitiveServicesConstantsFile = $CognitiveServicesConstantsFile

(Get-Content $CognitiveServicesConstantsFile).replace("Add Face API Key Here", $CognitiveServicesAPIKey) | Set-Content $CognitiveServicesConstantsFile
(Get-Content $CognitiveServicesConstantsFile).replace("#error Face API Key Missing", "//#error Face API Key Missing") | Set-Content $CognitiveServicesConstantsFile

(Get-Content $CognitiveServicesConstantsFile).replace("Add Face API Base Url Here", $CognitiveServicesBaseUri) | Set-Content $CognitiveServicesConstantsFile
(Get-Content $CognitiveServicesConstantsFile).replace("#error Base Url Missing", "//#error Base Url Missing") | Set-Content $CognitiveServicesConstantsFile

Write-Host "Finished Injecting Cognitive Services API Key"