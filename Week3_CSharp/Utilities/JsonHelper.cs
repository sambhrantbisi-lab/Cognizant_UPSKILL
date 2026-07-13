using System.Text.Json;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Utilities;

/// <summary>
/// Handles JSON file serialization and file-based persistence for employees.
/// </summary>
public class JsonHelper
{
    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true
    };

    /// <summary>
    /// Saves the supplied employees to a JSON file.
    /// </summary>
    /// <param name="filePath">The target file path.</param>
    /// <param name="employees">The employees to serialize.</param>
    public async Task SaveEmployeesAsync(string filePath, IEnumerable<Employee> employees)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        ArgumentNullException.ThrowIfNull(employees);

        try
        {
            string json = JsonSerializer.Serialize(employees.OrderBy(employee => employee.Id), SerializerOptions);
            await File.WriteAllTextAsync(filePath, json);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            throw new IOException($"Unable to write the JSON file '{filePath}'.", ex);
        }
    }

    /// <summary>
    /// Loads employees from a JSON file.
    /// </summary>
    /// <param name="filePath">The source file path.</param>
    /// <returns>The employees read from disk.</returns>
    public async Task<List<Employee>> LoadEmployeesAsync(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

        try
        {
            if (!File.Exists(filePath))
            {
                return new List<Employee>();
            }

            string json = await File.ReadAllTextAsync(filePath);
            List<Employee>? employees = JsonSerializer.Deserialize<List<Employee>>(json, SerializerOptions);

            return employees ?? new List<Employee>();
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or JsonException)
        {
            throw new IOException($"Unable to read the JSON file '{filePath}'.", ex);
        }
    }
}