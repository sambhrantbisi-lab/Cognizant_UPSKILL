-- Insert sample data into CompanyDB tables
USE CompanyDB;

-- Departments: at least 10 records
INSERT INTO Departments (dept_id, dept_name, location) VALUES
(1, 'Human Resources', 'New York'),
(2, 'Finance', 'Chicago'),
(3, 'IT', 'San Francisco'),
(4, 'Sales', 'Boston'),
(5, 'Marketing', 'Seattle'),
(6, 'R&D', 'Palo Alto'),
(7, 'Operations', 'Austin'),
(8, 'Legal', 'Washington'),
(9, 'Procurement', 'Denver'),
(10, 'Admin', 'Orlando');

-- Employees: at least 10 records
INSERT INTO Employees (emp_id, first_name, last_name, email, phone, hire_date, salary, dept_id, manager_id) VALUES
(101, 'Alice', 'Johnson', 'alice.johnson@example.com', '555-0101', '2018-02-15', 90000.00, 3, NULL),
(102, 'Bob', 'Smith', 'bob.smith@example.com', '555-0102', '2019-06-01', 75000.00, 3, 101),
(103, 'Carol', 'Williams', 'carol.williams@example.com', '555-0103', '2020-01-20', 65000.00, 4, 104),
(104, 'David', 'Brown', 'david.brown@example.com', '555-0104', '2017-08-12', 98000.00, 4, NULL),
(105, 'Eva', 'Davis', 'eva.davis@example.com', '555-0105', '2021-03-03', 58000.00, 5, 106),
(106, 'Frank', 'Miller', 'frank.miller@example.com', '555-0106', '2016-11-23', 120000.00, 2, NULL),
(107, 'Grace', 'Wilson', 'grace.wilson@example.com', '555-0107', '2022-07-10', 52000.00, 1, 101),
(108, 'Henry', 'Moore', 'henry.moore@example.com', '555-0108', '2015-05-05', 110000.00, 6, NULL),
(109, 'Ivy', 'Taylor', 'ivy.taylor@example.com', '555-0109', '2020-12-01', 67000.00, 7, 106),
(110, 'Jack', 'Anderson', 'jack.anderson@example.com', '555-0110', '2019-09-15', 72000.00, 3, 101);

-- Projects: at least 10 records
INSERT INTO Projects (project_id, project_name, start_date, end_date, budget, dept_id) VALUES
(1001, 'Website Revamp', '2022-01-01', '2022-06-30', 50000.00, 3),
(1002, 'Mobile App', '2021-05-10', '2022-03-31', 120000.00, 3),
(1003, 'Sales Outreach', '2022-02-01', NULL, 30000.00, 4),
(1004, 'Market Research', '2021-09-15', '2022-02-28', 25000.00, 5),
(1005, 'Procurement System', '2022-03-01', '2022-12-31', 80000.00, 9),
(1006, 'CRM Migration', '2021-11-01', '2022-04-30', 60000.00, 4),
(1007, 'AI Prototype', '2022-06-01', NULL, 200000.00, 6),
(1008, 'Compliance Revamp', '2020-01-10', '2021-12-31', 40000.00, 8),
(1009, 'Operations Optimization', '2022-04-01', NULL, 75000.00, 7),
(1010, 'Finance Dashboard', '2022-02-15', '2022-08-15', 45000.00, 2);

-- Employee_Project: at least 10 records (many-to-many)
INSERT INTO Employee_Project (emp_id, project_id, role, allocation_percent) VALUES
(101, 1001, 'Lead Developer', 50),
(102, 1001, 'Developer', 50),
(102, 1002, 'Developer', 60),
(104, 1003, 'Sales Lead', 40),
(103, 1006, 'Sales Rep', 60),
(106, 1010, 'Finance Manager', 70),
(108, 1007, 'Research Lead', 80),
(109, 1009, 'Operations Analyst', 50),
(105, 1004, 'Marketing Analyst', 60),
(110, 1002, 'QA Engineer', 40);

-- End of data inserts
