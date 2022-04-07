UWPCsproj=`find "$APPCENTER_SOURCE_DIRECTORY" -name FaceOff.UWP.csproj | head -1`
SlnFile=`find "$APPCENTER_SOURCE_DIRECTORY" -name FaceOff.sln | head -1`

echo UWPCsproj = $UWPCsproj
echo SlnFile = $SlnFile

echo "Removing UWP Project"
dotnet sln $SlnFile remove $UWPCsproj