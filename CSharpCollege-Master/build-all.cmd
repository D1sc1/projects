@echo off
rem Builds all course projects into the _exe folder (see build-all.ps1)
powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0build-all.ps1" %*
pause
