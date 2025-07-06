@echo off
setlocal EnableDelayedExpansion

REM Přepnutí na UTF-8 výstup (pro české znaky v názvech)
chcp 65001 >nul

REM Získání seznamu změněných souborů (staged i unstaged)
for /f "delims=" %%i in ('git diff --name-only') do (
    set FILES=!FILES!, %%i
)

REM Odstranění první čárky a mezery
set FILES=%FILES:~2%

REM Výpis pro kontrolu
echo Commit message bude:
echo Změny: %FILES%
echo.

REM Potvrzení
set /p CONFIRM=Potvrdit commit? (A/N): 
if /i "%CONFIRM%"=="A" (
    git add .
    git commit -m "Změny: %FILES%"
    echo Commit hotov ✅
) else (
    echo Commit zrušen ❌
)

pause