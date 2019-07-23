@echo off
setlocal enabledelayedexpansion

dotnet pack --configuration Release -p:PackageVersion=0.2.1 --output ..\artifacts System.Threading.Tasks.Helpers

rem .nuget\nuget.exe push -Source "System.Threading.Tasks.Helpers" -ApiKey AzureDevOps .\artifacts\*.nupkg

echo.
echo Done.
echo.
