using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Pustalorc.Libraries.DbConnectionWrapper.QueryAbstraction;

/// <summary>
/// Base class to wrap the basic requirements of a query.
/// </summary>
/// <remarks>
/// 2 callbacks are included in order to execute more queries, either synchronously or asynchronously, right after the
/// current query has finished execution and generated an output.
/// </remarks>
public class Query
{
    /// <summary>
    /// The string of the query that gets executed in the database.
    /// </summary>
    /// <remarks>
    /// An example of what makes a query string:
    /// SELECT * FROM `SampleTable` WHERE `Id`=@id;
    /// </remarks>
    public string QueryString { get; }

    /// <summary>
    /// The type of the query that will be executed.
    /// </summary>
    /// <remarks>
    /// For more information, see <see cref="EQueryType"/> and its documentation.
    /// </remarks>
    public EQueryType Type { get; }

    /// <summary>
    /// The parameters to be used on the underlying DbCommand to bind into the query string.
    /// </summary>
    public IEnumerable<DbParameter> Parameters { get; }

    /// <summary>
    /// The callback to run after the query has executed.
    /// </summary>
    /// <remarks>
    /// This callback passes DbConnection and DbTransaction to allow for further synchronous calls after the current query runs.
    /// For example: you run a query to check if a table exists, and then use a callback for it to execute a query to create the table if it doesnt exist.
    /// 
    /// Note that throwing an exception during a callback when the callback was from a ExecuteTransaction, will result in the transaction rolling back and not commit any of the data or changes on the transaction.
    /// This does not occur when using ExecuteQuery, as no transaction is started by default in said method.
    /// This behaviour can be overriden.
    /// </remarks>
    public Action<QueryOutput, DbConnection, DbTransaction?>? Callback { get; }

    /// <summary>
    /// The callback to run after the query has executed.
    /// </summary>
    /// <remarks>
    /// This callback passes DbConnection and DbTransaction to allow for further asynchronous calls after the current query runs.
    /// For example: you run a query to check if a table exists, and then use a callback for it to execute a query to create the table if it doesnt exist.
    /// 
    /// Note that throwing an exception during a callback when the callback was from a ExecuteTransaction, will result in the transaction rolling back and not commit any of the data or changes on the transaction.
    /// This does not occur when using ExecuteQuery, as no transaction is started by default in said method.
    /// This behaviour can be overriden.
    /// </remarks>
    public Func<QueryOutput, DbConnection, DbTransaction?, Task>? AsyncCallback { get; }

    /// <summary>
    /// Construct a new query with the provided information.
    /// </summary>
    /// <param name="queryString">The string of the query that gets executed in the database.</param>
    /// <param name="type">The type of the query that will be executed.</param>
    /// <param name="callback">The synchronous callback to run after the query has executed.</param>
    /// <param name="asyncCallback">The asynchronous callback to run after the query has executed.</param>
    /// <param name="parameters">The parameters to be used on the underlying DbCommand to bind into the query string.</param>
    /// <remarks>
    /// Since the most queries a program will run are either SELECT or INSERT, and SELECT queries do not expect a result
    /// <see cref="EQueryType.NonQuery"/> is the default type for all instantiated queries.
    /// </remarks>
    internal Query(string queryString, EQueryType type = EQueryType.NonQuery,
        Action<QueryOutput, DbConnection, DbTransaction?>? callback = null,
        Func<QueryOutput, DbConnection, DbTransaction?, Task>? asyncCallback = null, params DbParameter[] parameters)
    {
        QueryString = queryString;
        Type = type;
        Parameters = parameters;
        Callback = callback;
        AsyncCallback = asyncCallback;
    }

    /// <inheritdoc />
    public Query(string queryString, params DbParameter[] parameters) : this(queryString, EQueryType.NonQuery,
        parameters)
    {
    }

    /// <inheritdoc />
    public Query(string queryString, EQueryType type, params DbParameter[] parameters) : this(queryString, type, null,
        parameters)
    {
    }

    /// <inheritdoc />
    public Query(string queryString, Action<QueryOutput, DbConnection, DbTransaction?>? callback,
        params DbParameter[] parameters) : this(queryString, EQueryType.NonQuery, callback, parameters)
    {
    }

    /// <inheritdoc />
    public Query(string queryString, Func<QueryOutput, DbConnection, DbTransaction?, Task>? asyncCallback,
        params DbParameter[] parameters) : this(queryString, EQueryType.NonQuery, asyncCallback, parameters)
    {
    }

    /// <inheritdoc />
    public Query(string queryString, EQueryType type, Action<QueryOutput, DbConnection, DbTransaction?>? callback,
        params DbParameter[] parameters) : this(queryString, type, callback, null, parameters)
    {
    }

    /// <inheritdoc />
    public Query(string queryString, EQueryType type,
        Func<QueryOutput, DbConnection, DbTransaction?, Task>? asyncCallback, params DbParameter[] parameters) : this(
        queryString, type, null, asyncCallback, parameters)
    {
    }
}