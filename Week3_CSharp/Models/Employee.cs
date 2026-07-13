namespace EmployeeManagementSystem.Models;

/// <summary>
/// Represents an employee record and demonstrates inheritance, constructors, and encapsulation.
/// </summary>
public class Employee : EntityBase, IPrintable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Employee"/> class.
    /// </summary>
    public Employee()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Employee"/> class with the key identity fields.
    /// </summary>
    /// <param name="id">The employee identifier.</param>
    /// <param name="fullName">The employee full name.</param>
    public Employee(int id, string fullName)
        : this(id, fullName, 0, 0m, string.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Employee"/> class with all core values.
    /// </summary>
    /// <param name="id">The employee identifier.</param>
    /// <param name="fullName">The employee full name.</param>
    /// <param name="departmentId">The department identifier.</param>
    /// <param name="salary">The employee salary.</param>
    /// <param name="email">The employee email address.</param>
    public Employee(int id, string fullName, int departmentId, decimal salary, string email)
        : base(id)
    {
        FullName = fullName;
        DepartmentId = departmentId;
        Salary = salary;
        Email = email;
    }

    /// <summary>
    /// Gets or sets the employee full name.
    /// </summary>
    public string FullName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the department identifier.
    /// </summary>
    public int DepartmentId { get; set; }

    /// <summary>
    /// Gets or sets the department name when the employee is loaded from the database.
    /// </summary>
    public string? DepartmentName { get; set; }

    /// <summary>
    /// Gets or sets the employee salary.
    /// </summary>
    public decimal Salary { get; set; }

    /// <summary>
    /// Gets or sets the employee email address.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the employee hire date.
    /// </summary>
    public DateTime HireDate { get; set; } = DateTime.Today;

    /// <summary>
    /// Gets or sets a value indicating whether the employee is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Returns a descriptive string for the employee.
    /// </summary>
    /// <returns>The description text.</returns>
    public override string Describe()
    {
        return $"{Id}: {FullName} ({DepartmentName ?? "Unassigned"})";
    }

    /// <summary>
    /// Returns a formatted console string for the employee.
    /// </summary>
    /// <returns>The formatted string.</returns>
    public string Print()
    {
        return $"{Id,-6}{FullName,-24}{DepartmentName ?? "N/A",-20}{Salary,14:C}{Email,-30}{HireDate:yyyy-MM-dd}{IsActive,10}";
    }

    /// <summary>
    /// Returns a formatted console string and optionally includes the department identifier.
    /// </summary>
    /// <param name="includeDepartmentId">Indicates whether the department identifier should be appended.</param>
    /// <returns>The formatted string.</returns>
    public string Print(bool includeDepartmentId)
    {
        return includeDepartmentId ? $"{Print()} | DeptId: {DepartmentId}" : Print();
    }

    /// <summary>
    /// Applies a salary raise to the employee.
    /// </summary>
    /// <param name="percentage">The raise percentage.</param>
    public void ApplyRaise(decimal percentage)
    {
        Salary += Salary * percentage / 100m;
    }

    /// <summary>
    /// Applies a salary raise and optionally rounds the result.
    /// </summary>
    /// <param name="percentage">The raise percentage.</param>
    /// <param name="roundToWholeCurrency">Indicates whether the salary should be rounded.</param>
    public void ApplyRaise(decimal percentage, bool roundToWholeCurrency)
    {
        ApplyRaise(percentage);

        if (roundToWholeCurrency)
        {
            Salary = Math.Round(Salary, 0);
        }
    }
}