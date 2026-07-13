namespace EmployeeManagementSystem.Models;

/// <summary>
/// Provides the common base for entities used by the application.
/// </summary>
public abstract class EntityBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EntityBase"/> class.
    /// </summary>
    protected EntityBase()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="EntityBase"/> class with an identifier.
    /// </summary>
    /// <param name="id">The entity identifier.</param>
    protected EntityBase(int id)
    {
        Id = id;
    }

    /// <summary>
    /// Gets or sets the primary identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets the creation timestamp for the entity.
    /// </summary>
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;

    /// <summary>
    /// Returns a descriptive text for the entity.
    /// </summary>
    /// <returns>A human-readable description.</returns>
    public abstract string Describe();
}