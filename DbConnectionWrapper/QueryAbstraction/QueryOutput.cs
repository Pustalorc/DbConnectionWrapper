using System.Collections.Generic;
using System.Data.Common;
using JetBrains.Annotations;
using Pustalorc.Libraries.DbConnectionWrapper.ResultTableAbstraction;

namespace Pustalorc.Libraries.DbConnectionWrapper.QueryAbstraction;

/// <summary>
///     Base class to wrap the results from queries, with methods to help retrieving the information back.
/// </summary>
[UsedImplicitly]
public class QueryOutput
{
    /// <summary>
    ///     The query that was executed, with its details.
    /// </summary>
    [UsedImplicitly]
    public Query Query { get; }

    /// <summary>
    ///     The output from <see cref="DbCommand" /> after executing the query.
    /// </summary>
    /// <remarks>
    ///     This is type object? because DbCommand.ExecuteScalar() returns object?, compared to Reader and NonQuery.
    ///     This allows us to store all results in this property.
    /// </remarks>
    [UsedImplicitly]
    public object? Result { get; }

    /// <summary>
    ///     Construct a new output for a query.
    /// </summary>
    /// <param name="query">The query that was executed, with its details.</param>
    /// <param name="result">The output from <see cref="DbCommand" /> after executing the query.</param>
    [UsedImplicitly]
    public QueryOutput(Query query, object? result)
    {
        Query = query;
        Result = result;
    }

    /// <summary>
    ///     Attempts to get a value of type T from the result in this class.
    /// </summary>
    /// <typeparam name="T">The type to get or check from the result.</typeparam>
    /// <returns>The instance of type T from the result, or the default value of T if the value isn't of type T.</returns>
    [UsedImplicitly]
    public T? GetTFromResult<T>()
    {
        return Result is T t ? t : default;
    }

    /// <summary>
    ///     Gets the reader result from the query, as a <see cref="List{Row}" />
    /// </summary>
    /// <returns>
    ///     An empty <see cref="List{Row}" />, or a <see cref="List{Row}" /> with all the result rows from the reader query.
    /// </returns>
    [UsedImplicitly]
    public List<Row> GetReaderResult()
    {
        return GetTFromResult<List<Row>>() ?? new List<Row>();
    }

    /// <summary>
    ///     Gets the non query result from the query, which is the number of rows affected, as an integer, by default of type
    ///     int.
    /// </summary>
    /// <returns>
    ///     An integer of the number of affected rows.
    /// </returns>
    [UsedImplicitly]
    public int GetNonQueryResult()
    {
        return GetTFromResult<int>();
    }
}