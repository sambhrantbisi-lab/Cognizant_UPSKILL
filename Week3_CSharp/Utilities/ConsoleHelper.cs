using System.Globalization;

namespace EmployeeManagementSystem.Utilities;

/// <summary>
/// Centralizes console input and output helper methods.
/// </summary>
public static class ConsoleHelper
{
    /// <summary>
    /// Reads a required string from the console.
    /// </summary>
    /// <param name="prompt">The prompt text to display.</param>
    /// <returns>The trimmed user input.</returns>
    public static string PromptRequiredString(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? value = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }

            Console.WriteLine("Input cannot be empty.");
        }
    }

    /// <summary>
    /// Reads an optional string from the console.
    /// </summary>
    /// <param name="prompt">The prompt text to display.</param>
    /// <returns>The trimmed user input or null when the user presses enter.</returns>
    public static string? PromptOptionalString(string prompt)
    {
        Console.Write(prompt);
        string? value = Console.ReadLine();

        return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
    }

    /// <summary>
    /// Reads an integer from the console.
    /// </summary>
    /// <param name="prompt">The prompt text to display.</param>
    /// <returns>The parsed integer.</returns>
    public static int PromptInt(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? value = Console.ReadLine();

            if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int parsedValue))
            {
                return parsedValue;
            }

            Console.WriteLine("Please enter a valid integer.");
        }
    }

    /// <summary>
    /// Reads a decimal value from the console.
    /// </summary>
    /// <param name="prompt">The prompt text to display.</param>
    /// <returns>The parsed decimal.</returns>
    public static decimal PromptDecimal(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? value = Console.ReadLine();

            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal parsedValue))
            {
                return parsedValue;
            }

            Console.WriteLine("Please enter a valid decimal value.");
        }
    }

    /// <summary>
    /// Reads a date value from the console.
    /// </summary>
    /// <param name="prompt">The prompt text to display.</param>
    /// <returns>The parsed date.</returns>
    public static DateTime PromptDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string? value = Console.ReadLine();

            if (DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedValue))
            {
                return parsedValue;
            }

            Console.WriteLine("Please enter a valid date value.");
        }
    }

    /// <summary>
    /// Reads a yes or no answer from the console.
    /// </summary>
    /// <param name="prompt">The prompt text to display.</param>
    /// <param name="defaultValue">The default value when the user presses enter.</param>
    /// <returns>The parsed boolean answer.</returns>
    public static bool PromptYesNo(string prompt, bool defaultValue = true)
    {
        while (true)
        {
            Console.Write($"{prompt} {(defaultValue ? "[Y/n]" : "[y/N]")}: ");
            string? value = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            if (value.Equals("y", StringComparison.OrdinalIgnoreCase) || value.Equals("yes", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (value.Equals("n", StringComparison.OrdinalIgnoreCase) || value.Equals("no", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            Console.WriteLine("Please answer with yes or no.");
        }
    }

    /// <summary>
    /// Prints a visual divider line.
    /// </summary>
    public static void PrintDivider()
    {
        Console.WriteLine(new string('-', 120));
    }
}