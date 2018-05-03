$CognitiveServicesConstantsFile = Get-ChildItem -Path "$APPCENTER_SOURCE_DIRECTORY\*" -Filter  "CognitiveServicesConstants.cs" -Recurse -File | % { $_.FullName }

Write-Host CognitiveServicesConstantsFile = $CognitiveServicesConstantsFile

cat $CognitiveServicesConstantsFile | % { $_ -replace "Add Face API Key Here", $CognitiveServicesAPIKey }
cat $CognitiveServicesConstantsFile | % { $_ -replace "#error", "//#error" }

Write-Host "Finished Injecting Cognitive Services API Key"