@echo off
@setlocal

SET SOLUTIONDIR=%~dp0
SET PATH=%ProgramFiles%\WiX Toolset v3.7\bin;%PATH%

SET VERSION=%1
IF "%VERSION%" NEQ "" GOTO versionset
SET VERSION=0.0.0
:versionset

SET CONFIGURATIONNAME=%2
IF "%CONFIGURATIONNAME%" NEQ "" GOTO configurationnameset
SET CONFIGURATIONNAME=Release
:configurationnameset

CD %SOLUTIONDIR%

mkdir bin 2> nul
mkdir bin\Debug 2> nul
mkdir bin\Release 2> nul
mkdir obj 2> nul
mkdir obj\Debug 2> nul
mkdir obj\Release 2> nul

REM MSI
candle ViPR.wxs -o obj\%ConfigurationName%\ViPR.wixobj
candle AWSSDK\AWSSDK.wxs -o obj\%ConfigurationName%\AWSSDK.wixobj -dAWSSDKSrc.TargetDir=%SOLUTIONDIR%\AWSSDK\bin\%ConfigurationName%
candle AWS.Extensions\SessionProvider\AWS.SessionProvider.wxs -o obj\%ConfigurationName%\AWS.SessionProvider.wixobj -dSessionProvider.TargetDir=%SOLUTIONDIR%\AWS.Extensions\SessionProvider\bin\%ConfigurationName%
candle AWS.Extensions\TraceListener\AWS.TraceListener.wxs -o obj\%ConfigurationName%\AWS.TraceListener.wixobj -dTraceListener.TargetDir=%SOLUTIONDIR%\AWS.Extensions\TraceListener\bin\%ConfigurationName%

light -o bin\%ConfigurationName%\ViPRDataServicesSDK.%Version%.msi -pdbout obj\%ConfigurationName%\ViPRDataServicesSDK.%Version%.wixpdb obj\%ConfigurationName%\ViPR.wixobj obj\%ConfigurationName%\AWSSDK.wixobj obj\%ConfigurationName%\AWS.SessionProvider.wixobj obj\%ConfigurationName%\AWS.TraceListener.wixobj

REM NUGET
%SOLUTIONDIR%nuget pack %SOLUTIONDIR%package.nuspec -OutputDirectory %SOLUTIONDIR%bin\%CONFIGURATIONNAME% -Version %VERSION% -Properties "ConfigurationName=%CONFIGURATIONNAME%"
