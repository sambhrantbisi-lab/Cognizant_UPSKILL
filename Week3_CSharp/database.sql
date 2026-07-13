CREATE DATABASE IF NOT EXISTS EmployeeDB;
USE EmployeeDB;

DROP TABLE IF EXISTS Employees;
DROP TABLE IF EXISTS Departments;

CREATE TABLE Departments
(
    DepartmentId INT AUTO_INCREMENT PRIMARY KEY,
    DepartmentName VARCHAR(100) NOT NULL UNIQUE,
    Location VARCHAR(100) NOT NULL,
    Description VARCHAR(255) NULL
);

CREATE TABLE Employees
(
    EmployeeId INT AUTO_INCREMENT PRIMARY KEY,
    FullName VARCHAR(150) NOT NULL,
    DepartmentId INT NOT NULL,
    Salary DECIMAL(18,2) NOT NULL,
    Email VARCHAR(150) NOT NULL,
    HireDate DATE NOT NULL,
    IsActive TINYINT(1) NOT NULL DEFAULT 1,
    CONSTRAINT FK_Employees_Departments
        FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId)
        ON UPDATE CASCADE
        ON DELETE RESTRICT
);

INSERT INTO Departments (DepartmentName, Location, Description)
VALUES
    ('Human Resources', 'Chennai', 'Hiring, onboarding, and employee relations'),
    ('Information Technology', 'Bengaluru', 'Software development and support'),
    ('Finance', 'Hyderabad', 'Budgeting, payroll, and compliance'),
    ('Operations', 'Pune', 'Daily business operations and logistics');

INSERT INTO Employees (FullName, DepartmentId, Salary, Email, HireDate, IsActive)
VALUES
    ('Asha Menon', 1, 55000.00, 'asha.menon@company.com', '2023-04-10', 1),
    ('Rahul Sharma', 2, 72000.00, 'rahul.sharma@company.com', '2022-07-01', 1),
    ('Neha Iyer', 3, 68000.00, 'neha.iyer@company.com', '2024-01-15', 1);