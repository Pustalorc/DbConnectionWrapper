using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace Pustalorc.Libraries.DbConnectionWrapper.ResultTableAbstraction;

/// <summary>
///     Class that defines a row in a table, primarily from the Reader result.
/// </summary>
[UsedImplicitly]
public class Row
{
    /// <summary>
    ///     The columns and their respective values for this row.
    /// </summary>
    [UsedImplicitly]
    public IReadOnlyList<Column> Columns { get; }

    /// <summary>
    ///     The columns indexed by name for a quick lookup by name.
    /// </summary>
    internal Dictionary<string, int> IndexedColumns { get; }

    /// <summary>
    ///     Constructs a new row with the column information.
    /// </summary>
    /// <param name="columns">The columns and their respective values for this row.</param>
    [UsedImplicitly]
    public Row(IEnumerable<Column> columns)
    {
        Columns = new ReadOnlyCollection<Column>(columns.ToList());
        IndexedColumns = new Dictionary<string, int>();

        for (var i = 0; i < Columns.Count; i++)
            IndexedColumns.Add(Columns[i].Name, i);
    }

    /// <summary>
    ///     Gets a column identified by its column name. If no column is found, returns null.
    /// </summary>
    /// <param name="key">The name of the column to get.</param>
    /// <remarks>
    ///     This search by default is case sensitive. Use the exact column name.
    /// </remarks>
    [UsedImplicitly]
    public Column? this[string key] => !IndexedColumns.TryGetValue(key, out var value) ? null : this[value];

    /// <summary>
    ///     Gets a column by its index.
    /// </summary>
    /// <param name="index">The index of the column.</param>
    /// <exception cref="IndexOutOfRangeException"></exception>
    [UsedImplicitly]
    public Column this[int index] => Columns[index];

    /// <summary>
    ///     Attempts to get a value of type T from a specific column by name.
    /// </summary>
    /// <param name="columnName">The name of the column to get the value from.</param>
    /// <typeparam name="T">The type to get for the column's value.</typeparam>
    /// <returns>The instance of type T from the column's value, or the default value of T if it isn't of type T.</returns>
    /// <remarks>
    ///     This will not do a conversion to a different type.
    ///     You cannot get a string return from the database and convert it to an integer here.
    /// </remarks>
    [UsedImplicitly]
    public T? GetColumnValue<T>(string columnName)
    {
        return IndexedColumns.TryGetValue(columnName, out var value) ? this[value].GetTFromValue<T>() : default;
    }

    /// <summary>
    ///     Attempts to get a value of type T from a specific column by index.
    /// </summary>
    /// <param name="index">The index of the column to get the value from.</param>
    /// <typeparam name="T">The type to get for the column's value.</typeparam>
    /// <returns>The instance of type T from the column's value, or the default value of T if it isn't of type T.</returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    /// <remarks>
    ///     This will not do a conversion to a different type.
    ///     You cannot get a string return from the database and convert it to an integer here.
    /// </remarks>
    [UsedImplicitly]
    public T? GetColumnValue<T>(int index)
    {
        return this[index].GetTFromValue<T>();
    }
}