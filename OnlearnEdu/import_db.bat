@echo off
echo Starting database import process...

REM Check if MySQL is installed and accessible
where mysql >nul 2>nul
if %ERRORLEVEL% neq 0 (
    echo Error: MySQL is not installed or not in PATH
    echo Please install MySQL and try again
    pause
    exit /b 1
)

REM Get MySQL root password
set /p MYSQL_PASSWORD="Enter MySQL root password: "

REM Create database and import data
echo Creating database and importing data...
mysql -u root -p%MYSQL_PASSWORD% -e "CREATE DATABASE IF NOT EXISTS onlearndb;"
if %ERRORLEVEL% neq 0 (
    echo Error: Could not create database
    pause
    exit /b 1
)

mysql -u root -p%MYSQL_PASSWORD% onlearndb < "%~dp0OnLearnEducation.sql"
if %ERRORLEVEL% neq 0 (
    echo Error: Could not import database schema
    pause
    exit /b 1
)

echo Database import completed successfully!
echo.
echo The database 'onlearndb' has been created and populated with initial data.
echo.
pause