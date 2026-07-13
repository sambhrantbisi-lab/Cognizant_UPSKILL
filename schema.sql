-- CompanyDB schema for Week 2 ANSI SQL practice (MySQL compatible)

CREATE DATABASE IF NOT EXISTS CompanyDB;
USE CompanyDB;

-- Departments table
CREATE TABLE IF NOT EXISTS Departments (
  dept_id INT PRIMARY KEY,
  dept_name VARCHAR(100) NOT NULL UNIQUE,
  location VARCHAR(100)
);

-- Employees table
CREATE TABLE IF NOT EXISTS Employees (
  emp_id INT PRIMARY KEY,
  first_name VARCHAR(50) NOT NULL,
  last_name VARCHAR(50) NOT NULL,
  email VARCHAR(100) NOT NULL UNIQUE,
  phone VARCHAR(20),
  hire_date DATE NOT NULL,
  salary DECIMAL(10,2) NOT NULL CHECK (salary >= 0),
  dept_id INT,
  manager_id INT,
  CONSTRAINT fk_emp_dept FOREIGN KEY (dept_id) REFERENCES Departments(dept_id) ON DELETE SET NULL,
  CONSTRAINT fk_emp_manager FOREIGN KEY (manager_id) REFERENCES Employees(emp_id) ON DELETE SET NULL
);

-- Projects table
CREATE TABLE IF NOT EXISTS Projects (
  project_id INT PRIMARY KEY,
  project_name VARCHAR(150) NOT NULL,
  start_date DATE NOT NULL,
  end_date DATE,
  budget DECIMAL(12,2) CHECK (budget >= 0),
  dept_id INT,
  CONSTRAINT fk_proj_dept FOREIGN KEY (dept_id) REFERENCES Departments(dept_id) ON DELETE SET NULL,
  CONSTRAINT chk_dates CHECK (end_date IS NULL OR end_date >= start_date)
);

-- Employee_Project (many-to-many)
CREATE TABLE IF NOT EXISTS Employee_Project (
  id INT PRIMARY KEY AUTO_INCREMENT,
  emp_id INT NOT NULL,
  project_id INT NOT NULL,
  role VARCHAR(100),
  allocation_percent INT NOT NULL CHECK (allocation_percent BETWEEN 0 AND 100),
  CONSTRAINT fk_ep_emp FOREIGN KEY (emp_id) REFERENCES Employees(emp_id) ON DELETE CASCADE,
  CONSTRAINT fk_ep_proj FOREIGN KEY (project_id) REFERENCES Projects(project_id) ON DELETE CASCADE,
  CONSTRAINT uq_emp_proj UNIQUE (emp_id, project_id)
);

-- End of schema
