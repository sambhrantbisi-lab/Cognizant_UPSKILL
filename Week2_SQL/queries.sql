-- Basic queries demonstrating SELECT, WHERE, ORDER BY, aggregates, GROUP BY, HAVING, and joins
USE CompanyDB;

-- 1) Simple SELECT
-- Return all employees
-- Expected: 10 rows with emp_id 101..110
SELECT * FROM Employees;

-- 2) WHERE with AND, OR, BETWEEN, IN, LIKE
-- Employees in dept 3 (IT) with salary > 70000
-- Expected: emp_id 101 (Alice, 90000.00), 102 (Bob, 75000.00), 110 (Jack, 72000.00)
SELECT emp_id, first_name, last_name, salary
FROM Employees
WHERE dept_id = 3 AND salary > 70000;

-- Employees in dept 3 OR with salary > 100000 (demonstrates OR)
-- Expected emp_id: 101,102,110 (dept 3) plus 106,108 (salary >100000) => unique set: 101,102,106,108,110
SELECT emp_id, first_name, salary
FROM Employees
WHERE dept_id = 3 OR salary > 100000;

-- Employees hired between 2019-01-01 and 2021-12-31
-- Expected emp_id: 102, 103, 105, 109, 110
SELECT emp_id, first_name, hire_date
FROM Employees
WHERE hire_date BETWEEN '2019-01-01' AND '2021-12-31';

-- Employees whose last name is one of (Smith, Brown)
-- Expected: 102 Bob Smith, 104 David Brown
SELECT emp_id, first_name, last_name
FROM Employees
WHERE last_name IN ('Smith', 'Brown');

-- Employees with email LIKE '%example.com' (all)
-- Expected: 10 rows (all employees use @example.com in this dataset)
SELECT emp_id, email
FROM Employees
WHERE email LIKE '%@example.com';

-- 3) ORDER BY
-- List employees by salary descending
-- Expected order (emp_id): 106,108,104,101,102,110,109,103,105,107
SELECT emp_id, first_name, salary
FROM Employees
ORDER BY salary DESC;

-- 4) Aggregate functions
-- Count employees
-- Expected: total_employees = 10
SELECT COUNT(*) AS total_employees FROM Employees;

-- Average salary
-- Expected: avg_salary = 80700.00 (based on inserted sample salaries)
SELECT AVG(salary) AS avg_salary FROM Employees;

-- Min and Max salary
-- Expected: min_salary = 52000.00, max_salary = 120000.00
SELECT MIN(salary) AS min_salary, MAX(salary) AS max_salary FROM Employees;

-- Sum of budgets across projects
-- Expected: total_project_budget = 725000.00
SELECT SUM(budget) AS total_project_budget FROM Projects;

-- 5) GROUP BY and HAVING
-- Number of employees per department with at least 2 employees
-- Expected rows: IT (3), Sales (2)
SELECT d.dept_name, COUNT(e.emp_id) AS num_employees
FROM Departments d
LEFT JOIN Employees e ON d.dept_id = e.dept_id
GROUP BY d.dept_id, d.dept_name
HAVING COUNT(e.emp_id) >= 2;

-- 6) INNER JOIN
-- Employees and their department names
-- Expected: rows mapping each employee to their department name
SELECT e.emp_id, e.first_name, d.dept_name
FROM Employees e
INNER JOIN Departments d ON e.dept_id = d.dept_id;

-- 7) LEFT JOIN
-- All employees and project allocations (if any)
-- Expected: employees with assigned projects show project_id and role; employees without projects (e.g., emp 107) show NULL
SELECT e.emp_id, e.first_name, ep.project_id, ep.role
FROM Employees e
LEFT JOIN Employee_Project ep ON e.emp_id = ep.emp_id;

-- 8) RIGHT JOIN
-- All projects and assigned employees (projects with no employees will show NULL emp)
-- Expected: one row per Employee_Project assignment, for example project 1001 -> emp 101,102
SELECT p.project_id, p.project_name, ep.emp_id
FROM Projects p
RIGHT JOIN Employee_Project ep ON p.project_id = ep.project_id;

-- 9) CROSS JOIN
-- Cartesian product example (limit to small for demonstration)
-- Expected: 100 total combinations (10 employees x 10 projects) but only first 10 shown
SELECT e.emp_id, p.project_id
FROM Employees e
CROSS JOIN Projects p
LIMIT 10; -- show only first 10 rows

-- 10) SELF JOIN
-- Get employees and their manager's name (if manager exists)
-- Expected: rows where manager columns show the manager's emp_id/name; NULL for top-level managers
SELECT e.emp_id, e.first_name AS employee, m.emp_id AS manager_id, m.first_name AS manager
FROM Employees e
LEFT JOIN Employees m ON e.manager_id = m.emp_id;

-- 11) Subquery in SELECT
-- For each employee, list emp_id and total allocation across projects
-- Expected sample: emp 101 -> 50, 102 -> 110, 107 -> 0
SELECT e.emp_id, e.first_name,
  (SELECT IFNULL(SUM(allocation_percent),0) FROM Employee_Project ep WHERE ep.emp_id = e.emp_id) AS total_allocation
FROM Employees e;

-- 12) Subquery in WHERE
-- Employees assigned to project 1001
-- Expected: emp_id 101, 102
SELECT emp_id, first_name
FROM Employees
WHERE emp_id IN (SELECT emp_id FROM Employee_Project WHERE project_id = 1001);

-- 13) Subquery in FROM (derived table)
-- Department employee counts via subquery in FROM
-- Expected: dept_name and emp_count for departments with employees (counts derived from inserted data)
SELECT dept_name, emp_count
FROM (
  SELECT d.dept_name, COUNT(e.emp_id) AS emp_count
  FROM Departments d
  LEFT JOIN Employees e ON d.dept_id = e.dept_id
  GROUP BY d.dept_id, d.dept_name
) AS dept_counts
WHERE emp_count > 0;

-- 14) Correlated subquery
-- List projects where budget is greater than the average budget of projects in the same department
-- Expected: project_id 1002 (Mobile App, dept 3) and 1006 (CRM Migration, dept 4)
SELECT p1.project_id, p1.project_name, p1.budget, p1.dept_id
FROM Projects p1
WHERE p1.budget > (
  SELECT AVG(p2.budget) FROM Projects p2 WHERE p2.dept_id = p1.dept_id
);

-- 15) INSERT, UPDATE, DELETE examples
-- INSERT a new temp employee
INSERT INTO Employees (emp_id, first_name, last_name, email, phone, hire_date, salary, dept_id, manager_id)
VALUES (111, 'Kyle', 'Peters', 'kyle.peters@example.com', '555-0111', '2023-01-05', 60000.00, 5, 106);

-- UPDATE salary for emp 111
UPDATE Employees SET salary = 62000.00 WHERE emp_id = 111;

-- DELETE the temp employee
DELETE FROM Employees WHERE emp_id = 111;

-- 16) ALTER TABLE examples
-- Add a new column `status` to Projects (example, will run when needed)
-- ALTER TABLE Projects ADD COLUMN status VARCHAR(20) DEFAULT 'active';

-- 17) CREATE INDEX examples
-- Create index on Employees(dept_id)
-- CREATE INDEX idx_employees_dept ON Employees(dept_id);

-- End of queries.sql
