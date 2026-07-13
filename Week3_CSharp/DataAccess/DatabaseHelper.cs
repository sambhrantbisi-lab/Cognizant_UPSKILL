using System.Data;
using EmployeeManagementSystem.Models;
using MySqlConnector;

namespace EmployeeManagementSystem.DataAccess;

/// <summary>
/// Encapsulates MySQL ADO.NET operations for employees and departments.
/// </summary>
public class DatabaseHelper
{
    private const string EmployeeSelectSql = """
SELECT e.EmployeeId,
       e.FullName,
       e.DepartmentId,
       d.DepartmentName,
       e.Salary,
       e.Email,
       e.HireDate,
       e.IsActive
FROM Employees e
INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
""";

    private readonly string _connectionString;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseHelper"/> class.
    /// </summary>
    /// <param name="connectionString">The MySQL connection string.</param>
    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Creates a new MySQL connection instance.
    /// </summary>
    /// <returns>A configured connection object.</returns>
    private MySqlConnection CreateConnection()
    {
        return new MySqlConnection(_connectionString);
    }

    /// <summary>
    /// Verifies the database connection can be opened.
    /// </summary>
    /// <returns>A task representing the operation.</returns>
    public async Task EnsureConnectionAsync()
    {
        try
        {
            await using MySqlConnection connection = CreateConnection();
            await connection.OpenAsync();
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException("Unable to connect to the MySQL database. Check the connection string and server status.", ex);
        }
    }

    /// <summary>
    /// Inserts a new employee record.
    /// </summary>
    /// <param name="employee">The employee to insert.</param>
    /// <returns>The number of affected rows.</returns>
    public async Task<int> InsertEmployeeAsync(Employee employee)
    {
        const string sql = """
INSERT INTO Employees (FullName, DepartmentId, Salary, Email, HireDate, IsActive)
VALUES (@FullName, @DepartmentId, @Salary, @Email, @HireDate, @IsActive);
""";

        try
        {
            await using MySqlConnection connection = CreateConnection();
            await connection.OpenAsync();

            await using MySqlCommand command = new(sql, connection);
            AddEmployeeParameters(command, employee);
            return await command.ExecuteNonQueryAsync();
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException("Failed to insert the employee record.", ex);
        }
    }

    /// <summary>
    /// Updates an employee record.
    /// </summary>
    /// <param name="employee">The employee to update.</param>
    /// <returns>The number of affected rows.</returns>
    public async Task<int> UpdateEmployeeAsync(Employee employee)
    {
        const string sql = """
UPDATE Employees
SET FullName = @FullName,
    DepartmentId = @DepartmentId,
    Salary = @Salary,
    Email = @Email,
    HireDate = @HireDate,
    IsActive = @IsActive
WHERE EmployeeId = @EmployeeId;
""";

        try
        {
            await using MySqlConnection connection = CreateConnection();
            await connection.OpenAsync();

            await using MySqlCommand command = new(sql, connection);
            AddEmployeeParameters(command, employee);
            command.Parameters.AddWithValue("@EmployeeId", employee.Id);
            return await command.ExecuteNonQueryAsync();
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException("Failed to update the employee record.", ex);
        }
    }

    /// <summary>
    /// Deletes an employee record by identifier.
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    /// <returns>The number of affected rows.</returns>
    public async Task<int> DeleteEmployeeAsync(int employeeId)
    {
        const string sql = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId;";

        try
        {
            await using MySqlConnection connection = CreateConnection();
            await connection.OpenAsync();

            await using MySqlCommand command = new(sql, connection);
            command.Parameters.AddWithValue("@EmployeeId", employeeId);
            return await command.ExecuteNonQueryAsync();
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException("Failed to delete the employee record.", ex);
        }
    }

    /// <summary>
    /// Retrieves a single employee by identifier using a data reader.
    /// </summary>
    /// <param name="employeeId">The employee identifier.</param>
    /// <returns>The employee if found; otherwise null.</returns>
    public async Task<Employee?> GetEmployeeByIdAsync(int employeeId)
    {
        const string sql = EmployeeSelectSql + " WHERE e.EmployeeId = @EmployeeId;";

        try
        {
            await using MySqlConnection connection = CreateConnection();
            await connection.OpenAsync();

            await using MySqlCommand command = new(sql, connection);
            command.Parameters.AddWithValue("@EmployeeId", employeeId);

            await using MySqlDataReader reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync())
            {
                return null;
            }

            return MapEmployee(reader);
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException("Failed to search for the employee by ID.", ex);
        }
    }

    /// <summary>
    /// Retrieves employees that belong to a department using a data reader.
    /// </summary>
    /// <param name="departmentName">The department name to search for.</param>
    /// <returns>The matching employees.</returns>
    public async Task<List<Employee>> GetEmployeesByDepartmentAsync(string departmentName)
    {
        const string sql = EmployeeSelectSql + " WHERE d.DepartmentName LIKE CONCAT('%', @DepartmentName, '%') ORDER BY e.FullName;";

        try
        {
            List<Employee> employees = new();

            await using MySqlConnection connection = CreateConnection();
            await connection.OpenAsync();

            await using MySqlCommand command = new(sql, connection);
            command.Parameters.AddWithValue("@DepartmentName", departmentName);

            await using MySqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                employees.Add(MapEmployee(reader));
            }

            return employees;
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException("Failed to search for employees by department.", ex);
        }
    }

    /// <summary>
    /// Retrieves all employees in a data table using a data adapter.
    /// </summary>
    /// <returns>The filled data table.</returns>
    public async Task<DataTable> GetEmployeesDataTableAsync()
    {
        try
        {
            await using MySqlConnection connection = CreateConnection();
            await connection.OpenAsync();

            using MySqlDataAdapter adapter = new(EmployeeSelectSql + " ORDER BY e.EmployeeId;", connection);
            DataTable dataTable = new("Employees");
            adapter.Fill(dataTable);
            return dataTable;
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException("Failed to retrieve employee data as a DataTable.", ex);
        }
    }

    /// <summary>
    /// Retrieves employee and department data as a data set.
    /// </summary>
    /// <returns>The filled data set.</returns>
    public async Task<DataSet> GetEmployeesDataSetAsync()
    {
        try
        {
            await using MySqlConnection connection = CreateConnection();
            await connection.OpenAsync();

            DataSet dataSet = new("EmployeeDB");

            using MySqlDataAdapter employeeAdapter = new("SELECT * FROM Employees ORDER BY EmployeeId;", connection);
            using MySqlDataAdapter departmentAdapter = new("SELECT * FROM Departments ORDER BY DepartmentId;", connection);

            employeeAdapter.Fill(dataSet, "Employees");
            departmentAdapter.Fill(dataSet, "Departments");

            return dataSet;
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException("Failed to retrieve data as a DataSet.", ex);
        }
    }

    /// <summary>
    /// Retrieves all departments using a data reader.
    /// </summary>
    /// <returns>The list of departments.</returns>
    public async Task<List<Department>> GetDepartmentsAsync()
    {
        const string sql = "SELECT DepartmentId, DepartmentName, Location, Description FROM Departments ORDER BY DepartmentName;";

        try
        {
            List<Department> departments = new();

            await using MySqlConnection connection = CreateConnection();
            await connection.OpenAsync();

            await using MySqlCommand command = new(sql, connection);
            await using MySqlDataReader reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                departments.Add(MapDepartment(reader));
            }

            return departments;
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException("Failed to retrieve departments.", ex);
        }
    }

    /// <summary>
    /// Checks whether a department exists.
    /// </summary>
    /// <param name="departmentId">The department identifier.</param>
    /// <returns>True when the department exists; otherwise false.</returns>
    public async Task<bool> DepartmentExistsAsync(int departmentId)
    {
        const string sql = "SELECT COUNT(1) FROM Departments WHERE DepartmentId = @DepartmentId;";

        try
        {
            await using MySqlConnection connection = CreateConnection();
            await connection.OpenAsync();

            await using MySqlCommand command = new(sql, connection);
            command.Parameters.AddWithValue("@DepartmentId", departmentId);

            object? result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result, System.Globalization.CultureInfo.InvariantCulture) > 0;
        }
        catch (MySqlException ex)
        {
            throw new InvalidOperationException("Failed to validate the department.", ex);
        }
    }

    /// <summary>
    /// Adds the standard parameters for an employee command.
    /// </summary>
    /// <param name="command">The command being prepared.</param>
    /// <param name="employee">The employee values.</param>
    private static void AddEmployeeParameters(MySqlCommand command, Employee employee)
    {
        command.Parameters.AddWithValue("@FullName", employee.FullName);
        command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);
        command.Parameters.AddWithValue("@Salary", employee.Salary);
        command.Parameters.AddWithValue("@Email", employee.Email);
        command.Parameters.AddWithValue("@HireDate", employee.HireDate);
        command.Parameters.AddWithValue("@IsActive", employee.IsActive);
    }

    /// <summary>
    /// Maps a data reader row to an employee object.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <returns>The mapped employee.</returns>
    private static Employee MapEmployee(MySqlDataReader reader)
    {
        object activeValue = reader.GetValue(reader.GetOrdinal("IsActive"));

        return new Employee
        {
            Id = reader.GetInt32("EmployeeId"),
            FullName = reader.GetString("FullName"),
            DepartmentId = reader.GetInt32("DepartmentId"),
            DepartmentName = reader.IsDBNull(reader.GetOrdinal("DepartmentName")) ? null : reader.GetString("DepartmentName"),
            Salary = reader.GetDecimal("Salary"),
            Email = reader.GetString("Email"),
            HireDate = reader.GetDateTime("HireDate"),
            IsActive = Convert.ToBoolean(activeValue, System.Globalization.CultureInfo.InvariantCulture)
        };
    }

    /// <summary>
    /// Maps a data reader row to a department object.
    /// </summary>
    /// <param name="reader">The data reader.</param>
    /// <returns>The mapped department.</returns>
    private static Department MapDepartment(MySqlDataReader reader)
    {
        return new Department
        {
            Id = reader.GetInt32("DepartmentId"),
            DepartmentName = reader.GetString("DepartmentName"),
            Location = reader.GetString("Location"),
            Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? string.Empty : reader.GetString("Description")
        };
    }
}