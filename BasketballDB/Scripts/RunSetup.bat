@echo off
echo =============================================
echo  Basketball League DB Setup
echo =============================================
echo.
echo Running Setup.sql against (localdb)\MSSQLLocalDB...
echo.
sqlcmd -S "(localdb)\MSSQLLocalDB" -i "%~dp0Setup.sql"
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ERROR: Something went wrong. See messages above.
) else (
    echo.
    echo SUCCESS: Database is ready!
)
echo.
pause
