---
title: 3D-Prints-ASP.NET-Core-MVC-Application

---

# 3D-Prints-ASP.NET-Core-MVC-Application

A web application for managing 3D printing content with role-based functionality for administrators and users.

The application allows administrators to create printer and filament options, while regular users can use those predefined options to build their own 3D printing workflow. Users can create personal prints, publish them publicly, rate prints shared by others, and save interesting models to their personal collection.

---

## Authentication and Authorization

The application uses ASP.NET Core Identity for authentication and authorization.

### Roles
- Administrator
- User

### Access Control
- Administrators manage printer and filament options
- Regular users can use predefined options but cannot edit them
- Users can create and manage only their own prints

---

## Main Features

### Printers
- Dropdown navigation for printer-related actions
- My Printers
- Create Printer
- Printer Options (Admin only)
- Add Printer Option (Admin only)

Administrators create the available printer options.
Users can add and use only those predefined printer options.

### Filaments
- Dropdown navigation for filament-related actions
- My Filaments
- Create Filament
- Filament Options (Admin only)
- Add Filament Option (Admin only)

Administrators create the available filament options.
Users can add and use only those predefined filament options.

### Prints
- My Prints
- Create Print
- Users can create their own 3D prints
- Users can manage only their own prints
- Prints can be published publicly in World Prints

### World Prints
- Displays publicly shared prints from users
- Search bar for searching prints by keyword
- Users can open print details
- Users can rate prints from 1 to 5 stars
- Users can add prints to My Collection

### My Collection
- Users can save public prints created by other users
- Personal collection for favorite or interesting 3D prints

---

## Business Rules

- Printer options are created by administrators
- Filament options are created by administrators
- Regular users cannot edit administrator-created options
- Users can create and manage only their own prints
- Public prints are visible to all users in World Prints
- Public prints can be searched, rated, and added to personal collections

---

## Project Architecture

The project follows a layered architecture:

- Controllers – handle requests and responses
- Services – contain business logic
- Repositories – handle data access
- Data Models – represent database entities
- ViewModels – transfer data to the UI
- Configurations – Entity Framework Core configurations and seed data
- Enums – predefined constants used in the application
- Validations – model validation with data annotations
- Identity – authentication and authorization

---

## Technologies Used

- ASP.NET Core MVC
- Entity Framework Core (Code First)
- SQL Server
- ASP.NET Core Identity
- Repository Pattern
- Service Layer Pattern
- Razor Views
- Bootstrap
- HTML / CSS / JavaScript
- LINQ

---

## Database

- The database is generated using Entity Framework Core migrations
- Seed data is used for initial setup and demonstration
- Entity configurations are separated in dedicated configuration classes

---

## How to Run the Project

1. Clone the repository

```bash
git clone <repository-url>

```


---

2. Configure the database connection in appsettings.json
```
"ConnectionStrings": {
  "DefaultConnection": "Server=(yourServerName);Database=3DPrintsDb;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true"
}
```

##### Replace YOUR_SERVER with your SQL Server instance.


---

3. Apply migrations
- Open Package Manager Console and run:
```
Update-Database
```
---
4. Run the project from Visual Studio


---

5. Register a new account and log in


---

    Admin profile
     - admin@abv.bg 
     - password: admin123456

---


### Screenshots
 Example sections:

![Login](https://github.com/Winchestur/3D-Prints-ASP.NET-Core-MVC-APP/blob/main/3D-Prints-ASP.NET-Core-MVC-APP/wwwroot/images/Login.png)

![register](https://github.com/Winchestur/3D-Prints-ASP.NET-Core-MVC-APP/blob/main/3D-Prints-ASP.NET-Core-MVC-APP/wwwroot/images/register.png)

![HomePage](https://github.com/Winchestur/3D-Prints-ASP.NET-Core-MVC-APP/blob/main/3D-Prints-ASP.NET-Core-MVC-APP/wwwroot/images/HomePage.png)

![MyPrinters](https://github.com/Winchestur/3D-Prints-ASP.NET-Core-MVC-APP/blob/main/3D-Prints-ASP.NET-Core-MVC-APP/wwwroot/images/MyPrinters.png)

![CreatePrinter](https://github.com/Winchestur/3D-Prints-ASP.NET-Core-MVC-APP/blob/main/3D-Prints-ASP.NET-Core-MVC-APP/wwwroot/images/CreatePrinter.png)

![CreatePrinterOption](https://github.com/Winchestur/3D-Prints-ASP.NET-Core-MVC-APP/blob/main/3D-Prints-ASP.NET-Core-MVC-APP/wwwroot/images/CreatePrinterOption.png)

![PrinterOption](https://github.com/Winchestur/3D-Prints-ASP.NET-Core-MVC-APP/blob/main/3D-Prints-ASP.NET-Core-MVC-APP/wwwroot/images/PrinterOption.png)

![EditFilamentOption](https://github.com/Winchestur/3D-Prints-ASP.NET-Core-MVC-APP/blob/main/3D-Prints-ASP.NET-Core-MVC-APP/wwwroot/images/EditFilamentOption.png)

![CreateFilamentOption](https://github.com/Winchestur/3D-Prints-ASP.NET-Core-MVC-APP/blob/main/3D-Prints-ASP.NET-Core-MVC-APP/wwwroot/images/CreateFilamentOption.png)

![MyPrints](https://github.com/Winchestur/3D-Prints-ASP.NET-Core-MVC-APP/blob/main/3D-Prints-ASP.NET-Core-MVC-APP/wwwroot/images/MyPrints.png)

![WorldPrints](https://github.com/Winchestur/3D-Prints-ASP.NET-Core-MVC-APP/blob/main/3D-Prints-ASP.NET-Core-MVC-APP/wwwroot/images/WorldPrints.png)

![MyCollection](https://github.com/Winchestur/3D-Prints-ASP.NET-Core-MVC-APP/blob/main/3D-Prints-ASP.NET-Core-MVC-APP/wwwroot/images/MyCollection.png)

![AdminPages](https://github.com/Winchestur/3D-Prints-ASP.NET-Core-MVC-APP/blob/main/3D-Prints-ASP.NET-Core-MVC-APP/wwwroot/images/AdminPages.png)

---


Project Structure
1. 3D-Prints-ASP.NET-Core-MVC-APP – main web project
1. 3D-Prints-APP.Data – database context and data access
1. 3D-Prints-APP.Data.Model – entity models
1. 3D-Prints-APP-Services – business logic layer
1. 3D-Prints-APP.Web.ViewModels – view models for the UI
1. 3D-Prints-APP.GCommon – common constants, enums, and helpers



---

**Role-based access**
- **admin creates options**
- **users use predefined options**
- **users create only their own prints**
- **World Prints**
- **My Collection**
- **rating 1–5**
- **search bar**
- **Controller + Service + Repository architecture**
- **ViewModels / Configurations / Enums / Validations**


---
## Student project created for ASP.NET Fundamentals course.