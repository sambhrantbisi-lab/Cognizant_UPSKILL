# Employee Management System

Week 3 project for Cognizant Digital Nurture 5.0 on C# and ADO.NET.

## Features

- .NET 8 C# console application
- Object-oriented design with classes, inheritance, abstraction, and interfaces
- Constructors, encapsulation, properties, and method overloading
- Exception handling for invalid input, database errors, file errors, and null safety
- Collections: `List`, `Dictionary`, `Queue`, and `Stack`
- LINQ-based mapping and in-memory processing
- JSON export with file handling
- Async and await throughout the data layer and service layer
- Multithreading demo with both `Task` and `Thread`
- MySQL ADO.NET CRUD operations using `MySqlConnection`, `MySqlCommand`, `MySqlDataReader`, `MySqlDataAdapter`, `DataSet`, and `DataTable`

## Project Structure

- `Models` - domain classes and abstractions
- `Services` - business logic and demos
- `DataAccess` - ADO.NET/MySQL operations
- `Utilities` - console and JSON helpers
- `Program.cs` - menu-driven application entry point

## Database Script

Run [`database.sql`](database.sql) in MySQL to create the `EmployeeDB` database, tables, and sample data.

## Prerequisites

- .NET 8 SDK
- MySQL Server
- Visual Studio 2022 or Visual Studio Code with C# support

## How to Run in Visual Studio

1. Open the `Employee Management` folder in Visual Studio.
2. Restore NuGet packages when prompted.
3. Run `database.sql` in MySQL to create the database and tables.
4. Open [`Program.cs`](Program.cs) and update `DefaultConnectionString` with your MySQL password if needed.
5. Build the solution.
6. Start the application with F5 or Ctrl+F5.

## How to Run in Visual Studio Code

1. Open the `Employee Management` folder in VS Code.
2. Install the C# extension if it is not already installed.
3. Run `database.sql` in MySQL to create the database and tables.
4. Open [`Program.cs`](Program.cs) and update `DefaultConnectionString` with your MySQL password if needed.
5. Restore packages with `dotnet restore`.
6. Start the app with `dotnet run`.

## Menu

1. Add Employee
2. View Employees
3. Search Employee
4. Update Employee
5. Delete Employee
6. Export Employees to JSON
7. Demonstrate Multithreading
8. Exit

## Notes

- The application assumes the `Departments` table already contains valid records.
- Add new departments through `database.sql` or directly in MySQL before inserting employees.
- JSON exports are written to `employees.json` in the application root.