cd ../*.Test
dotnet test /p:CollectCoverage=true /p:CoverletOutputFormat=opencover /p:CoverletOutput=../Report/coverage.xml
dotnet reportgenerator -reports:../Report/coverage.xml -targetdir:../Report
cd ../Report
start index.htm
	