CompanyDB — Week 2 ANSI SQL Practice (MySQL)

Project Overview
- Purpose: A hands-on practice project for core ANSI SQL concepts using MySQL.
- Database: `CompanyDB` with tables: `Employees`, `Departments`, `Projects`, `Employee_Project`.

Files
- schema.sql: Creates the database and tables with PK, FK, UNIQUE, and CHECK constraints.
- insert_data.sql: Inserts at least 10 sample records into each table.
- queries.sql: Basic queries demonstrating SELECT, WHERE, ORDER BY, aggregates, GROUP BY/HAVING, joins, subqueries, and DML (INSERT/UPDATE/DELETE).
- advanced_queries.sql: Correlated subqueries, derived tables, window function example, and index examples.

How to run (MySQL client)
1. Open MySQL CLI or MySQL Workbench connected to your server.
2. Execute schema and data scripts:

```sql
SOURCE c:/Users/priyanshu/SQL/schema.sql;
SOURCE c:/Users/priyanshu/SQL/insert_data.sql;
```

3. Run queries interactively or source `queries.sql` and `advanced_queries.sql`.

Concepts Covered
- Data definition: CREATE DATABASE, CREATE TABLE, constraints (PK, FK, UNIQUE, CHECK)
- Data manipulation: INSERT, UPDATE, DELETE
- Querying: SELECT, WHERE, AND/OR, BETWEEN, IN, LIKE, ORDER BY
- Aggregation: COUNT, SUM, AVG, MIN, MAX, GROUP BY, HAVING
- Joins: INNER, LEFT, RIGHT, CROSS, SELF
- Subqueries: in SELECT, WHERE, FROM; correlated subqueries
- Indexing: CREATE INDEX examples
- ALTER TABLE example (commented) and window functions (MySQL 8+ example)

Notes
- MySQL historically ignored CHECK constraints prior to 8.0.16; they are included for ANSI compliance but behavior may vary by server version.
- The sample data and expected outputs are included in `queries.sql` and `advanced_queries.sql` as comments; results may vary if you alter data.

Next steps
- Run the scripts in your MySQL environment.
- Try modifying data and re-running queries to see different outputs.

