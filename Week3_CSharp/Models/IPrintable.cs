namespace EmployeeManagementSystem.Models;

/// <summary>
/// Defines a contract for types that can render themselves for the console.
/// </summary>
public interface IPrintable
{
    /// <summary>
    /// Returns a formatted console string for the object.
    /// </summary>
    /// <returns>The formatted string.</returns>
    string Print();
}