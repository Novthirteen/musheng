@echo off
Rem C:\WINDOWS\Microsoft.NET\Framework\v3.5\MSBuild.exe Ext.xml /t:Main @Ext.rsp %1

if "%MSBuild_HOME%" == "" set "MSBuild_HOME=C:\WINDOWS\Microsoft.NET\Framework\v3.5"

if "" == "%1" goto errorCreate 
Rem if "" == "%1" set CompanyName=Ext

if ""%1""==""h"" goto printHelpMessage

if not exist "%MSBuild_HOME%\MSBuild.exe" goto MSBuildNotFound
echo.
echo		%date% %time%
echo		MSBuild_HOME:			"%MSBuild_HOME%"
echo		��˾�ؼ���:			"%1"

if ""%2""=="""" goto defaultArgs
call "%MSBuild_HOME%\MSBuild.exe" build.xml /t:%2 /p:CompanyName=%1
echo.
echo		%date% %time%
goto end


:defaultArgs
	echo.
	call "%MSBuild_HOME%\MSBuild.exe" build.xml /t:All /p:CompanyName=%1
	echo		%date% %time%
	goto end


:MSBuildNotFound
	echo.
	echo		�������� MSBuild_HOME ���岻��ȷ.���¶��廷�������������¿���δ���
	goto end


:printHelpMessage
:errorCreate
	echo.
	echo Usage: create.bat ChunShen All
	echo.
	echo ��һ�� ChunShen ����Ĺ�˾�ؼ���. ��������ؼ��ֻᴴ��һ���µ�Ŀ¼
	echo.
	echo �ڶ��� All ��ִ������Ĺؼ���,����ȱʡ.���¿�ѡ��:
	echo		Clean					�����չĿ¼
	echo		Compile					����Service,Entity,Persistence,Utility��Ŀ
	echo		BuildExt				������չ����
	echo		All					Clean �� BuildExt,Ĭ��
	echo.
	goto end


:end

