$CognitiveServicesConstantsFile = Get-ChildItem -Path "$APPCENTER_SOURCE_DIRECTORY\*" -Filter  "CognitiveServicesConstants.cs" -Recurse -File | % { $_.FullName }

Write-Host CognitiveServicesConstantsFile = $CognitiveServicesConstantsFile

(Get-Content $CognitiveServicesConstantsFile).replace("Add Face API Key Here", $CognitiveServicesAPIKey) | Set-Content $CognitiveServicesConstantsFile
(Get-Content $CognitiveServicesConstantsFile).replace("#error", "//#error") | Set-Content $CognitiveServicesConstantsFile

Write-Host "Finished Injecting Cognitive Services API Key"