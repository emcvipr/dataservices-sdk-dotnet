@echo off
@setlocal

mkdir bin 2> nul
mkdir bin\Debug 2> nul
mkdir bin\Release 2> nul

SET SOLUTIONDIR=%~dp0

SET VERSION=%1
IF "%VERSION%" NEQ "" GOTO versionset
SET VERSION=0.0.0
:versionset

SET CONFIGURATIONNAME=%2
IF "%CONFIGURATIONNAME%" NEQ "" GOTO configurationnameset
SET CONFIGURATIONNAME=Release
:configurationnameset

%SOLUTIONDIR%nuget pack %SOLUTIONDIR%package.nuspec -OutputDirectory %SOLUTIONDIR%bin\%CONFIGURATIONNAME% -Version %VERSION% -Properties "ConfigurationName=%CONFIGURATIONNAME%"
