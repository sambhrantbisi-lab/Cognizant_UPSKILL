namespace EmployeeManagementSystem.Models;

/// <summary>
/// Represents a department record and demonstrates inheritance and polymorphism.
/// </summary>
public class Department : EntityBase, IPrintable
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Department"/> class.
    /// </summary>
    public Department()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Department"/> class with the essential values.
    /// </summary>
    /// <param name="id">The department identifier.</param>
    /// <param name="departmentName">The department name.</param>
    public Department(int id, string departmentName)
        : this(id, departmentName, string.Empty, string.Empty)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Department"/> class with all values.
    /// </summary>
    /// <param name="id">The department identifier.</param>
    /// <param name="departmentName">The department name.</param>
    /// <param name="location">The department location.</param>
    /// <param name="description">The department description.</param>
    public Department(int id, string departmentName, string location, string description)
        : base(id)
    {
        DepartmentName = departmentName;
        Location = location;
        Description = description;
    }

    /// <summary>
    /// Gets or sets the department name.
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the department location.
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the department description.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Returns a descriptive string for the department.
    /// </summary>
    /// <returns>The description text.</returns>
    public override string Describe()
    {
        return $"{Id}: {DepartmentName} ({Location})";
    }

    /// <summary>
    /// Returns a formatted console string for the department.
    /// </summary>
    /// <returns>The formatted string.</returns>
    public string Print()
    {
        return $"{Id,-6}{DepartmentName,-24}{Location,-20}{Description}";
    }

    /// <summary>
    /// Returns a formatted console string and optionally includes the description.
    /// </summary>
    /// <param name="includeDescription">Indicates whether the description should be appended.</param>
    /// <returns>The formatted string.</returns>
    public string Print(bool includeDescription)
    {
        return includeDescription ? $"{Print()} | {Description}" : Print();
    }
}