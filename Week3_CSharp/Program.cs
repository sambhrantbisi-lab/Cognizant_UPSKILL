using EmployeeManagementSystem.DataAccess;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using EmployeeManagementSystem.Utilities;

namespace EmployeeManagementSystem;

/// <summary>
/// Hosts the menu-driven console application entry point.
/// </summary>
public static class Program
{
    private const string ApplicationName = "Employee Management System";
    private const string DefaultConnectionString = "Server=localhost;Port=3306;Database=EmployeeDB;User ID=root;Password=your_password;SslMode=None;";
    private const string ExportFilePath = "employees.json";

    /// <summary>
    /// Starts the console application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static async Task Main(string[] args)
    {
        Console.Title = ApplicationName;
        Console.WriteLine($"{ApplicationName} - .NET 8 Console Application");
        ConsoleHelper.PrintDivider();

        DatabaseHelper databaseHelper = new(DefaultConnectionString);
        JsonHelper jsonHelper = new();
        EmployeeService service = new(databaseHelper, jsonHelper);

        try
        {
            await databaseHelper.EnsureConnectionAsync();
            Console.WriteLine("Database connection verified.");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine("Update the connection string in Program.cs before using database features.");
        }

        await RunMenuAsync(service);
    }

    /// <summary>
    /// Runs the interactive menu loop.
    /// </summary>
    /// <param name="service">The employee service.</param>
    private static async Task RunMenuAsync(EmployeeService service)
    {
        bool exitRequested = false;

        while (!exitRequested)
        {
            ShowMenu();
            int choice = ConsoleHelper.PromptInt("Enter your choice: ");

            try
            {
                switch (choice)
                {
                    case 1:
                        await HandleAddEmployeeAsync(service);
                        break;
                    case 2:
                        await HandleViewEmployeesAsync(service);
                        break;
                    case 3:
                        await HandleSearchEmployeeAsync(service);
                        break;
                    case 4:
                        await HandleUpdateEmployeeAsync(service);
                        break;
                    case 5:
                        await HandleDeleteEmployeeAsync(service);
                        break;
                    case 6:
                        await HandleExportEmployeesAsync(service);
                        break;
                    case 7:
                        await HandleMultithreadingAsync(service);
                        break;
                    case 8:
                        exitRequested = true;
                        Console.WriteLine("Exiting application.");
                        break;
                    default:
                        Console.WriteLine("Please choose a valid menu option.");
                        break;
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Input error: {ex.Message}");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Operation error: {ex.Message}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"File error: {ex.Message}");
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine($"Null reference error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }

            if (!exitRequested)
            {
                ConsoleHelper.PrintDivider();
            }
        }
    }

    /// <summary>
    /// Displays the application menu.
    /// </summary>
    private static void ShowMenu()
    {
        Console.WriteLine("1. Add Employee");
        Console.WriteLine("2. View Employees");
        Console.WriteLine("3. Search Employee");
        Console.WriteLine("4. Update Employee");
        Console.WriteLine("5. Delete Employee");
        Console.WriteLine("6. Export Employees to JSON");
        Console.WriteLine("7. Demonstrate Multithreading");
        Console.WriteLine("8. Exit");
    }

    /// <summary>
    /// Handles the Add Employee menu option.
    /// </summary>
    /// <param name="service">The employee service.</param>
    private static async Task HandleAddEmployeeAsync(EmployeeService service)
    {
        ConsoleHelper.PrintDivider();
        Console.WriteLine("Available departments:");
        List<Department> departments = await service.GetDepartmentsAsync();
        PrintDepartments(departments);

        Dictionary<int, Department> departmentLookup = departments.ToDictionary(department => department.Id);
        Console.WriteLine($"Loaded {departmentLookup.Count} departments into a dictionary cache.");

        Employee employee = PromptEmployeeDetails(includeId: false);
        await service.AddEmployeeAsync(employee);

        Console.WriteLine($"Employee {employee.FullName} added successfully.");
    }

    /// <summary>
    /// Handles the View Employees menu option.
    /// </summary>
    /// <param name="service">The employee service.</param>
    private static async Task HandleViewEmployeesAsync(EmployeeService service)
    {
        List<Employee> employees = await service.GetAllEmployeesAsync();
        PrintEmployees(employees);
    }

    /// <summary>
    /// Handles the Search Employee menu option.
    /// </summary>
    /// <param name="service">The employee service.</param>
    private static async Task HandleSearchEmployeeAsync(EmployeeService service)
    {
        Console.WriteLine("1. Search by ID");
        Console.WriteLine("2. Search by Department");
        int searchChoice = ConsoleHelper.PromptInt("Enter your search choice: ");

        switch (searchChoice)
        {
            case 1:
                int employeeId = ConsoleHelper.PromptInt("Enter employee ID: ");
                Employee? employee = await service.SearchEmployeeAsync(employeeId);
                if (employee is null)
                {
                    Console.WriteLine("No employee found for the supplied ID.");
                }
                else
                {
                    PrintEmployee(employee);
                }

                break;
            case 2:
                string departmentName = ConsoleHelper.PromptRequiredString("Enter department name: ");
                List<Employee> employees = await service.SearchEmployeeAsync(departmentName);
                PrintEmployees(employees);
                break;
            default:
                Console.WriteLine("Invalid search option.");
                break;
        }
    }

    /// <summary>
    /// Handles the Update Employee menu option.
    /// </summary>
    /// <param name="service">The employee service.</param>
    private static async Task HandleUpdateEmployeeAsync(EmployeeService service)
    {
        int employeeId = ConsoleHelper.PromptInt("Enter employee ID to update: ");
        Employee employee = PromptEmployeeDetails(includeId: true, employeeId: employeeId);
        await service.UpdateEmployeeAsync(employee);

        Console.WriteLine($"Employee {employee.Id} updated successfully.");
    }

    /// <summary>
    /// Handles the Delete Employee menu option.
    /// </summary>
    /// <param name="service">The employee service.</param>
    private static async Task HandleDeleteEmployeeAsync(EmployeeService service)
    {
        int employeeId = ConsoleHelper.PromptInt("Enter employee ID to delete: ");
        bool confirmDeletion = ConsoleHelper.PromptYesNo("Are you sure you want to delete this employee?", defaultValue: false);

        if (!confirmDeletion)
        {
            Console.WriteLine("Delete cancelled.");
            return;
        }

        await service.DeleteEmployeeAsync(employeeId);
        Console.WriteLine($"Employee {employeeId} deleted successfully.");
    }

    /// <summary>
    /// Handles the Export Employees to JSON menu option.
    /// </summary>
    /// <param name="service">The employee service.</param>
    private static async Task HandleExportEmployeesAsync(EmployeeService service)
    {
        await service.ExportEmployeesToJsonAsync(ExportFilePath);
        Console.WriteLine($"Employees exported to {ExportFilePath}.");
    }

    /// <summary>
    /// Handles the multithreading demo menu option.
    /// </summary>
    /// <param name="service">The employee service.</param>
    private static async Task HandleMultithreadingAsync(EmployeeService service)
    {
        IReadOnlyList<string> messages = await service.DemonstrateMultithreadingAsync();

        foreach (string message in messages)
        {
            Console.WriteLine(message);
        }
    }

    /// <summary>
    /// Prompts the user for employee details.
    /// </summary>
    /// <param name="includeId">Indicates whether the employee ID should be entered.</param>
    /// <param name="employeeId">The employee identifier when editing an existing employee.</param>
    /// <returns>The collected employee details.</returns>
    private static Employee PromptEmployeeDetails(bool includeId, int employeeId = 0)
    {
        if (includeId)
        {
            employeeId = employeeId == 0 ? ConsoleHelper.PromptInt("Enter employee ID: ") : employeeId;
        }

        string fullName = ConsoleHelper.PromptRequiredString("Enter full name: ");
        int departmentId = ConsoleHelper.PromptInt("Enter department ID: ");
        decimal salary = ConsoleHelper.PromptDecimal("Enter salary: ");
        string email = ConsoleHelper.PromptRequiredString("Enter email address: ");
        DateTime hireDate = ConsoleHelper.PromptDate("Enter hire date (yyyy-MM-dd or your local format): ");
        bool isActive = ConsoleHelper.PromptYesNo("Is the employee active?", defaultValue: true);

        Employee employee = includeId ? new Employee(employeeId, fullName, departmentId, salary, email) : new Employee(0, fullName, departmentId, salary, email);
        employee.HireDate = hireDate;
        employee.IsActive = isActive;

        EntityBase entity = employee;
        Console.WriteLine($"Captured employee: {entity.Describe()}");

        return employee;
    }

    /// <summary>
    /// Prints all employees in a tabular format.
    /// </summary>
    /// <param name="employees">The employees to print.</param>
    private static void PrintEmployees(IEnumerable<Employee> employees)
    {
        List<Employee> employeeList = employees.ToList();

        if (employeeList.Count == 0)
        {
            Console.WriteLine("No employee records found.");
            return;
        }

        ConsoleHelper.PrintDivider();
        Console.WriteLine($"{"ID",-6}{"Name",-24}{"Department",-20}{"Salary",14}{"Email",-30}{"Hire Date",-12}{"Active",10}");
        ConsoleHelper.PrintDivider();

        foreach (Employee employee in employeeList)
        {
            Console.WriteLine(employee.Print());
        }

        ConsoleHelper.PrintDivider();
    }

    /// <summary>
    /// Prints a single employee in a tabular format.
    /// </summary>
    /// <param name="employee">The employee to print.</param>
    private static void PrintEmployee(Employee employee)
    {
        PrintEmployees(new[] { employee });
    }

    /// <summary>
    /// Prints all departments in a tabular format.
    /// </summary>
    /// <param name="departments">The departments to print.</param>
    private static void PrintDepartments(IEnumerable<Department> departments)
    {
        List<Department> departmentList = departments.ToList();

        if (departmentList.Count == 0)
        {
            Console.WriteLine("No departments found.");
            return;
        }

        ConsoleHelper.PrintDivider();
        Console.WriteLine($"{"ID",-6}{"Department",-24}{"Location",-20}{"Description"}");
        ConsoleHelper.PrintDivider();

        foreach (Department department in departmentList)
        {
            Console.WriteLine(department.Print());
        }

        ConsoleHelper.PrintDivider();
    }
}