using System.Data;
using EmployeeManagementSystem.DataAccess;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Utilities;

namespace EmployeeManagementSystem.Services;

/// <summary>
/// Contains the business logic for employees, departments, collections, LINQ, and threading demos.
/// </summary>
public class EmployeeService
{
    private readonly DatabaseHelper _databaseHelper;
    private readonly JsonHelper _jsonHelper;
    private readonly Queue<Employee> _recentEmployees = new();
    private readonly Stack<string> _auditTrail = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="EmployeeService"/> class.
    /// </summary>
    /// <param name="databaseHelper">The database helper.</param>
    /// <param name="jsonHelper">The JSON helper.</param>
    public EmployeeService(DatabaseHelper databaseHelper, JsonHelper jsonHelper)
    {
        _databaseHelper = databaseHelper;
        _jsonHelper = jsonHelper;
    }

    /// <summary>
    /// Adds an employee to the database.
    /// </summary>
    /// <param name="employee">The employee to add.</param>
    public async Task AddEmployeeAsync(Employee employee)
    {
        ValidateEmployee(employee);
        await EnsureDepartmentExistsAsync(employee.DepartmentId);

        await _databaseHelper.InsertEmployeeAsync(employee);
        TrackActivity($"Added employee {employee.Id}", employee);
    }

    /// <summary>
    /// Updates an employee in the database.
    /// </summary>
    /// <param name="employee">The employee to update.</param>
    public async Task UpdateEmployeeAsync(Employee employee)
    {
        ValidateEmployee(employee);
        await EnsureDepartmentExistsAsync(employee.DepartmentId);

        await _databaseHelper.UpdateEmployeeAsync(employee);
        TrackActivity($"Updated employee {employee.Id}", employee);
    }

    /// <summary>
    /// Deletes an employee from the database.
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    public async Task DeleteEmployeeAsync(int employeeId)
    {
        await _databaseHelper.DeleteEmployeeAsync(employeeId);
        TrackActivity($"Deleted employee {employeeId}");
    }

    /// <summary>
    /// Returns all employees using a DataSet, DataTable, and LINQ projection.
    /// </summary>
    /// <returns>The employees in a list.</returns>
    public async Task<List<Employee>> GetAllEmployeesAsync()
    {
        DataSet dataSet = await _databaseHelper.GetEmployeesDataSetAsync();
        DataTable employeesTable = dataSet.Tables["Employees"] ?? new DataTable("Employees");
        List<Employee> employees = employeesTable.AsEnumerable().Select(MapEmployeeRow).ToList();
        UpdateRecentEmployees(employees);
        return employees;
    }

    /// <summary>
    /// Searches for a single employee by identifier.
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    /// <returns>The matching employee or null.</returns>
    public Task<Employee?> SearchEmployeeAsync(int employeeId)
    {
        return _databaseHelper.GetEmployeeByIdAsync(employeeId);
    }

    /// <summary>
    /// Searches for employees by department name.
    /// </summary>
    /// <param name="departmentName">The department name.</param>
    /// <returns>The matching employees.</returns>
    public Task<List<Employee>> SearchEmployeeAsync(string departmentName)
    {
        return _databaseHelper.GetEmployeesByDepartmentAsync(departmentName);
    }

    /// <summary>
    /// Returns the departments available in the database.
    /// </summary>
    /// <returns>The departments in a list.</returns>
    public Task<List<Department>> GetDepartmentsAsync()
    {
        return _databaseHelper.GetDepartmentsAsync();
    }

    /// <summary>
    /// Exports all employees to a JSON file.
    /// </summary>
    /// <param name="filePath">The destination file.</param>
    public async Task ExportEmployeesToJsonAsync(string filePath)
    {
        List<Employee> employees = await GetAllEmployeesAsync();
        await _jsonHelper.SaveEmployeesAsync(filePath, employees);
    }

    /// <summary>
    /// Demonstrates Task and Thread based concurrency.
    /// </summary>
    /// <returns>The combined activity log.</returns>
    public async Task<IReadOnlyList<string>> DemonstrateMultithreadingAsync()
    {
        List<string> taskMessages = new();
        List<string> threadMessages = new();
        using ManualResetEventSlim threadFinished = new(false);

        Task task = Task.Run(() =>
        {
            for (int step = 1; step <= 3; step++)
            {
                taskMessages.Add($"Task worker processed step {step} on managed thread {Environment.CurrentManagedThreadId}.");
                Thread.Sleep(150);
            }
        });

        Thread thread = new(() =>
        {
            for (int step = 1; step <= 3; step++)
            {
                threadMessages.Add($"Thread worker processed step {step} on managed thread {Environment.CurrentManagedThreadId}.");
                Thread.Sleep(150);
            }

            threadFinished.Set();
        });

        thread.Start();
        await task;
        threadFinished.Wait();
        thread.Join();

        List<string> results = new();
        results.AddRange(taskMessages);
        results.AddRange(threadMessages);
        results.Add("Multithreading demo completed successfully.");
        return results;
    }

    /// <summary>
    /// Validates an employee object before persistence.
    /// </summary>
    /// <param name="employee">The employee to validate.</param>
    private static void ValidateEmployee(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);

        if (string.IsNullOrWhiteSpace(employee.FullName))
        {
            throw new ArgumentException("Employee name cannot be empty.", nameof(employee));
        }

        if (string.IsNullOrWhiteSpace(employee.Email))
        {
            throw new ArgumentException("Employee email cannot be empty.", nameof(employee));
        }
    }

    /// <summary>
    /// Ensures the selected department exists.
    /// </summary>
    /// <param name="departmentId">The department identifier.</param>
    private async Task EnsureDepartmentExistsAsync(int departmentId)
    {
        bool exists = await _databaseHelper.DepartmentExistsAsync(departmentId);

        if (!exists)
        {
            throw new InvalidOperationException($"Department {departmentId} does not exist.");
        }
    }

    /// <summary>
    /// Updates the in-memory recent employee queue.
    /// </summary>
    /// <param name="employees">The employees that were loaded.</param>
    private void UpdateRecentEmployees(IEnumerable<Employee> employees)
    {
        foreach (Employee employee in employees.Take(5))
        {
            _recentEmployees.Enqueue(employee);
        }

        while (_recentEmployees.Count > 10)
        {
            _recentEmployees.Dequeue();
        }
    }

    /// <summary>
    /// Adds an entry to the audit trail and tracks recently touched employees.
    /// </summary>
    /// <param name="message">The activity message.</param>
    /// <param name="employee">An optional employee reference.</param>
    private void TrackActivity(string message, Employee? employee = null)
    {
        _auditTrail.Push(message);

        if (employee is not null)
        {
            _recentEmployees.Enqueue(employee);
        }
    }

    /// <summary>
    /// Converts a data row into an employee object.
    /// </summary>
    /// <param name="row">The source row.</param>
    /// <returns>The mapped employee.</returns>
    private static Employee MapEmployeeRow(DataRow row)
    {
        return new Employee
        {
            Id = row.Field<int>("EmployeeId"),
            FullName = row.Field<string>("FullName") ?? string.Empty,
            DepartmentId = row.Field<int>("DepartmentId"),
            Salary = row.Field<decimal>("Salary"),
            Email = row.Field<string>("Email") ?? string.Empty,
            HireDate = row.Field<DateTime>("HireDate"),
            IsActive = row.Field<bool>("IsActive")
        };
    }
}