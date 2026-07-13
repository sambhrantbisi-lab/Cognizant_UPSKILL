-- Advanced queries: correlated subqueries, complex joins, and examples with expected output comments
USE CompanyDB;

-- 1) Complex correlated subquery: for each employee, show how many colleagues have higher salary in same department
SELECT e.emp_id, e.first_name, e.salary,
  (SELECT COUNT(*) FROM Employees e2 WHERE e2.dept_id = e.dept_id AND e2.salary > e.salary) AS num_higher_paid_in_dept
FROM Employees e
ORDER BY num_higher_paid_in_dept DESC, e.salary DESC;

-- Expected sample (based on inserted data):
-- emp_id | first_name | salary   | num_higher_paid_in_dept
-- 110    | Jack       | 72000.00 | 2
-- 102    | Bob        | 75000.00 | 1
-- 103    | Carol      | 65000.00 | 1
-- 106    | Frank      |120000.00 | 0
-- 108    | Henry      |110000.00 | 0

-- 2) Subquery in FROM to compute average allocation per employee then filter
SELECT t.emp_id, t.total_alloc
FROM (
  SELECT emp_id, SUM(allocation_percent) AS total_alloc
  FROM Employee_Project
  GROUP BY emp_id
) AS t
WHERE t.total_alloc > 50;

-- Expected: emp_id and total_alloc rows such as
-- 102 | 110
-- 103 | 60
-- 105 | 60
-- 106 | 70
-- 108 | 80

-- 3) Use EXISTS (correlated) to find employees assigned to any project with budget > 100000
SELECT e.emp_id, e.first_name
FROM Employees e
WHERE EXISTS (
  SELECT 1 FROM Employee_Project ep
  JOIN Projects p ON ep.project_id = p.project_id
  WHERE ep.emp_id = e.emp_id AND p.budget > 100000
);

-- 4) Demonstrate window function (MySQL 8+) to rank employees by salary within department
-- Note: Window functions are supported in MySQL 8+. If not available, skip this.
SELECT emp_id, first_name, dept_id, salary,
  RANK() OVER (PARTITION BY dept_id ORDER BY salary DESC) AS salary_rank_in_dept
FROM Employees;

  -- Expected for EXISTS query (employees assigned to project(s) with budget >100000):
  -- 102 (Bob), 110 (Jack), 108 (Henry)

-- 5) Update with JOIN example: give 5% raise to employees in dept 3
UPDATE Employees e
JOIN Departments d ON e.dept_id = d.dept_id
SET e.salary = e.salary * 1.05
WHERE d.dept_id = 3;

-- Revert the change for repeatability
UPDATE Employees SET salary = salary / 1.05 WHERE dept_id = 3;

-- 6) Demonstrate DELETE with subquery: remove Employee_Project rows for non-existing employees (safe cleanup)
DELETE FROM Employee_Project
WHERE emp_id NOT IN (SELECT emp_id FROM Employees);

-- 7) Create indexes (examples to run manually)
-- CREATE INDEX idx_projects_dept ON Projects(dept_id);
-- CREATE INDEX idx_ep_project ON Employee_Project(project_id);

-- 8) Show expected output example for correlated subquery 1 (sample)
-- Expected sample rows:
-- emp_id | first_name | salary | num_higher_paid_in_dept
-- 106    | Frank      | 120000 | 0
-- 108    | Henry      | 110000 | 1
-- 101    | Alice      | 90000  | 2

-- End of advanced_queries.sql
