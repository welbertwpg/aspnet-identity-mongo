REM optional clean
REM rm -rf artifacts CoreTests/bin CoreTests/obj CoreIntegrationTests/bin CoreIntegrationTests/obj MongoDB.Identity/bin MongoDB.Identity/obj

dotnet restore src
dotnet test -c Release src/CoreTests
dotnet test -c Release src/CoreIntegrationTests
dotnet pack -c Release -o artifacts src/MongoDB.Identity

REM nuget add artifacts\X.nupkg -Source C:\Code\scratch\localnugetfeedtesting
REM nuget publish artifacts\X.nupkg