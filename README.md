# OnLearn Login and Register System

This is a simple C# Windows Forms application using .NET Framework that connects to a MySQL database (XAMPP) for login and registration functionality.

## 📦 Features
- Login and registration forms
- MySQL (XAMPP) connection using MySQL.Data
- Basic credential checking (non-encrypted)
- Single executable installer for easy deployment
- Excel file handling with ClosedXML and EPPlus

## 🛠️ Requirements
- Windows operating system
- .NET 8.0 Runtime
- XAMPP with MySQL running
- Required NuGet packages:
  - MySQL.Data
  - ClosedXML
  - EPPlus

## 📝 Database Setup
1. Start XAMPP and run MySQL.
2. You can use either:
   - phpMyAdmin: Open in browser and run the SQL script
   - MySQL Workbench: 
     - Connect to localhost:3306
     - Open the SQL script file (OnLearnEducation.sql)
     - Execute the script
3. The installer will automatically set up the database during installation

## 🚀 Installation
1. Download the `OnlearnEducationInstaller.exe` from the release
2. Run the installer as administrator
3. Follow the installation wizard
4. When prompted, enter your MySQL root password to set up the database
5. The installer will:
   - Install the application to Program Files
   - Create start menu shortcuts
   - Set up the database automatically
   - Configure all necessary files

## 💻 Usage
1. Start the application from the Start Menu or desktop shortcut
2. The application will connect to the local MySQL database
3. Use the login form to access your account
4. New users can register through the registration form

## 🔧 Troubleshooting
If you encounter any issues:
1. Ensure XAMPP is running and MySQL service is active
2. Verify your MySQL root password is correct
3. Check that .NET 8.0 Runtime is installed
4. If database setup fails, you can manually run the import script:
   - Navigate to `C:\Program Files\OnlearnEducation\db`
   - Run `import_db.bat` as administrator

## 📦 Development
To build from source:
1. Clone the repository
2. Open the solution in Visual Studio
3. Restore NuGet packages
4. Build the solution
5. Run the application

## 🔒 Security Note
- The current implementation uses non-encrypted credentials
- For production use, implement proper password hashing
- Consider using environment variables for database credentials