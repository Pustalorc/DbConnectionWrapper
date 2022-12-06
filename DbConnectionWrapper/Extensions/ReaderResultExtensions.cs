using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Pustalorc.Libraries.DbConnectionWrapper.ResultTableAbstraction;

namespace Pustalorc.Libraries.DbConnectionWrapper.Extensions;

/// <summary>
///     Extensions to reader results
/// </summary>
[UsedImplicitly]
public static class ReaderResultExtensions
{
    /// <summary>
    ///     Binds a reader result set to the specific type where possible.
    /// </summary>
    /// <param name="rows">The rows to bind.</param>
    /// <typeparam name="T">The type to bind to. This type should have an empty constructor.</typeparam>
    /// <returns>An IEnumerable of the rows bound to T.</returns>
    /// <remarks>
    ///     Binding is done on properties only, fields are completely ignored.
    ///     All properties must be non-static and public to be bound to.
    ///     All properties you wish to bind must be named exactly as the column names are in the database.
    /// </remarks>
    [UsedImplicitly]
    public static IEnumerable<T> Bind<T>(this IEnumerable<Row> rows) where T : new()
    {
        return rows.Select(Bind<T>);
    }

    /// <summary>
    ///     Binds a single row from a reader result to the specified type where possible.
    /// </summary>
    /// <param name="row">The row to bind.</param>
    /// <typeparam name="T">The type to bind to. This type should have an empty constructor.</typeparam>
    /// <returns>A new object of type T.</returns>
    /// <remarks>
    ///     Binding is done on properties only, fields are completely ignored.
    ///     All properties must be non-static and public to be bound to.
    ///     All properties you wish to bind must be named exactly as the column names are in the database.
    /// </remarks>
    [UsedImplicitly]
    public static T Bind<T>(this Row row) where T : new()
    {
        var t = new T();

        var type = typeof(T);
        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty);

        foreach (var prop in properties)
        {
            var propType = prop.PropertyType;

            if (!row.IndexedColumns.TryGetValue(prop.Name, out var index))
                continue;

            var value = row[index].Value;

            if (value is null || value.GetType() != propType)
                continue;

            prop.SetValue(t, value);
        }

        return t;
    }
}