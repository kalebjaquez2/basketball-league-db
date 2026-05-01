@echo off
echo =============================================
echo  Basketball League DB Setup
echo =============================================
echo.
echo Step 1: Running Setup.sql...
echo.
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "%~dp0Setup.sql"
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ERROR in Setup.sql. Stopping.
    pause
    exit /b 1
)
echo.
echo Step 2: Running Populate.sql...
echo.
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "%~dp0Populate.sql"
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ERROR in Populate.sql. See messages above.
) else (
    echo.
    echo SUCCESS: Database is ready!
)
echo.
pause
