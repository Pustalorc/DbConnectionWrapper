namespace Pustalorc.Libraries.DbConnectionWrapper.ResultTableAbstraction;

/// <summary>
/// Class that defines a column with a value for a row in a table, primarily from the Reader result.
/// </summary>
public class Column
{
    /// <summary>
    /// The name of the column.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The value within the column.
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// Constructs a new column instance.
    /// </summary>
    /// <param name="name">The name of the column.</param>
    /// <param name="value">The value in the column.</param>
    public Column(string name, object? value)
    {
        Name = name;
        Value = value;
    }

    /// <summary>
    /// Attempts to get a value of type T from the value of this column.
    /// </summary>
    /// <typeparam name="T">The type to get or check from the value.</typeparam>
    /// <returns>The instance of type T from the value, or the default value of T if it isn't of type T.</returns>
    /// <remarks>
    /// This will not do a conversion to a different type.
    /// You cannot get a string return from the database and convert it to an integer here.
    /// </remarks>
    public T? GetTFromValue<T>()
    {
        return Value is T t ? t : default;
    }
}