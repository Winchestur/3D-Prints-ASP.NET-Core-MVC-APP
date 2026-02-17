# \# 3D-Prints-ASP.NET-Core-MVC-Application

# 

# 

# \### Web application for managing personal 3D printing workflow — printers, filaments, and printed objects with photos and details.

# 

# 

# 

# 1\. This project allows users to store and organize information about:

# ~~~~

# \* 3D Printers

# \* Filaments

# \* Prints

# \* Relationships between them

# \* Photos and technical details

# ~~~~

# 

# ---

# 

# \### Authentication

# 1\. The application uses ASP.NET Core Identity.

# ~~~~

# To access most features:

# 

# \* Register a new account

# \* Log in

# \* Navigate through the main sections

# ~~~~

# 

# 

# ---

# 

# \### Main Features

# 1\.  Printers

# 

# ~~~~

# \* Add printer with model, nozzle size, description, AMS support and photo

# \* Edit and delete printers

# \* View detailed printer information

# ~~~~

# 2\. Filaments

# 

# ~~~~

# \* Store filament brand, material, color, diameter and weight

# \* Assign filament to a printer

# \* Upload filament image

# ~~~~

# 

# 3\. Prints

# 

# ~~~~

# \* Add printed objects with:

# \* Title

# \* Description

# \* Print time

# \* Printer used

# \* Filaments used

# \* Photo

# \* Many-to-many relationship between prints and filaments

# ~~~~

# 

# 

# 

# ---

# 

# \### Home Page

# 

# 1\. The home page displays:

# 

# ~~~~

# \* Introduction section

# \* Navigation buttons

# \* List of prints (after login)

# \* Search functionality

# ~~~~

# If the user is not logged in, buttons for registration and login are shown.

# 

# ---

# 

# \### Technologies Used

# 

# 1\. ASP.NET Core MVC

# 1\. Entity Framework Core (Code-First)

# 1\. SQL Server

# 1\. ASP.NET Core Identity

# 1\. Bootstrap

# 1\. Razor Views

# 1\. LINQ

# 1\. HTML / CSS / JavaScript

# 

# ---

# 

# \### Database

# 

# 1\. The database is generated using Entity Framework Core migrations.

# 1\. Seed data is for demonstration purpose only.

# 

# ---

# 

# \## How to Run the Project

# 

# 1\. Clone repository

# ~~~~

# \* git clone <repository-url>

# ~~~~

# 2\. Configure database connection

# 

# \* Open appsettings.json and update the connection string:

# ~~~~

# "ConnectionStrings": {

# &nbsp; "DefaultConnection": "Server=(yourServerName);Database=3DPrintsDb;Trusted\_Connection=True;Encrypt=False;MultipleActiveResultSets=true"

# },

# ~~~~

# 

# Replace YOUR\_SERVER with your SQL Server instance.

# 

# 3\. Apply migrations

# 

# \* Open Package Manager Console and run:

# ~~~~

# Update-Database

# ~~~~

# 

# ---

# 

# 4\. Run the application

# 

# \* Start the project from Visual Studio.

# 

# \* \* Default address:

# 

# ~~~~

# https://localhost:xxxx/

# ~~~~

# 

# 

# ---

# 

# 5\. Create account

# 

# \* Register a new user to access the full functionality.

# 

# ---

# 

# \### Screenshots

# &nbsp;Example sections:

# 

# !\[Login](wwwroot/images/Login.png)

# 

# !\[register](wwwroot/images/register.png)

# 

# !\[HomePage1](wwwroot/images/HomePage1.png)

# 

# !\[HomePage2](wwwroot/images/HomePage2.png)

# 

# !\[PrinterIndex](wwwroot/images/PrinterIndex.png)

# 

# !\[AddNewPrinter](wwwroot/images/AddNewPrinter.png)

# 

# !\[FilamentDetails](wwwroot/images/AddNewPrint.png)

# 

# !\[FilamentDelete](wwwroot/images/FilamentDetails.png)

# 

# !\[AddNewPrint](wwwroot/images/FilamentDelete.png)

# 

# !\[DBContext](wwwroot/images/DBContext.png)

# 

# ---

# 

# 

# \### Project Structure

# 

# 1\. Models — database entities

# 1\. ViewModels — models used in views

# 1\. Controllers — application logic

# 1\. Views — Razor UI pages

# 1\. Configurations — EF Core configurations and seed data

# 1\. Identity — authentication system

# 

# 

# 

# ---

# 

# 

# \## Student project created for ASP.NET Fundamentals course.

# 



